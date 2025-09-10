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
        public float BorderRadius = 8;
        public Color BehindColor;
        public float HoverLightenFactor = 0.6f;
        public float PressDarkenFactor = -0.35f;
        bool isHovered = false;
        bool isPressed = false;

        public RoundedButton() {
            this.SetStyle(ControlStyles.UserPaint |
                      ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.OptimizedDoubleBuffer, true);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
        }


        #region EVENTOS DO MOUSE

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            //Clarear ao passar o mouse
            isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            //Voltar pra cor padrão ao tirar o mouse
            isHovered = false;
            isPressed = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            //Escurecer ao clicar
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; //Permitir bordas suaves
            e.Graphics.Clear(Parent?.BackColor ?? BehindColor);


            //Cor do fundo
            Color backColor = BackColor;
            if (isPressed) backColor = Global.ChangeColorBrightness(backColor, PressDarkenFactor);    //Escurecer
            if (isHovered) backColor = Global.ChangeColorBrightness(backColor, HoverLightenFactor);   //Clarear


            //Desenhar borda arredondada
            Brush brush = new SolidBrush(FlatAppearance.BorderColor);
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            GraphicsPath roundedRectPath = Global.RoundedRectangle(rect, BorderRadius);
            e.Graphics.FillPath(brush, roundedRectPath);


            //Desenhar botão em si (área menor para aparecer a borda)
            int borderSize = FlatAppearance.BorderSize;
            int areaHeight = Height - 2 * borderSize;
            int areaWidth = Width - 2 * borderSize;
            float areaBorderRadius = (BorderRadius / Height) * areaHeight;
            rect = new Rectangle(borderSize, borderSize, areaWidth - 1, areaHeight - 1);
            brush = new SolidBrush(backColor);
            roundedRectPath = Global.RoundedRectangle(rect, areaBorderRadius);
            e.Graphics.FillPath(brush, roundedRectPath);



            //Desenhar texto
            TextRenderer.DrawText(
                e.Graphics,
                this.Text,
                this.Font,
                this.ClientRectangle,
                this.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );
        }
    }
}
