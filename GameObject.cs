using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectTitan
{
    public class GameObject
    {
        protected Texture2D m_texture;
        protected Vector2 m_position;
        public Vector2 Position
        {
            get { return new Vector2(m_position.X, m_position.Y + m_texture.Height); }
            set { m_position = value; }
        }

        public Vector2 TopLeftPosition
        {
            get { return m_position; }
        }

        public Vector2 TopRightPosition
        {
            get { return new Vector2(m_position.X + m_texture.Width, m_position.Y); }
        }

        public Vector2 BottomRightPosition
        {
            get { return new Vector2(m_position.X + m_texture.Width, m_position.Y + m_texture.Height); }
        }

        public int TextureLength { get { return m_texture.Width; } }
        public int TextureHeight { get { return m_texture.Height; } }

        public GameObject(Vector2 position, Texture2D texture)
        {
            m_position = position;
            m_texture = texture;
        }

        virtual public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(m_texture, m_position, Color.White);
        }
    }
}
