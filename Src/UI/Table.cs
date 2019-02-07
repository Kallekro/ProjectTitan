using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectTitan.UI
{
    public class Table : UI_Element
    {
        Texture2D m_table_texture;
        Texture2D m_row_texture;
        List<TableRow> m_table_rows;
        float m_row_offset = 0;
        int m_max_row_offset = 0;
        float m_scroll_speed = 0.6f;
        private const int MAX_TOTAL_ROWS = 9;
        Slider m_slider;

        public float RowOffset { get { return m_row_offset; } }
        public float MaxRowOffset { get { return m_max_row_offset; } }

        public Table(Panel parent, Vector2 relpos, Texture2D table_texture, Texture2D row_texture, UI_Manager ui_manager) 
            : base(parent, new Vector4(relpos.X, relpos.Y, 0, 0), ui_manager)
        {
            m_table_texture = table_texture;
            m_row_texture = row_texture;
            m_table_rows = new List<TableRow>();
        }

        public void InitTable()
        {
            base.Init(new Vector2(m_table_texture.Width, m_table_texture.Height));
            m_slider = m_parent.AddSlider(new Vector2(m_relrect.X + m_relrect.Z, m_relrect.Y), 0, false);
        }

        public void AddRow()
        {
            float new_y = m_pos.Y;
            if (m_table_rows.Count > 0)
            {
                new_y = m_table_rows[m_table_rows.Count - 1].Position.Y + m_row_texture.Height - 2;
            }
            m_table_rows.Add(new TableRow(new Vector2(m_pos.X, new_y), m_row_texture, this));
            m_max_row_offset = (m_table_rows.Count - MAX_TOTAL_ROWS - 1) * m_row_texture.Height + 8;
        }

        public void Scroll(int direction)
        {
            m_row_offset += direction * m_scroll_speed / m_table_rows.Count;
            TruncRowOffset();
            m_slider.SetValue(m_row_offset);
        }

        public void TruncRowOffset()
        {
            if (m_row_offset < 0) { m_row_offset = 0; }
            else if (m_row_offset > 1) { m_row_offset = 1; }
        }

        public override void Update(MouseState mouse_state)
        {
            if (m_slider.ValueChanged)
            {
                m_row_offset = m_slider.GetValue();
                TruncRowOffset();
            }
            int scrollwheel_delta = m_last_mousestate.ScrollWheelValue - mouse_state.ScrollWheelValue;
            if (scrollwheel_delta != 0)
            {
                Scroll(Math.Sign(scrollwheel_delta));
            }
            base.Update(mouse_state);
        }

        public override void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(m_table_texture, m_pos, Color.White);
            Rectangle cur_scissor_rect = sprite_batch.GraphicsDevice.ScissorRectangle;
            sprite_batch.GraphicsDevice.ScissorRectangle = m_pos;
            for (int i=0; i < m_table_rows.Count; i++)
            {
                m_table_rows[i].Draw(sprite_batch);
            }
            sprite_batch.GraphicsDevice.ScissorRectangle = cur_scissor_rect;
        }
    }

    public class TableRow
    {
        Vector2 m_position;
        Texture2D m_texture;
        Table m_table;

        public Vector2 Position
        {
            get { return m_position; }
        }

        public TableRow(Vector2 position, Texture2D texture, Table table)
        {
            m_position = position;
            m_texture = texture;
            m_table = table;
        }

        public void Draw(SpriteBatch sprite_batch)
        {
            sprite_batch.Draw(m_texture, m_position - Vector2.UnitY * m_table.RowOffset * m_table.MaxRowOffset, Color.White);
        }
    }
}
