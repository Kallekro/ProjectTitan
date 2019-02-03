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
        Texture2D[] m_road_textures;
        Texture2D[][] m_scenery_textures;
        Texture2D m_bike_tex;
        Hashtable m_gui_object_textures;

        Texture2D pinky; // Pink square for debugging

        int m_screen_width;
        int m_screen_height;

        Camera m_camera;
        KeyboardState m_last_keyboardstate;
        bool m_game_started = false;

        Texture2D dots_texture;
        Texture2D UI_overlay;

        UI ui;
        Panel panel1;
        Panel panel2;
        public AnimatedGameObject dots;
        DB_Manager db_manager;

        StageManager m_stagemanager;

        public const bool DEBUG = false;


        public GameManager(int screen_width, int screen_height)
        {
            m_screen_width  = screen_width;
            m_screen_height = screen_height;

            m_gui_object_textures = new Hashtable();
            m_road_textures    = new Texture2D[5];
            m_scenery_textures = new Texture2D[5][];
            m_camera = new Camera(m_screen_width, m_screen_height);

            // setup UI
            ui = new UI(m_screen_width, m_screen_height);
            db_manager = new DB_Manager();

            //db_manager.CreateSaveDB("hallelujah");
            //db_manager.DeleteSaveDB("hallelujah");
            //db_manager.LoadDB("hallelujah");

            //db_manager.LoadDataIntoDB();

            m_last_keyboardstate = Keyboard.GetState();
        }

        public void LoadResources(Game game)
        {
#pragma warning disable CS0162 // Unreachable code detected
            if (DEBUG)
            {
                // Road segments with red boxes around
                m_road_textures[0] = game.Content.Load<Texture2D>("road_textures/flat_roadsection_1_debug");
                m_road_textures[1] = game.Content.Load<Texture2D>("road_textures/up_5degree_roadsection_debug");
                m_road_textures[2] = game.Content.Load<Texture2D>("road_textures/down_5degree_roadsection_debug");
                m_road_textures[3] = game.Content.Load<Texture2D>("road_textures/up_10degree_roadsection_debug");
                m_road_textures[4] = game.Content.Load<Texture2D>("road_textures/down_10degree_roadsection_debug");

            }
#pragma warning restore CS0162 // Unreachable code detected
            else
            {
                // Road segments
                m_road_textures[0] = game.Content.Load<Texture2D>("road_textures/flat_roadsection_1");
                m_road_textures[1] = game.Content.Load<Texture2D>("road_textures/up_5degree_roadsection");
                m_road_textures[2] = game.Content.Load<Texture2D>("road_textures/down_5degree_roadsection");
                m_road_textures[3] = game.Content.Load<Texture2D>("road_textures/up_10degree_roadsection");
                m_road_textures[4] = game.Content.Load<Texture2D>("road_textures/down_10degree_roadsection");

                // Scenery
                m_scenery_textures[0] = new Texture2D[1] { game.Content.Load<Texture2D>("road_textures/scenery/flat_scenery_1") };
                m_scenery_textures[1] = new Texture2D[1] { game.Content.Load<Texture2D>("road_textures/scenery/up_5degree_scenery_1") };
                m_scenery_textures[2] = new Texture2D[1] { game.Content.Load<Texture2D>("road_textures/scenery/down_5degree_scenery_1") };
                m_scenery_textures[3] = new Texture2D[1] { game.Content.Load<Texture2D>("road_textures/scenery/up_10degree_scenery_1") };
                m_scenery_textures[4] = new Texture2D[1] { game.Content.Load<Texture2D>("road_textures/scenery/down_10degree_scenery_1") };
            }
            m_bike_tex = game.Content.Load<Texture2D>("misc/bike");

            // GUI
            m_gui_object_textures["Slider"] = game.Content.Load<Texture2D>("gui/slider");
            m_gui_object_textures["SliderContainer"] = game.Content.Load<Texture2D>("gui/slider_container");


            // Pink rect in middle
            pinky = game.Content.Load<Texture2D>("misc/pinky");
            dots_texture = game.Content.Load<Texture2D>("textures/dots_sheet");
            UI_overlay = game.Content.Load<Texture2D>("textures/UI_overlay");

        }

        public void Init()
        {
            dots = new AnimatedGameObject(new Vector2(100, 100), dots_texture, 2, 3);

            // Init UI
            panel1 = new Panel(ui.GetRootPanel, new Vector4(0.10f, 0.10f, 0.5f, 0.5f));
            panel2 = new Panel(panel1, new Vector4(0.10f, 0.10f, 0.5f, 0.5f));
            panel2.Enable = true;

            panel1.Texture = UI_overlay;
            panel2.Texture = UI_overlay;
            m_stagemanager = new StageManager(m_road_textures, m_scenery_textures, m_bike_tex, m_camera, pinky);
            panel2.AddSlider(new Vector4(0.1f, 0.1f, 0.6f, 0.2f), (Texture2D)m_gui_object_textures["SliderContainer"], (Texture2D)m_gui_object_textures["Slider"], 50);
            m_game_started = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!m_game_started) { return; }
            KeyboardState keyboard_state = Keyboard.GetState();
            MouseState mouse_state = Mouse.GetState();
            m_stagemanager.HandleInput(keyboard_state, mouse_state);
            m_stagemanager.Update(gameTime, mouse_state);

            dots.Update(gameTime);
            ui.UpdateUI(mouse_state);
            m_last_keyboardstate = keyboard_state;
        }

        public void Draw(SpriteBatch sprite_batch)
        {   
            var viewMatrix = m_camera.GetTransform();

            // World
            sprite_batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, viewMatrix);
            m_stagemanager.Draw(sprite_batch);
            dots.Draw(sprite_batch);
            sprite_batch.End();

            // draw UI
            sprite_batch.Begin();
            ui.DrawUI(sprite_batch);
            sprite_batch.End();

        }
    }
}
