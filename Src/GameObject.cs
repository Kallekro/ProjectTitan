using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectTitan
{
    public class GameObject
    {
        protected Texture2D m_texture;
        protected Vector2 m_position;
        protected float m_rotation;
        protected Rectangle m_bounding_rect;
        protected MouseState m_last_mousestate;

        public GameObject(Vector2 position, Texture2D texture, float rotation)
        {
            m_position = position;
            m_texture = texture;
            m_rotation = rotation;
            m_bounding_rect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            m_last_mousestate = new MouseState();
        }

        public float Rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; }
        }
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

        public Vector2 Center
        {
            get { return new Vector2(m_position.X + m_texture.Width / 2, m_position.Y + m_texture.Height / 2); }
            set { m_position = new Vector2(value.X - m_texture.Width / 2, value.Y - m_texture.Height / 2); }
        }

        public int TextureLength { get { return m_texture.Width; } }
        public int TextureHeight { get { return m_texture.Height; } }

        public bool InBoundingRect(Point point)
        {
            return m_bounding_rect.Contains(point);
        }

        virtual public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(m_texture, m_position, null, Color.White, m_rotation, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        }

        virtual public void Update()
        {
            m_bounding_rect.X = (int)m_position.X;
            m_bounding_rect.Y = (int)m_position.Y;
        }
    }
}
