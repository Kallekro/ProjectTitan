using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectTitan.UI
{
    public class Button : UI_Element
    {

        private Texture2D m_curr_texture;

        private bool m_pressed = false;
        private bool m_held = false;
        private bool m_hover = false;

        private readonly ButtonClickFuntion m_button_click_function;

        public Button(Panel parent, Vector4 relrect, UI_Manager ui_manager, ButtonClickFuntion del_func) : base(parent, relrect, ui_manager)
        {
            m_button_click_function = del_func;
            m_curr_texture = (Texture2D)m_ui_manager.TextureDict["ButtonUp"];
            base.Init(new Vector2(m_curr_texture.Width, m_curr_texture.Height));
        }

        public override void Update(MouseState mouse_state)
        {
            m_hover = m_pos.Contains(mouse_state.Position);
            m_pressed = (mouse_state.LeftButton == ButtonState.Released);
            m_pressed = mouse_state.LeftButton == ButtonState.Pressed && m_pos.Contains(mouse_state.Position);

            if (m_pressed) { m_curr_texture = (Texture2D)m_ui_manager.TextureDict["ButtonDown"];}
            else if (m_hover) { m_curr_texture = (Texture2D)m_ui_manager.TextureDict["ButtonHover"];}
            else if (!m_pressed) { m_curr_texture = (Texture2D)m_ui_manager.TextureDict["ButtonUp"];}

            if (!m_pressed && (m_pos.Contains(m_last_mousestate.Position) && m_last_mousestate.LeftButton == ButtonState.Pressed))
            {
                m_button_click_function("That's a clicking yall!");
            }
            base.Update(mouse_state);
        }

        public override void Draw(SpriteBatch sprite_batch)
        {

            if (m_enabled)
            {
                sprite_batch.Draw(m_curr_texture, m_pos, Color.White);
            }
        }
        

    }
}
