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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Ir para página de configurações
            loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlSettings));
        }

        private void DrawEllipse(int x, int y, int largura, int altura)
        {
            Pen myPen = new Pen(Color.Red);
            Graphics formGraphics;
            formGraphics = this.CreateGraphics();
            formGraphics.DrawEllipse(myPen, new Rectangle(x, y, largura, altura));
            myPen.Dispose();
            formGraphics.Dispose();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            //Pintura padrão da página
            base.OnPaint(e);

            //Círculo em volta do microfone
            DrawEllipse(100, 100, 100, 100);
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
    }
}
