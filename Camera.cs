using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectTitan
{
    public class Camera
    {
        public Camera(int screen_width, int screen_height)
        {
            Zoom = 1;
            Position = Vector2.Zero;
            Rotation = 0;
            Origin = Vector2.Zero;
            Position = Vector2.Zero;

            ScreenWidth = screen_width;
            ScreenHeight = screen_height;
        }

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        public float Zoom { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Center 
        { 
            get { return new Vector2(Position.X - ScreenWidth/2, Position.Y - ScreenHeight/2); } 
            set { Position = new Vector2(value.X + ScreenWidth/2, value.Y + ScreenHeight/2); }
        }
        public Vector2 TopRightPosition 
        { 
            get { return new Vector2(Position.X - ScreenWidth, Position.Y); }
            set { Position = new Vector2(value.X + ScreenWidth, value.Y); }
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

        public Point ScreenPointToWorld(Point screen_point) 
        {
            return screen_point - new Point((int)Position.X, (int)Position.Y); 
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
