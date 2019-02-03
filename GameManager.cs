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
        private List<GUIObject> m_gui_object_list;

        Texture2D pinky; // Pink square for debugging

        int m_screen_width;
        int m_screen_height;

        Camera m_camera;
        KeyboardState m_last_keyboardstate;
        bool m_game_started = false;

        StageManager m_stagemanager;

        public const bool DEBUG = false;


        public GameManager(int screen_width, int screen_height)
        {
            m_screen_width  = screen_width;
            m_screen_height = screen_height;

            m_gui_object_list = new List<GUIObject>();
            m_gui_object_textures = new Hashtable();
            m_road_textures    = new Texture2D[5];
            m_scenery_textures = new Texture2D[5][];
            m_camera = new Camera(m_screen_width, m_screen_height);
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

            // Pink rect in middle (DEBUG)
            pinky = game.Content.Load<Texture2D>("misc/pinky");

        }

        public void Init()
        {
            m_stagemanager = new StageManager(m_road_textures, m_scenery_textures, m_bike_tex, m_camera, pinky);
            m_gui_object_list.Add(new Slider(Vector2.Zero, (Texture2D)m_gui_object_textures["SliderContainer"],(Texture2D)m_gui_object_textures["Slider"], 50));
            m_game_started = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!m_game_started) { return; }
            KeyboardState keyboard_state = Keyboard.GetState();
            MouseState mouse_state = Mouse.GetState();
            m_stagemanager.HandleInput(keyboard_state, mouse_state);
            m_stagemanager.Update(gameTime, mouse_state);

            // GUI
            for (int i = 0; i < m_gui_object_list.Count; i++)
            {
                m_gui_object_list[i].Update(mouse_state);
            }

            m_last_keyboardstate = keyboard_state;
        }

        public void Draw(SpriteBatch sprite_batch)
        {   
            var viewMatrix = m_camera.GetTransform();

            // World
            sprite_batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, viewMatrix);
            m_stagemanager.Draw(sprite_batch);
            sprite_batch.End();

            // GUI
            sprite_batch.Begin();
            for (int i = 0; i < m_gui_object_list.Count; i++)
            {
                m_gui_object_list[i].Draw(sprite_batch);
            }
            sprite_batch.End();
        }
    }
}
