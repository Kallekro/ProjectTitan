using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectTitan
{
    public class AnimatedGameObject : GameObject
    {
        private int m_num_rows;
        private int m_num_columns;

        private int m_current_frame;
        private int m_total_frames;

        // active animation
        protected int m_current_animation;
        protected int m_current_animation_len;

        // timer 
        float m_interval;
        float m_timer;


        public AnimatedGameObject(Vector2 position, Texture2D texture, int rows, int columns) : base(position, texture)
        {
            // size of spritesheet
            m_num_rows = rows;
            m_num_columns = columns;

            m_current_frame = 0;

            // animation control
            m_current_animation = 0;
            m_current_animation_len = columns;
            m_timer = 0.0f;
            m_interval = 500.0f;
        }

        public void SetCurrentAnimation(int anim_idx)
        {
            m_current_animation = anim_idx;
            m_current_frame = m_current_animation * m_num_columns;
        }


        public void Update(GameTime gameTime)
        { 
            m_timer += gameTime.ElapsedGameTime.Milliseconds;

            if (m_timer > m_interval)
            {
                m_current_frame++;
                if (m_current_frame == (m_current_animation*m_num_columns)+ m_num_columns)
                {
                    m_current_frame = m_current_animation * m_num_columns;
                }
                m_timer = 0.0f;
            }
        }

        override public void Draw(SpriteBatch spritebatch)
        {
            int frame_width = m_texture.Width / m_num_columns;
            int frame_height = m_texture.Height / m_num_rows;

            int row_i = (int)(m_current_frame / m_num_columns);
            int col_i = m_current_frame % m_num_columns;

            Rectangle sourceRectangle = new Rectangle(frame_width * col_i, frame_height * row_i, frame_width, frame_height);
            Rectangle destinationRectangle = new Rectangle((int)m_position.X, (int)m_position.Y, frame_width, frame_height);

            spritebatch.Draw(m_texture, destinationRectangle, sourceRectangle, Color.White);
        }


    }
}
