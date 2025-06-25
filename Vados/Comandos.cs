using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vados
{
    internal class Comandos
    {
        public static bool VerificarAppAbertas(string nome)    // ia usar mas acabei nao usando mas pode ser util
        {
            Process[] processes = Process.GetProcessesByName(nome);

            if (processes.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }


        public static void AbrirGerenciador(string path)
        {// comando pra abrir o gerenciador depois de realizar uma função


            path = Path.GetDirectoryName(path);
            if (Directory.Exists(path))
            {
                Process.Start("explorer.exe", path);
            }




        }
        public static string SearchFolders(string aprocurar)
        {
            string root = @"" + driveverifica(null);
            var caminhos = new List<string>();
            var ignorar = new List<string>
    {
        "$RECYCLE.BIN",
        "System Volume Information",
        "Recovery",
        "Config.Msi",
        "Windows",
        "Program Files (x86)",
        "Program Files"
    };
            var prioridades = new List<string>
    {
        Path.Combine(root, @"Users\Vados"),
        Path.Combine(root, @"Users\Popel"),
        Path.Combine(root, @"Users\Public")
    };

            var fila = new Queue<string>();
            foreach (var pasta in prioridades)
            {
                if (Directory.Exists(pasta))
                {
                    fila.Enqueue(pasta);
                    //caminhos.Add(pasta);
                }
            }

            fila.Enqueue(root);

            while (fila.Count > 0)
            {
                var atual = fila.Dequeue();
                try
                {
                    foreach (var caminho in Directory.GetDirectories(atual))
                    {
                        string nomePasta = Path.GetFileName(caminho);
                        if (ignorar.Any(ign => nomePasta.Equals(ign, StringComparison.OrdinalIgnoreCase)))
                            continue;

                        caminhos.Add(caminho);
                        fila.Enqueue(caminho);

                        if (caminho.Contains(aprocurar))
                        {
                            MessageBox.Show($"Foram encontrados {caminhos.Count} caminhos de pastas.");
                            return caminho;
                        }

                    }
                }

                catch (Exception)
                {

                }
            }
            MessageBox.Show("Nenhum arquivo encontrado com o nome especificado.");
            return null;
        }




        public static string driveverifica(string[] args)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {

                    return drive.Name;
                }
            }
            MessageBox.Show("Nenhum drive disponível encontrado.");
            return null;
        }

        public static void CriarPastaPadrao()
        {
            
            string path = @"C:\Vados";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void CriarPasta(string nome, string path) // cria pasta
        {
            
            MessageBox.Show(path);
            if (path=="")
            {
                path = SearchFolders(path)+@"\"+nome;
                MessageBox.Show(path);
            }
            else {
                path = Path.Combine(Global.DefaultFolder + nome);
            }
                

            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Pasta" + nome + "Criada com sucesso");
                AbrirGerenciador(path);
            }
            else
            {
                MessageBox.Show("Erro, há um arquivo com o mesmo nome da sua pasta");
            }

        }

        public static void CriarArquivo(string nome, string extension) // cria arquivo
        {

            string path = Path.Combine(Global.DefaultFolder + nome + "." + extension);
            using (FileStream fs = File.Create(path)) ;
            Console.WriteLine("Arquivo" + nome + "Criado com sucesso");
            AbrirGerenciador(path);

        }

        public static void ExcluirArquivo(string nome, string extension) //exclui arquivo
        {
            string path = Path.Combine(Global.DefaultFolder + nome + "." + extension);
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine("Arquivo" + nome + "Excluido com sucesso");
            }
            else
            {

                MessageBox.Show("esse arquivo não existe");
            }


        }

        public static void ExcluirPasta(string nome) // exclui pasta
        {
            string path = Path.Combine(Global.DefaultFolder + nome);
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
                Console.WriteLine("Pasta" + nome + "Excluida com sucesso");
            }
            else
            {
                MessageBox.Show("essa pasta não existe");

            }


        }

        public static void RenomearArquivo(string nome, string novoNome, string extensao) // renomear arquivo(erro de logica, falta implementar o bagulho de procurar o arquivo o mesmo serve para o bagulho de excluir)
        {
            string path = Path.Combine(Global.DefaultFolder + nome + "." + extensao);
            string novoPath = Path.Combine(Global.DefaultFolder + novoNome + "." + extensao);
            if (File.Exists(path))
            {
                File.Move(path, novoPath);
                Console.WriteLine("Arquivo" + nome + "Renomeado para " + novoNome);
            }
            else
            {
                MessageBox.Show("esse arquivo não existe");
            }
        }

        public static void RenomearPasta(string nome, string novoNome) // renomear pasta(mesmo erro de logica do renomear arquivo)
        {
            string path = Path.Combine(Global.DefaultFolder + nome);
            string novoPath = Path.Combine(Global.DefaultFolder + novoNome);
            if (Directory.Exists(path))
            {
                Directory.Move(path, novoPath);
                Console.WriteLine("Pasta" + nome + "Renomeada para " + novoNome);
            }
            else
            {
                MessageBox.Show("essa pasta não existe");
            }
        }

        public static void DarAdm()// da permissões de administrador
        {
            try
            {
                ProcessStartInfo proc = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = Application.ExecutablePath,
                    Verb = "runas"
                };

                Process.Start(proc);
                Application.Exit();
            }
            catch
            {
                MessageBox.Show("O programa precisa de permissões de administrador para funcionar corretamente.", "Permissão negada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }




    }

    



}
