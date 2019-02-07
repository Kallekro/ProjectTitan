using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectTitan
{
    public class Rider : GameObject
    {
        float m_speed;
        float m_road_offset;
        StageManager m_current_stage;

        public Rider(Vector2 position, Texture2D texture, float rotation, float speed, float road_offset, StageManager stage) : base(position, texture, rotation)
        {
            m_speed = speed;
            m_current_stage = stage;
            m_road_offset = road_offset;
        }

        public void Update(GameTime gameTime, int time_multiplier, MouseState mousestate)
        {
            float delta_time = gameTime.ElapsedGameTime.Milliseconds / 1000f;
            float new_x = Center.X + m_speed * delta_time * time_multiplier;
            StagePositionInformation info_obj = m_current_stage.InfoAt(-new_x);
            if (info_obj.valid)
            {
                Center = new Vector2(new_x, -info_obj.height + m_road_offset + m_texture.Height / 2);
                Rotation = GetRotation(); 
            }
            base.Update();
        }

        public float GetRotation()
        {
            StagePositionInformation info_left = m_current_stage.InfoAt(-TopLeftPosition.X);
            StagePositionInformation info_right = m_current_stage.InfoAt(-TopRightPosition.X);
            if (!info_left.valid || !info_right.valid) { return 0f; }
            Vector2 diff = new Vector2(TopRightPosition.X, info_right.height) - new Vector2(TopLeftPosition.X, info_left.height);
            return (float)Math.Atan2(-diff.Y, diff.X);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
        }
    }
}
