using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ProjectTitan
{


    public class GameManager
    {
        private List<GameObject> m_game_object_list;
        Texture2D[] m_road_textures;
        Texture2D[][] m_scenery_textures;

        Texture2D pinky; // Pink square for debugging

        int m_screen_width;
        int m_screen_height;

        Camera m_camera;

        Texture2D texture;
        Texture2D dots_texture;

        public AnimatedGameObject dots;
        Stage test_stage;

        public const bool DEBUG = false;


        public GameManager(int screen_width, int screen_height)
        {
            m_screen_width  = screen_width;
            m_screen_height = screen_height;

            m_game_object_list = new List<GameObject>();
            m_road_textures    = new Texture2D[3];
            m_scenery_textures = new Texture2D[3][];
            m_camera = new Camera(m_screen_width, m_screen_height);

        }

        public void LoadResources(Game game)
        {
            if (DEBUG)
            {
                // Road segments with red boxes around
                m_road_textures[0] = game.Content.Load<Texture2D>("road_textures/flat_roadsection_1_debug");
                m_road_textures[1] = game.Content.Load<Texture2D>("road_textures/sloped_up_roadsection_1_debug");
                m_road_textures[2] = game.Content.Load<Texture2D>("road_textures/sloped_down_roadsection_1_debug");

            }
            else
            {
                // Road segments
                m_road_textures[0] = game.Content.Load<Texture2D>("road_textures/flat_roadsection_1");
                m_road_textures[1] = game.Content.Load<Texture2D>("road_textures/sloped_up_roadsection_1");
                m_road_textures[2] = game.Content.Load<Texture2D>("road_textures/sloped_down_roadsection_1");

                // Flat scenery
                m_scenery_textures[0] = new Texture2D[1] { game.Content.Load<Texture2D>("road_textures/flat_scenery_1") };
                // Sloped up scenery
                m_scenery_textures[1] = new Texture2D[1] { game.Content.Load<Texture2D>("road_textures/sloped_up_scenery_1") };
                // Sloped down scenery
                m_scenery_textures[2] = new Texture2D[1] { game.Content.Load<Texture2D>("road_textures/sloped_down_scenery_1") };
            }
            dots = new AnimatedGameObject(new Vector2(100, 100), dots_texture, 2, 3); 

            // Pink rect in middle
            pinky = game.Content.Load<Texture2D>("road_textures/pinky");

        }

        public void Init()
        {
            texture = game.Content.Load<Texture2D>("textures/keyboard_chords_schism");
            dots_texture = game.Content.Load<Texture2D>("textures/dots_sheet");

            test_stage = new Stage(20, m_road_textures, m_scenery_textures, m_camera, pinky);
            test_stage.PlaceRoadSegments(0, m_screen_height / 2);
            test_stage.InitializeCamera();
        }

        public void Update(GameTime gameTime)
        {
            /*
            for (int i=0; i < game_object_list.Count; i++)
            KeyboardState keyboard_state = Keyboard.GetState();
            Vector2 direction = Vector2.Zero;
            float speed = 4.0f;

            if (keyboard_state.IsKeyDown(Keys.A) || keyboard_state.IsKeyDown(Keys.Left))
            {
                direction.X = 1.0f;
            }
            else if (keyboard_state.IsKeyDown(Keys.D) || keyboard_state.IsKeyDown(Keys.Right))
            {
                direction.X = -1.0f;
            }
            if (keyboard_state.IsKeyDown(Keys.W) || keyboard_state.IsKeyDown(Keys.Up))
            {
                direction.Y = 1.0f;
            }
            else if (keyboard_state.IsKeyDown(Keys.S) || keyboard_state.IsKeyDown(Keys.Down))
            {
                direction.Y = -1.0f;
            }
            if (direction != Vector2.Zero)
            {
                test_stage.MoveCamera(direction * speed);
            }
            */

            dots.Update(gameTime);
        }

        public void Draw(SpriteBatch sprite_batch)
        {   
            var viewMatrix = m_camera.GetTransform();

            sprite_batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, viewMatrix);
            test_stage.Draw(sprite_batch);
            for (int i=0; i < m_game_object_list.Count; i++)
            {
                m_game_object_list[i].Draw(sprite_batch);
            }
            dots.Draw(sprite_batch); 
            sprite_batch.End();

        }
    }
}
