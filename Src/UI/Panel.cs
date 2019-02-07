using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectTitan.UI
{
    public class Panel : UI_Element
    {
        protected Texture2D m_texture;
        public Texture2D Texture { set { m_texture = value; } }

        protected List<UI_Element> m_childlist;

        public Panel(Panel parent, Vector4 relpos, UI_Manager ui_manager) : base(parent, relpos, ui_manager)
        {
            m_childlist = new List<UI_Element>();
        }

        public void AddChild(UI_Element ui_ele) => m_childlist.Add(ui_ele);

        public Panel AddPanel(Vector4 relpos)
        {
            Panel new_panel = new Panel(this, relpos, m_ui_manager);
            new_panel.Init();
            AddChild(new_panel);
            return new_panel;
        }
        public Slider AddSlider(Vector2 relpos, int start_value, bool horizontal)
        {
            Slider new_slider = new Slider(this, relpos, m_ui_manager, horizontal);
            new_slider.InitSlider(start_value);
            AddChild(new_slider);
            return new_slider;
        }
        public Table AddTable(Vector2 relpos, Texture2D table_texture, Texture2D row_texture)
        {
            Table new_table = new Table(this, relpos, table_texture, row_texture, m_ui_manager);
            new_table.InitTable();
            AddChild(new_table);
            return new_table;
        }
        public Button AddButton(Vector4 relrect, ButtonClickFuntion click_delegate)
        {
            Button new_button = new Button(this, relrect, m_ui_manager, click_delegate);
            AddChild(new_button);
            return new_button;
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            if (m_enabled && m_texture != null)
            {
                sprite_batch.Draw(m_texture, m_pos, Color.White);
            }
            // If the ui_element that is being drawn has children - invoke their draw methods
            if (m_childlist.Count > 0)
            {
                for (int i = 0; i < m_childlist.Count; i++)
                {
                    m_childlist[i].Draw(sprite_batch);
                }
            }
        }

        public override void Update(MouseState mouse_state)
        {
            if (m_childlist.Count > 0)
            {
                for (int i = 0; i < m_childlist.Count; i++)
                {
                    m_childlist[i].Update(mouse_state);
                }
            }
            base.Update(mouse_state);
        }
    }
}
