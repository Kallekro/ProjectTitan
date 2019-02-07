﻿using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjectTitan.UI;

// Globals
public delegate int ButtonClickFuntion(params object[] args);


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

        UI_Manager ui_manager;
        Panel panel1;
        Panel panel2;
        Button button1;


        public AnimatedGameObject dots;
        DB_Manager db_manager;

        StageManager m_stagemanager;


        public const bool DEBUG = false;


        public GameManager(int screen_width, int screen_height)
        {
            m_screen_width = screen_width;
            m_screen_height = screen_height;

            m_gui_object_textures = new Hashtable();
            m_road_textures = new Texture2D[5];
            m_scenery_textures = new Texture2D[5][];
            m_camera = new Camera(m_screen_width, m_screen_height);

            // setup UI
            ui_manager = new UI_Manager(m_screen_width, m_screen_height);
            db_manager = new DB_Manager();

            db_manager.DeleteSaveDB("hallelujah");
            db_manager.CreateSaveDB("hallelujah");
            //db_manager.LoadDB("hallelujah");

            db_manager.LoadDataIntoDB();

            m_last_keyboardstate = Keyboard.GetState();
        }

        public int Print(params object[] args)
        {
            Console.WriteLine(args[0]);
            return 0;
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
            m_gui_object_textures["SliderContainerHorizontal"] = game.Content.Load<Texture2D>("gui/slider_container");
            m_gui_object_textures["SliderContainerVertical"] = game.Content.Load<Texture2D>("gui/slider_container_vertical");
            m_gui_object_textures["TableContainer"] = game.Content.Load<Texture2D>("gui/table");
            m_gui_object_textures["TableRow"] = game.Content.Load<Texture2D>("gui/row");
            m_gui_object_textures["ButtonDown"] = game.Content.Load<Texture2D>("gui/button_down");
            m_gui_object_textures["ButtonUp"] = game.Content.Load<Texture2D>("gui/button_up");
            m_gui_object_textures["ButtonHover"] = game.Content.Load<Texture2D>("gui/button_hover");
            ui_manager.TextureDict = m_gui_object_textures;

            // Pink rect in middle
            pinky = game.Content.Load<Texture2D>("misc/pinky");
            dots_texture = game.Content.Load<Texture2D>("textures/dots_sheet");
            UI_overlay = game.Content.Load<Texture2D>("textures/UI_overlay");

        }

        public void Init()
        {
            dots = new AnimatedGameObject(new Vector2(100, 100), dots_texture, 2, 3);

            // Init UI
            panel1 = ui_manager.GetRootPanel.AddPanel(new Vector4(0.0f, 0.00f, 1f, 1f));
            //panel2 = new Panel(ui_manager.GetRootPanel, new Vector4(0.2f, 0.1f, 0.6875f, 0.5083333f));
            //panel2 = new Panel(ui_manager.GetRootPanel, new Vector4(0.40f, 0.00f, 0.6f, 0.6f));
            //panel2.Enable = true;
            button1 = panel1.AddButton(new Vector4(0.0f, 0.0f, 0.2f, 0.2f), Print);

            panel1.Texture = UI_overlay;
            //panel2.Texture = UI_overlay;
            m_stagemanager = new StageManager(m_road_textures, m_scenery_textures, m_bike_tex, m_camera, pinky);
            //panel1.AddSlider(new Vector2(0.1f, 0.1f), 50, false);
            /*
            Table table1 = panel1.AddTable(new Vector2(0.15f, 0.2f), (Texture2D)m_gui_object_textures["TableContainer"], (Texture2D)m_gui_object_textures["TableRow"]);
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            table1.AddRow();
            */           
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
            ui_manager.UpdateUI(mouse_state);
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
            sprite_batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, new RasterizerState { ScissorTestEnable = true }, null, null);
            ui_manager.DrawUI(sprite_batch);
            sprite_batch.End();

        }
    }
}
