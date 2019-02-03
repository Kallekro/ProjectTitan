using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectTitan
{
    public class Slider : GUIObject
    {
        Rectangle m_slider_rect;
        Texture2D m_slider_texture;
        bool m_slider_held = false;
        int m_min_x;
        int m_max_x;
        float m_length;

        const int m_jump_length = 10;

        public Slider(Vector2 container_position, Texture2D container_texture, Texture2D slider_texture, int start_value) : base(container_position, container_texture, 0)
        {
            start_value -= slider_texture.Width / 2;
            if (start_value < 0) { start_value = 0; }
            m_slider_texture = slider_texture;
            m_slider_rect = new Rectangle((int)container_position.X + start_value, (int)container_position.Y, slider_texture.Width, slider_texture.Height);
            m_min_x = m_rect.Left;
            m_max_x = m_rect.Right - m_slider_rect.Width;
            m_length = m_max_x - m_min_x;
        }

        public float GetValue()
        {
            return 1 - (m_max_x - m_slider_rect.X) / m_length;
        }

        public override void Update(MouseState mouse_state)
        {
            if (mouse_state.LeftButton == ButtonState.Released)
            {
                m_slider_held = false;
            }
            if (!m_slider_held && mouse_state.LeftButton == ButtonState.Pressed )
            {
                if (m_slider_rect.Contains(mouse_state.Position))
                {
                    m_slider_held = true;
                }
                else if (m_rect.Contains(mouse_state.Position) && m_last_mousestate.LeftButton != ButtonState.Pressed)
                {
                    ContainerClicked(mouse_state.Position);
                }
            }
            if (m_slider_held)
            {
                m_slider_rect.X = mouse_state.Position.X - m_slider_rect.Width / 2;
                TruncToBounds(); 
                Console.WriteLine("Slider value: " + GetValue());
            }
            base.Update(mouse_state);
        }

        public void TruncToBounds()
        {
            if (m_slider_rect.X < m_min_x)
            {
                m_slider_rect.X = m_min_x;
            }
            else if (m_slider_rect.X > m_max_x)
            {
                m_slider_rect.X = m_max_x;
            }
        }

        public void ContainerClicked(Point mousepos)
        {
            if (mousepos.X > m_slider_rect.X)
            {
                m_slider_rect.X += m_jump_length;
            }
            else
            {
                m_slider_rect.X -= m_jump_length;
            }
            TruncToBounds();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            spritebatch.Draw(m_slider_texture, m_slider_rect, Color.White);
        }
    }
}
