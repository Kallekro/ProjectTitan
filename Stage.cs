using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ProjectTitan
{
    public class Stage
    {
        StageSegment[] m_stage_segments;

        public Stage(int n_segments)
        {
            m_stage_segments = new StageSegment[n_segments];
        }
    }

    public class StageSegment : GameObject
    {
        static public int Length { get { return 10; } }

        public StageSegment(Vector2 position, Texture2D texture) : base(position, texture)
        {

        }
    }

}
