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
using static System.Windows.Forms.DataFormats;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Xml.Linq;

namespace Vados
{
    internal class Comandos
    {
        Task<bool> commandSuccess;

        #region FUNÇÕES DO WINDOWS

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        #endregion


        #region TEXTO PARA COMANDO

        #region DICIONÁRIOS / LISTAS

        //Nomes inválidos para arquivos / pastas
        public static string[] reservedNames =
        {
            "CON", "PRN", "AUX", "NUL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        };


        #region COMANDOS

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
            //Abrir
            { "abrir", "abrir" },
            { "abra", "abrir" },
            { "iniciar", "abrir" },
            { "inicie", "abrir" },
            { "inicializar", "abrir" },
            { "inicialize", "abrir" },
            { "comecar", "abrir" },
            { "comece", "abrir" },
            { "despertar", "abrir" },
            { "desperte", "abrir" },
            { "acordar", "abrir" },
            { "acorde", "abrir" },
        };

        //Todas as variações dos comandos
        public static List<string> allCommands = new List<string>(commandSynonyms.Keys);

        #endregion

        #region OBJETOS

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
            { "aplicativo", "aplicativo" },
            { "programa", "aplicativo" },
            { "app", "aplicativo" },
            { "executavel", "aplicativo" },
        };

        //Todas as variações de objetos (pasta / arquivo)
        public static List<string> allObjects = new List<string>(objectSynonyms.Keys);

        #endregion

        #region TAMANHO

        //Variações das unidades de tamanho de arquivo
        static Dictionary<string, string> sizeUnitSynonyms = new Dictionary<string, string>()
        {
            //Quantidade
            { "byte", "byte" },
            { "bytes", "byte" },
            { "kilo", "kilo" },
            { "kilos", "kilo" },
            { "kilobyte", "kilo" },
            { "kilobytes", "kilo" },
            { "kb", "kilo" },
            { "kbs", "kilo" },
            { "kbyte", "kilo" },
            { "kbytes", "kilo" },
            { "mega", "mega" },
            { "megas", "mega" },
            { "megabyte", "mega" },
            { "megabytes", "mega" },
            { "mb", "mega" },
            { "mbs", "mega" },
            { "giga", "giga" },
            { "gigas", "giga" },
            { "gigabyte", "giga" },
            { "gigabytes", "giga" },
            { "gbs", "giga" },
        };

        public static List<string> allSizeUnitWords = new List<string>(sizeUnitSynonyms.Keys);

        //Modificadores de tamanho
        static Dictionary<string, string> sizeModifierSynonyms = new Dictionary<string, string>()
        {
            //Maior
            { "maior que", "maior" },
            { "maiores que", "maior" },
            { "superior a", "maior" },
            { "acima de", "maior" },
            { "mais alto que", "maior" },
            { "mais alta que", "maior" },
            { "mais altos que", "maior" },
            { "mais altas que", "maior" },
            { "mais que", "maior" },
            //Menor
            { "menor que", "menor" },
            { "menores que", "menor" },
            { "inferior a", "menor" },
            { "abaixo de", "menor" },
            { "mais baixo que", "menor" },
            { "mais baixa que", "menor" },
            { "mais baixos que", "menor" },
            { "mais baixas que", "menor" },
            { "menos que", "menor" },
            //Igual
            { "igual a", "igual" },
            { "iguais a", "igual" },
            { "semelhante a", "igual" },
            { "semelhantes a", "igual" },
            { "parecido com", "igual" },
            { "parecidos com", "igual" },
            { "de", "igual" },
            { "de exatamente", "igual" },
            { "com exatamente", "igual" },
            { "com exatos", "igual" },
            { "similar a", "igual" },
            { "similares a", "igual" },
            { "proximo a", "igual" },
            { "proximos a", "igual" },
            { "identico a", "igual" },
            { "identicos a", "igual" },
        };

        public static List<string> allSizeModifierWords = new List<string>(sizeModifierSynonyms.Keys);

        //Formas de indicar tamanho
        public static List<string> sizeWords = new List<string>()
        {
            "de tamanho",
            "com tamanho",
            "de peso",
            "com peso",
            "pesando",
            "que pesa",
            "que pesam",
            "que ocupa",
            "que ocupam",
            "que ocupa o espaço de",
            "que ocupam o espaço de",
        };

        #endregion

        #region ALL SYNONYMS

