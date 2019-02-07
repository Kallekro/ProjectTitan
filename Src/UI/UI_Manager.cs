using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//  TODO 
//  * Make panels have position rectangle and the relative position
//    and make Panels and other UI elements inherit the properties


namespace ProjectTitan.UI
{
    public class UI_Manager
    {

        private int m_scr_width;
        private int m_scr_height;

        private Panel m_root_panel;
        public Panel GetRootPanel { get { return m_root_panel; } }

        public Hashtable TextureDict { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProjectTitan.UI"/> class.
        /// </summary>
        /// <param name="scr_width">Screen width.</param>
        /// <param name="scr_height">Screen height.</param>
        public UI_Manager(int scr_width, int scr_height)
        {
            m_scr_width = scr_width;
            m_scr_height = scr_height;

            m_root_panel = new Panel(null, new Vector4(0, 0, scr_width, scr_height), this);
            m_root_panel.Init();
        }

        public void DrawUI(SpriteBatch sprite_batch)
        {
            m_root_panel.Draw(sprite_batch);
        }

        public void UpdateUI(MouseState mousestate)
        {
            m_root_panel.Update(mousestate);
        }


        /// 
    }

    public class UI_Element
    {
        protected Panel m_parent;
        protected Vector4 m_relrect;
        protected Rectangle m_pos;

        protected bool m_enabled;
        public bool Enable { get { return m_enabled;} set { m_enabled = value;} }
        public Rectangle GetPosRect { get { return m_pos; } }

        protected MouseState m_last_mousestate;

        protected RasterizerState m_non_scissor_rast_state;
        protected RasterizerState m_scissor_rast_state;


        protected UI_Manager m_ui_manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProjectTitan.UI_Element"/> class.
        /// </summary>
        /// <param name="parent">Panel Parent of the UI Element.</param>
        /// <param name="relpos">Relative position to the parent.</param>
        /// <param name="texture">Texture.</param>
        public UI_Element (Panel parent, Vector4 relrect, UI_Manager ui_manager)
        {
            m_ui_manager = ui_manager;
            m_parent = parent;
            m_relrect = relrect;
        }

        public void Init(Vector2 size)
        {
            m_relrect = new Vector4(m_relrect.X, m_relrect.Y, size.X / m_parent.GetPosRect.Width, size.Y / m_parent.GetPosRect.Height);
            Init();
        }

        public void Init()
        {
            m_enabled = true;
            m_last_mousestate = new MouseState();

            if (m_parent != null)
            {
                m_pos = RelativeToRealPosition(m_parent.GetPosRect, m_relrect);
                m_parent.AddChild(this);
            }
            else
            { // If root panel, the relative position is not relative
                m_pos = new Rectangle((int)m_relrect.X, (int)m_relrect.Y, (int)m_relrect.Z, (int)m_relrect.W);
            }

            m_non_scissor_rast_state = new RasterizerState { ScissorTestEnable = false };
            m_scissor_rast_state = new RasterizerState { ScissorTestEnable = true };
        }


        virtual public void Draw(SpriteBatch sprite_batch)
        {
            Console.WriteLine("DRAW BASE FOR UI_ELEMENT METHOD SHOULD NOT BE CALLED");
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
}
