using System;
using System.Collections.Generic;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectTitan
{
    public class StageGenerator
    {
        StageManager m_stagemanager;
        Texture2D[] m_road_textures;
        Texture2D[][] m_scenery_textures;
        Hashtable m_slope_height_increase_dict;

        StageParser stageparser;
        List<SegmentType>[] m_stages_to_generate;

        public StageGenerator(Texture2D[] road_textures, Texture2D[][] scenery_textures, StageManager stagemanager)
        {
            m_stagemanager = stagemanager;
            m_road_textures = road_textures;
            m_scenery_textures = scenery_textures;
            m_stagemanager.RoadWidth = m_road_textures[(int)SegmentType.Flat].Width;
            m_stagemanager.RoadHeight = m_road_textures[(int)SegmentType.Flat].Height;
            m_slope_height_increase_dict = CreateHeightDictionary();
            stageparser = new StageParser(m_stagemanager.RoadWidth);
            m_stages_to_generate = stageparser.ParseStages();
        }

        public Hashtable CreateHeightDictionary()
        {

            Hashtable flat_table = new Hashtable
            {
                { SegmentType.Flat,   Vector2.Zero },
                { SegmentType.Up5,    Vector2.Zero },
                { SegmentType.Up10,   new Vector2(0, -1) },
                { SegmentType.Down5,  new Vector2(m_road_textures[(int)SegmentType.Flat].Height - m_road_textures[(int)SegmentType.Down5].Height, 0) },
                { SegmentType.Down10, new Vector2(m_road_textures[(int)SegmentType.Flat].Height - m_road_textures[(int)SegmentType.Down10].Height, 0) },
            };

            Hashtable up5_table = new Hashtable
            {
                { SegmentType.Up5,    new Vector2(m_road_textures[(int)SegmentType.Up5].Height - m_stagemanager.RoadHeight, 0) },
                { SegmentType.Flat,   new Vector2(m_road_textures[(int)SegmentType.Up5].Height - m_stagemanager.RoadHeight, 0) },
                { SegmentType.Up10,   new Vector2(m_road_textures[(int)SegmentType.Up5].Height - m_stagemanager.RoadHeight, -1) },
            };

            Hashtable up10_table = new Hashtable
            {
                { SegmentType.Up10,   new Vector2(m_road_textures[(int)SegmentType.Up10].Height - m_stagemanager.RoadHeight, -2) },
                { SegmentType.Flat,   new Vector2(m_road_textures[(int)SegmentType.Up10].Height - m_stagemanager.RoadHeight, 0) },
                { SegmentType.Up5,    new Vector2(m_road_textures[(int)SegmentType.Up10].Height - m_stagemanager.RoadHeight, -1) },
            };

            Hashtable down5_table = new Hashtable
            {
                { SegmentType.Flat,   new Vector2(0, 1) },
                { SegmentType.Down5,  new Vector2(m_stagemanager.RoadHeight - m_road_textures[(int)SegmentType.Down5].Height, 0) },
                { SegmentType.Down10, new Vector2(m_stagemanager.RoadHeight - m_road_textures[(int)SegmentType.Down10].Height, 2) },
            };

            Hashtable down10_table = new Hashtable
            {
                { SegmentType.Flat,   new Vector2(0, 1) },
                { SegmentType.Down5,  new Vector2(m_stagemanager.RoadHeight - m_road_textures[(int)SegmentType.Down5].Height, 2) },
                { SegmentType.Down10, new Vector2(m_stagemanager.RoadHeight - m_road_textures[(int)SegmentType.Down10].Height, 2) },
            };

            return new Hashtable
            {
                { SegmentType.Flat,   flat_table },
                { SegmentType.Up5,    up5_table },
                { SegmentType.Up10,   up10_table },
                { SegmentType.Down5,  down5_table },
                { SegmentType.Down10, down10_table },
            };

        }

        public void PlaceRoadSegments(int start_x, int start_y, int stage_num)
        {
            m_stagemanager.StageSegments.Clear();
            int x = start_x;
            int y = start_y;

            StageSegment previous_segment = FirstStageSegment(start_x, start_y);

            List<SegmentType> stage = m_stages_to_generate[stage_num];
            for (int i = 0; i < stage.Count; i++)
            {
                AddStageSegment(ref previous_segment, stage[i]);
            }

            /*
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Up5);
            AddStageSegment(ref previous_segment, SegmentType.Up10);
            AddStageSegment(ref previous_segment, SegmentType.Up10);
            AddStageSegment(ref previous_segment, SegmentType.Up5);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Up5);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Down5);
            AddStageSegment(ref previous_segment, SegmentType.Down10);
            AddStageSegment(ref previous_segment, SegmentType.Down10);
            AddStageSegment(ref previous_segment, SegmentType.Down5);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Up10);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Down10);
            AddStageSegment(ref previous_segment, SegmentType.Down10);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Flat);
            AddStageSegment(ref previous_segment, SegmentType.Up5);
            AddStageSegment(ref previous_segment, SegmentType.Up10);
            AddStageSegment(ref previous_segment, SegmentType.Up10);
            AddStageSegment(ref previous_segment, SegmentType.Up5);
            AddStageSegment(ref previous_segment, SegmentType.Up5);
            AddStageSegment(ref previous_segment, SegmentType.Up10);
            AddStageSegment(ref previous_segment, SegmentType.Up10);
            AddStageSegment(ref previous_segment, SegmentType.Up5);
            AddStageSegment(ref previous_segment, SegmentType.Flat);*/

            m_stagemanager.StageTotalLength = (int)previous_segment.Position.X + m_stagemanager.RoadWidth;
        }


        public StageSegment FirstStageSegment(int x, int y)
        {
            StageSegment first_stage_segment = CreateStageSegment(x, y, SegmentType.Flat, 0);
            m_stagemanager.StageSegments.Add(first_stage_segment);
            return first_stage_segment;
        }

        public void AddStageSegment(ref StageSegment previous_segment, SegmentType new_segment_type)
        {
            float x = previous_segment.Position.X + m_stagemanager.RoadWidth;
            float y = previous_segment.Position.Y;

            Vector2 slope_increase = (Vector2)((Hashtable)m_slope_height_increase_dict[previous_segment.SegmentType])[new_segment_type];
            y -= slope_increase.X + slope_increase.Y;

            StageSegment new_stage_segment = CreateStageSegment(x, y, new_segment_type, slope_increase.Y);
            m_stagemanager.StageSegments.Add(new_stage_segment);
            previous_segment = new_stage_segment;
        }

        public StageSegment CreateStageSegment(float x, float y, SegmentType segment_type, float corrector)
        {
            Texture2D texture = m_road_textures[(int)segment_type];
            Texture2D scenery_texture = null;
            Texture2D[] scenery_texture_array = m_scenery_textures[(int)segment_type];
            if (scenery_texture_array != null)
            {
                scenery_texture = scenery_texture_array[0];
            }
            StageSegment new_segment = new StageSegment(new Vector2(x, y - texture.Height), texture, scenery_texture, segment_type, corrector);
            return new_segment;
        }
    }


    public class StageSegment : GameObject
    {
        SegmentType m_segment_type;
        Texture2D m_scenery_texture;
        public SegmentType SegmentType { get { return m_segment_type; } }
        public float Corrector { get; set; }
        public float Slope { get; set; }

        private const int m_scenery_height_offset = 413;

        public StageSegment(Vector2 position, Texture2D texture, Texture2D scenery_texture, SegmentType segment_type, float corrector) : base(position, texture, 0)
        {
            m_segment_type = segment_type;
            m_scenery_texture = scenery_texture;
            Corrector = corrector;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (m_scenery_texture != null && !GameManager.DEBUG)
            {
                // Draw scenery
                spritebatch.Draw(m_scenery_texture, new Vector2(TopLeftPosition.X, TopLeftPosition.Y - m_scenery_height_offset), Color.White);
            }
            // Draw road segment
            base.Draw(spritebatch);
        }
    }

    public enum SegmentType
    {
        Flat = 0,
        Up5,
        Down5,
        Up10,
        Down10,
    };

}
