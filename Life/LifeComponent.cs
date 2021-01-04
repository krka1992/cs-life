using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Life
{
    class LifeComponent:ContainerControl
    {
        //private LifeObject Life;
        public LifeObject Life { get;  }
        private int CellSize = 16;
        private int CellPadding = 2;
        private int BorderWidth = 1;
        private int LifeWidth;
        private int LifeHeight;
        public LifeComponent()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            Life = new LifeObject();
            SetLifeSize();
        }

        private void SetLifeSize()
        {
            if (Life == null)
                return;

            LifeWidth = (Width + BorderWidth) / (CellSize + CellPadding * 2 + BorderWidth);
            LifeHeight = (Height + BorderWidth) / (CellSize + CellPadding * 2 + BorderWidth);

            Life.SetSize(LifeWidth, LifeHeight);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetLifeSize();
        }

        private void RenderGrid(PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(0x00, 0x00, 0x00));
            int x, y;
            int value = (CellSize + CellPadding * 2 + BorderWidth);
            for (int i = 1; i < LifeWidth; i++)
            {
                x = i * value;
                e.Graphics.DrawLine(pen, x, 0, x, Height);
            }
            for (int i = 1; i < LifeHeight; i++)
            {
                y = i * value;
                e.Graphics.DrawLine(pen, 0, y, Width, y);
            }
        }

        private void RenderPoints(PaintEventArgs e)
        {
            using var brush = new SolidBrush(Color.FromArgb(0x00, 0x00, 0x00));
            Rectangle area;
            int x, y;

            for (int i = 0; i < LifeWidth; i++)
            {
                for (int j = 0; j < LifeHeight; j++)
                {
                    if (!Life.GetPointValue(i, j)) continue;

                    x = (CellSize + CellPadding * 2 + BorderWidth) * i + CellPadding + BorderWidth;
                    y = (CellSize + CellPadding * 2 + BorderWidth) * j + CellPadding + BorderWidth;

                    area = new Rectangle(new Point(x, y), new Size(CellSize, CellSize));

                    e.Graphics.FillRectangle(brush, area);
                }
            }
        }

        private void Render(PaintEventArgs e)
        {
            using var brush = new SolidBrush(Color.FromArgb(0xFF, 0xFF, 0xFF));
            var area = new Rectangle(new Point(0, 0), new Size(this.Size.Width - 1, this.Size.Height - 1));

            e.Graphics.FillRectangle(brush, area);

            RenderGrid(e);
            RenderPoints(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Render(e);
        }

        public void UpdateLife()
        {
            if (Life == null)
                return;

            Life.Mutate();
            Invalidate();
        }

    }
}
