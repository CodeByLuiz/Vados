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
using NAudio.Wave;

namespace Vados
{


    public partial class UserControlSettings : UserControl
    {
        public event EventHandler<LoadPageEventArgs> loadPage;

        private ReconhecimentoVoz reconhecedor;
        private bool estaPausado = false;

        public UserControlSettings()
        {
            InitializeComponent();
            


        }
        private void UserControlSettings_Load(object sender, EventArgs e)
        {
            PopularDispositivosAudio();
           
        }




        private void button1_Click(object sender, EventArgs e)
        {
            //Ir para página inicial
            loadPage?.Invoke(this, new LoadPageEventArgs(Global.userControlHome));
        }

        private async void btnCriar_Click(object sender, EventArgs e)
        {
            string extension = "";
            string nome = txtNome.Text;
            string destino = txtDestinatario.Text;


           
            Comandos.CriarPasta(nome, destino);
            List<string> nomecompleto = (await Comandos.SearchPaths(nome, true)).ToList();
            BancoDeDados.AdicionarEntrada(
                comando: "Criar Pasta",
                titulo:""
                
            );

            //BancoDeDados.ListarTodasEntradas();

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

            foreach (var item in (await Comandos.SearchPaths(SearchArquivo, false, pathAmount: null)))
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
            //Comandos.AbrirArquivo(NomeArquivo);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Comandos.ExecutarCaminho(txtNomeAplicativo.Text);
        }

        #region RECONHECIMENTO DE VOZ

        private void ResultadoFinalRecebido(string texto)
        {
            Invoke(new Action(() =>
            {
                txtTranscriçãoTest.AppendText(texto + " ");
            }));
        }

        private void ResultadoParcialRecebido(string parcial)
        {
            // Você pode ignorar isso ou mostrar preview em algum label
            Console.WriteLine($"Parcial: {parcial}");
        }

        private async void btnStartRecTest_Click(object sender, EventArgs e)
        {

            if (reconhecedor == null) { 

               string x = (await Comandos.SearchPaths("vosk-model-small-pt-0.3",true)).FirstOrDefault();

                reconhecedor = new ReconhecimentoVoz(x);
                reconhecedor.OnFinalResult += ResultadoFinalRecebido;
                reconhecedor.OnPartialResult += ResultadoParcialRecebido;
            }

            reconhecedor.Start();
            estaPausado = false;
            btnPauseTest.Text = "Pausar";
        }

        private void btnPauseTest_Click(object sender, EventArgs e)
        {
            int selectedDeviceIndex = 0;
            if (cbMicrofones.SelectedIndex >= 0)
                selectedDeviceIndex = deviceIds[cbMicrofones.SelectedIndex];

            reconhecedor = new ReconhecimentoVoz(@"caminho\do\modelo", selectedDeviceIndex);
            reconhecedor.OnFinalResult += ResultadoFinalRecebido;
            reconhecedor.OnPartialResult += ResultadoParcialRecebido;

            reconhecedor.Start();
            estaPausado = false;
            btnPauseTest.Text = "Pausar";
        }

        private void btnStopRecTest_Click(object sender, EventArgs e)
        {
            reconhecedor.Stop();
            estaPausado = false;
            btnPauseTest.Text = "Pausar";
        }

        private List<int> deviceIds = new List<int>();

        private void PopularDispositivosAudio()
        {
            cbMicrofones.Items.Clear();
            deviceIds.Clear();

            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var cap = WaveIn.GetCapabilities(i);
                deviceIds.Add(i);
                cbMicrofones.Items.Add(cap.ProductName);
            }

            if (cbMicrofones.Items.Count > 0)
                cbMicrofones.SelectedIndex = 0; // seleciona o primeiro por padrão
            else
                MessageBox.Show("Nenhum microfone detectado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            reconhecedor?.Dispose();
        }
    }
}
