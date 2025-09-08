using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

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

            string extension = "";
            string nome = txtNome.Text;
            string destino = txtDestinatario.Text;


            
                Comandos.CriarPasta(nome, destino);
           
            //else
            //{
            //    var nomes = new List<string>()
            //{
            //    @"C:\Users\ETEC\Desktop\pasta teste\11111.txt",
            //    @"C:\Users\ETEC\Desktop\pasta teste\awddsa.txt",
            //    @"C:\Users\ETEC\Desktop\pasta teste\mhgfnbvbvxvcxvc cnv.txt"

            //};

            //    foreach (string x in nomes)
            //    {
            //        Comandos.CriarArquivo(x, @"C:\Users\ETEC\Desktop\pasta teste");
            //    }
            //}



        }

        private void btnExluir_Click(object sender, EventArgs e)
        {
            string extension = cbExtensoes.Text;
            string nome = txtNome.Text;
            var nomes = new List<string>()
            {
                @"C:\Users\ETEC\Desktop\pasta teste\11111.txt",
                @"C:\Users\ETEC\Desktop\pasta teste\awddsa.txt",
                @"C:\Users\ETEC\Desktop\pasta teste\mhgfnbvbvxvcxvc cnv.txt"

            };

            if (extension == "pasta")
            {
                //Comandos.ExcluirPasta(nome);
            }
            else
            {
                Comandos.ExcluirArquivo(nomes);
            }
        }

        private void btnRenomear_Click(object sender, EventArgs e)
        {
            string extension = cbExtensoes.Text;
            string nome = txtNome.Text;
            string novoNome = txtNovoNome.Text;
            if (extension == "pasta")
            {
                Comandos.RenomearPasta(nome, novoNome, "");
            }
            else
            {
                Comandos.RenomearArquivo(nome, novoNome, "");
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

        private async void btnLog_Click(object sender, EventArgs e)
        {
            string SearchArquivo = txtSearch.Text;
            
            foreach (var item in (await Comandos.SearchPaths(SearchArquivo, false, pathAmount:null)))
            {
                listateste.Items.Add(item);
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
            string extension = "";
            string nome = txtNome.Text;
            string destino = txtDestinatario.Text;
            var nomes = new List<string>()
            {
                @"C:\Users\ETEC\Desktop\pasta teste\11111.txt",
                @"C:\Users\ETEC\Desktop\pasta teste\awddsa.txt",
                @"C:\Users\ETEC\Desktop\pasta teste\mhgfnbvbvxvcxvc cnv.txt"

            };

            if (extension == "pasta")
            {
               // Comandos.MoverPasta(nome, destino);

            }
            else
            {
                Comandos.MoverArquivo(nomes, @"C:\Users\ETEC\\Desktop\moveraqui");
            }
        }
        

        private void btnDupe_Click(object sender, EventArgs e)
        {
            string extension = cbExtensoes.Text;
            string nome = txtNome.Text;
            string destino = txtDestinatario.Text;

            if (extension == "pasta")
            {
               // Comandos.DuplicarPasta(nome, destino);

            }
            else
            {
                //Comandos.DuplicarArquivo(nome, destino);
            }
        }
        
        
        private void btnAbrirArquivo_Click(object sender, EventArgs e)
        {
            string NomeArquivo = txtNome.Text;
            Comandos.AbrirArquivo(NomeArquivo);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Comandos.ExecutarCaminho(txtNomeAplicativo.Text);
        }
    }
}
