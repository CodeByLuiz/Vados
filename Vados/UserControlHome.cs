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

        private void txtComando_Click(object sender, EventArgs e)
        {
            //if (txtComando.ForeColor == Color.FromArgb(88, 99, 152))
            //{
            txtComando.Text = "";
            txtComando.ForeColor = Color.Black;
            //}
        }

        private void txtComando_LostFocus(object sender, EventArgs e)
        {
            if (txtComando.Text == "")
            {
                txtComando.Text = "Escreva um comando...";
                txtComando.ForeColor = Color.FromArgb(88, 99, 152);
            }
        }
    }
}
