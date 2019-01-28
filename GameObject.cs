﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectTitan
{
    public class GameObject
    {
        private Vector2 m_position;
        public Vector2 Position
        {
            get { return m_position;  }
            set { m_position = value; }
        }

        private Texture2D m_texture;

        public GameObject(Vector2 position, Texture2D texture)
        {
            m_position = position;
            m_texture = texture;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(m_texture, m_position, Color.White);
        }
    }
}
