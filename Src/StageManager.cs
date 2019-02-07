using System;
using System.Collections.Generic;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectTitan
{
    public class StageManager
    {
        Camera m_camera;
        KeyboardState m_last_keyboardstate;
        MouseState m_last_mousestate;
        int m_following_rider = 0;
        bool m_follow_mode = true;
        int m_time_multiplier = 1;
        public bool Paused { get { return m_time_multiplier == 0; } }

        StageGenerator m_stage_generator;
        List<StageSegment> m_stage_segments;
        public List<StageSegment> StageSegments { get { return m_stage_segments; } }
        Texture2D m_bike_tex;

        public int RoadWidth { get; set; }
        public int RoadHeight { get; set; }
        public int StageTotalLength { get; set; }

        List<Rider> m_riders;
        public List<Rider> Riders { get { return m_riders; } }

        // Camera
        Vector2 m_camera_initial_position;

        // Debug
        Texture2D m_pinky;

        public StageManager(Texture2D[] road_textures, Texture2D[][] scenery_textures, Texture2D biketex, Camera camera, Texture2D pinky)
        {
            m_stage_segments = new List<StageSegment>();
            m_bike_tex = biketex;
            m_camera = camera;
            m_pinky = pinky;

            m_riders = new List<Rider>();
            m_last_keyboardstate = Keyboard.GetState();
            m_last_mousestate = Mouse.GetState();
            
            m_stage_generator = new StageGenerator(road_textures, scenery_textures, this);
            m_stage_generator.PlaceRoadSegments(0, m_camera.ScreenHeight / 2, 0);
            PlaceRiders();
            InitializeCamera();
        }


        public void Update(GameTime gameTime, MouseState mouse_state)
        {
            for (int i = 0; i < m_riders.Count; i++)
            {
                m_riders[i].Update(gameTime, m_time_multiplier, mouse_state);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < m_stage_segments.Count; i++)
            {
                m_stage_segments[i].Draw(spriteBatch);
            }
            for (int i = 0; i < m_riders.Count; i++)
            {
                m_riders[i].Draw(spriteBatch);
            }
            //spriteBatch.Draw(m_pinky, -m_camera.Center, Color.White);
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
            float new_x = m_camera.Center.X + direction.X;
            m_camera.Center = new Vector2(new_x, InfoAt(new_x).height);

            if (m_camera.Position.X > m_camera_initial_position.X)
            {
                m_camera.Position = new Vector2(m_camera_initial_position.X, m_camera.Position.Y);
            }
            else if (-m_camera.TopRightPosition.X > StageTotalLength)
            {
                m_camera.TopRightPosition = new Vector2(-StageTotalLength, m_camera.TopRightPosition.Y);
            }
            //m_camera.Center = new Vector2(m_camera.Center.X, InfoAt(m_camera.Center.X).height);
        }

        public void CameraFollow(Rider target)
        {
            float new_x = -target.Position.X - target.TextureLength;
            m_camera.Center = new Vector2(new_x, InfoAt(new_x).height);

            if (m_camera.Position.X > m_camera_initial_position.X)
            {
                m_camera.Position = new Vector2(m_camera_initial_position.X, m_camera.Position.Y);
            }
            else if (-m_camera.TopRightPosition.X > StageTotalLength)
            {
                m_camera.TopRightPosition = new Vector2(-StageTotalLength, m_camera.TopRightPosition.Y);
            }
        }

        public void HandleInput(KeyboardState keyboard_state, MouseState mouse_state)
        {
            // Keyboard
            TimeInput(keyboard_state);
            if (keyboard_state.IsKeyUp(Keys.R) && m_last_keyboardstate.IsKeyDown(Keys.R))
            {
                PlaceRiders();
            }
            if (keyboard_state.IsKeyUp(Keys.F) && m_last_keyboardstate.IsKeyDown(Keys.F))
            {
                m_follow_mode = !m_follow_mode;
            }
            if (m_follow_mode)
            {
                FollowModeInput(keyboard_state);
            }
            else
            {
                PanModeInput(keyboard_state);
            }
            m_last_keyboardstate = keyboard_state;

            // Mouse
            if (mouse_state.LeftButton == ButtonState.Pressed && m_last_mousestate.LeftButton != ButtonState.Pressed)
            {
                for (int i = 0; i < m_riders.Count; i++)
                {
                    int closest_rider = -1;
                    if (m_riders[i].InBoundingRect(m_camera.ScreenPointToWorld(mouse_state.Position)))
                    {
                        if (closest_rider == -1 || m_riders[closest_rider].Position.Y > m_riders[i].Position.Y)
                        {
                            closest_rider = i;
                        }
                    }
                    if (closest_rider != -1)
                    {
                        m_following_rider = closest_rider;
                    }
                }
            }
            m_last_mousestate = mouse_state;
        }

        public void TimeInput(KeyboardState keyboard_state)
        {
            if (keyboard_state.IsKeyUp(Keys.Space) && m_last_keyboardstate.IsKeyDown(Keys.Space))
            {
                if (Paused) { m_time_multiplier = 1; }
                else { m_time_multiplier = 0; }
            }

            for (int i = 0; i < 10; i++)
            {
                if (keyboard_state.IsKeyDown(i + Keys.D1)) { m_time_multiplier = i + 1; break; }
            }
        }
        public void FollowModeInput(KeyboardState keyboard_state)
        {
            if (keyboard_state.IsKeyDown(Keys.Left) && !m_last_keyboardstate.IsKeyDown(Keys.Left))
            {
                if (m_following_rider == 0)
                {
                    m_following_rider = m_riders.Count - 1;
                }
                else
                {
                    m_following_rider -= 1;
                }
            }
            else if (keyboard_state.IsKeyDown(Keys.Right) && !m_last_keyboardstate.IsKeyDown(Keys.Right))
            {
                if (m_following_rider == m_riders.Count - 1)
                {
                    m_following_rider = 0;
                }
                else
                {
                    m_following_rider += 1;
                }
            }
            CameraFollow(m_riders[m_following_rider]);
        }

        public void PanModeInput(KeyboardState keyboard_state)
        {
            float speed = 4.0f;
            float speed_multiplier = 1f;
            int direction = 0;
            if (keyboard_state.IsKeyDown(Keys.LeftShift))
            {
                speed_multiplier = 3f;
            }

            if (keyboard_state.IsKeyDown(Keys.A) || keyboard_state.IsKeyDown(Keys.Left))
            {
                direction = 1;
            }
            else if (keyboard_state.IsKeyDown(Keys.D) || keyboard_state.IsKeyDown(Keys.Right))
            {
                direction = -1;
            }
            if (direction != 0)
            {
                MoveCamera(Vector2.UnitX * direction * speed * speed_multiplier);
            }
        }

        public StagePositionInformation InfoAt(float x)
        {
            StagePositionInformation info_object = new StagePositionInformation();
            x = -x;
            StageSegment target_segment = null;
            for (int i=(int)Math.Floor(x/RoadWidth); i < m_stage_segments.Count; i++)
            {
                if (m_stage_segments[i].Position.X <= x && m_stage_segments[i].Position.X + RoadWidth >= x)
                {
                    target_segment = m_stage_segments[i];
                    break;
                }
            }
            if (target_segment == null)
            {
                Console.WriteLine("ERROR: InfoAt(" + x + ") - Could not find target segment.");
                info_object.valid = false;
                return info_object;
            }

            float start_y  = target_segment.TopLeftPosition.Y;
            float target_y = start_y;
            switch (target_segment.SegmentType)
            {
                case SegmentType.Down5:
                case SegmentType.Down10:
                    start_y = target_segment.TopLeftPosition.Y + target_segment.Corrector;
                    target_y = target_segment.BottomRightPosition.Y - RoadHeight;
                    break;
                case SegmentType.Up5:
                case SegmentType.Up10:
                    start_y = target_segment.Position.Y - RoadHeight + target_segment.Corrector;
                    target_y = target_segment.TopRightPosition.Y;
                    break;
            }

            info_object.valid = true;
            info_object.stage_segment = target_segment;
            info_object.height = -MathHelper.Lerp(start_y, target_y, (x - target_segment.TopLeftPosition.X) / RoadWidth);
            return info_object;
        }

        public void PlaceRiders()
        {
            m_riders.Clear();
            AddRider(m_stage_segments[0].Position.X, 5f,  140f);
            AddRider(m_stage_segments[0].Position.X, 30f, 155f);
            AddRider(m_stage_segments[0].Position.X, 65f, 145f);
            AddRider(m_stage_segments[0].Position.X + 50, 6f,  160f);
            AddRider(m_stage_segments[0].Position.X + 50, 29f, 165f);
            AddRider(m_stage_segments[0].Position.X + 50, 64f, 155f);     
        }

        public void AddRider(float x, float road_offset, float speed)
        {
            Rider new_rider = new Rider(new Vector2(x, InfoAt(-x).height), m_bike_tex, -0.25f, speed, road_offset, this);
            m_riders.Add(new_rider);
        }

    }

    public struct StagePositionInformation
    {
        public bool valid;
        public StageSegment stage_segment;
        public float height;
    }

}
