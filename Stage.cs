using System;
using System.Collections.Generic;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ProjectTitan
{
    public enum SegmentType
    {
        Flat = 0,
        SlopedUp,
        SlopedDown
    };

    public struct StagePositionInformation
    {
        public bool valid;
        public StageSegment stage_segment;
        public float height;
    }

    public class Stage
    {
        Camera m_camera;

        List<StageSegment> m_stage_segments;
        Texture2D[] m_road_textures;
        Texture2D[][] m_scenery_textures;

        int m_road_width;
        int m_road_height;
        int m_flat_sloped_height_diff;

        // Camera
        Vector2 m_camera_initial_position;

        // Debug
        Texture2D m_pinky;

        public Stage(int n_segments, Texture2D[] road_textures, Texture2D[][] scenery_textures, Camera camera, Texture2D pinky)
        {
            m_stage_segments = new List<StageSegment>(n_segments);
            m_road_textures = road_textures;
            m_scenery_textures = scenery_textures;
            m_camera = camera;
            m_pinky = pinky;

            m_road_width  = m_road_textures[(int)SegmentType.Flat].Width;
            m_road_height = m_road_textures[(int)SegmentType.Flat].Height;
            m_flat_sloped_height_diff = m_road_textures[(int)SegmentType.SlopedUp].Height - m_road_textures[(int)SegmentType.Flat].Height;
        }

        public void InitializeCamera()
        {
            if (m_stage_segments.Count == 0) { Console.WriteLine("ERROR: Camera should be initialized after generating stage segments."); return; }

            m_camera.Position = new Vector2(m_stage_segments[0].Position.X, 0);
            m_camera.Center = new Vector2(m_camera.Center.X, InfoAt(m_camera.Center.X).height);
            m_camera_initial_position = m_camera.Position;
        }

        public void MoveCamera(Vector2 direction)
        {
            m_camera.SetHorizontal(m_camera.Position.X + direction.X);
            if (m_camera.Position.X > m_camera_initial_position.X)
            {
                m_camera.Position = new Vector2(m_camera_initial_position.X, m_camera.Position.Y);
            }
            m_camera.Center = new Vector2(m_camera.Center.X, InfoAt(m_camera.Center.X).height);
            //Console.WriteLine("Height at " + -m_camera.Center.X + " is " + HeightAt(m_camera.Center.X));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i=0; i < m_stage_segments.Count; i++)
            {
                m_stage_segments[i].Draw(spriteBatch);
            }
            //spriteBatch.Draw(m_pinky, -m_camera.Center, Color.White);
        }

        public StagePositionInformation InfoAt(float x)
        {
            StagePositionInformation info_object = new StagePositionInformation();
            x = -x;
            StageSegment target_segment = null;
            for (int i=0; i < m_stage_segments.Count; i++)
            {
                if (m_stage_segments[i].Position.X <= x && m_stage_segments[i].Position.X + m_road_width >= x)
                {
                    target_segment = m_stage_segments[i];
                    break;
                }
            }
            if (target_segment == null)
            {
                Console.WriteLine("ERROR: HeightAt() - Could not find target segment.");
                info_object.valid = false;
                return info_object;
            }

            float start_y  = target_segment.TopLeftPosition.Y;
            float target_y = start_y;
            switch (target_segment.SegmentType)
            {
                case SegmentType.SlopedDown:
                    start_y = target_segment.TopLeftPosition.Y + target_segment.Corrector;
                    target_y = target_segment.BottomRightPosition.Y - m_road_height;
                    break;
                case SegmentType.SlopedUp:
                    start_y = target_segment.Position.Y - m_road_height + target_segment.Corrector;
                    target_y = target_segment.TopRightPosition.Y;
                    break;
            }

            info_object.valid = true;
            info_object.stage_segment = target_segment;
            info_object.height = -MathHelper.Lerp(start_y, target_y, (x - target_segment.TopLeftPosition.X) / m_road_width);
            return info_object;
        }

        public void PlaceRoadSegments(int start_x, int start_y)
        {
            int x = start_x;
            int y = start_y;

            StageSegment previous_segment = FirstStageSegment(start_x, start_y);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.SlopedUp);
            AddStageSegment(ref previous_segment, SegmentType.SlopedUp);
            AddStageSegment(ref previous_segment, SegmentType.SlopedUp);
            AddStageSegment(ref previous_segment, SegmentType.SlopedUp);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.SlopedDown);
            AddStageSegment(ref previous_segment, SegmentType.SlopedDown);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.SlopedUp); 
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.SlopedDown);
            AddStageSegment(ref previous_segment, SegmentType.SlopedDown);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
        }

        public StageSegment FirstStageSegment(int x, int y)
        {
            StageSegment first_stage_segment = CreateStageSegment(x, y, SegmentType.Flat, 0, 0);
            m_stage_segments.Add(first_stage_segment);
            return first_stage_segment;
        }

        public void AddStageSegment(ref StageSegment previous_segment, SegmentType new_segment_type)
        {
            float x = previous_segment.Position.X + m_road_width;
            float y = previous_segment.Position.Y;

            float height_delta = 0;
            float corrector = 0;
            SegmentType previous_segment_type = previous_segment.SegmentType;
            if (previous_segment_type == SegmentType.Flat)
            {
                if (new_segment_type == SegmentType.SlopedUp)
                {
                    corrector = -1;
                    height_delta -= corrector;
                }
                else if (new_segment_type == SegmentType.SlopedDown)
                {
                    corrector = 1;
                    height_delta += m_flat_sloped_height_diff - corrector;
                }
            }
            else if (previous_segment_type == SegmentType.SlopedUp)
            {
                if (new_segment_type == SegmentType.SlopedUp)
                {
                    corrector = -2;
                    height_delta -= m_flat_sloped_height_diff + corrector;
                }
                else if (new_segment_type == SegmentType.Flat)
                {
                    corrector = -1;
                    height_delta -= m_flat_sloped_height_diff + corrector;
                }
            }
            else if (previous_segment_type == SegmentType.SlopedDown)
            {
                if (new_segment_type == SegmentType.SlopedDown)
                {
                    corrector = 2;
                    height_delta += m_flat_sloped_height_diff - corrector;
                }
                else if (new_segment_type == SegmentType.Flat)
                {
                    //y += m_flat_sloped_height_diff - 1;
                }
            }
            y += height_delta;

            StageSegment new_stage_segment = CreateStageSegment(x, y, new_segment_type, height_delta, corrector);
            m_stage_segments.Add(new_stage_segment);
            previous_segment = new_stage_segment;
        }

        public StageSegment CreateStageSegment(float x, float y, SegmentType segment_type, float height_delta, float corrector)
        {
            Texture2D texture = m_road_textures[(int)segment_type];
            Texture2D scenery_texture = m_scenery_textures[(int)segment_type][0];
            StageSegment new_segment = new StageSegment(new Vector2(x, y - texture.Height), texture, scenery_texture, segment_type, height_delta, corrector);
            return new_segment;
        }
    }

    public class StageSegment : GameObject
    {
        SegmentType m_segment_type;
        Texture2D m_scenery_texture;
        public SegmentType SegmentType { get { return m_segment_type; } }
        public float HeightDelta { get; set; }
        public float Corrector { get; set; }

        private const int m_scenery_height_offset = 413;

        public StageSegment(Vector2 position, Texture2D texture, Texture2D scenery_texture, SegmentType segment_type, float height_delta, float corrector) : base(position, texture)
        {
            m_segment_type = segment_type;
            m_scenery_texture = scenery_texture;
            HeightDelta = height_delta;
            Corrector = corrector;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (!GameManager.DEBUG)
            {
                // Draw scenery
                spritebatch.Draw(m_scenery_texture, new Vector2(TopLeftPosition.X, TopLeftPosition.Y - m_scenery_height_offset), Color.White);
            }
            // Draw road segment
            base.Draw(spritebatch);
        }
    }
}
