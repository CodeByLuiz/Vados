using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vados
{
    internal class RoundedButton : Button
    {
        static float BorderRadius = 0;

        public RoundedButton() {
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Desenhar fundo arredondado
            Brush brush = new SolidBrush(Color.Red);
            GraphicsPath roundedRectPath = Global.RoundedRectangle(Bounds, BorderRadius);
            e.Graphics.FillPath(brush, roundedRectPath);

            ////Desenhar texto
            //TextRenderer.DrawText(
            //    e.Graphics,
            //    this.Text,
            //    this.Font,
            //    this.ClientRectangle,
            //    this.ForeColor,
            //    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            //);
        }
    }
}
