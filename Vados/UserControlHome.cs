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

        public UserControlHome()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
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
            int sizeOffset = 50;
            int width = 216 + sizeOffset;
            int height = 216 + sizeOffset;
            int xx = this.Width / 2 - width / 2 - sizeOffset / 2;
            int yy = 150 - sizeOffset/ 2;

            //Sombra do círculo
            int shadowOffset = 15;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(xx, yy + shadowOffset, width, height);

                PathGradientBrush pathBrush = new PathGradientBrush(path);

                pathBrush.CenterColor = Color.FromArgb(100, Color.Black);
                pathBrush.SurroundColors = new[] { Color.FromArgb(2, Color.Black) };
                e.Graphics.FillPath(pathBrush, path);
            }


            //Contorno do círculo
            int outlineSize = 15;

            Brush brush = new SolidBrush(Colors.bluePrimary);
            Rectangle rect = new Rectangle(xx, yy, width, height);
            e.Graphics.FillEllipse(brush, rect);


            //Círculo atrás do microfone
            brush = new SolidBrush(Color.White);
            rect = new Rectangle(xx + outlineSize / 2, yy + outlineSize / 2, width - outlineSize, height - outlineSize);
            e.Graphics.FillEllipse(brush, rect);


            //Microfone
            string imgPath = Path.Combine(Application.StartupPath, @"Images\Icons\micIcon.png");
            Image micIcon = Image.FromFile(imgPath);
            e.Graphics.DrawImage(micIcon, new Rectangle(xx + sizeOffset / 2, yy + sizeOffset / 2, width - sizeOffset, height - sizeOffset));
        }
    }
}
