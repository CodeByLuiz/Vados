using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using SHDocVw;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Vados
{
    internal class Comandos
    {
        #region FUNÇÕES DO WINDOWS

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        #endregion


        #region TEXTO PARA COMANDO

        #region DICIONÁRIOS / LISTAS

        //Sinonimo chave de cada variação dos comandos
        static Dictionary<string, string> commandSynonyms = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            //Criar
            { "criar", "criar" },
            { "crie", "criar" },
            { "gerar", "criar" },
            { "gere", "criar" },
            { "produzir", "criar" },
            { "produza", "criar" },
            { "formar", "criar" },
            { "forme", "criar" },
            { "construir", "criar" },
            { "construa", "criar" },
            { "fazer", "criar" },
            { "faca", "criar" },
            { "inventar", "criar" },
            { "invente", "criar" },
            { "dar a luz", "criar" },
            { "de a luz", "criar" },
            { "originar", "criar" },
            { "origine", "criar" },
            //Renomear
            { "renomear", "renomear" },
            { "renomeie", "renomear" },
            { "alterar o nome", "renomear" },
            { "altere o nome", "renomear" },
            { "rebatizar", "renomear" },
            { "rebatize", "renomear" },
            { "mudar o nome", "renomear" },
            { "mude o nome", "renomear" },
            { "trocar o nome", "renomear" },
            { "troque o nome", "renomear" },
            { "modificar o nome", "renomear" },
            { "modifique o nome", "renomear" },
            { "substituir o nome", "renomear" },
            { "substitua o nome", "renomear" },
            //Excluir
            { "excluir", "excluir" },
            { "exclua", "excluir" },
            { "deletar", "excluir" },
            { "delete", "excluir" },
            { "apagar", "excluir" },
            { "apague", "excluir" },
            { "remover", "excluir" },
            { "remova", "excluir" },
            { "eliminar", "excluir" },
            { "elimine", "excluir" },
            { "destruir", "excluir" },
            { "destrua", "excluir" },
            { "quebrar", "excluir" },
            { "quebre", "excluir" },
            { "anular", "excluir" },
            { "anule", "excluir" },
            { "desfazer", "excluir" },
            { "desfaca", "excluir" },
            { "extinguir", "excluir" },
            { "extinga", "excluir" },
            { "obliterar", "excluir" },
            { "oblitere", "excluir" },
            //Mover
            { "mover", "mover" },
            { "mova", "mover" },
            { "transferir", "mover" },
            { "transfera", "mover" },
            { "realocar", "mover" },
            { "realoque", "mover" },
            { "colocar", "mover" },
            { "coloque", "mover" },
            { "deslocar", "mover" },
            { "desloque", "mover" },
            { "levar", "mover" },
            { "leve", "mover" },
            { "transportar", "mover" },
            { "transporte", "mover" },
            { "enviar", "mover" },
            { "envie", "mover" },
            { "movimentar", "mover" },
            { "movimente", "mover" },
            { "translocar", "mover" },
            { "transloque", "mover" },
            //Duplicar
            { "duplicar", "duplicar" },
            { "duplique", "duplicar" },
            { "copiar", "duplicar" },
            { "copie", "duplicar" },
            { "repetir", "duplicar" },
            { "repita", "duplicar" },
            { "reproduzir", "duplicar" },
            { "reproduza", "duplicar" },
            { "imitar", "duplicar" },
            { "imite", "duplicar" },
        };

        //Todas as variações dos comandos
        public static List<string> allCommands = new List<string>(commandSynonyms.Keys);


        //Todas as variações de pasta
        public static List<string> folderWords = new List<string>()
        {
            "pasta", "pastas", "diretorio", "diretorios"
        };

        //Sinonimos chave de cada sinônimo dos objetos (pasta / arquivo)
        static Dictionary<string, string> objectSynonyms = new Dictionary<string, string>()
        {
            { "pastas", "pasta" },
            { "pasta", "pasta" },
            { "diretorios", "pasta" },
            { "diretorio", "pasta" },
            { "arquivos", "arquivo" },
            { "arquivo", "arquivo" },
            { "documentos", "arquivo" },
            { "documento", "arquivo" },
        };

        //Todas as variações de objetos (pasta / arquivo)
        public static List<string> allObjects = new List<string>(objectSynonyms.Keys);

        //Todos os sinônimos
        static Dictionary<string, string> currentSynonyms = commandSynonyms.Concat(objectSynonyms).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        static Dictionary<string, string> wordSynonyms = new Dictionary<string, string>(currentSynonyms)
        {
            //Quantidade
            { "todos", "todos" },
            { "todos os", "todos" },
            { "metade dos", "metade" },

            //Tamanho
            { "maior que", "maior" },
            { "maiores que", "maior" },
            { "superior a", "maior" },
            { "acima de", "maior" },
            { "mais alto que", "maior" },
            { "mais alta que", "maior" },
            { "mais altos que", "maior" },
            { "mais altas que", "maior" },
            { "menor", "menor" },
            { "inferior a", "menor" },
            { "abaixo de", "menor" },
            { "mais baixo que", "menor" },
            { "mais baixa que", "menor" },
            { "mais baixos que", "menor" },
            { "mais baixas que", "menor" },
        };


        //Extensões relacionadas as palavras
        static Dictionary<string, List<string>> wordExtensions = new Dictionary<string, List<string>>()
        {
            { "texto", new List<string>() { "txt", "doc", "docx", "rtf", "odt", "md" } },
            { "imagem", new List<string>() { "png", "jpg", "jpeg", "bmp", "ico" } },
            { "video", new List<string>() { "mp4", "avi", "mov" } },
            { "audio", new List<string>() { "mp3", "wav", "ogg" } },
            { "apresentacao", new List<string>() { "odp", "ppt", "pptx" } },
            { "web", new List<string>() { "htm", "html", "css", "js", "php", "xps", "asp" } },
            { "executavel", new List<string>() { "exe", "lnk" } },
            { "atalho", new List<string>() { "lnk" } },
            { "compactado", new List<string>() { "zip", "rar", "7z" } },
            { "power point", new List<string>() { "ppt", "pptx" } },
            { "word", new List<string>() { "doc", "docx" } },
            { "excel", new List<string>() { "xls", "xlsx" } },
        };

        //Todas as palavras que indicam extensões
        public static List<string> allExtensionsWords = new List<string>(wordExtensions.Keys);


        //Formas de começar o comando
        public static List<string> startWords = new List<string>()
        {
            "quero",
            "eu quero",
            "quero que",
            "eu quero que",
            "quero que voce",
            "eu quero que voce",
            "por favor",
            "por obsequio",
            "por gentileza",
        };

        //Formas de indicar a pasta de criação (comando criar)
        public static List<string> destinationWords = new List<string>()
        {
            "na",
            "no",
            "dentro da",
            "dentro do",
            "com origem na",
            "com origem no",
            "com destino na",
            "com destino no",
        };

        //Formas de indicar a pasta de destino (comando mover)
        public static List<string> insideWords = new List<string>()
        {
            "na",
            "no",
            "pra",
            "pro",
            "para a",
            "para o",
            "pra dentro da",
            "pra dentro do",
            "para dentro da",
            "para dentro do",
            "pra o interior da",
            "pra o interior do",
            "para o interior da",
            "para o interior do",
        };

        public static List<string> fromWords = new List<string>()
        {
            "da",
            "do",
            "pertencente a",
            "pertencentes a",
            "que pertence a",
            "que pertencem a",
            "que esta dentro da",
            "que estao dentro da",
            "que esta no interior da",
            "que estao no interior da",
        };

        //Formas de indicar a pasta de posse

        //Formas de nomear o arquivo / pasta
        public static List<string> namingWords = new List<string>()
        {
            "chamado",
            "chamados",
            "chamada",
            "chamadas",
            "nomeado",
            "nomeados",
            "nomeada",
            "nomeadas",
            "denominado",
            "denominados",
            "denominada",
            "denominadas",
            "intitulado",
            "intitulados",
            "intitulada",
            "intituladas",
            "de nome",
            "de titulo",
            "com nome",
            "com titulo",
            "com o nome",
            "com o titulo",
        };

        //Formas de indicar a quantidade de arquivos / pastas
        public static List<string> amountWords = new List<string>()
        {
            "todos",
            "todos os",
            "metade dos",
        };


        public static string WordGetSynonym(string word)
        {
            if (wordSynonyms.TryGetValue(word, out string synonym))
            {
                return synonym;
            }

            return "";
        }

        public static List<string> WordGetExtensions(string word)
        {
            if (wordExtensions.TryGetValue(word, out List<string> extensions))
            {
                return extensions;
            }

            return new List<string>();
        }

        #endregion


        public static CommandCriteria CommandGetArguments(string command)
        {
            CommandCriteria criteria = new CommandCriteria();

            //Definir tipo de comando
            ActionExtractor actionExtractor = new ActionExtractor(Comandos.startWords, Comandos.allCommands);
            actionExtractor.Extract(command, criteria);


            //Extrair argumentos para cada tipo de comando
            CommandParser parser = new CommandParser(criteria, new List<CriteriaExtractor>());

            switch (criteria.Action)
            {
                case "criar":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                        {
                            new ObjectExtractor(Comandos.amountWords, Comandos.allObjects, Comandos.allExtensionsWords, Comandos.namingWords),
                            new DestinationExtractor(Comandos.destinationWords, Comandos.folderWords, Comandos.namingWords)
                        });
                    break;

                case "renomear":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                        {
                            new ObjectExtractor(Comandos.amountWords, Comandos.allObjects, Comandos.allExtensionsWords, Comandos.namingWords),
                            new NewNameExtractor(),
                            new OriginExtractor(Comandos.fromWords, Comandos.folderWords, Comandos.namingWords)
                        });
                    break;

                case "excluir":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                        {
                            new ObjectExtractor(Comandos.amountWords, Comandos.allObjects, Comandos.allExtensionsWords, Comandos.namingWords, Comandos.fromWords),
                            new OriginExtractor(Comandos.fromWords, Comandos.folderWords, Comandos.namingWords)
                        });
                    break;


                case "mover":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                        {
                            new ObjectExtractor(Comandos.amountWords, Comandos.allObjects, Comandos.allExtensionsWords, Comandos.namingWords, Comandos.fromWords),
                            new OriginExtractor(Comandos.fromWords, Comandos.folderWords, Comandos.namingWords),
                            new DestinationExtractor(Comandos.insideWords, Comandos.folderWords, Comandos.namingWords)
                        });
                    break;


                case "duplicar":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                        {
                            new ObjectExtractor(Comandos.amountWords, Comandos.allObjects, Comandos.allExtensionsWords, Comandos.namingWords),
                            new OriginExtractor(Comandos.fromWords, Comandos.folderWords, Comandos.namingWords),
                            new DestinationExtractor(Comandos.insideWords, Comandos.folderWords, Comandos.namingWords)
                        });
                    break;
            }

            var arguments = parser.Parse(command);
            MessageBox.Show($"Comando: --{arguments.Action}*\r\nObjeto: --{arguments.ObjectType}*\r\nFormato: --{arguments.ObjectFormat}\r\nQuantidade: --{arguments.ObjectAmount}\r\nNome: --{arguments.ObjectName}\r\nNovo nome: --{arguments.ObjectNewName}\r\nOrigem: --{arguments.Origin}\r\nDestino: --{arguments.Destination}\r\nTamanho: --{arguments.SizeModifier} {arguments.SizeAmount} {arguments.SizeUnit}");
            return arguments;
        }


        public static void ExecuteCommand(CommandCriteria arguments)
        {
            string commandType = arguments.Action;
            string objectType = arguments.ObjectType;
            string name = arguments.ObjectName;
            string newName = arguments.ObjectNewName;
            string format = arguments.ObjectFormat;
            string origin = arguments.Origin;
            string destination = arguments.Destination;
            string amount = arguments.ObjectAmount;


            //Adicionar extensão ao nome
            if (name != "")
            {
                var extension = Comandos.WordGetExtensions(format);
                if (extension.Count() != 0)
                {
                    name += "." + extension[0];
                }
            }


            //Retorna os caminhos encontrados conforme os critérios
            List<string> GetPaths(string name, string objectType, string amountIndicator, string folder)
            {
                //Buscar apenas um arquivo
                if (amountIndicator == "") {
                    return SearchPaths(name, objectType == "pasta", pastaRoot: folder, varcontrole: 1).ToList();
                }

                //Retornar todos os caminhos correspondentes
                List<string> paths = new List<string>();

                if (name == "")
                {
                    //Retornar todos os arquivos de determinado formato (pode englobar mais de uma extensão)
                    foreach(string extension in Comandos.WordGetExtensions(format))
                    {
                        List<string> newPaths = SearchPaths("." + extension, objectType == "pasta", pastaRoot: folder, varcontrole: null).ToList();
                        paths.AddRange(newPaths);
                    }
                }
                else
                {
                    //Procura normal usando o nome
                    paths = SearchPaths(name, objectType == "pasta", pastaRoot: folder, varcontrole: null).ToList();
                }

                switch (amountIndicator)
                {
                    case "todos":
                        break;
                    case "metade":
                        int half = (int)Math.Ceiling((decimal)paths.Count / 2);
                        paths = paths.GetRange(0, half);
                        break;
                }

                return paths;
            }


            //Caminho da pasta de origem e de destino
            string destinationPath = "";
            if (!string.IsNullOrEmpty(destination)) destinationPath = SearchPaths(destination, true).FirstOrDefault();
            string originPath = "";
            if (!string.IsNullOrEmpty(origin)) originPath = SearchPaths(origin, true).FirstOrDefault();

            List<string> paths = new List<string>();


            //Realizar comando
            switch (commandType)
            {
                //Criar
                case "criar":
                    if (objectType == "pasta") { CriarPasta(name, destination); }
                    if (objectType == "arquivo") { CriarArquivo(name, destination); }
                    break;

                //Renomear
                case "renomear":
                    if (objectType == "pasta") { RenomearPasta(name, newName, origin); }
                    if (objectType == "arquivo") { RenomearArquivo(name, newName, origin); }
                    break;

                case "excluir":
                    paths = GetPaths(name, objectType, amount, origin);
                    if (objectType == "pasta") { ExcluirPasta(paths); }
                    if (objectType == "arquivo") { ExcluirArquivo(paths); }
                    break;

                
                case "mover":
                    paths = GetPaths(name, objectType, amount, origin);
                    if (objectType == "pasta") { MoverPasta(paths, destinationPath); }
                    if (objectType == "arquivo") { MoverArquivo(paths, destinationPath); }
                    break;

                case "duplicar":
                    paths = GetPaths(name, objectType, amount, origin);
                    if (objectType == "pasta") { DuplicarPasta(paths, destinationPath); }
                    if (objectType == "arquivo") { DuplicarArquivo(paths, destinationPath); }
                    break;

            }
        }


        //Remover acentos das palavras
        public static string RemoveDiacritics(string text)
        {
            var normalizedStr = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedStr.EnumerateRunes())
            {
                var unicodeCategory = Rune.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        #endregion


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


        public static void OpenFileExplorer(string path, bool forceOpen)
        {
            path = Path.GetDirectoryName(path);

            //Função para abrir o gerenciador
            void OpenNewWindow(string path)
            {
                if (Directory.Exists(path))
                {
                    Process.Start("explorer.exe", path);
                }
            }

            //Abrir nova janela forçadamente
            if (forceOpen)
            {
                OpenNewWindow(path);
                return;
            }


           //Janelas abertas do explorador de arquivos (+ internet explorer)
           ShellWindows shellWindows = new ShellWindows();

           foreach(InternetExplorer window in shellWindows)
           {
                string filename = Path.GetFileNameWithoutExtension(window.FullName).ToLower();

                //Checar se é mesmo o explorador de arquivos, e não o internet explorer
                if (filename == "explorer")
                {
                    string openPath = new Uri(window.LocationURL).LocalPath;

                    //Checar se está aberto no caminho correto
                    if (path == openPath)
                    {
                        //Abrir janela (focalizar)
                        IntPtr windowHandle = (IntPtr)window.HWND;
                        int SW_RESTORE = 9; //Valor da api do windows que significa restaurar a janela (caso minimizada ou em tela cheia)

                        ShowWindow(windowHandle, SW_RESTORE);
                        SetForegroundWindow(windowHandle);

                        return;
                    
                    }
                }
           }


            //Abrir nova janela se não encontrar nenhuma aberta
            OpenNewWindow(path);
        }


        #region BUSCA 

        public static HashSet<string> SearchPaths(string aprocurar, bool comando, long criteriosize = 0, string criterio2 = null, string pastaRoot = "", int? varcontrole = 1, int[] data = null) // busca recursivamente multiplas pastas ou arquivos, retornando o caminho do arquivo ou pasta encontrado, ou uma mensagem de erro se não encontrar nada
        {

            // PRA QUE SERVE CADA PARÂMETRO:

            // aprocurar: o nome do arquivo ou pasta que você quer procurar
            // comando: se for TRUE, ele procura por pastas, se for FALSE, ele procura por arquivos
            // criteriosize: se for diferente de 0, ele procura por arquivos com tamanho próximo ao valor especificado
            // criterio2: é um segundo critério de busca, se for especificado, ele procura por arquivos que contenham esse critério no nome
            // pastaRoot: é a pasta aonde ele vai procurar, se for nulo, ele procura em todas as pastas do computador
            // varcontrole: é um controle de quantas pastas ou arquivos ele vai procurar, se for nulo, ele procura em todas as pastas ou arquivos

            string root = driveverifica(null);
            var visitados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var resultados = new HashSet<string>();
            var ignorar = new List<string> // lista de pastas que serão ignoradas na busca
            {
                "$RECYCLE.BIN",
                "System Volume Information",
                "Recovery",
                "Config.Msi",
                "Windows",
                "Program Files (x86)",
                "Program Files"
            };

            var prioridades = new List<string> // lista de pastas que serão priorizadas na busca
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

            // determina se ele ira usar a quantidade pastas ou arquivos para o controle da função
            int? maxLength = varcontrole;

            if (!string.IsNullOrEmpty(pastaRoot)) {
                pastaRoot = SearchPaths(pastaRoot, true).FirstOrDefault();
                if (string.IsNullOrEmpty(pastaRoot)) return null;
                //Buscar apenas na pasta determinada
                prioridades = new List<string>() { pastaRoot };

                //Quantidade máxima a ser buscada
                if (comando == true)
                {
                    //Pastas
                    maxLength = Directory.GetDirectories(pastaRoot).Length;

                }
                else if (comando == false)
                {
                    //Arquivos
                    maxLength = Directory.GetFiles(pastaRoot).Length;
                }
            }

            //Redefinir quantidade de caminhos a serem buscados
            if (varcontrole != null)
            {
                varcontrole = Math.Min((int)varcontrole, (int)maxLength);
            }
            else
            {
                varcontrole = maxLength;
            }

            //MessageBox.Show(varcontrole.ToString());


            var fila = new Queue<string>();
            foreach (var pasta in prioridades)
            {
                if (Directory.Exists(pasta))
                {
                    fila.Enqueue(pasta);

                }
            }

            while (fila.Count > 0)
            {
                var atual = fila.Dequeue();// remove o primeiro elemento da fila e o retorna
                try
                {
                    switch (comando)
                    {
                        case true:



                            foreach (var caminho in Directory.GetDirectories(atual)) // percorre todas as pastas dentro da pasta atual
                            {
                                string nomePasta = Path.GetFileName(caminho); // pega o nome da pasta atual a partir do caminho completo

                                if (ignorar.Any(ign => nomePasta.Equals(ign, StringComparison.OrdinalIgnoreCase)))
                                    continue;

                                if (visitados.Add(caminho)) // adiciona o caminho atual ao conjunto de visitados se ja nao tiver sido visitado
                                {

                                    //fila.Enqueue(caminho);
                                    InserirNoInicio(fila, caminho); // insere o caminho atual no inicio da fila
                                    FileInfo caminhoinfo = new FileInfo(caminho); // cria um objeto FileInfo a partir do caminho atual para pegar suas informaões

                                    // adicionam o caminho atual ou o caminho completo a lista de resultados
                                    if (data!= null && caminho!=pastaRoot && filtroData(caminho, caminhoinfo, data, pastaRoot))
                                    {
                                        MessageBox.Show(caminhoinfo.Directory.Parent.ToString() + ", aaaaaaaaaaaaaa");

                                        resultados.Add(caminho);
                                        if (varcontrole != null && varcontrole > 0)
                                        {
                                            varcontrole -= 1;
                                        }
                                        
                                    }
                                    else if (criteriosize != 0 && caminho!=pastaRoot) // compara o tamanho do arquivo, ainda tem coisa pra mudar depois
                                    {
                                        long arquivoSize = getFolderSize(caminho, criterio: criteriosize);
                                        //MessageBox.Show(arquivoSize.ToString());
                                        if (filtroSize(arquivoSize, criteriosize, caminho, pastaRoot))
                                        {
                                            MessageBox.Show("deu certo eu acho caminho: " + arquivoSize + " " + criteriosize);
                                            resultados.Add(caminho);

                                            if (varcontrole != null && varcontrole > 0)
                                            {
                                                varcontrole -= 1;
                                            }

                                        }


                                    }
                                    else if (!string.IsNullOrEmpty(pastaRoot) && caminho.Contains(aprocurar, StringComparison.OrdinalIgnoreCase) && caminho.Contains(pastaRoot)) // retorna o caminho atual caso ele contenha o caminho desejado e a pastaroot
                                    {
                                        //MessageBox.Show($"Foram encontrados d {visitados.Count} caminhos de pastas.");
                                        //MessageBox.Show(caminho + " situação 3 " + aprocurar);
                                        resultados.Add(caminho);

                                        if (varcontrole != null && varcontrole > 0)
                                        {
                                            varcontrole -= 1;
                                        }

                                    }
                                    else if (caminho.Contains(aprocurar) && !string.IsNullOrEmpty(aprocurar) && atual.Contains(aprocurar)) // caso o caminho desejado seja o caminho atual, ele o retorna
                                    {
                                        //MessageBox.Show($"Foram encontrados d {visitados.Count} caminhos de pastas.");
                                        //MessageBox.Show(atual + " situação 1 " + aprocurar);
                                        resultados.Add(atual);

                                        if (varcontrole != null && varcontrole > 0)
                                        {
                                            varcontrole -= 1;
                                        }

                                    }
                                    else if (caminho.Contains(aprocurar) && !string.IsNullOrEmpty(aprocurar)) // caso o caminho desejado esteja dentro do caminho atual, ele o retorna
                                    {
                                        //MessageBox.Show($"Foram encontrados d {visitados.Count} caminhos de pastas.");
                                        //MessageBox.Show(caminho + " situação 2 " + aprocurar);
                                        resultados.Add(caminho);

                                        if (varcontrole != null && varcontrole > 0)
                                        {
                                            varcontrole -= 1;
                                        }

                                    }



                                    if (varcontrole != null && varcontrole <= 0) // se todas as pastas dentro da pastaRoot forem visitadas, elas são retornadas
                                    {
                                      //  MessageBox.Show("A quantidade de pastas encontradas foi: " + resultados.Count);

                                        return resultados;
                                    }

                                }
                            }

                            break;
                        case false:

                            foreach (var arquivo in Directory.GetFiles(atual)) // percorre todos os arquivos dentro da pasta atual
                            {

                                FileInfo caminhoinfo = new FileInfo(arquivo); // mesma coisa do bglh de pasta
                                if (visitados.Add(arquivo)) // adiciona o caminho atual ao conjunto de visitados se ja nao tiver sido visitado
                                {
                                    if (criteriosize != 0 && filtroSize(caminhoinfo.Length,criteriosize,arquivo,pastaRoot)) // compara o tamanho do arquivo, ainda tem coisa pra mudar depois
                                    {
                                        MessageBox.Show("deu erradopracacete eu acho caminho: " + caminhoinfo.Length + " " + criteriosize);
                                        resultados.Add(arquivo);

                                        if (varcontrole != null && varcontrole > 0)
                                        {
                                            varcontrole -= 1;
                                        }
                                        
                                    }
                                    else if (data!=null && filtroData(arquivo, caminhoinfo, data, pastaRoot))
                                    {
                                        
                                        resultados.Add(arquivo);
                                        if (varcontrole != null && varcontrole > 0)
                                        {
                                            varcontrole -= 1;
                                        }
                                        
                                    }

                                    else if (!string.IsNullOrEmpty(pastaRoot) && arquivo.Contains(aprocurar, StringComparison.OrdinalIgnoreCase) && arquivo.Contains(pastaRoot, StringComparison.OrdinalIgnoreCase)) // retorna o arquivo desejado que esta dentro da pasta root
                                    {
                                        //MessageBox.Show($"Arquivo encontrado pr: {arquivo}");
                                        resultados.Add(arquivo);

                                        if (varcontrole != null && varcontrole > 0)
                                        {
                                            varcontrole -= 1;
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(aprocurar) && arquivo.Contains(aprocurar, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(pastaRoot)) // retorna o arquivo desejado que esta dentro da pasta atual
                                    {
                                        //MessageBox.Show($"Arquivo encontrado nro: {arquivo}");
                                        resultados.Add(arquivo);

                                        if (varcontrole != null && varcontrole > 0)
                                        {
                                            varcontrole -= 1;
                                        }


                                    }
                                }
                                    if (varcontrole != null && varcontrole <= 0)
                                    {
                                        //MessageBox.Show("A quantidade de pastas encontradas foi: " + resultados.Count);
                                        return resultados;
                                    }
                            }


                            foreach (var caminho in Directory.GetDirectories(atual)) // percorre todas as pastas dentro da pasta atual
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
            if (resultados != null)
            {
                MessageBox.Show("bolete");
                foreach(var x in resultados)
                {
                    MessageBox.Show(x);
                }
                return resultados;
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

        public static bool filtroData(string arquivo, FileInfo fileinfo, int[] datas, string pastaroot)
        {
            int dia = datas[0], mes = datas[1], ano = datas[2];

            if (dia != 0 && mes != 0 && ano != 0 && fileinfo.LastWriteTime.Day == dia && fileinfo.LastWriteTime.Month == mes && fileinfo.LastWriteTime.Year == ano && arquivo.Contains(pastaroot))
            {
                MessageBox.Show(fileinfo.LastWriteTime.Day + ", "+ fileinfo.LastWriteTime.Month + ", "+ fileinfo.LastWriteTime.Year);

                return true;
            }
            else if (dia == 0 )
            {
                if (mes != 0 && ano != 0 && fileinfo.LastWriteTime.Month == mes && fileinfo.LastWriteTime.Year == ano && arquivo.Contains(pastaroot))
                {
                    MessageBox.Show("2");
                    return true;
                }
                else if ( mes == 0 && ano != 0 && fileinfo.LastWriteTime.Year == ano && arquivo.Contains(pastaroot))
                {
                    MessageBox.Show("3");
                    return true;
                }
            }
            return false;
        }


        public static bool filtroSize(long arquivosize, long criteriosize, string arquivo, string pastaroot)
        {
            //MessageBox.Show(arquivosize.ToString());

            if ((arquivosize >= criteriosize * 0.8 && arquivosize <= criteriosize * 1.2))
            {
                //MessageBox.Show(criteriosize.ToString() + ", " + arquivosize);
                return true;
                
            }
            //MessageBox.Show("merda");
            return false;
        }

        public static long getFolderSize(string caminho,long criterio=0, long control=0)
        {
            long tamanhoTotal = 0;

            try
            {
                
                tamanhoTotal += Directory.GetFiles(caminho).Sum(arquivo => new FileInfo(arquivo).Length);

                control += tamanhoTotal;

                if( control== 1.5 * criterio && criterio!=0 || tamanhoTotal>= 1.5*criterio)
                {
                    return control;
                }

                foreach (var subPasta in Directory.GetDirectories(caminho))
                {
                    tamanhoTotal += getFolderSize(subPasta, control:control);
                }
            }
            catch (Exception ex)
            {
                
            }

            return tamanhoTotal;
        }
        #endregion
          

        public static void MoverUnsArquivos(string criterio, string Pastaroot1,string destino, string crit2="")
        {
            // os criterios são os criterios de busca, a var pastaAbuscar é a pasta aonde ele vai procurar os multplos arquivos a serem buscados, a var destino é aonde colocar esses arquivos 

            var caminhos = new HashSet<string>();
             caminhos = SearchPaths(criterio,false,pastaRoot:Pastaroot1, criterio2:crit2);



            foreach (var item in caminhos)
            {
                
               // MoverArquivo(item, destino);

            }



            return;
            
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
            //MessageBox.Show(caminhoPadrao);
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
                if (string.IsNullOrEmpty(path)) 
                {
                    path = Path.Combine(Global.DefaultFolder, nome);
                }
                else
                {
                    path = Path.Combine(SearchPaths(path, true).FirstOrDefault(), nome);

                }


                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Console.WriteLine("Pasta" + nome + "Criada com sucesso");
                    OpenFileExplorer(path, false);
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

        public static void CriarArquivo(string nome, string path) // cria arquivo
        {

            try
            {
                //Redefinir nome caso já exista um igual
                string nomefinal = CriarNome(nome, path);
                string destination = Global.DefaultFolder;

                //Definir pasta informada como destino
                if (!string.IsNullOrEmpty(path))
                {
                    destination = SearchPaths(path, true).FirstOrDefault();
                }

                //Caminho a ser criado
                string newPath = Path.Combine(destination, nomefinal);

                using (FileStream fs = File.Create(newPath))
                Console.WriteLine("Arquivo " + nomefinal + " criado com sucesso");

                OpenFileExplorer(newPath, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar arquivo: " + ex.Message);
            }
        }


        public static void ExcluirArquivo(List<string> paths) //exclui arquivo
        {


           try { 

            //string path = SearchPaths(nome, false).FirstOrDefault();

            foreach (string path in paths) {

                if (File.Exists(path))
                {
                    File.Delete(path);
                    //Console.WriteLine("Arquivo" + nome + "Excluido com sucesso");
                }
                else
                {

                    MessageBox.Show("esse arquivo não existe");
                }

            }
           }
           catch (Exception ex){
                MessageBox.Show("Não foi possivel excluir estes arquivos: "+ex.Message);
           }
            


        }

        public static void ExcluirPasta(List<string> paths) // exclui pasta
        {
            //string path = SearchPaths(nome, true).FirstOrDefault();

            try
            {

                foreach (string path in paths)
                {

                    if (Directory.Exists(path))
                    {

                        foreach (string arquivo in Directory.GetFiles(path))
                        {
                            File.Delete(arquivo);
                        }
                        foreach (string subPasta in Directory.GetDirectories(path))
                        {
                            var subpastas = new List<string>();
                            subpastas.Add(subPasta);
                            ExcluirPasta(subpastas);

                        }
                        Directory.Delete(path);
                        //Console.WriteLine("Pasta" + nome + "Excluida com sucesso");
                    }
                    else
                    {
                        MessageBox.Show("essa pasta não existe");

                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Não foi possivel excluir estas pastas: " + ex.Message);
            }

        }


        public static void RenomearArquivo(string nome, string novoNome, string pastaOrigem) // renomear arquivo(erro de logica, falta implementar o bagulho de procurar o arquivo o mesmo serve para o bagulho de excluir)
        {
            string path = SearchPaths(nome, false, pastaRoot:pastaOrigem).FirstOrDefault();
            if (path == null) return;

            string novoPath = Path.Combine(Path.GetDirectoryName(path), novoNome);

            if (File.Exists(path))
            {
                File.Move(path, novoPath);
                Console.WriteLine("Arquivo " + nome + " renomeado para " + novoNome);
            }
            else
            {
                MessageBox.Show("esse arquivo não existe");
            }
        }

        public static void RenomearPasta(string nome, string novoNome, string pastaOrigem) // renomear pasta(mesmo erro de logica do renomear arquivo)
        {

            string path = SearchPaths(nome, true, pastaRoot:pastaOrigem).FirstOrDefault();
            //MessageBox.Show(path);

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


        public static void MoverPasta(List<string> Pathnomes, string destino)
        {
            try
            {
                // destino = SearchPaths(destino, true) + @"\" + nome;
                //nome = SearchPaths(nome, true).FirstOrDefault();
                //MessageBox.Show(destino);

                foreach (string nome in Pathnomes)
                {
                    string destinoNovo = Path.Combine(destino, Path.GetDirectoryName(nome));

                    if (Directory.Exists(destino))
                    {
                        MessageBox.Show("Já existe uma pasta com esse nome no destino.");
                        return;
                    }

                    Directory.Move(nome, destino);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao mover pasta: " + ex.Message);
            }
        }

        public static void MoverArquivo(List<string> Pathnomes, string destino) 
        {
            //nome = SearchPaths(nome, false).FirstOrDefault();
            // SearchPaths(destino, true) + @"\" + nome + "." + ext;
            foreach (string nome in Pathnomes)
            {
                try
                {
                    MessageBox.Show("Nome do arquivo: " + Path.GetFileName(nome));
                    string destinoNovo = Path.Combine(destino,Path.GetFileName(nome));
                    if (File.Exists(destino))
                    {
                        MessageBox.Show("Já existe um arquivo com esse nome no destino.");
                    }
                    File.Move(nome, destinoNovo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao mover arquivo: " + ex.Message);
                }
            }

            return;
        }


        public static void DuplicarPasta(List<string> paths, string destino)
        {
            try
            {
                //nome = SearchPaths(nome, true).FirstOrDefault();

                foreach (string nome in paths)
                {
                    string destinoNovo;
                    if (string.IsNullOrEmpty(destino))
                    {
                         destinoNovo = Path.Combine(Global.DefaultFolder, Path.GetFileName(nome));
                    }
                    else
                    {
                         destinoNovo = Path.Combine(destino, Path.GetFileName(nome));

                    }


                    //MessageBox.Show(destinoNovo + " negocio infernal que pode estar dando erro");
                    if (Directory.Exists(destinoNovo))
                    {
                        MessageBox.Show("Já existe uma pasta com esse nome no destino.");
                        return;
                    }

                    Directory.CreateDirectory(destinoNovo);
                    foreach (string arquivo in Directory.GetFiles(nome))
                    {


                        string nomeArquivo = Path.GetFileName(arquivo);
                        string destinoArquivo = Path.Combine(destinoNovo, nomeArquivo);
                        File.Copy(arquivo, destinoArquivo, true);

                    }
                    foreach (string subPasta in Directory.GetDirectories(nome))
                    {
                        string nomeSubPasta = Path.GetFileName(subPasta);
                        string destinoSubPasta = Path.Combine(destinoNovo, nomeSubPasta);
                        //MessageBox.Show(destinoSubPasta);
                        //MessageBox.Show(nomeSubPasta);
                        var subpastas = new List<string>();
                        subpastas.Add(subPasta);
                        DuplicarPasta(subpastas, destinoNovo);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao duplicar pasta: " + ex.Message);
            }
        }

        public static void DuplicarArquivo(List<string> Pathnomes, string destination)
        {

            try
            {
                string newDestination = "";

                foreach (string nome in Pathnomes)
                {
                    //Definir destino como a mesma pasta caso não seja informado
                    if (string.IsNullOrEmpty(destination))
                    {
                        destination = Path.GetDirectoryName(nome);
                    }

                    string fileName = CriarNome(Path.GetFileName(nome), destination);
                    newDestination = Path.Combine(destination, fileName);

                    //if (File.Exists(destinoNovo))
                    //{
                    //    MessageBox.Show("Já existe um arquivo com esse nome no destino.");
                    //}
                    MessageBox.Show("pasta: " + destination);
                    MessageBox.Show("destino novo: " + newDestination);

                    File.Copy(nome, newDestination, false);
                }

                OpenFileExplorer(newDestination, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao duplicar arquivo: " + ex.Message);
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
           string arquivo = SearchPaths(nome,false).FirstOrDefault();

            var psi = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = arquivo,
            };
            Process.Start(psi);
        }


        public static string CriarNome(string nome, string destination)
        {
            int i = 1;
            string newName = nome;

            MessageBox.Show(Path.Combine(destination, newName));

            while (File.Exists(Path.Combine(destination, newName)))
            {
                newName = $"{nome}({i})";
                i++;
            }

            MessageBox.Show("Nome: " + newName);
            return newName;
        }


    }

}
