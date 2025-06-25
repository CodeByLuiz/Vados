using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            //Círculo atrás do microfone
            int width = 216;
            int height = 216;
            int xx = this.Width / 2 - width / 2;
            int yy = 150;
            int offset = 50;

            Brush brush = new SolidBrush(Color.White);
            Rectangle rect = new Rectangle(xx - offset / 2, yy - offset / 2, width + offset, height + offset);
            e.Graphics.FillEllipse(brush, rect);


            //Microfone
            Image micIcon = Image.FromFile("Imagens/Ícones/micIcon.png");
            e.Graphics.DrawImage(micIcon, new Rectangle(xx, yy, width, height));
        }
    }
}
