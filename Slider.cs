﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectTitan
{
    public class Slider : UI_Element
    {
        Rectangle m_slider_rect;
        Texture2D m_slider_texture;
        Texture2D m_container_texture;
        bool m_slider_held = false;
        int m_min_x;
        int m_max_x;
        float m_length;

        const int m_jump_length = 10;

        public Slider(Panel parent, Vector4 container_rel_position, Texture2D container_texture, Texture2D slider_texture, int start_value) : base(parent, container_rel_position)
        {
            start_value -= slider_texture.Width / 2;
            if (start_value < 0) { start_value = 0; }
            m_slider_texture = slider_texture;
            m_container_texture = container_texture;

            m_slider_rect = new Rectangle(base.m_pos.X + start_value, base.m_pos.Y, slider_texture.Width, m_pos.Height);
            m_min_x = base.m_pos.Left;
            m_max_x = base.m_pos.Right - m_slider_rect.Width;
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
                else if (m_pos.Contains(mouse_state.Position) && m_last_mousestate.LeftButton != ButtonState.Pressed)
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

        public override void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(m_container_texture, m_pos, Color.White);
            sprite_batch.Draw(m_slider_texture, m_slider_rect, Color.White);
        }
    }
}
