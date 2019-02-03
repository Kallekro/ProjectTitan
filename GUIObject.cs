using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectTitan
{
    public class GUIObject
    {
        protected Rectangle m_rect;
        protected Texture2D m_texture;
        protected MouseState m_last_mousestate;
        float m_rotation;

        public GUIObject(Vector2 position, Texture2D texture, float rotation)
        {
            m_rect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            m_texture = texture;
            m_rotation = rotation;
            m_last_mousestate = new MouseState();
        }

        virtual public bool InBoundingBox(Point point)
        {
            return m_rect.Contains(point);
        }

        virtual public void Update(MouseState mouse_state)
        {
            m_last_mousestate = mouse_state;
        }


        virtual public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(m_texture, new Vector2(m_rect.X, m_rect.Y), null, Color.White, m_rotation, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        }
    }
}