        //Todos os sinônimos
        static Dictionary<string, string> currentSynonyms0 = commandSynonyms.Concat(objectSynonyms).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        static Dictionary<string, string> currentSynonyms1 = currentSynonyms0.Concat(sizeUnitSynonyms).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        static Dictionary<string, string> currentSynonyms = currentSynonyms1.Concat(sizeModifierSynonyms).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        static Dictionary<string, string> wordSynonyms = new Dictionary<string, string>(currentSynonyms)
        {
            //Quantidade
            { "todos", "todos" },
            { "todos os", "todos" },
            { "metade dos", "metade" },
        };

        #endregion

        #region EXTENSÕES

        //Extensões relacionadas as palavras
        static Dictionary<string, List<string>> wordExtensions = new Dictionary<string, List<string>>()
        {
            { "texto", new List<string>() { "txt", "doc", "docx", "rtf", "odt", "md" } },
            { "imagem", new List<string>() { "png", "jpg", "jpeg", "bmp", "ico" } },
            { "video", new List<string>() { "mp4", "avi", "mov" } },
            { "audio", new List<string>() { "mp3", "wav", "ogg" } },
            { "apresentacao", new List<string>() { "odp", "ppt", "pptx" } },
            { "web", new List<string>() { "htm", "html", "css", "js", "php", "xps", "asp" } },
            { "executavel", new List<string>() { "exe" } },
            { "atalho", new List<string>() { "lnk" } },
            { "compactado", new List<string>() { "zip", "rar", "7z" } },
            { "power point", new List<string>() { "ppt", "pptx" } },
            { "word", new List<string>() { "doc", "docx" } },
            { "excel", new List<string>() { "xls", "xlsx" } },
        };

        //Todas as palavras que indicam extensões
        public static List<string> allExtensionsWords = new List<string>(wordExtensions.Keys);

        #endregion


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

        //Formas de indicar a pasta de origem
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


        #region FUNÇÕES

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

        #endregion


        public static (CommandCriteria criteria, bool success) CommandGetArguments(string command)
        {
            CommandCriteria criteria = new CommandCriteria();

            //Definir tipo de comando
            ActionExtractor actionExtractor = new ActionExtractor(startWords, allCommands);
            actionExtractor.Extract(command, criteria);


            //Extrair argumentos para cada tipo de comando
            CommandParser parser = new CommandParser(criteria, new List<CriteriaExtractor>());

            switch (criteria.Action)
            {
                case "criar":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                    {
                        new ObjectExtractor((amountWords, false), (allObjects, true), (allExtensionsWords, false), (namingWords, false), true, null, true),
                        new DestinationExtractor(destinationWords, folderWords, namingWords)
                    });
                    break;

                case "renomear":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                    {
                        new ObjectExtractor((amountWords, false), (allObjects, true), (allExtensionsWords, false), (namingWords, false), false, null, true),
                        new NewNameExtractor(),
                        new OriginExtractor(fromWords, folderWords, namingWords),
                        new SizeExtractor(sizeWords, allSizeModifierWords, allSizeUnitWords),
                    });
                    break;

