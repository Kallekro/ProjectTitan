using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectTitan.UI
{
    public class Slider : UI_Element
    {
        Rectangle m_slider_rect;
        Texture2D m_slider_texture; 
        Texture2D m_container_texture;
        public Texture2D ContainerTexture { get { return m_container_texture; } }
        bool m_horizontal;
        bool m_slider_held = false;
        bool m_value_changed = false;
        public bool ValueChanged { get { bool tmp_value_changed = m_value_changed; m_value_changed = false ; return tmp_value_changed; }  }
        int m_min_val;
        int m_max_val;
        float m_total_length;

        public int SliderPosition 
        { 
            get { return m_horizontal ? m_slider_rect.X : m_slider_rect.Y; }
            set
            {
                if (m_horizontal) { m_slider_rect.X = value; }
                else { m_slider_rect.Y = value; }
            }
        }

        const int m_jump_length = 10;

        public Slider(Panel parent, Vector2 container_rel_position, UI_Manager ui_manager, bool horizontal = true) 
            : base(parent, new Vector4(container_rel_position.X, container_rel_position.Y, 0, 0), ui_manager)
        {
            m_horizontal = horizontal;
            m_slider_texture = (Texture2D)m_ui_manager.TextureDict["Slider"];
            if (m_horizontal)
            {
                m_container_texture = (Texture2D)m_ui_manager.TextureDict["SliderContainerHorizontal"];
            }
            else
            {
                m_container_texture = (Texture2D)m_ui_manager.TextureDict["SliderContainerVertical"];
            }
        }

        public void InitSlider(int start_value=0)
        {
            base.Init(new Vector2(m_container_texture.Width, m_container_texture.Height));
            int x = m_pos.X;
            int y = m_pos.Y;
            if (m_horizontal)
            {
                start_value -= m_slider_texture.Width / 2;
                if (start_value < 0) { start_value = 0; }
                x += start_value;
            }
            else
            {
                start_value -= m_slider_texture.Height / 2;
                if (start_value < 0) { start_value = 0; }
                y += start_value;
            }
            m_slider_rect = new Rectangle(x, y, m_slider_texture.Width, m_slider_texture.Height);
            if (m_horizontal)
            {
                m_min_val = base.m_pos.Left;
                m_max_val = base.m_pos.Right - m_slider_rect.Width;
            }
            else
            {
                m_min_val = base.m_pos.Top;
                m_max_val = base.m_pos.Bottom - m_slider_rect.Height;
            }
            m_total_length = m_max_val - m_min_val;
        }

        public float GetValue()
        {
            return 1 - (m_max_val - SliderPosition) / m_total_length;
        }

        public void SetValue(float val)
        {
            if (m_horizontal)
            {
                SliderPosition = m_pos.X + (int)Math.Round(m_total_length * val);
            }
            else
            {
                SliderPosition = m_pos.Y + (int)Math.Round(m_total_length * val);
            }
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
                    ContainerClickedOutsideSlider(mouse_state.Position);
                }
            }
            if (m_slider_held)
            {
                SliderDragged(mouse_state.Position);
            }
            base.Update(mouse_state);
        }

        public void SliderDragged(Point mousepos)
        {
            if (m_horizontal)
            {
                SliderPosition = mousepos.X - m_slider_rect.Width / 2;
            }
            else
            {
                SliderPosition = mousepos.Y - m_slider_rect.Height / 2;
            }
            TruncToBounds();
            m_value_changed = true;
        }

        public void ContainerClickedOutsideSlider(Point mousepos)
        {
            float mouse_relaxis;
            if (m_horizontal) { mouse_relaxis = mousepos.X; }
            else { mouse_relaxis = mousepos.Y; }    
            if (mouse_relaxis > SliderPosition)
            {
                SliderPosition += m_jump_length;
            }
            else
            {
                SliderPosition -= m_jump_length;
            }
            TruncToBounds();
            m_value_changed = true;
        }

        public void TruncToBounds()
        {
            if (SliderPosition < m_min_val)
            {
                SliderPosition = m_min_val;
            }
            else if (SliderPosition > m_max_val)
            {
                SliderPosition = m_max_val;
            }
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(m_container_texture, m_pos, Color.White);
            sprite_batch.Draw(m_slider_texture, m_slider_rect, Color.White);
        }
    }
}
