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

        private void btnLog_Click(object sender, EventArgs e)
        {

            string SearchArquivo = txtSearch.Text;

            MessageBox.Show(Environment.UserName);
            if (cbExtensoes.Text == "pasta")
            {
                
                foreach (var item in Comandos.SearchFolders(".txt", false, pastaRoot: "Desktop")) 
                {
                    listateste.Items.Add(item);
                }

                //listateste.Items.Add(Comandos.SearchFolders(SearchArquivo, false, 24000));

            }
            else
            {

                //HashSet<string> porra = new HashSet<string>();
                //porra = Comandos.MultiSearch(SearchArquivo, "inferno 2", ".txt");

                //foreach (var x in Comandos.MultiSearch(SearchArquivo, "inferno 2", ".txt")) 
                //{
                //    listateste.Items.Add(x);
                //}
                //Task.Run(() => Comandos.MoverUnsArquivos(".txt", "Desktop", "pasta de coisa"));

                

            }
        }
        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void txtDestinatario_TextChanged(object sender, EventArgs e)
        {

        }


        private void btnMover_Click(object sender, EventArgs e)
        {
            string extension = cbExtensoes.Text;
            string nome = txtNome.Text;
            string destino = txtDestinatario.Text;

            if (extension == "pasta")
            {
                Comandos.MoverPasta(nome, destino);

            }
            else
            {
                Comandos.MoverArquivo(nome, destino);
            }
        }
        

        private void btnDupe_Click(object sender, EventArgs e)
        {
            string extension = cbExtensoes.Text;
            string nome = txtNome.Text;
            string destino = txtDestinatario.Text;

            if (extension == "pasta")
            {
                Comandos.DuplicarPasta(nome, destino);

            }
            else
            {
                Comandos.DuplicarArquivo(nome, destino);
            }
        }
        
        
        private void btnAbrirArquivo_Click(object sender, EventArgs e)
        {
            string NomeArquivo = txtNome.Text;
            Comandos.AbrirArquivo(NomeArquivo);
        }
    }
}