                case "excluir":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                    {
                        new ObjectExtractor((amountWords, false), (allObjects, true), (allExtensionsWords, false), (namingWords, false), false, fromWords, true),
                        new OriginExtractor(fromWords, folderWords, namingWords),
                        new SizeExtractor(sizeWords, allSizeModifierWords, allSizeUnitWords),
                    });
                    break;


                case "mover":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                    {
                        new ObjectExtractor((amountWords, false), (allObjects, true), (allExtensionsWords, false), (namingWords, false), false, fromWords, true),
                        new OriginExtractor(fromWords, folderWords, namingWords),
                        new DestinationExtractor(insideWords, folderWords, namingWords, true),
                        new SizeExtractor(sizeWords, allSizeModifierWords, allSizeUnitWords),
                    });
                    break;


                case "duplicar":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                    {
                        new ObjectExtractor((amountWords, false), (allObjects, true), (allExtensionsWords, false), (namingWords, false), false, null, true),
                        new OriginExtractor(fromWords, folderWords, namingWords),
                        new DestinationExtractor(insideWords, folderWords, namingWords),
                        new SizeExtractor(sizeWords, allSizeModifierWords, allSizeUnitWords),
                    });
                    break;

                case "abrir":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                    {
                        new ObjectExtractor((amountWords, false), (allObjects, true), (allExtensionsWords, false), (namingWords, false), true, null, true),
                        new OriginExtractor(fromWords, folderWords, namingWords),
                    });
                    break;

                //Comando não identificado
                default:
                    return (criteria, false);
            }

            var arguments = parser.Parse(command);
            MessageBox.Show($"Comando: --{criteria.Action}*\r\nObjeto: --{criteria.ObjectType}*\r\nFormato: --{criteria.ObjectFormat}\r\nQuantidade: --{criteria.ObjectAmount}\r\nNome: --{criteria.ObjectName}\r\nNovo nome: --{criteria.ObjectNewName}\r\nOrigem: --{criteria.Origin}\r\nDestino: --{criteria.Destination}\r\nTamanho: --{criteria.SizeModifier} {criteria.SizeAmount} {criteria.SizeUnit}");

            return arguments;
        }


        //Retorna os caminhos encontrados conforme os critérios
        static async Task<List<string>> GetPaths(string objectType, string name, string format, string origin, string amountModifier, string size, string sizeUnit, string sizeModifier, List<string> priorities = null, List<string> exceptions = null)
        {
            if (priorities == null) priorities = Global.defaultPriorities;
            if (exceptions == null) priorities = Global.defaultExceptions;


            //---------ERRO: pasta de origem não existe---------
            string rootFolderPath = (await SearchPaths(origin, true, pathAmount: 1)).FirstOrDefault();

            if (string.IsNullOrEmpty(rootFolderPath))
            {
                Global.AppendPlainText(Global.nextErrorMessage, "Não foi possível encontrar a pasta de origem chamada ");
                Global.AppendFormattedText(Global.nextErrorMessage, origin, Colors.greenHighlight, FontStyle.Bold);
                Global.AppendPlainText(Global.nextErrorMessage, ".");
                return new List<string>();
            }
            //----------------------------------------


            #region TAMANHO

            //Definir quantidade de bytes conforme a unidade de tamanho
            long lowerBound = -1;
            long upperBound = -1;

            if (!string.IsNullOrEmpty(size))
            {
                long actualSize = (long)Convert.ToDouble(size);

                switch (sizeUnit)
                {
                    case "byte":
                        break;
                    case "kilo":
                        actualSize *= 1000;
                        break;
                    case "mega":
                        actualSize *= 1000 * 1000;
                        break;
                    case "giga":
                        actualSize *= 1000 * 1000 * 1000;
                        break;
                }

                //Definir margens de erro
                lowerBound = actualSize;
                upperBound = actualSize;

                switch (sizeModifier)
                {
                    case "":
                    case "igual":
                        float errorPercentage = 0.25f;
                        lowerBound = (long)Math.Floor(actualSize * (1 - errorPercentage));
                        upperBound = (long)Math.Ceiling(actualSize * (1 + errorPercentage));
                        break;

                    case "menor":
                        lowerBound = 0;
                        break;

                    case "maior":
                        upperBound = long.MaxValue;
                        break;
                }
            }

            #endregion


            int? amountNumber = null;
            if (amountModifier == "") amountNumber = 1;

            //Buscar mais de um caminho correspondente
            List<string> paths = new List<string>();

            if (format != "")
            {
                //Retornar todos os arquivos de determinado formato (pode englobar mais de uma extensão)
                foreach (string extension in Comandos.WordGetExtensions(format))
                {
                    //MessageBox.Show("extension: " + extension);
                    List<string> newPaths = (await SearchPaths(name + "." + extension, objectType == "pasta",
                                                        rootFolder: origin, pathAmount: amountNumber, sizeLowerBound: lowerBound, sizeUpperBound: upperBound, exceptions: exceptions, priorities: priorities)).ToList();
                    paths.AddRange(newPaths);
                }
            }
            else
            {
                //Procura normal usando o nome
                paths = (await SearchPaths(name, objectType == "pasta", rootFolder: origin, pathAmount: amountNumber, sizeLowerBound: lowerBound, sizeUpperBound: upperBound)).ToList();
            }


            //---------ERRO: arquivo / pasta não encontrada---------
            if (paths.Count == 0)
            {
                string objectIndication = $"nenhum {objectType} chamado ";
                if (objectType == "pasta") objectIndication = "nenhuma pasta chamada ";

                Global.AppendPlainText(Global.nextErrorMessage, "Não foi possível encontrar " + objectIndication);
                Global.AppendFormattedText(Global.nextErrorMessage, name, Colors.greenHighlight, FontStyle.Bold);
                if (origin != "")
                {
                    Global.AppendPlainText(Global.nextErrorMessage, " dentro da pasta ");
                    Global.AppendFormattedText(Global.nextErrorMessage, origin, Colors.greenHighlight, FontStyle.Bold);
                }
                Global.AppendPlainText(Global.nextErrorMessage, ".");
                return paths;
            }
            //----------------------------------------



            //Corrigir quantidade de caminhos
            switch (amountModifier)
            {
                case "todos":
                    break;
                case "metade":
                    int half = (int)Math.Ceiling((decimal)paths.Count / 2);
                    paths = paths.GetRange(0, half);
                    break;
                case "":
                    paths = paths.GetRange(0, 1);
                    break;
            }

            return paths;
        }


        public static async Task<string> ExecuteCommand(CommandCriteria arguments)    //Retorna uma possível mensagem de erro
        {
            string
            commandType = arguments.Action,
            objectType = arguments.ObjectType,
            name = arguments.ObjectName,
            newName = arguments.ObjectNewName,
            format = arguments.ObjectFormat,
            origin = arguments.Origin,
            destination = arguments.Destination,
            amount = arguments.ObjectAmount,
            size = arguments.SizeAmount,
            sizeUnit = arguments.SizeUnit,
            sizeModifier = arguments.SizeModifier;


            //Definições para arquivo executável (programa)
            if (objectType == "aplicativo")
            {
                name.Replace(" ", "");
                format = "executavel";
            }


            #region---------ERRO: nome inválido (nome, origem ou destino)---------
            Global.nextErrorMessage.Clear();

            //Nome do arquivo inválido
            if (objectType != "pasta" && !IsValidFileName(name))
            {
                Global.AppendFormattedText(Global.nextErrorMessage, name, Colors.greenHighlight, FontStyle.Bold);
                Global.AppendPlainText(Global.nextErrorMessage, " é um nome de " + objectType + " inválido.");
                return Global.nextErrorMessage.Rtf;
            }


            //Nome da pasta (objeto, origem ou destino) inválido
            char[] invalidPathChars = Path.GetInvalidPathChars();

            if (objectType == "pasta") {
                if (name != "" && !IsValidFolderName(name))                 //Pasta indicada
                    Global.AppendFormattedText(Global.nextErrorMessage, name, Colors.greenHighlight, FontStyle.Bold);
                else if (origin != "" && !IsValidFolderName(origin))          //Pasta de origem
                    Global.AppendFormattedText(Global.nextErrorMessage, origin, Colors.greenHighlight, FontStyle.Bold);
                else if (destination != "" && !IsValidFolderName(destination))     //Pasta de destino
                    Global.AppendFormattedText(Global.nextErrorMessage, destination, Colors.greenHighlight, FontStyle.Bold);

                if (Global.nextErrorMessage.Text != "")
                {
                    Global.AppendPlainText(Global.nextErrorMessage, " é um nome de pasta inválido.");
                    return Global.nextErrorMessage.Rtf;
                }
            }
            #endregion----------------------------------------


            //Caminho da pasta de origem e de destino
            string destinationPath = "";
            if (!string.IsNullOrEmpty(destination)) destinationPath = (await SearchPaths(destination, true, pathAmount: 1)).FirstOrDefault();

            string originPath = "";
            if (!string.IsNullOrEmpty(origin)) originPath = (await SearchPaths(origin, true, pathAmount: 1)).FirstOrDefault();

            List<string> paths = new List<string>();


            //Realizar comando
            switch (commandType)
            {
                //Criar
                case "criar":
                    if (objectType == "pasta") { return await CriarPasta(name, destination); }
                    if (objectType == "arquivo") { return await CriarArquivo(name, destination); }
                    break;


                //Renomear
                case "renomear":
                    if (objectType == "pasta") { RenomearPasta(name, newName, origin); }
                    if (objectType == "arquivo") { RenomearArquivo(name, newName, origin); }
                    break;


                //Excluir
                case "excluir":
                    paths = await GetPaths(objectType, name, format, origin, amount, size, sizeUnit, sizeModifier);
                    //Erro na busca
                    if (Global.nextErrorMessage.Text != "") return Global.nextErrorMessage.Rtf;

                    if (objectType == "pasta") { ExcluirPasta(paths); }
                    if (objectType == "arquivo") { ExcluirArquivo(paths); }
                    break;


                //Mover
                case "mover":
                    paths = await GetPaths(objectType, name, format, origin, amount, size, sizeUnit, sizeModifier);
                    //Erro na busca
                    if (Global.nextErrorMessage.Text != "") return Global.nextErrorMessage.Rtf;

                    if (objectType == "pasta") { MoverPasta(paths, destinationPath); }
                    if (objectType == "arquivo") { MoverArquivo(paths, destinationPath); }
                    break;


                //Duplicar
                case "duplicar":
                    paths = await GetPaths(objectType, name, format, origin, amount, size, sizeUnit, sizeModifier);
                    //Erro na busca
                    if (Global.nextErrorMessage.Text != "") return Global.nextErrorMessage.Rtf;

                    if (objectType == "pasta") { DuplicarPasta(paths, destinationPath); }
                    if (objectType == "arquivo") { DuplicarArquivo(paths, destinationPath); }
                    break;


                //Abrir
                case "abrir":
                    if (objectType == "pasta") break;

                    //Procurar caminhos mais eficientemente
                    if (objectType == "arquivo")
                    {
                        paths = await GetPaths("arquivo", name, format, origin, amount, size, sizeUnit, sizeModifier);
                    } 
                    else
                    {
                        //Prioridades e excessões para procurar programas
                        paths = await GetPaths("arquivo", name, format, origin, amount, size, sizeUnit, sizeModifier, Global.exePriorities, Global.exeExceptions);
                    }

                    ExecutarCaminho(paths.FirstOrDefault());
                    break;
            }

            return "";
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


        //Checar se o nome do arquivo é valido
        public static bool IsValidFileName(string fileName)
        {
            //Nome não pode ser vazio ou apenas espaços
            if (string.IsNullOrWhiteSpace(fileName)) return false;

            //Caracteres inválidos
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return false;

            //Sem espaço ou ponto no final do nome
            if (fileName.EndsWith(" ") || fileName.EndsWith("."))
                return false;

            //Nomes reservados para dispositivos
            if (Array.Exists(reservedNames, r => string.Equals(r, fileName, StringComparison.OrdinalIgnoreCase)))
                return false;

            return true;
        }

        //Checar se o nome da pasta é valido
        public static bool IsValidFolderName(string name)
        {
            if (!IsValidFileName(name)) return false;

            //Pastas não podem ter nome "." ou ".."
            if (name == "." || name == "..")
                return false;

            return true;
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

        //Busca recursivamente multiplas pastas ou arquivos, retornando o caminho do arquivo ou pasta encontrado, ou uma mensagem de erro se não encontrar nada
        public static async Task<HashSet<string>> SearchPaths(string searchName, bool isFolder, long sizeLowerBound = -1, long sizeUpperBound = -1, string rootFolder = "", int? pathAmount = 1, int[] dateStart = null, int[] dateEnd = null, List<string> exceptions = null, List<string> priorities = null)
        {
            // PRA QUE SERVE CADA PARÂMETRO:

            // searchName: o nome do arquivo ou pasta que você quer procurar
            // isFolder: se for TRUE, ele procura por pastas, se for FALSE, ele procura por arquivos
            // sizeLowerBound: se diferente de -1, define a margem inferior do tamanho do arquivo
            // sizeLowerBound: se diferente de -1, define a margem superior do tamanho do arquivo
            // rootFolder: é a pasta aonde ele vai procurar, se for nulo, ele procura em todas as pastas do computador
            // pathAmount: é um controle de quantas pastas ou arquivos ele vai procurar, se for nulo, ele procura em todas as pastas ou arquivos

            var visitados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var resultados = new HashSet<string>();

            //Definir prioridades e exceções
            if (priorities == null) priorities = Global.defaultPriorities;
            if (exceptions == null) exceptions = Global.defaultExceptions;

            //Determinar a quantidade de caminhos a serem buscados
            int? maxLength = pathAmount;

            if (!string.IsNullOrEmpty(rootFolder)) {
                //Procurar pasta de origem
                rootFolder = (await SearchPaths(rootFolder, true)).FirstOrDefault();
                if (string.IsNullOrEmpty(rootFolder))   //Retornar lista vazia se não encontrar a pasta de origem
                    return resultados;

                //Buscar apenas na pasta determinada
                priorities = new List<string>() { rootFolder };

                //Quantidade máxima a ser buscada
                if (isFolder == true)
                {
                    //Pastas
                    maxLength = await Task.Run(()=> Directory.GetDirectories(rootFolder).Length);
                }
                else
                {
                    //Arquivos
                    maxLength = await Task.Run(() => Directory.GetFiles(rootFolder).Length);
                }
            }

            //Redefinir quantidade de caminhos a serem buscados
            if (pathAmount != null)
            {
                pathAmount = Math.Min((int)pathAmount, (int)maxLength);
            }
            else
            {
                pathAmount = int.MaxValue;  //Sem limite
            }


            //Adicionar prioridades à fila
            var fila = new Queue<string>();
            foreach (var pasta in priorities)
            {
                if (Directory.Exists(pasta))
                {
                    fila.Enqueue(pasta);

                }
            }

            while (fila.Count > 0)
            {
                var atual = fila.Dequeue();

                try
                {
                    //Pastas
                    if (isFolder)
                    {
                        var subpasta = await Task.Run(() =>
                        {
                            try
                            {
                                return Directory.GetDirectories(atual);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                Console.WriteLine($"Acesso negado ao diretório: {atual}");
                                return Array.Empty<string>();
                            }
                        }).ConfigureAwait(false);

                        //Percorrer todas as pastas dentro da pasta atual
                        foreach (var folderPath in subpasta)
                        {
                            Console.WriteLine(folderPath);

                            //Checar se a pasta tem o nome correto
                            string actualName = Path.GetFileName(folderPath);
                            if (!string.IsNullOrEmpty(searchName) && !actualName.Contains(searchName, StringComparison.OrdinalIgnoreCase))
                                continue;

                            //Checar se está na pasta especificada
                            if (!string.IsNullOrEmpty(rootFolder) && !folderPath.Contains(rootFolder))
                                continue;


                            FileInfo caminhoinfo = new FileInfo(folderPath);

                            //Filtro de data
                            if (dateStart != null && !DateFilter(folderPath, caminhoinfo, dateStart, rootFolder))
                                continue;

                            //Filtro de tamanho
                            if (sizeLowerBound != -1 && sizeUpperBound != -1)
                            {
                                long folderSize = await FolderGetSize(folderPath, criterio: sizeLowerBound);

                                if (!SizeFilter(folderSize, sizeLowerBound, sizeUpperBound))
                                    continue;
                            }

                            //Se passar por todos os critérios, adicionar a lista de resultados
                            resultados.Add(folderPath);
                            pathAmount -= 1;
                        }

                        //Retornar resultados ao chegar na quantidade necessária
                        if (pathAmount <= 0)
                        {
                            return resultados;
                        }
                    }
                  

                    //Arquivos
                    else
                    {
                        var files = await Task.Run(() =>
                        {
                            try
                            {
                                return Directory.GetFiles(atual);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                Console.WriteLine($"Acesso negado ao diretório: {atual}");
                                return Array.Empty<string>();
                            }
                        });

                        //MessageBox.Show("Pasta: " + atual + "\r\nArquivos: " + string.Join("|", files.Select(Regex.Escape)));

                        //Percorrer todos os arquivos da pasta atual
                        foreach (var filePath in files)
                        {
                            //Parar se o arquivo já tiver sido visitado
                            if (!visitados.Add(filePath)) 
                               continue;

                            Console.WriteLine(filePath);

                            //Checar se o arquivo tem o nome correto
                            string actualName = Path.GetFileName(filePath);

                            //MessageBox.Show("Nome: " + actualName + "\r\nEsperado: " + searchName);
                            if (!string.IsNullOrEmpty(searchName) && !actualName.Contains(searchName, StringComparison.OrdinalIgnoreCase))
                                continue;

                            //Checar se está na pasta especificada
                            if (!string.IsNullOrEmpty(rootFolder) && !filePath.Contains(rootFolder, StringComparison.OrdinalIgnoreCase))
                                continue;


                            FileInfo pathInfo = new FileInfo(filePath);

                            //Filtro de data
                            if (dateStart != null && !DateFilter(filePath, pathInfo, dateStart, rootFolder))
                                continue;

                            //Filtro de tamanho
                            if (sizeLowerBound != -1 && sizeUpperBound != -1 && !SizeFilter(pathInfo.Length, sizeLowerBound, sizeUpperBound))
                                continue;

                            //Se passar por todos os critérios, adicionar a lista de resultados
                            resultados.Add(filePath);
                            pathAmount -= 1;
                        }

                        //Retornar resultados ao chegar na quantidade necessária
                        if (pathAmount <= 0)
                            return resultados;
                    }


                    //Adicionar subpastas no início da fila
                    foreach (var caminho in Directory.GetDirectories(atual))
                    {
                        string nomePasta = Path.GetFileName(caminho);

                        //Ignorar pasta se ela for alguma das exceções
                        if (exceptions.Any(expt => nomePasta.Equals(expt, StringComparison.OrdinalIgnoreCase)))
                            continue;

                        if (visitados.Add(caminho))
                        {
                            InserirNoInicio(fila, caminho);
                        }
                    }
                }
                //Pular pastas inacessíveis
                catch (UnauthorizedAccessException)
                {
                    continue;
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
                
            }

            
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

        public static bool DateFilter(string arquivo, FileInfo fileinfo, int[] datas, string pastaroot)
        {
            int dia = datas[0], mes = datas[1], ano = datas[2];

            if (dia != 0 && mes != 0 && ano != 0 && fileinfo.LastWriteTime.Day == dia && fileinfo.LastWriteTime.Month == mes && fileinfo.LastWriteTime.Year == ano && arquivo.Contains(pastaroot))
            {
                //MessageBox.Show(fileinfo.LastWriteTime.Day + ", "+ fileinfo.LastWriteTime.Month + ", "+ fileinfo.LastWriteTime.Year);

                return true;
            }
            else if (dia == 0 )
            {
                if (mes != 0 && ano != 0 && fileinfo.LastWriteTime.Month == mes && fileinfo.LastWriteTime.Year == ano && arquivo.Contains(pastaroot))
                {
                    //MessageBox.Show("2");
                    return true;
                }
                else if ( mes == 0 && ano != 0 && fileinfo.LastWriteTime.Year == ano && arquivo.Contains(pastaroot))
                {
                    //MessageBox.Show("3");
                    return true;
                }
            }
            return false;
        }


        public static bool SizeFilter(long size, long lowerBound, long upperBound)
        {
            return (size >= lowerBound) && (size <= upperBound);
        }

        public static async Task<long> GetFolderSize(string caminho,long criterio=0, long control=0)
        {
            long tamanhoTotal = 0;

            try
            {
                
                tamanhoTotal += await Task.Run(()=> Directory.GetFiles(caminho).Sum(arquivo => new FileInfo(arquivo).Length));

                control += tamanhoTotal;

                if( control== 1.5 * criterio && criterio!=0 || tamanhoTotal>= 1.5*criterio)
                {
                    return control;
                }

                foreach (var subPasta in await Task.Run(()=> Directory.GetDirectories(caminho)))
                {
                    tamanhoTotal += (await FolderGetSize(subPasta, control:control));
                }
            }
            catch (Exception ex)
            {
                
            }

            return tamanhoTotal;
        }

        public static async Task<long> FolderGetSize(string caminho, long criterio = 0, long control = 0)
        {
            long tamanhoTotal = 0;

            try
            {

                tamanhoTotal += await Task.Run(() => Directory.GetFiles(caminho).Sum(arquivo => new FileInfo(arquivo).Length));

                control += tamanhoTotal;

                if (control == 1.5 * criterio && criterio != 0 || tamanhoTotal >= 1.5 * criterio)
                {
                    return control;
                }

                foreach (var subPasta in await Task.Run(() => Directory.GetDirectories(caminho)))
                {
                    tamanhoTotal += (await FolderGetSize(subPasta, control: control));
                }
            }
            catch (Exception ex)
            {

            }

            return tamanhoTotal;
        }

        #endregion


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


        public static async Task<string> CriarPasta(string nome, string destination)     //Retorna uma possível mensagem de erro
        {
            try
            {
                string path;

                //Definir pasta padrão como pasta de destino
                if (string.IsNullOrEmpty(destination)) 
                {
                    path = Path.Combine(Global.DefaultFolder, nome);
                }
                //Procurar pasta de destino
                else
                {
                    HashSet<string> destinationPath = (await SearchPaths(destination, true, pathAmount: 1));

                    //---------ERRO: pasta de destino não encontrada---------
                    if (destinationPath.Count == 0)
                    {
                        Global.nextErrorMessage.Clear();
                        Global.AppendPlainText(Global.nextErrorMessage, "Não foi possível encontrar a pasta de destino chamada ");
                        Global.AppendFormattedText(Global.nextErrorMessage, destination, Colors.greenHighlight, FontStyle.Bold);
                        Global.AppendPlainText(Global.nextErrorMessage, ".");
                        return Global.nextErrorMessage.Rtf;
                    }
                    //----------------------------------------


                    //Definir caminho caso a pasta seja encontrada
                    path = Path.Combine(destinationPath.FirstOrDefault(), nome);
                }


                if (!File.Exists(path))
                {
                    //Criar a pasta
                    Directory.CreateDirectory(path);
                    OpenFileExplorer(path, false);
                }
                //---------ERRO: arquivo com mesmo nome---------
                else
                {
                    Global.nextErrorMessage.Clear();
                    Global.AppendPlainText(Global.nextErrorMessage, "Não foi possível criar uma pasta chamada ");
                    Global.AppendFormattedText(Global.nextErrorMessage, destination, Colors.greenHighlight, FontStyle.Bold);
                    Global.AppendPlainText(Global.nextErrorMessage, ", pois já existe um arquivo com esse nome.");
                    return Global.nextErrorMessage.Rtf;
                }
                //----------------------------------------

                return "";
            }

            //---------ERRO: erro não especificado---------
            catch (Exception ex)
            {
                Global.nextErrorMessage.Clear();
                Global.AppendPlainText(Global.nextErrorMessage, "Não foi possível criar a pasta.\n");
                Global.AppendFormattedText(Global.nextErrorMessage, destination, Color.Gray, FontStyle.Regular);
                return Global.nextErrorMessage.Rtf;
            }
            //----------------------------------------
        }


        public static async Task<string> CriarArquivo(string nome, string destination) //Retorna uma possível mensagem de erro
        {
            //Redefinir nome caso já exista um igual
            string nomeFinal = CriarNome(nome, destination);
            string destinationPath = Global.DefaultFolder;

            try
            {

                //Definir pasta informada como destino
                if (!string.IsNullOrEmpty(destination))
                {
                    destinationPath = (await SearchPaths(destination, true, pathAmount: 1)).FirstOrDefault();

                    //---------ERRO: pasta de destino não encontrada---------
                    if (string.IsNullOrEmpty(destinationPath))
                    {
                        Global.nextErrorMessage.Clear();
                        Global.AppendPlainText(Global.nextErrorMessage, "Não foi possível encontrar a pasta de destino chamada ");
                        Global.AppendFormattedText(Global.nextErrorMessage, destination, Colors.greenHighlight, FontStyle.Bold);
                        Global.AppendPlainText(Global.nextErrorMessage, ".");
                        return Global.nextErrorMessage.Rtf;
                    }
                    //----------------------------------------
                }


                string newPath = Path.Combine(destinationPath, nomeFinal);

                if (!Directory.Exists(newPath))
                {
                    //Criar arquivo
                    using (FileStream fs = File.Create(newPath))
                    OpenFileExplorer(newPath, false);
                }
                //---------ERRO: pasta com mesmo nome---------
                else
                {
                    Global.nextErrorMessage.Clear();
                    Global.AppendPlainText(Global.nextErrorMessage, "Não foi possível criar um arquivo chamado ");
                    Global.AppendFormattedText(Global.nextErrorMessage, nomeFinal, Colors.greenHighlight, FontStyle.Bold);
                    Global.AppendPlainText(Global.nextErrorMessage, ", pois já existe uma pasta com esse nome.");
                    return Global.nextErrorMessage.Rtf;
                }
                //----------------------------------------

                return "";
            }

            //---------ERRO: erro não especificado---------
            catch (Exception ex)
            {
                Global.nextErrorMessage.Clear();
                Global.AppendPlainText(Global.nextErrorMessage, "Não foi possível criar o arquivo.\n");
                Global.AppendFormattedText(Global.nextErrorMessage, ex.Message, Color.Gray, FontStyle.Regular);
                return Global.nextErrorMessage.Rtf;
            }
            //----------------------------------------
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


        public static async void RenomearArquivo(string nome, string novoNome, string pastaOrigem) // renomear arquivo(erro de logica, falta implementar o bagulho de procurar o arquivo o mesmo serve para o bagulho de excluir)
        {
            string path = (await SearchPaths(nome, false, rootFolder: pastaOrigem)).FirstOrDefault();
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

        public static async void RenomearPasta(string nome, string novoNome, string pastaOrigem) // renomear pasta(mesmo erro de logica do renomear arquivo)
        {
            string path = (await SearchPaths(nome, true, rootFolder: pastaOrigem)).FirstOrDefault();
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
                foreach (string nome in Pathnomes)
                {
                    string destinoNovo = Path.Combine(destino, Path.GetFileName(nome));
                    //MessageBox.Show(destinoNovo);

                    if (Directory.Exists(destinoNovo))
                    {
                        MessageBox.Show("Já existe uma pasta com esse nome no destino.");
                        continue;
                    }

                    Directory.Move(nome, destinoNovo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao mover pasta: " + ex.Message);
            }
        }

        public static void MoverArquivo(List<string> Pathnomes, string destino) 
        {
            foreach (string nome in Pathnomes)
            {
                try
                {
                    string destinoNovo = Path.Combine(destino, Path.GetFileName(nome));

                    if (File.Exists(destinoNovo))
                    {
                        MessageBox.Show("Já existe um arquivo com esse nome no destino.");
                        continue;
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
                    //MessageBox.Show("pasta: " + destination);
                    //MessageBox.Show("destino novo: " + newDestination);

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


        public static string CriarNome(string nome, string destination)
        {
            int i = 1;
            string newName = nome;

            while (File.Exists(Path.Combine(destination, newName)))
            {
                newName = $"{nome}({i})";
                i++;
            }

            //MessageBox.Show("Nome: " + newName);
            return newName;
        }
        
        public static void ExecutarCaminho(string caminho)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            MessageBox.Show("caminho: " + caminho);

            if (caminho == null)
            {
                MessageBox.Show("Caminho não encontrado");
                return;
            }

            processInfo.FileName = caminho;
            processInfo.UseShellExecute = true;
            Process.Start(processInfo);
        }
    }

}
