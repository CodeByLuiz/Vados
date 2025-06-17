using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vados
{
    public partial class UserControlSettings : UserControl
    {
        public event EventHandler<LoadPageEventArgs> loadPage;

        public UserControlSettings()
        {
            InitializeComponent();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            //Ir para página inicial
            loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlHome));
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {

            string extension = cbExtensoes.Text;
            string nome = txtNome.Text;


            if (extension == "pasta")
            {
                Comandos.CriarPasta(nome);
            }
            else
            {
                Comandos.CriarArquivo(nome, extension);
            }



        }

        private void btnExluir_Click(object sender, EventArgs e)
        {
            string extension = cbExtensoes.Text;
            string nome = txtNome.Text;

            if (extension == "pasta")
            {
                Comandos.ExcluirPasta(nome);
            }
            else
            {
                Comandos.ExcluirArquivo(nome, extension);
            }
        }
    }
}
