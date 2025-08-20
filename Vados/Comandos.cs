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

namespace Vados
{
    internal class Comandos
    {
        #region TEXTO PARA COMANDO

        #region DICIONÁRIOS / LISTAS

        //Funções das palavras
        public enum WordType
        {
            Command,
            Object,
            Connector,  //Não será mais necessário
            Value,
        };

        static Dictionary<string, WordType> wordTypes = new Dictionary<string, WordType>(StringComparer.OrdinalIgnoreCase)
        {
            //Comandos
            { "criar", WordType.Command },
            { "renomear", WordType.Command },
            { "excluir", WordType.Command },
            { "mover", WordType.Command },
            { "duplicar", WordType.Command },
            //Objetos
            { "pasta", WordType.Object },
            { "arquivo", WordType.Object },
        };

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
            { "faça", "criar" },
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
        };

        static Dictionary<string, List<string>> wordExtensions = new Dictionary<string, List<string>>()
        {
            //Criar
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

        static List<string> wordGroups = new List<string>()
        {
            "com o nome",
            "com nome",
            "de nome",
            "com o titulo",
            "com titulo",
            "de titulo",
            "mudar o nome",
            "mude o nome",
            "trocar o nome",
            "troque o nome",
            "alterar o nome",
            "altere o nome",
            "dentro da",
            "power point"
        };


        public static WordType WordGetType(string word)
        {
            if (wordTypes.TryGetValue(word, out WordType result))
            {
                return result;
            }

            return WordType.Value;
        }


        public static string CommandGetSynonym(string command)
        {
            if (commandSynonyms.TryGetValue(command, out string synonym))
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


        //Retorna a ordem de palavras necessárias para realizar o comando
        public static List<List<object>> CommandGetWordOrder(string command)
        {
            //Lista de palavras para indicar o nome do arquivo/pasta
            List<string> namingConnectors = new List<string>() { "chamada", "chamado", "nomeado", "nomeada", "de nome", "com o nome", "com nome", "de titulo", "com o titulo", "com titulo", "denominado", "denominada", "intitulado", "intitulada", "" };
            //Indica a pasta de destino
            List<string> dentro = new List<string>() { "na", "dentro da" };
            //Palavras específicas
            List<string> pasta = new List<string>() { "pasta" };
            List<string> para = new List<string>() { "para" };
            List<string> de = new List<string>() { "de" };

            switch (command)
            {
                case "criar":
                    return new List<List<object>>() {
                        /* ---- SITUAÇÕES ----
                        0.0 -> sem extensão, sem pasta de destino
                        0.1 -> sem extensão, pasta de destino
                        1.0 -> extensão, sem pasta de destino
                        1.0 -> extensão, pasta de destino
                        */

                        new List<object>() { 1.1, WordType.Object, de, WordType.Value, namingConnectors, WordType.Value, dentro, pasta, namingConnectors, WordType.Value },
                        new List<object>() { 1.0, WordType.Object, de, WordType.Value, namingConnectors, WordType.Value },
                        new List<object>() { 0.1, WordType.Object, namingConnectors, WordType.Value, dentro, pasta, namingConnectors, WordType.Value },
                        new List<object>() { 0.0, WordType.Object, namingConnectors, WordType.Value },
                    };

                case "renomear":
                    return new List<List<object>>()
                    {
                        new List<object>() { 0.0, WordType.Object, namingConnectors, WordType.Value, para, WordType.Value },
                    };

                case "excluir":
                    return new List<List<object>>()
                    {
                        new List<object>() { 0.0, WordType.Object, namingConnectors, WordType.Value },
                    };

                case "mover":
                    return new List<List<object>>()
                    {
                        new List<object>() { 0.0, WordType.Object, namingConnectors, WordType.Value, new List<string>() { "para", "dentro da" }, pasta, namingConnectors, WordType.Value },
                    };


                //Lista vazia se não identificar o comando
                default:
                    return new List<List<object>>();
            }
        }


        public static void ExecuteCommand(List<string> arguments)
        {
            double situation = Convert.ToDouble(arguments[0]);
            int mainSituation = (int)(Math.Floor(situation));
            int subSituation = (int)((situation - mainSituation) * 10);
            MessageBox.Show(mainSituation.ToString() + "." + subSituation.ToString());

            string obj = arguments[2];
            string name = arguments[3];
            List<string> paths;

            switch (arguments[1])
            {
                case "criar":
                    if (obj == "pasta") { CriarPasta(name, ""); }
                    if (obj == "arquivo")
                    {
                        string fileName = name;
                        string destinyFolder = "";

                        //Quando o formato do arquivo está por extenso (ex: "arquivo de texto")
                        if (mainSituation == 1)
                        {
                            fileName = arguments[4];

                            //Definir extensão
                            string extensionWord = arguments[3];
                            List<string> extensions = WordGetExtensions(extensionWord);
                            if (extensions.Count != 0)
                            {
                                fileName += "." + WordGetExtensions(extensionWord)[0];
                            }
                        }

                        //Pasta de destino
                        if (subSituation == 1)
                        {
                            destinyFolder = arguments.Last();
                        }

                        MessageBox.Show(destinyFolder);
                        CriarArquivo(fileName, destinyFolder);
                    }
                    break;


                case "renomear":
                    string oldName = arguments[3];
                    string newName = arguments[4];

                    if (obj == "pasta") { RenomearPasta(oldName, newName); }
                    if (obj == "arquivo") { RenomearArquivo(oldName, newName); }
                    break;

                case "excluir":
                    paths = SearchPaths(name, obj == "pasta").ToList();

                    if (obj == "pasta") { ExcluirPasta(paths); }
                    if (obj == "arquivo") { ExcluirArquivo(paths); }
                    break;

                
                case "mover":
                    string destiny = arguments[4];
                    string destinyPath = SearchPaths(destiny, true).FirstOrDefault();
                    paths = SearchPaths(name, obj == "pasta").ToList();

                    if (obj == "pasta") { MoverPasta(paths, destiny); }
                    if (obj == "arquivo") { MoverArquivo(paths, destinyPath); }
                    break;
                
            }
        }


        public static List<string> ValidateCommand(List<string> words)
        {
            string command = "";
            List<string> arguments = new List<string>();
            List<List<object>> orderList = null;
            int startIndex = 0;

            //Definir comando
            for (int i = 0; i < words.Count; i++)
            {
                string word = words[i].ToLower();
                string possibleCommand = CommandGetSynonym(word);
                WordType type = WordGetType(possibleCommand);

                //Definir comando
                if (type == WordType.Command)
                {
                    command = possibleCommand;
                    orderList = CommandGetWordOrder(command);
                    startIndex = i + 1;
                    break;
                }
            }

            //Retornar nulo se não for nenhum comando
            if (command == "") return null;


            //Checar todas as ordens de palavras aceitas pelo comando
            for (int i = 0; i < orderList.Count; i++)
            {
                List<object> typeOrder = orderList[i];
                int typeIndex = 1;  //Pular primeiro elemento, que indica a situação do comando

                //Adicionar situação e commando nos argumentos
                arguments.Clear();
                double commandSituation = (double)(typeOrder[0]);
                arguments.Add(commandSituation.ToString());
                arguments.Add(command);

                int j = startIndex;

                //Checar se o comando possui todas as palavras necessárias
                for (j = startIndex; j < words.Count; j++)
                {
                    if (typeIndex >= typeOrder.Count) break;    //Parar se tiver mais palavras que o esperado

                    string word = words[j].ToLower();
                    var expected = typeOrder[typeIndex];

                    //Se for tipo de palavra
                    if (expected is WordType)
                    {
                        WordType actualType = WordGetType(word);

                        if (actualType == (WordType)expected)
                        {
                            //Definir argumentos pro comando
                            if (actualType == WordType.Object || actualType == WordType.Value)
                            {
                                arguments.Add(word);
                            }

                            typeIndex += 1;
                            continue;
                        }

                    }

                    //Se for palavra específica aceita
                    if (expected is List<string>)
                    {
                        List<string> list = (List<string>)expected;
                        bool containsWord = list.Contains(word);

                        //Seguir para a próxima palavra se a atual estiver na lista
                        if (containsWord)
                        {
                            typeIndex += 1;
                        }

                        //Checar a mesma palavra mas com o próximo tipo esperado (caso não esteja na lista, que não é obrigatória)
                        if (!containsWord && list.Contains(""))
                        {
                            j -= 1;
                            typeIndex += 1;
                        }
                    }
                }


                //Retornar argumentos se o comando estiver correto
                if (typeIndex == typeOrder.Count && j == words.Count)
                {
                    return arguments;
                }
            }

            return null;
        }


        public static List<string> IdentifyWordGroups(List<string> words)
        {
            List<string> newWords = new List<string>();

            //Para cada palavra de determinada lista
            for (int i = 0; i < words.Count(); i++)
            {
                string temporaryWord = "";
                int wordCount = 0;

                for (int j = 0; j < wordGroups.Count(); j++)
                {
                    string actualWordGroup = wordGroups[j];
                    List<string> groupSeparateWords = actualWordGroup.Split(" ").ToList();
                    wordCount = groupSeparateWords.Count;

                    //Checar se formam um grupo conhecido
                    for (var k = 0; k < groupSeparateWords.Count; k++)
                    {
                        string wordToCheck = words[i + k];

                        //Parar de juntar as palavras se alguma não fizer parte do grupo
                        if (groupSeparateWords[k] != wordToCheck)
                        {
                            temporaryWord = "";
                            break;
                        }

                        //Juntar palavras
                        temporaryWord += wordToCheck;

                        if (k != groupSeparateWords.Count - 1)
                        {
                            //Adicionar espaço entre as palavras
                            temporaryWord += " ";
                        }
                    }

                    //Parar de checar outros grupos de palavras se já encontrar um
                    if (temporaryWord != "")
                    {
                        break;
                    }
                }

                //Adicionar grupo à nova lista (como uma única palavra)
                if (temporaryWord != "")
                {
                    newWords.Add(temporaryWord);
                    i += wordCount - 1;
                }
                //Adicionar palavra única à nova lista caso não seja um grupo
                else
                {
                    newWords.Add(words[i]);
                }
            }

            return newWords;
        }


        public static List<string> SeparateWords(string str)
        {
            var words = new List<string>();
            if (str == "") return words;

            char[] separators = { ' ', ',' };
            char[] charList = str.ToCharArray();
            char namer = '\0';

            string currentWord = "";

            for (var i = 0; i < charList.Length; i++)
            {
                char c = charList[i];
                bool breakWord = false;

                //Checar se o caractere é um nomeador   (junta palavras que estão entre eles)
                if (c == '\"' || c == '\'')
                {
                    //Fechar junção
                    if (c == namer)
                    {
                        namer = '\0';
                        continue;
                    }

                    //Iniciar junção
                    namer = c;
                    continue;
                }

                //Checar se o caractere é um separador
                if (namer == '\0')
                {
                    for (var j = 0; j < separators.Length; j++)
                    {
                        char s = separators[j];

                        if (c == s)
                        {
                            breakWord = true;
                            break;
                        }
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
            if (currentWord != "")
            {
                words.Add(currentWord);
            }

            return words;
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

        //public static HashSet<string> funcaoteste(string aprocurar, bool comando, long criteriosize = 0, string criterio2 = null, string pastaRoot = null, int? varcontrole = 1) // busca recursivamente multiplas pastas ou arquivos, retornando o caminho do arquivo ou pasta encontrado, ou uma mensagem de erro se não encontrar nada
        //{

        //    // PRA QUE SERVE CADA PARÂMETRO:

        //    // aprocurar: o nome do arquivo ou pasta que você quer procurar
        //    // comando: se for TRUE, ele procura por pastas, se for FALSE, ele procura por arquivos
        //    // criteriosize: se for diferente de 0, ele procura por arquivos com tamanho próximo ao valor especificado
        //    // criterio2: é um segundo critério de busca, se for especificado, ele procura por arquivos que contenham esse critério no nome
        //    // pastaRoot: é a pasta aonde ele vai procurar, se for nulo, ele procura em todas as pastas do computador
        //    // varcontrole: é um controle de quantas pastas ou arquivos ele vai procurar, se for nulo, ele procura em todas as pastas ou arquivos

        //    string root = driveverifica(null);
        //    var visitados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        //    var resultados = new HashSet<string>();
        //    var ignorar = new List<string> // lista de pastas que serão ignoradas na busca
        //    {
        //        "$RECYCLE.BIN",
        //        "System Volume Information",
        //        "Recovery",
        //        "Config.Msi",
        //        "Windows",
        //        "Program Files (x86)",
        //        "Program Files"
        //    };

        //    var prioridades = new List<string> // lista de pastas que serão priorizadas na busca
        //    {
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\AppData\Roaming\Vados"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\Desktop"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\Contacts"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\Documents"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\Downloads"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\Favorites"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\Pictures"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\Saved Games"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\Links"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\Music"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\3D Objects"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\OneDrive"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\Searches"),
        //        Path.Combine(root, @"Users\"+Environment.UserName+@"\Videos"),
        //        //Path.Combine(root, @"Users\"+Environment.UserName+@""),

        //        Path.Combine(root),
        //    };

        //    // determina se ele ira usar a quantidade pastas ou arquivos para o controle da função
        //    if (!string.IsNullOrEmpty(pastaRoot) && comando == true)
        //    {
        //        pastaRoot = funcaoteste(pastaRoot, true).FirstOrDefault();
        //        varcontrole = null;//Directory.GetDirectories(pastaRoot).Length;
        //        MessageBox.Show(aprocurar);
        //        prioridades.Add(pastaRoot);

        //    }
        //    else if (!string.IsNullOrEmpty(pastaRoot) && comando == false)
        //    {
        //        MessageBox.Show("antes pasta "+pastaRoot+" aprocurar: "+aprocurar);
        //        pastaRoot = funcaoteste(pastaRoot, true).FirstOrDefault();
        //        varcontrole = null;//Directory.GetFiles(pastaRoot).Length;
        //        MessageBox.Show("de pasta " + pastaRoot + " aprocurar: " + aprocurar);
        //        prioridades.Add(pastaRoot);
        //    }

        //    var fila = new Queue<string>();
        //    foreach (var pasta in prioridades)
        //    {
        //        if (Directory.Exists(pasta))
        //        {
        //            fila.Enqueue(pasta);

        //        }
        //    }

        //    while (fila.Count > 0)
        //    {
        //        var atual = fila.Dequeue();// remove o primeiro elemento da fila e o retorna
        //        try
        //        {
        //            switch (comando)
        //            {
        //                case true:



        //                    foreach (var caminho in Directory.GetDirectories(atual)) // percorre todas as pastas dentro da pasta atual
        //                    {
        //                        string nomePasta = Path.GetFileName(caminho); // pega o nome da pasta atual a partir do caminho completo

        //                        if (ignorar.Any(ign => nomePasta.Equals(ign, StringComparison.OrdinalIgnoreCase)))
        //                            continue;

        //                        if (visitados.Add(caminho)) // adiciona o caminho atual ao conjunto de visitados se ja nao tiver sido visitado
        //                        {

        //                            //fila.Enqueue(caminho);
        //                            InserirNoInicio(fila, caminho); // insere o caminho atual no inicio da fila
        //                            FileInfo caminhoinfo = new FileInfo(caminho); // cria um objeto FileInfo a partir do caminho atual para pegar suas informaões

        //                            // adicionam o caminho atual ou o caminho completo a lista de resultados

        //                            if (!string.IsNullOrEmpty(pastaRoot) && caminho.Contains(aprocurar, StringComparison.OrdinalIgnoreCase) && caminho.Contains(pastaRoot)) // retorna o caminho atual caso ele contenha o caminho desejado e a pastaroot
        //                            {
        //                                MessageBox.Show($"Foram encontrados d {visitados.Count} caminhos de pastas.");
        //                                MessageBox.Show(caminho + " situação 3 " + aprocurar);
        //                                resultados.Add(caminho);

        //                                if (varcontrole != null && varcontrole > 0)
        //                                {
        //                                    varcontrole -= 1;
        //                                }

        //                            }else if (caminho.Contains(aprocurar) && atual.Contains(aprocurar)) // caso o caminho desejado seja o caminho atual, ele o retorna
        //                            {
        //                                MessageBox.Show($"Foram encontrados d {visitados.Count} caminhos de pastas.");
        //                                MessageBox.Show(atual + " situação 1 " + aprocurar);
        //                                resultados.Add(atual);

        //                                if (varcontrole != null && varcontrole > 0)
        //                                {
        //                                    varcontrole -= 1;
        //                                }

        //                            }
        //                            else if (caminho.Contains(aprocurar)) // caso o caminho desejado esteja dentro do caminho atual, ele o retorna
        //                            {
        //                                MessageBox.Show($"Foram encontrados d {visitados.Count} caminhos de pastas.");
        //                                MessageBox.Show(caminho + " situação 2 " + aprocurar);
        //                                resultados.Add(caminho);

        //                                if (varcontrole != null && varcontrole > 0)
        //                                {
        //                                    varcontrole -= 1;
        //                                }

        //                            }



        //                            if (varcontrole != null && varcontrole <= 0) // se todas as pastas dentro da pastaRoot forem visitadas, elas são retornadas
        //                            {
        //                                MessageBox.Show("A quantidade de pastas encontradas foi: " + resultados.Count);

        //                                return resultados;
        //                            }

        //                        }
        //                    }

        //                    break;
        //                case false:



        //                    foreach (var arquivo in Directory.GetFiles(atual)) // percorre todos os arquivos dentro da pasta atual
        //                    {

        //                        FileInfo caminhoinfo = new FileInfo(arquivo); // mesma coisa do bglh de pasta
        //                        if (visitados.Add(arquivo)) // adiciona o caminho atual ao conjunto de visitados se ja nao tiver sido visitado
        //                        {
        //                            if (criteriosize != 0) // compara o tamanho do arquivo, ainda tem coisa pra mudar depois
        //                            {
        //                                if (caminhoinfo.Length >= criteriosize * 0.8 && caminhoinfo.Length <= criteriosize * 1.2)
        //                                {
        //                                    MessageBox.Show("deu certo eu acho caminho: " + caminhoinfo.Length + " " + criteriosize);
        //                                    resultados.Add(arquivo);

        //                                    if (varcontrole != null && varcontrole > 0)
        //                                    {
        //                                        varcontrole -= 1;
        //                                    }

        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (!string.IsNullOrEmpty(pastaRoot) && arquivo.Contains(aprocurar, StringComparison.OrdinalIgnoreCase) && arquivo.Contains(pastaRoot)) // retorna o arquivo desejado que esta dentro da pasta root
        //                                {
        //                                    MessageBox.Show($"Arquivo encontrado pr: {arquivo}");
        //                                    resultados.Add(arquivo);

        //                                    if (varcontrole != null && varcontrole > 0)
        //                                    {
        //                                        varcontrole -= 1;
        //                                    }


        //                                }
        //                                else if (arquivo.Contains(aprocurar, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(pastaRoot)) // retorna o arquivo desejado que esta dentro da pasta atual
        //                                {
        //                                    MessageBox.Show($"Arquivo encontrado nro: {arquivo}");
        //                                    resultados.Add(arquivo);

        //                                    if (varcontrole != null && varcontrole > 0)
        //                                    {
        //                                        varcontrole -= 1;
        //                                    }


        //                                }
        //                            }
        //                            if (varcontrole != null && varcontrole <= 0)
        //                            {
        //                                MessageBox.Show("A quantidade de pastas encontradas foi: " + resultados.Count);
        //                                return resultados;
        //                            }

        //                        }
        //                    }


        //                    foreach (var caminho in Directory.GetDirectories(atual)) // percorre todas as pastas dentro da pasta atual
        //                    {
        //                        string nomePasta = Path.GetFileName(caminho);
        //                        if (ignorar.Any(ign => nomePasta.Equals(ign, StringComparison.OrdinalIgnoreCase)))
        //                            continue;

        //                        //fila.Enqueue(caminho);


        //                        if (visitados.Add(caminho))
        //                        {
        //                            Comandos.InserirNoInicio(fila, caminho);
        //                        }

        //                    }

        //                    break;
        //            }




        //        }

        //        catch (Exception)
        //        {

        //        }
        //    }
        //    if (resultados != null)
        //    {
        //        MessageBox.Show("bolete");
        //        return resultados;
        //    }
        //    MessageBox.Show("Nenhum arquivo encontrado com o nome especificado.");
        //    return null;
        //}


        #endregion


        #region busca 
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
            if (!string.IsNullOrEmpty(pastaRoot) && comando == true)
            {
                pastaRoot = SearchPaths(pastaRoot, true).FirstOrDefault();
                varcontrole = Directory.GetDirectories(pastaRoot).Length;
               
                prioridades.Add(pastaRoot);

            }
            else if (!string.IsNullOrEmpty(pastaRoot) && comando == false)
            {
                
                pastaRoot = SearchPaths(pastaRoot, true).FirstOrDefault();
                varcontrole = Directory.GetFiles(pastaRoot).Length;
               
                prioridades.Add(pastaRoot);
            }

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
                                    if (data!= null && caminho!=pastaRoot)
                                    {
                                       
                                        if (filtroData(caminho, caminhoinfo, data, pastaRoot))
                                        {
                                            MessageBox.Show(caminhoinfo.Directory.Parent.ToString() + ", aaaaaaaaaaaaaa");

                                            resultados.Add(caminho);
                                            if (varcontrole != null && varcontrole > 0)
                                            {
                                                varcontrole -= 1;
                                            }
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(pastaRoot) && !string.IsNullOrEmpty(aprocurar) && caminho.Contains(aprocurar, StringComparison.OrdinalIgnoreCase) && caminho.Contains(pastaRoot)) // retorna o caminho atual caso ele contenha o caminho desejado e a pastaroot
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
                                    if (criteriosize != 0) // compara o tamanho do arquivo, ainda tem coisa pra mudar depois
                                    {
                                        if (caminhoinfo.Length >= criteriosize * 0.8 && caminhoinfo.Length <= criteriosize * 1.2)
                                        {
                                            //MessageBox.Show("deu certo eu acho caminho: " + caminhoinfo.Length + " " + criteriosize);
                                            resultados.Add(arquivo);

                                            if (varcontrole != null && varcontrole > 0)
                                            {
                                                varcontrole -= 1;
                                            }

                                        }
                                    }
                                    else if (data!=null)
                                    {
                                        if (filtroData(arquivo, caminhoinfo, data, pastaRoot))
                                        {
                                            resultados.Add(arquivo);
                                            if (varcontrole != null && varcontrole > 0)
                                            {
                                                varcontrole -= 1;
                                            }
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
                                    else if (arquivo.Contains(aprocurar, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(pastaRoot)) // retorna o arquivo desejado que esta dentro da pasta atual
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
                    path = Path.Combine(Global.DefaultFolder , nome);
                }
                else
                {
                    path = Path.Combine(SearchPaths(path, true).FirstOrDefault(), nome);

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

        public static void CriarArquivo(string nome, string path) // cria arquivo
        {

            try
            {
                //Criar na pasta padrão
                if (path == "")
                {
                    path = Path.Combine(Global.DefaultFolder, nome);
                }
                //Criar na pasta especificada
                else
                {
                    path = Path.Combine( SearchPaths(path, true).FirstOrDefault(), nome);
                    //MessageBox.Show(path);
                }

                string nomefinal = CriarNome(nome, path);
                string pathfinal = Path.Combine(Path.GetDirectoryName(path), nomefinal);

                using (FileStream fs = File.Create(pathfinal))
                Console.WriteLine("Arquivo " + nomefinal + " criado com sucesso");
                AbrirGerenciador(pathfinal);
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


        public static void RenomearArquivo(string nome, string novoNome) // renomear arquivo(erro de logica, falta implementar o bagulho de procurar o arquivo o mesmo serve para o bagulho de excluir)
        {
            string path = SearchPaths(nome, false).FirstOrDefault();
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

        public static void RenomearPasta(string nome, string novoNome) // renomear pasta(mesmo erro de logica do renomear arquivo)
        {

            string path = SearchPaths(nome, true).FirstOrDefault();
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
            try
            {
                //nome = SearchPaths(nome, false).FirstOrDefault();
                // SearchPaths(destino, true) + @"\" + nome + "." + ext;
                foreach (string nome in Pathnomes)
                {
                    MessageBox.Show("Nome do arquivo: " + Path.GetFileName(nome));
                    string destinoNovo = Path.Combine(destino,Path.GetFileName(nome));
                    if (File.Exists(destino))
                    {
                        MessageBox.Show("Já existe um arquivo com esse nome no destino.");
                        return;
                    }
                    File.Move(nome, destinoNovo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao mover arquivo: " + ex.Message);
            }

            return;
        }


        public static void DuplicarPasta(List<string> Pathnome, string destino)
        {
            try
            {
                //nome = SearchPaths(nome, true).FirstOrDefault();

                foreach (string nome in Pathnome)
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


                    MessageBox.Show(destinoNovo + " negocio infernal que pode estar dando erro");
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
                        MessageBox.Show(destinoSubPasta);
                        MessageBox.Show(nomeSubPasta);
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

        public static void DuplicarArquivo(List<string> Pathnomes, string destino)
        {

            try
            {

                //nome = SearchPaths(nome, false).FirstOrDefault();
                //MessageBox.Show(nome + " nome do arquivo que pode estar dando erro");
                //string ext = Path.GetExtension(nome);
                

                foreach (string nome in Pathnomes)
                {
                    string destinoNovo = Path.Combine(destino, Path.GetFileName(nome));

                    if (File.Exists(destino))
                    {
                        MessageBox.Show("Já existe um arquivo com esse nome no destino.");
                        return;
                    }

                    File.Move(nome, destino);
                }

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


        public static string CriarNome(string nome, string path)
        {
            int contador = 1;
            string nomefinal = nome;

            if (File.Exists(path))
            {
                while (File.Exists(path))
                {
                    nomefinal = $"{nome}({contador})";
                    contador++;
                  
                    path = Path.Combine(Path.GetDirectoryName(path), nomefinal);
                }

            }
            MessageBox.Show("Nome: " + nomefinal);
            return nomefinal;
        }


    }

}
