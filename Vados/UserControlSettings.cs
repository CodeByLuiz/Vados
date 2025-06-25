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
            string destino = txtDestinatario.Text;


            if (extension == "pasta")
            {
                Comandos.CriarPasta(nome, destino);
            }
            else
            {
                Comandos.CriarArquivo(nome, extension, destino);
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

        private void btnRenomear_Click(object sender, EventArgs e)
        {
            string extension = cbExtensoes.Text;
            string nome = txtNome.Text;
            string novoNome = txtNovoNome.Text;
            if (extension == "pasta")
            {
                Comandos.RenomearPasta(nome, novoNome);
            }
            else
            {
                Comandos.RenomearArquivo(nome, novoNome, extension);
            }

        }

        private void UserControlSettings_Load(object sender, EventArgs e)
        {

        }

        private void btnAdm_Click(object sender, EventArgs e)
        {
            Comandos.DarAdm();
        }

        private void btnMudarIdioma_Click(object sender, EventArgs e)
        {
            string idioma = cbIdioma.Text;
            if (idioma == "Português do Brasil")
            {
                idioma = "pt-BR";
                Comandos.MudarIdioma(idioma);
                Comandos.ReiniciarPC();
            }
            else if (idioma == "Inglês")
            {
                idioma = "en-US";
                Comandos.MudarIdioma(idioma);
                Comandos.ReiniciarPC();
            }
            else
            {
                MessageBox.Show("Selecione um idioma válido.");
            }


        }
    }
}
