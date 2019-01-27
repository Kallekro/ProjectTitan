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
        }

        public void LoadResources(Game game)
        {
            texture = game.Content.Load<Texture2D>("snakebit_head_up");
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            for (int i=0; i < game_object_list.Count; i++)
            {
                game_object_list[i].Draw(sprite_batch);
            }
        }
    }
}
