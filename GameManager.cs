using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ProjectTitan
{
    public class GameManager
    {
        private List<GameObject> game_object_list;

        Texture2D texture;
        Texture2D dots_texture;

        public AnimatedGameObject dots;

        public GameManager()
        {
            game_object_list = new List<GameObject>();
        }

        public void Init()
        {
            for (int i = 0; i < 10; i++)
            {
                game_object_list.Add(new GameObject(new Vector2(i * texture.Width, 0), texture));
            }
            dots = new AnimatedGameObject(new Vector2(100, 100), dots_texture, 2, 3); 
        }

        public void LoadResources(Game game)
        {
            texture = game.Content.Load<Texture2D>("textures/keyboard_chords_schism");
            dots_texture = game.Content.Load<Texture2D>("textures/dots_sheet");
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            /*
            for (int i=0; i < game_object_list.Count; i++)
            {
                game_object_list[i].Draw(sprite_batch);
            }
            */
            dots.Draw(sprite_batch);
        }
    }
}
