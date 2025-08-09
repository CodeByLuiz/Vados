using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace Vados
{
    internal class Comandos
    {
        #region TEXTO PARA COMANDO

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
            //Objetos
            { "pasta", WordType.Object },
            { "arquivo", WordType.Object },
            //Conectores
            { "para", WordType.Connector },
        };

        static Dictionary<string, string> commandSynonyms = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            //Criar
            { "criar", "criar" },
            { "crie", "criar" },
            //Renomear
            { "renomear", "renomear" },
            { "renomeie", "renomear" },
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


        public static List<List<object>> CommandGetWordOrder(string command)
        {
            //Retorna a ordem de palavras necessárias para realizar o comando
            List<string> namingConnectors = new List<string>() { "chamada", "chamado", "nomeado", "nomeada", "de nome", "com o nome", "com nome", "de titulo", "com o titulo", "com titulo" };

            switch (command)
            {
                case "criar":
                    return new List<List<object>>() {
                        new List<object>() {WordType.Object, namingConnectors, WordType.Value },
                        new List<object>() {WordType.Object, WordType.Value },
                    };

                case "renomear":
                    List<string> para = new List<string>() { "para" };

                    return new List<List<object>>()
                    {
                        new List<object>() { WordType.Object, WordType.Value, para, WordType.Value },
                        new List<object>() { WordType.Object, namingConnectors, WordType.Value, para, WordType.Value },
                    };

                case "excluir":
                    return new List<List<object>>()
                    {
                        new List<object>() { WordType.Object, WordType.Value },
                        new List<object>() { WordType.Object, namingConnectors, WordType.Value },
                    };


                //Lista vazia se não identificar o comando
                default:
                    return new List<List<object>>();
            }
        }


        public static void ExecuteCommand(List<string> arguments)
        {
            string obj = arguments[1];
            string name = arguments[2];

            switch (arguments[0])
            {
                case "criar":
                    if (obj == "pasta") { CriarPasta(name, ""); }
                    if (obj == "arquivo") { CriarArquivo(name, "txt", ""); }
                    break;

                case "renomear":
                    string oldName = arguments[2];
                    string newName = arguments[3];

                    if (obj == "pasta") { RenomearPasta(oldName, newName); }
                    if (obj == "arquivo") { RenomearArquivo(oldName, newName, "txt"); }
                    break;

                case "excluir":
                    if (obj == "pasta") { ExcluirPasta(name); }
                    if (obj == "arquivo") { ExcluirArquivo(name, "txt"); }
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
                MessageBox.Show(possibleCommand);
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
                arguments.Clear();
                arguments.Add(command);
                List<object> typeOrder = orderList[i];
                int typeIndex = 0;

                //Checar se o comando possui todas as palavras necessárias
                for (int j = startIndex; j < words.Count; j++)
                {
                    string word = words[j].ToLower();
                    var expected = typeOrder[typeIndex];
                    MessageBox.Show(word + ", " + expected.ToString());

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
                        if (list.Contains(word))
                        {
                            typeIndex += 1;
                        }
                    }
                }


                //Retornar argumentos se o comando estiver correto
                if (typeIndex == typeOrder.Count)
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
            for (int i = 0; i < words.Count(); i++) {
                string temporaryWord = "";
                int wordCount = 0;

                for (int j = 0; j < wordGroups.Count(); j++)
                {
                    string actualWordGroup = wordGroups[j];
                    List<string> groupSeparateWords = actualWordGroup.Split(" ").ToList();
                    wordCount = groupSeparateWords.Count;

                    //Checar se formam um grupo conhecido
                    for(var k = 0; k < groupSeparateWords.Count; k++)
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

      
        public static HashSet<string> MultiSearch(string aprocurar, string pastaRoot, string outrocriterio) 
        {
            //pasta root é a pasta aonde ele vai procurar, se for vazio ele procura em todas as pastas do computador
            pastaRoot = SearchFolders(pastaRoot,true);
            
            string root = @"" + driveverifica(null);
            var caminhos = new List<string>();
            var visitados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var resultados = new HashSet<string>();
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
                    foreach (var arquivo in Directory.GetFiles(atual))
                    {
                        string nome = Path.GetFileName(arquivo);
                        if (nome.Contains(aprocurar, StringComparison.OrdinalIgnoreCase) && nome.Contains(outrocriterio) && arquivo.Contains(pastaRoot) )
                        {
                           MessageBox.Show($"Arquivo encontrado: {arquivo}");
                            resultados.Add(arquivo);

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


                    }
                }

                catch (Exception)
                {

                }
            }
            MessageBox.Show("Nenhum arquivo encontrado com o nome especificado.");
            return resultados;
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

        public static string MultiTask(string criterio, string pastaAbuscar, string criterio2)
        {
            // esse aqui pode buscar pelo nome e extensão   
            MultiSearch(criterio, pastaAbuscar, criterio2);



            return "oi";
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
                string nomefinal;

                if (path == "")
                {
                    path = Path.Combine(Global.DefaultFolder + @"\" + nome + "." + extension);
                    nomefinal = CriarNome(nome, path,extension);
                }
                else
                {
                    path = SearchFolders(path, true) + @"\" + nome + "." + extension;
                    MessageBox.Show(path);
                    nomefinal = CriarNome(nome, path, extension);
                }
               
                string pathfinal = Path.Combine(Path.GetDirectoryName(path)+ @"\" + nomefinal + "." + extension);

                using (FileStream fs = File.Create(pathfinal))
                Console.WriteLine("Arquivo" + nomefinal + "Criado com sucesso");
                AbrirGerenciador(pathfinal);
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

                foreach (string arquivo in Directory.GetFiles(path))
                {
                    File.Delete(arquivo);
                }
                foreach(string subPasta in Directory.GetDirectories(path))
                {
                    ExcluirPasta(subPasta);
                    
                }
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


        public static void MoverPasta(string nome, string destino)
        {
            try
            {
                destino = SearchFolders(destino, true) + @"\" + nome;
                nome = SearchFolders(nome, true);
                MessageBox.Show(destino);


                if (Directory.Exists(destino))
                {
                    MessageBox.Show("Já existe uma pasta com esse nome no destino.");
                    return;
                }

                Directory.Move(nome, destino);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao mover pasta: " + ex.Message);
            }  
        }

        public static void MoverArquivo(string nome, string destino,string ext) 
        {
            try
            {
                nome = SearchFolders(nome, false);
                destino = Path.Combine(SearchFolders(destino, true), Path.GetFileName(nome));
                MessageBox.Show(destino," dsdasdasdadasdaasda");
                // SearchFolders(destino, true) + @"\" + nome + "." + ext;

                MessageBox.Show(destino);


                if (File.Exists(destino))
                {
                    MessageBox.Show("Já existe um arquivo com esse nome no destino.");
                    return;
                }

                File.Move(nome, destino);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao mover arquivo: " + ex.Message);
            }


        }

        public static void DuplicarPasta(string nome, string destino)
        {
            try
            {
                nome = SearchFolders(nome, true);
                if (string.IsNullOrWhiteSpace(destino))
                {
                    destino = Path.Combine(Global.DefaultFolder, Path.GetFileName(nome));
                }
                else
                {
                    destino = Path.Combine(SearchFolders(destino, true), Path.GetFileName(nome));

                }


                MessageBox.Show(destino + " negocio infernal que pode estar dando erro");
                if (Directory.Exists(destino))
                {
                    MessageBox.Show("Já existe uma pasta com esse nome no destino.");
                    return;
                }

                Directory.CreateDirectory(destino);
                foreach (string arquivo in Directory.GetFiles(nome))
                {


                    string nomeArquivo = Path.GetFileName(arquivo);
                    string destinoArquivo = Path.Combine(destino, nomeArquivo);
                    File.Copy(arquivo, destinoArquivo, true);

                }
                foreach (string subPasta in Directory.GetDirectories(nome))
                {
                    string nomeSubPasta = Path.GetFileName(subPasta);
                    string destinoSubPasta = Path.Combine(destino, nomeSubPasta);
                    MessageBox.Show(destinoSubPasta);
                    MessageBox.Show(nomeSubPasta);
                    DuplicarPasta(subPasta, destino);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao duplicar pasta: " + ex.Message);
            }
        }

        public static void DuplicarArquivo(string nome, string destino)
        {

            try
            {

                nome = SearchFolders(nome, false);
                MessageBox.Show(nome + " nome do arquivo que pode estar dando erro");
                string ext = Path.GetExtension(nome);
                destino = Path.Combine(SearchFolders(destino, true), Path.GetFileName(nome));

                if (File.Exists(destino))
                {
                    MessageBox.Show("Já existe um arquivo com esse nome no destino.");
                    return;
                }

                File.Move(nome, destino);

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
           string arquivo = SearchFolders(nome,false);

            var psi = new ProcessStartInfo()
            {
                UseShellExecute= true,
                FileName = arquivo,
            };
            Process.Start(psi);
        }

        public static string CriarNome(string nome, string path,string extension)
        {
            int contador = 1;
            string nomefinal = nome;

            if (File.Exists(path))
            {
                while (File.Exists(path))
                {
                    nomefinal = $"{nome}({contador})";
                    contador++;
                    path = Path.Combine(Path.GetDirectoryName(path) + @"/" + nomefinal + "."+ extension);
                }
               
            }
            MessageBox.Show(nomefinal);
            return nomefinal;
        }
        

    }

}
