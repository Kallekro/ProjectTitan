using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//  TODO 
//  * Make panels have position rectangle and the relative position
//    and make Panels and other UI elements inherit the properties


namespace ProjectTitan
{

    public class UI
    {
        private int m_scr_width;
        private int m_scr_height;

        private Panel m_root_panel;
        public Panel GetRootPanel { get { return m_root_panel; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProjectTitan.UI"/> class.
        /// </summary>
        /// <param name="scr_width">Screen width.</param>
        /// <param name="scr_height">Screen height.</param>
        public UI(int scr_width, int scr_height)
        {
            m_scr_width = scr_width;
            m_scr_height = scr_height;

            m_root_panel = new Panel(null, new Vector4(0, 0, scr_width, scr_height));
        }

        public void DrawUI(SpriteBatch sprite_batch)
        {
            m_root_panel.Draw(sprite_batch);
        }

        public void UpdateUI(MouseState mousestate)
        {
            m_root_panel.Update(mousestate);
        }
    }

    public class UI_Element
    {
        protected Panel m_parent;
        protected Vector4 m_relpos;
        protected Rectangle m_pos;

        protected bool m_enabled;
        public bool Enable { get { return m_enabled;} set { m_enabled = value;} }
        public Rectangle GetPosRect { get { return m_pos; } }

        protected MouseState m_last_mousestate;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProjectTitan.UI_Element"/> class.
        /// </summary>
        /// <param name="parent">Panel Parent of the UI Element.</param>
        /// <param name="relpos">Relative position to the parent.</param>
        /// <param name="texture">Texture.</param>
        public UI_Element (Panel parent, Vector4 relpos) {
            m_parent = parent;
            m_relpos = relpos;
            m_enabled = true;
            m_last_mousestate = new MouseState();

            if (m_parent != null) {
                m_pos = RelativeToRealPosition(m_parent.GetPosRect, m_relpos);
                m_parent.AddChild(this);
            }
            else { // If root panel, the relative position is not relative
                m_pos = new Rectangle((int)relpos.X, (int)relpos.Y, (int)relpos.Z, (int)relpos.W);
            }
        }

        virtual public void Draw(SpriteBatch sprite_batch)
        {
           
        }

        virtual public void Update(MouseState mouse_state)
        {
            m_last_mousestate = mouse_state;
        }

        // helper methods
        protected Rectangle RelativeToRealPosition(Rectangle parent_rect, Vector4 rel_rect)
        {
            int x = (int)(parent_rect.X + parent_rect.Width * rel_rect.X);
            int y = (int)(parent_rect.Y + parent_rect.Height * rel_rect.Y);
            int width = (int)(parent_rect.Width * rel_rect.Z);
            int height = (int)(parent_rect.Height * rel_rect.W);

            return new Rectangle(x, y, width, height);
        }

    }

    public class Panel : UI_Element
    {
        protected Texture2D m_texture;
        public Texture2D Texture { set { m_texture = value; } }

        protected List<UI_Element> m_childlist;

        public void AddChild(UI_Element ui_ele) => m_childlist.Add(ui_ele);
        public void AddSlider(Vector4 relpos, Texture2D container_texture, Texture2D slider_texture, int start_value)
        {
            AddChild(new Slider(this, relpos, container_texture, slider_texture, start_value));
        }

        public Panel(Panel parent, Vector4 relpos) : base(parent, relpos)
        {
            m_childlist = new List<UI_Element>();
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

    public class Button : UI_Element
    {
        public Button(Panel parent, Vector4 relpos) : base(parent, relpos)
        {

        }
    }
}
