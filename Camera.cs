using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectTitan
{
    public class Camera
    {
        int m_screen_width;
        int m_screen_height;

        public Camera(int screen_width, int screen_height)
        {
            Zoom = 1;
            Position = Vector2.Zero;
            Rotation = 0;
            Origin = Vector2.Zero;
            Position = Vector2.Zero;

            m_screen_width = screen_width;
            m_screen_height = screen_height;
        }

        public float Zoom { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Center 
        { 
            get { return new Vector2(Position.X - m_screen_width/2, Position.Y - m_screen_height/2); } 
            set { Position = new Vector2(value.X + m_screen_width/2, value.Y + m_screen_height/2); }
        }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }

        public void Move(Vector2 direction)
        {
            Position += direction;
        }

        public void SetHorizontal(float x)
        {
            Position = new Vector2(x, Position.Y);
        }

        public void SetVertical(float y)
        {
            Position = new Vector2(Position.X, y);
        }

        public Matrix GetTransform()
        {
            var translationMatrix = Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0));
            var rotationMatrix = Matrix.CreateRotationZ(Rotation);
            var scaleMatrix = Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
            var originMatrix = Matrix.CreateTranslation(new Vector3(Origin.X, Origin.Y, 0));

            return translationMatrix * rotationMatrix * scaleMatrix * originMatrix;
        }
    }
}
