using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace ProjectTitan
{
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch; 

        GameManager gameManager;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            int screen_width = graphics.GraphicsDevice.Viewport.Bounds.Width;
            int screen_height = graphics.GraphicsDevice.Viewport.Bounds.Height;
            gameManager = new GameManager(screen_width, screen_height);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameManager.LoadResources(this);
            gameManager.Init();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                gameManager.dots.SetCurrentAnimation(0);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.B))
            {
                gameManager.dots.SetCurrentAnimation(1);
            }

            gameManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSeaGreen);

            // TODO: Add your drawing code here
            gameManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}