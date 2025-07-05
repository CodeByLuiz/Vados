using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vados
{
    public partial class UserControlHome : UserControl
    {
        public event EventHandler<LoadPageEventArgs> loadPage;

        System.Windows.Forms.Timer timer;

        //Variáveis do botão do microfone
        float circleSizeDefault = 266;
        float circleSize = 266;
        float circleSizeTarget = 266;
        float circleX = 0;
        float circleY = 0;
        bool circleHovering = false;
        bool lastCircleHovering = false;


        public UserControlHome()
        {
            InitializeComponent();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 16; // ~60 FPS
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Ir para página de configurações
            loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlSettings));
        }

        private void txtComando_Click(object sender, EventArgs e)
        {
            //Apagar texto temporário
            if (txtComando.ForeColor.Equals(Colors.blueTernary))
            {
                txtComando.Text = "";
                txtComando.ForeColor = Color.Black;
            }
        }

        private void txtComando_LostFocus(object sender, EventArgs e)
        {
            //Retornar texto temporário
            if (txtComando.Text == "")
            {
                txtComando.Text = "Escreva um comando...";
                txtComando.ForeColor = Color.FromArgb(88, 99, 152);
                txtComando.ForeColor = Colors.blueTernary;
            }
        }

        private void pnlBottom_Paint(object sender, PaintEventArgs e)
        {
            int middleX = this.Width / 2;
            int middleY = this.Height / 2;

            circleX = middleX - circleSize / 2;
            circleY = 278 - circleSize / 2;

            //Sombra do círculo
            int shadowOffset = 15;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(circleX, circleY + shadowOffset, circleSize, circleSize);

                PathGradientBrush pathBrush = new PathGradientBrush(path);

                pathBrush.CenterColor = Color.FromArgb(100, Color.Black);
                pathBrush.SurroundColors = new[] { Color.FromArgb(2, Color.Black) };
                e.Graphics.FillPath(pathBrush, path);
            }


            //Contorno do círculo
            int outlineSize = 15;

            Brush brush = new SolidBrush(Colors.bluePrimary);
            RectangleF rect = new RectangleF(circleX, circleY, circleSize, circleSize);
            e.Graphics.FillEllipse(brush, rect);


            //Círculo atrás do microfone
            brush = new SolidBrush(Color.White);
            rect = new RectangleF(circleX + outlineSize / 2, circleY + outlineSize / 2, circleSize - outlineSize, circleSize - outlineSize);
            e.Graphics.FillEllipse(brush, rect);


            //Microfone
            int sizeOffset = 50;
            string imgPath = Path.Combine(Application.StartupPath, @"Images\Icons\micIcon.png");
            Image micIcon = Image.FromFile(imgPath);
            e.Graphics.DrawImage(micIcon, new RectangleF(circleX + sizeOffset / 2, circleY + sizeOffset / 2, circleSize - sizeOffset, circleSize - sizeOffset));
        }
        
        private void pnlBottom_MouseMove(object sender, MouseEventArgs e)
        {
            lastCircleHovering = circleHovering;

            //Aumentar tamanho do botão do microfone quando passar o mouse
            Point mousePos = this.PointToClient(Cursor.Position);
            int mouseX = mousePos.X;
            int mouseY = mousePos.Y;

            PointF middle = new PointF(circleX + circleSize / 2, circleY + circleSize / 2);
            float distanceX = middle.X - mouseX;
            float distanceY = middle.Y - mouseY;
            double distance = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

            //Checar se o mouse está em dentro do círculo
            if (distance <= circleSize / 2) {
                //Aumentar tamanho do círculo
                circleSizeTarget = 300;
                circleHovering = true;
                pnlBottom.Cursor = Cursors.Hand;
            } else {
                //Resetar tamanho do botão
                circleHovering = false;
                circleSizeTarget = circleSizeDefault;
                pnlBottom.Cursor = Cursors.Default;
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            //Ajustar tamanho do botão do microfone
            circleSize += (circleSizeTarget - circleSize) / 3;
            lblDebug.Text = circleSize.ToString() + ", " + circleSizeTarget.ToString() + ", " + ((circleSizeTarget - circleSize) / 10).ToString();

            if (Math.Abs(circleSizeTarget - circleSize) < 1)
            {
                circleSize = circleSizeTarget;
                return;
            }

            pnlBottom.Invalidate();
        }
    }
}
