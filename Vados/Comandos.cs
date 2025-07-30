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
        public static void ExecutarComando(string[] palavras)
        {

        }


        public static List<string> SepararPalavras(string comando)
        {
            var words = new List<string>();
            if (comando == "") return words;

            char[] separators = { ' ', ',' };
            char[] charList = comando.ToCharArray();

            string currentWord = "";

            for (var i = 0; i < charList.Length; i++)
            {
                char c = charList[i];
                bool breakWord = false;

                //Checar se o caractere é um separador
                for (var j = 0; j < separators.Length; j++)
                {
                    char s = separators[j];

                    if (c == s)
                    {
                        breakWord = true;
                        break;
                    }
                }

                //Adicionar caractere à palavra
                if (breakWord == false)
                {
                    currentWord += c;
                    continue;
                }

                //Ir para a próxima palavra (se o caractere for um separador)
                if (currentWord != "")  //Ignorar palavras vazias
                {
                    words.Add(currentWord);
                    currentWord = "";
                }
            }

            //Adicionar última palavra
            if (currentWord != "") {
                words.Add(currentWord);
            }

            return words;
        }
        
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
        {
            //Comando pra abrir o gerenciador depois de realizar uma função
            path = Path.GetDirectoryName(path);
            if (Directory.Exists(path))
            {
                Process.Start("explorer.exe", path);
            }
        }


        #region FUNÇÃO TESTE

        //    public static List<string> funcaoteste(string aprocurar, bool comando)
        //    {
        //        string root = @"" + driveverifica(null);
        //        var caminhos = new List<string>();
        //        var visitados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        //        var ignorar = new List<string>
        //{
        //    "$RECYCLE.BIN",
        //    "System Volume Information",
        //    "Recovery",
        //    "Config.Msi",
        //    "Windows",
        //    "Program Files (x86)",
        //    "Program Files"
        //};
        //        var prioridades = new List<string>
        //{
        //    Path.Combine(root, @"Vados"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\Desktop"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\Contacts"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\Documents"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\Downloads"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\Favorites"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\Pictures"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\Saved Games"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\Links"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\Music"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\3D Objects"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\OneDrive"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\Searches"),
        //    Path.Combine(root, @"Users\"+Environment.UserName+@"\Videos"),
        //    //Path.Combine(root, @"Users\"+Environment.UserName+@""),

        //    Path.Combine(root),
        //};

        //        var fila = new Queue<string>();
        //        foreach (var pasta in prioridades)
        //        {
        //            if (Directory.Exists(pasta))
        //            {
        //                fila.Enqueue(pasta);
        //                //caminhos.Add(pasta);
        //            }
        //        }

        //        fila.Enqueue(root);

        //        while (fila.Count > 0)
        //        {
        //            var atual = fila.Dequeue();
        //            try
        //            {
        //                switch (comando)
        //                {
        //                    case true:

        //                        foreach (var caminho in Directory.GetDirectories(atual))
        //                        {
        //                            string nomePasta = Path.GetFileName(caminho);
        //                            if (ignorar.Any(ign => nomePasta.Equals(ign, StringComparison.OrdinalIgnoreCase)))
        //                                continue;

        //                            if (visitados.Add(caminho))
        //                            {
        //                                caminhos.Add(caminho);
        //                                //fila.Enqueue(caminho);
        //                                Comandos.InserirNoInicio(fila, caminho);
        //                                if (caminho.Contains(aprocurar))
        //                                {
        //                                    MessageBox.Show($"Foram encontrados d {caminhos.Count} caminhos de pastas.");
        //                                    return caminhos;
        //                                }
        //                                // MessageBox.Show(caminho);
        //                            }


        //                        }

        //                        break;
        //                    case false:


        //                        foreach (var arquivo in Directory.GetFiles(atual))
        //                        {
        //                            string nome = Path.GetFileName(arquivo);
        //                            if (nome.Contains(aprocurar, StringComparison.OrdinalIgnoreCase))
        //                            {
        //                                MessageBox.Show($"Arquivo encontrado: {arquivo}");
        //                                return caminhos;
        //                            }
        //                        }


        //                        foreach (var caminho in Directory.GetDirectories(atual))
        //                        {
        //                            string nomePasta = Path.GetFileName(caminho);
        //                            if (ignorar.Any(ign => nomePasta.Equals(ign, StringComparison.OrdinalIgnoreCase)))
        //                                continue;

        //                            //fila.Enqueue(caminho);
        //                            Comandos.InserirNoInicio(fila, caminho);
        //                        }

        //                        break;
        //                }


        //            }

        //            catch (Exception)
        //            {

        //            }
        //        }
        //        MessageBox.Show("Nenhum arquivo encontrado com o nome especificado.");
        //        return null;
        //    }


        #endregion


        public static string SearchFolders(string aprocurar, bool comando) // busca recursivamente por pastas ou arquivos
        {
            //muito cuidado quando usar o "comando", TRUE é para quando ele age diretamente em pastas e FALSE é para quando ele age em arquivos
            // por exemplo no comando de criar arquivos, ele sera TRUE, pq ele ira localizar a PASTA onde o arquivo sera criado


            string root = @"" + driveverifica(null);
            var caminhos = new List<string>();
            var visitados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
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
                Path.Combine(root, @"Users\"+Environment.UserName+@"\AppData\Roaming\Vados"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\Desktop"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\Contacts"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\Documents"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\Downloads"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\Favorites"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\Pictures"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\Saved Games"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\Links"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\Music"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\3D Objects"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\OneDrive"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\Searches"),
                Path.Combine(root, @"Users\"+Environment.UserName+@"\Videos"),
                //Path.Combine(root, @"Users\"+Environment.UserName+@""),

                Path.Combine(root),
            };

            var fila = new Queue<string>();
            foreach (var pasta in prioridades)
            {
                if (Directory.Exists(pasta))
                {
                    fila.Enqueue(pasta);
                    caminhos.Add(pasta);
                }
            }

            fila.Enqueue(root);

            while (fila.Count > 0)
            {
                var atual = fila.Dequeue();
                try
                {
                    switch (comando)
                    {
                        case true:

                            foreach (var caminho in Directory.GetDirectories(atual))
                            {
                                string nomePasta = Path.GetFileName(caminho);
                                if (ignorar.Any(ign => nomePasta.Equals(ign, StringComparison.OrdinalIgnoreCase)))
                                    continue;

                                if (visitados.Add(caminho))
                                {
                                    caminhos.Add(caminho);
                                    //fila.Enqueue(caminho);
                                    InserirNoInicio(fila, caminho);
                                    if (caminho.Contains(aprocurar) && atual.Contains(aprocurar) )
                                    {
                                        MessageBox.Show($"Foram encontrados d {caminhos.Count} caminhos de pastas.");
                                        MessageBox.Show(atual+" situação 1 "+ aprocurar);
                                        return atual;
                                    }
                                    else if (caminho.Contains(aprocurar)) 
                                    {
                                        MessageBox.Show($"Foram encontrados d {caminhos.Count} caminhos de pastas.");
                                        MessageBox.Show(caminho+" situação 2 " + aprocurar);
                                        return caminho;
                                    }
                                        
                                }
                            }

                            break;
                        case false:

                            
                            foreach (var arquivo in Directory.GetFiles(atual))
                            {
                                string nome = Path.GetFileName(arquivo);
                                if (nome.Contains(aprocurar, StringComparison.OrdinalIgnoreCase))
                                {
                                    MessageBox.Show($"Arquivo encontrado: {arquivo}");
                                    return arquivo;
                                }
                            }

                            
                            foreach (var caminho in Directory.GetDirectories(atual))
                            {
                                string nomePasta = Path.GetFileName(caminho);
                                if (ignorar.Any(ign => nomePasta.Equals(ign, StringComparison.OrdinalIgnoreCase)))
                                    continue;

                                //fila.Enqueue(caminho);


                                if (visitados.Add(caminho))
                                {
                                    Comandos.InserirNoInicio(fila, caminho);
                                }
                            }

                            break;
                    }
                    
                    
                }

                catch (Exception)
                {

                }
            }
            MessageBox.Show("Nenhum arquivo encontrado com o nome especificado.");
            return null;
        }


        public static void InserirNoInicio<T>(Queue<T> fila, T novoElemento)
        {
            Queue<T> filaTemporaria = new Queue<T>();

            
            while (fila.Count > 0)
            {
                filaTemporaria.Enqueue(fila.Dequeue());
            }

           
            fila.Enqueue(novoElemento);

           
            while (filaTemporaria.Count > 0)
            {
                fila.Enqueue(filaTemporaria.Dequeue());
            }
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


        public static string CriarPastaPadrao()
        {

            string caminhoPadrao = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vados");
            MessageBox.Show(caminhoPadrao);
            try
            {
                if (!Directory.Exists(caminhoPadrao))
                    Directory.CreateDirectory(caminhoPadrao);

                return caminhoPadrao;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar a pasta padrão: " + ex.Message);
                return null;
            }
        }


        public static void CriarPasta(string nome, string path) // cria pasta
        {
            try
            {
                if (path == "") 
                {
                    path = Path.Combine(Global.DefaultFolder + @"\" + nome);
                }
                else
                {
                    path = SearchFolders(path, true) + @"\" + nome;

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
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar pasta: " + ex.Message);



            }
        }


        public static void CriarArquivo(string nome, string extension, string path) // cria arquivo
        {
            try
            {
                if (path == "")
                {
                    path = Path.Combine(Global.DefaultFolder + @"\" + nome + "." + extension);
                }
                else
                {
                    path = SearchFolders(path, true) + @"\" + nome + "." + extension;
                    MessageBox.Show(path);
                }

                using (FileStream fs = File.Create(path)) ;
                Console.WriteLine("Arquivo" + nome + "Criado com sucesso");
                AbrirGerenciador(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar arquivo: " + ex.Message);


            }
        }


        public static void ExcluirArquivo(string nome, string extension) //exclui arquivo
        {
            string path = SearchFolders(nome /*+ "." + extension*/, false);
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
            string path = SearchFolders(nome, true);
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

            //string path = Path.Combine(Global.DefaultFolder + nome + "." + extensao);
            nome = nome + "." + extensao;
            string path = SearchFolders(nome,false);
            

            string novoPath = Path.Combine(Path.GetDirectoryName(path) +@"\"+ novoNome + "." + extensao);
          
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

            string path = SearchFolders(nome, true);
            MessageBox.Show(path);

            string novoPath = Path.Combine(Path.GetDirectoryName(path) + @"\" + novoNome);

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


        public static void MudarIdioma(string idioma) /// essa porra vai mudar o idoma da infarce do windows, mas só funciona no windows 10 e 11, e tem que reiniciar o pc para funcionar(tenho que aprender poweshell)
        {
            //o idioma tem que tar baixado caso o contrario ele só renicia a maquina e nao muda nada, dar pra fazer baixar o bagulho por comando powershell mas mesmo assim nao consigo testar pq meu windows tem licença só pra uma lingua 

            string comando = $"Set-WinUILanguageOverride -Language '{idioma}'";

            var processo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{comando}\"",
                UseShellExecute = true,
                Verb = "runas"
            };

            Process.Start(processo);

            MessageBox.Show("O idioma da interface foi alterado. O computador será reiniciado em 5 segundos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //nao consigo testar isso aqui,meu pc só tem o idioma pt-BR e nao consigo mudar, mas deve funcionar, testem no de vcs se der 


        }


        public static void ReiniciarPC() // reinicia o pc
        {
            Process.Start("shutdown", "/r /t 5");
            Application.Exit();
        }

        public static void AbrirArquivo(string nome)
        {
           string arquivo = SearchFolders(nome,false);

            var psi = new ProcessStartInfo()
            {
                UseShellExecute= true,
                FileName = arquivo,
            };
            Process.Start(psi);
        }


    }

}
