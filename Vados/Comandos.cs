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
using Vosk;
using Microsoft.VisualBasic;
using NAudio.Wave;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Reflection.Metadata.Ecma335;
//using static System.Net.Mime.MediaTypeNames;

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


        #region RECONHECIMENTO DE VOZ

        //Termos ignorados no reconhecimento de voz
        public static List<string> speechIgnoreWords = new List<string>()
        {
            "[música]",
            "[Música]",
            "[MÚSICA]",
            "[música de fundo]",
            "[Música de fundo]",
            "[MÚSICA DE FUNDO]",
            "[aplausos]",
            "[Aplausos]",
            "[APLAUSOS]",
            "[risos]",
            "[Risos]",
            "[RISOS]",
            "[inaudível]",
            "[Inaudível]",
            "[INAUDÍVEL]",
            "[inaudible]",
            "[Inaudible]",
            "[INAUDIBLE]",
            "[ruído]",
            "[Ruído]",
            "[RUÍDO]",
            "[conversas]",
            "[Conversas]",
            "[CONVERSAS]",
            "[som de fundo]",
            "[Som de fundo]",
            "[SOM DE FUNDO]",
        };

        //Palavras aceitas/esperadas no reconhecimento de voz que não estão em nenhuma outra lista
        public static List<string> extraSpeechWords = new List<string>()
        {
            "para",
        };

        //Palavras comumente confundidas
        public static Dictionary<string, string> commonErrorSynonyms = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            //Vados
            { "matos", "Vados" },
            //Criar
            { "fiar", "criar" },
            //Renomear
            { "renomei", "renomeie" },
            { "procar", "trocar" },
            //Duplicar
            { "piar", "copiar" },
            //Arquivo
            { "aqui o", "arquivo" },
        };

        #endregion

        #region COMANDOS

        //Sinonimo chave de cada variação dos comandos
        static Dictionary<string, string> commandSynonyms = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            //Criar
            { "criar", "criar" },
            { "crie", "criar" },
            { "criasse", "criar" },
            { "criaria", "criar" },
            { "gerar", "criar" },
            { "gere", "criar" },
            { "gerasse", "criar" },
            { "geraria", "criar" },
            { "produzir", "criar" },
            { "produza", "criar" },
            { "produzisse", "criar" },
            { "produziria", "criar" },
            { "formar", "criar" },
            { "forme", "criar" },
            { "formasse", "criar" },
            { "formaria", "criar" },
            { "construir", "criar" },
            { "construa", "criar" },
            { "construisse", "criar" },
            { "construiria", "criar" },
            { "fazer", "criar" },
            { "faca", "criar" },
            { "fizesse", "criar" },
            { "faria", "criar" },
            { "inventar", "criar" },
            { "invente", "criar" },
            { "inventasse", "criar" },
            { "inventaria", "criar" },
            { "dar a luz", "criar" },
            { "de a luz", "criar" },
            { "desse a luz", "criar" },
            { "daria a luz", "criar" },
            { "originar", "criar" },
            { "origine", "criar" },
            { "originasse", "criar" },
            { "originaria", "criar" },
            //Renomear
            { "renomear", "renomear" },
            { "renomeie", "renomear" },
            { "renomeasse", "renomear" },
            { "renomearia", "renomear" },
            { "alterar o nome", "renomear" },
            { "altere o nome", "renomear" },
            { "alterasse o nome", "renomear" },
            { "alteraria o nome", "renomear" },
            { "rebatizar", "renomear" },
            { "rebatize", "renomear" },
            { "rebatizasse", "renomear" },
            { "rebatizaria", "renomear" },
            { "mudar o nome", "renomear" },
            { "mude o nome", "renomear" },
            { "mudasse o nome", "renomear" },
            { "mudaria o nome", "renomear" },
            { "trocar o nome", "renomear" },
            { "troque o nome", "renomear" },
            { "trocasse o nome", "renomear" },
            { "trocaria o nome", "renomear" },
            { "modificar o nome", "renomear" },
            { "modifique o nome", "renomear" },
            { "modificasse o nome", "renomear" },
            { "modificaria o nome", "renomear" },
            { "substituir o nome", "renomear" },
            { "substitua o nome", "renomear" },
            { "substitisse o nome", "renomear" },
            { "substituiria o nome", "renomear" },
            //Excluir
            { "excluir", "excluir" },
            { "exclua", "excluir" },
            { "excluisse", "excluir" },
            { "excluiria", "excluir" },
            { "deletar", "excluir" },
            { "delete", "excluir" },
            { "deletasse", "excluir" },
            { "deletaria", "excluir" },
            { "apagar", "excluir" },
            { "apague", "excluir" },
            { "apagasse", "excluir" },
            { "apagaria", "excluir" },
            { "remover", "excluir" },
            { "remova", "excluir" },
            { "removesse", "excluir" },
            { "removeria", "excluir" },
            { "eliminar", "excluir" },
            { "elimine", "excluir" },
            { "eliminasse", "excluir" },
            { "eliminaria", "excluir" },
            { "destruir", "excluir" },
            { "destrua", "excluir" },
            { "destruisse", "excluir" },
            { "destruiria", "excluir" },
            { "quebrar", "excluir" },
            { "quebre", "excluir" },
            { "quebrasse", "excluir" },
            { "quebraria", "excluir" },
            { "anular", "excluir" },
            { "anule", "excluir" },
            { "anulasse", "excluir" },
            { "anularia", "excluir" },
            { "desfazer", "excluir" },
            { "desfaca", "excluir" },
            { "desfizesse", "excluir" },
            { "desfaria", "excluir" },
            { "extinguir", "excluir" },
            { "extinga", "excluir" },
            { "extinguisse", "excluir" },
            { "extinguiria", "excluir" },
            { "obliterar", "excluir" },
            { "oblitere", "excluir" },
            { "obliterasse", "excluir" },
            { "obliteraria", "excluir" },
            //Mover
            { "mover", "mover" },
            { "mova", "mover" },
            { "movesse", "mover" },
            { "moveria", "mover" },
            { "transferir", "mover" },
            { "transfera", "mover" },
            { "transferisse", "mover" },
            { "transferiria", "mover" },
            { "realocar", "mover" },
            { "realoque", "mover" },
            { "realocasse", "mover" },
            { "realocaria", "mover" },
            { "colocar", "mover" },
            { "coloque", "mover" },
            { "colocasse", "mover" },
            { "colocaria", "mover" },
            { "deslocar", "mover" },
            { "desloque", "mover" },
            { "deslocasse", "mover" },
            { "deslocaria", "mover" },
            { "levar", "mover" },
            { "leve", "mover" },
            { "levasse", "mover" },
            { "levaria", "mover" },
            { "transportar", "mover" },
            { "transporte", "mover" },
            { "transportasse", "mover" },
            { "transportaria", "mover" },
            { "enviar", "mover" },
            { "envie", "mover" },
            { "enviasse", "mover" },
            { "enviaria", "mover" },
            { "movimentar", "mover" },
            { "movimente", "mover" },
            { "movimentasse", "mover" },
            { "movimentaria", "mover" },
            { "translocar", "mover" },
            { "transloque", "mover" },
            { "translocasse", "mover" },
            { "translocaria", "mover" },
            { "separar", "mover" },
            { "separe", "mover" },
            { "separasse", "mover" },
            { "separaria", "mover" },
            //Duplicar
            { "duplicar", "duplicar" },
            { "duplique", "duplicar" },
            { "duplicasse", "duplicar" },
            { "duplicaria", "duplicar" },
            { "copiar", "duplicar" },
            { "copie", "duplicar" },
            { "copiasse", "duplicar" },
            { "copiaria", "duplicar" },
            { "repetir", "duplicar" },
            { "repita", "duplicar" },
            { "repetisse", "duplicar" },
            { "repetiria", "duplicar" },
            { "reproduzir", "duplicar" },
            { "reproduza", "duplicar" },
            { "reproduzisse", "duplicar" },
            { "reproduziria", "duplicar" },
            { "imitar", "duplicar" },
            { "imite", "duplicar" },
            { "imitasse", "duplicar" },
            { "imitaria", "duplicar" },
            //Abrir
            { "abrir", "abrir" },
            { "abra", "abrir" },
            { "abrisse", "abrir" },
            { "abriria", "abrir" },
            { "iniciar", "abrir" },
            { "inicie", "abrir" },
            { "iniciasse", "abrir" },
            { "iniciaria", "abrir" },
            { "inicializar", "abrir" },
            { "inicialize", "abrir" },
            { "inicializasse", "abrir" },
            { "inicializaria", "abrir" },
            { "comecar", "abrir" },
            { "comece", "abrir" },
            { "comecasse", "abrir" },
            { "comecaria", "abrir" },
            { "despertar", "abrir" },
            { "desperte", "abrir" },
            { "despertasse", "abrir" },
            { "despertaria", "abrir" },
            { "acordar", "abrir" },
            { "acorde", "abrir" },
            { "acordasse", "abrir" },
            { "acordaria", "abrir" },
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
            { "site", "site" },
            { "link", "site" },
            { "pagina", "site" },
        };

        //Todas as variações de objetos (pasta / arquivo)
        public static List<string> allObjects = new List<string>(objectSynonyms.Keys);

        #endregion

        #region CAMINHOS / LINKS

        //Sinônimos da pasta padrão
        public static List<string> defaultFolderWords = new List<string>()
        {
            "padrao",
            "padrão",
            "nativa",
            "do app",
            "do aplicativo",
            "do programa",
            "do vados",
            "vados"
        };

        //Links associados à palavras
        static Dictionary<string, string> linkSynonyms = new Dictionary<string, string>()
        {
            //Bytes
            { "google", "https://google.com" },
            { "youtube", "https://www.youtube.com" },
            { "netflix", "https://www.netflix.com" },
            { "whatsapp", "https://www.whatsapp.com" },
            { "github", "https://github.com" },
            { "tradutor", "https://translate.google.com.br" },
            { "wikipedia", "https://pt.wikipedia.org/wiki/" },
        };

        public static List<string> allLinkWords = new List<string>(linkSynonyms.Keys);

        //Palavras para indicar o site
        public static List<string> linkNamingWords = new List<string>()
        {
            "da",
            "do",
            "do site",
        };

        #endregion

        #region TAMANHO

        //Variações das unidades de tamanho de arquivo
        static Dictionary<string, string> sizeUnitSynonyms = new Dictionary<string, string>()
        {
            //Bytes
            { "byte", "byte" },
            { "bytes", "byte" },
            { "bite", "byte" },
            { "bites", "byte" },
            //Kilobytes
            { "kilo", "kilo" },
            { "kilos", "kilo" },
            { "kilobyte", "kilo" },
            { "kilobytes", "kilo" },
            { "kilobite", "kilo" },
            { "kilobites", "kilo" },
            { "kbyte", "kilo" },
            { "kbytes", "kilo" },
            { "kbite", "kilo" },
            { "kbites", "kilo" },
            { "kb", "kilo" },
            { "kbs", "kilo" },
            //Megabytes
            { "mega", "mega" },
            { "megas", "mega" },
            { "megabyte", "mega" },
            { "megabytes", "mega" },
            { "megabite", "mega" },
            { "megabites", "mega" },
            { "mb", "mega" },
            { "mbs", "mega" },
            //Gigabytes
            { "giga", "giga" },
            { "gigas", "giga" },
            { "gigabyte", "giga" },
            { "gigabytes", "giga" },
            { "gigabite", "giga" },
            { "gigabites", "giga" },
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
            { "superiores a", "maior" },
            { "acima de", "maior" },
            { "mais alto que", "maior" },
            { "mais alta que", "maior" },
            { "mais altos que", "maior" },
            { "mais altas que", "maior" },
            { "mais que", "maior" },
            { "com mais que", "maior" },
            { "com mais de", "maior" },
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
            { "com menos que", "menor" },
            { "com menos de", "menor" },
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
            "que pesem",
            "que pesarem",
            "que tenha peso",
            "que tenham peso",
            "que tenha tamanho",
            "que tenham tamanho",
            "que ocupa",
            "que ocupam",
            "que ocupem",
            "que ocuparem",
            "que ocupa o espaço de",
            "que ocupam o espaço de",
            "que ocupem o espaço de",
            "que ocuparem o espaço de",
        };

        #endregion

        #region TODOS OS SINÔNIMOS

        //Todos os sinônimos
        static Dictionary<string, string> currentSynonyms0 = commandSynonyms.Concat(objectSynonyms).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        static Dictionary<string, string> currentSynonyms1 = currentSynonyms0.Concat(sizeUnitSynonyms).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        static Dictionary<string, string> currentSynonyms = currentSynonyms1.Concat(sizeModifierSynonyms).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        static Dictionary<string, string> wordSynonyms = new Dictionary<string, string>(currentSynonyms)
        {
            //Quantidade
            { "os", "todos" },
            { "as", "todos" },
            { "todos", "todos" },
            { "todos os", "todos" },
            { "todas as", "todos" },
            { "cada", "todos" },
            { "metade dos", "metade" },
            { "metade das", "metade" },
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

        #region PALAVRAS DE CONEXÃO

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
            "no interior da",
            "no interior do",
        };

        //Formas de indicar a pasta de destino (comando mover)
        public static List<string> insideWords = new List<string>()
        {
            "na",
            "no",
            "pra",
            "pro",
            "para",
            "para a",
            "para o",
            "pra dentro da",
            "pra dentro do",
            "para dentro da",
            "para dentro do",
            "pro interior da",
            "pro interior do",
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
            "associado a",
            "associada a",
            "associados a",
            "associadas a",
            "ligado a",
            "ligada a",
            "ligados a",
            "associadas a",
            "que pertence a",
            "que pertencem a",
            "que esta dentro da",
            "que estao dentro da",
            "que estiver dentro da",
            "que estiverem dentro da",
            "que esta no interior da",
            "que estao no interior da",
            "que estiver no interior da",
            "que estiverem no interior da",
            "que esta presente na",
            "que estao presentes na",
            "que estiver presente na",
            "que estiverem presentes na",
        };


        //Formas de nomear o arquivo / pasta
        public static List<string> namingWords = new List<string>()
        {
            "chamados",
            "chamado",
            "chamadas",
            "chamada",
            "que se chamam",
            "que se chamem",
            "que se chama",
            "que se chame",
            "nomeados",
            "nomeado",
            "nomeadas",
            "nomeada",
            "que se nomeam",
            "que se nomea",
            "que se nomee",
            "denominados",
            "denominado",
            "denominadas",
            "denominada",
            "que se denominam",
            "que se denominem",
            "que se denomina",
            "que se denomine",
            "intitulados",
            "intitulado",
            "intituladas",
            "intitulada",
            "que se intitulam",
            "que se intitulem",
            "que se intitula",
            "que se intitule",
            "de nomes",
            "de nome",
            "de titulos",
            "de titulo",
            "com nomes",
            "com nome",
            "com titulos",
            "com titulo",
            "que tenham o nome",
            "que tenham os nomes",
            "que tenha o nome",
            "que tenham os titulos",
            "que tenham o titulo",
            "que tenha o titulo",
            "que detenham os nomes",
            "que detenham o nome",
            "que detenha o nome",
            "que detenham os titulos",
            "que detenham o titulo",
            "que detenha o titulo",
            "que contenham os nomes",
            "que contenham o nome",
            "que contenha o nome",
            "que contenham os titulos",
            "que contenham o titulo",
            "que contenha o titulo",
            "que possuam os nomes",
            "que possuam o nome",
            "que possua o nome",
            "que possuam os titulos",
            "que possuam o titulo",
            "que possua o titulo",
            "que portam os nomes",
            "que portam o nome",
            "que porta o nome",
            "que portam os titulos",
            "que portam o titulo",
            "que porta o titulo",
            "que retenham os nomes",
            "que retenham o nome",
            "que retenha o nome",
            "que retenham os titulos",
            "que retenham o titulo",
            "que retenha o titulo",
            "que apresentam os nomes",
            "que apresentam o nome",
            "que apresenta o nome",
            "que apresentam os titulos",
            "que apresentam o titulo",
            "que apresenta o titulo",
        };

        //Formas de indicar a quantidade de arquivos / pastas
        public static List<string> amountWords = new List<string>()
        {
            "os",
            "as",
            "todos",
            "todas",
            "todos os",
            "todas os",
            "metade dos",
            "metade das",
            "cada",
        };

        #endregion

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

        public static string WordGetLink(string word)
        {
            //Retornar o link associado a palavra
            if (linkSynonyms.TryGetValue(word, out string link))
            {
                return link;
            }

            //Checar se já é um url válido
            Uri uriResult;
            bool isUrl = Uri.TryCreate(word, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (isUrl)
                return word;

            return "";
        }

        #endregion

        #endregion


        public static (CommandCriteria criteria, bool success) CommandGetArguments(string command)
        {
            CommandCriteria criteria = new CommandCriteria();

            //Definir tipo de comando
            ActionExtractor actionExtractor = new ActionExtractor(allCommands);
            actionExtractor.Extract(command, criteria);


            //Extrair argumentos para cada tipo de comando
            CommandParser parser = new CommandParser(criteria, new List<CriteriaExtractor>());

            switch (criteria.Action)
            {
                case "criar":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                    {
                        new DestinationExtractor(destinationWords, folderWords, namingWords),
                        new ObjectExtractor((amountWords, false), (allObjects, true), (allExtensionsWords, false), (namingWords, false), true, null, true)
                    });
                    break;

                case "renomear":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                    {
                        new SizeExtractor(sizeWords, allSizeModifierWords, allSizeUnitWords),
                        new NewNameExtractor(true),
                        new OriginExtractor(fromWords, folderWords, namingWords),
                        new ObjectExtractor((amountWords, false), (allObjects, true), (allExtensionsWords, false), (namingWords, false), false, null, true),
                    });
                    break;

                case "excluir":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                    {
                        new OriginExtractor(fromWords, folderWords, namingWords),
                        new SizeExtractor(sizeWords, allSizeModifierWords, allSizeUnitWords),
                        new ObjectExtractor((amountWords, false), (allObjects, true), (allExtensionsWords, false), (namingWords, false), false, fromWords, true),
                    });
                    break;


                case "mover":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                    {
                        new DestinationExtractor(insideWords, folderWords, namingWords, true),
                        new SizeExtractor(sizeWords, allSizeModifierWords, allSizeUnitWords),
                        new OriginExtractor(fromWords, folderWords, namingWords),
                        new ObjectExtractor((amountWords, false), (allObjects, true), (allExtensionsWords, false), (namingWords, false), false, fromWords, true),
                    });
                    break;


                case "duplicar":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                    {
                        new DestinationExtractor(insideWords, folderWords, namingWords),
                        new SizeExtractor(sizeWords, allSizeModifierWords, allSizeUnitWords),
                        new OriginExtractor(fromWords, folderWords, namingWords),
                        new ObjectExtractor((amountWords, false), (allObjects, true), (allExtensionsWords, false), (namingWords, false), false, null, true),
                    });
                    break;

                case "abrir":
                    parser = new CommandParser(criteria, new List<CriteriaExtractor>()
                    {
                        new OriginExtractor(fromWords, folderWords, namingWords),
                        new ObjectExtractor((amountWords, false), (allObjects, true), (allExtensionsWords, false), (namingWords, false), true, null, true),
                    });
                    break;

                //Comando não identificado
                default:
                    return (criteria, false);
            }

            var arguments = parser.Parse(command);
            //MessageBox.Show($"Comando: --{criteria.Action}*\r\nObjeto: --{criteria.ObjectType}*\r\nFormato: --{criteria.ObjectFormat}\r\nQuantidade: --{criteria.ObjectAmount}\r\nNome: --{criteria.ObjectName}\r\nNovo nome: --{criteria.ObjectNewName}\r\nOrigem: --{criteria.Origin}\r\nDestino: --{criteria.Destination}\r\nTamanho: --{criteria.SizeModifier} {criteria.SizeAmount} {criteria.SizeUnit}");

            return arguments;
        }


        //Retorna os caminhos encontrados conforme os critérios
        static async Task<(List<string> list, string errorMessage)> GetPaths(string objectType, string name, string format, string origin, string amountModifier, string size, string sizeUnit, string sizeModifier, List<string> priorities = null, List<string> exceptions = null)
        {
            if (priorities == null) priorities = Global.defaultPriorities;
            if (exceptions == null) priorities = Global.defaultExceptions;


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
                    List<string> newPaths = (await SearchPaths(name + "." + extension, objectType == "pasta",
                                                        rootFolder: origin, pathAmount: amountNumber, sizeLowerBound: lowerBound, sizeUpperBound: upperBound,
                                                        exceptions: exceptions, priorities: priorities).ConfigureAwait(false)).ToList();
                    paths.AddRange(newPaths);
                }
            }
            else
            {
                //Procura normal usando o nome                
                paths = (await SearchPaths(name, objectType == "pasta", rootFolder: origin, pathAmount: amountNumber, sizeLowerBound: lowerBound, sizeUpperBound: upperBound).ConfigureAwait(false)).ToList();
            }


            //---------ERRO: arquivo / pasta não encontrada---------
            if (paths.Count == 0)
            {
                var rtb = new RichTextBox();
                var bold = new Font(rtb.Font, FontStyle.Bold);

                string objectIndication = $"nenhum {objectType} chamado ";
                if (objectType == "pasta") objectIndication = "nenhuma pasta chamada ";

                Global.AppendPlainText(rtb, "Não foi possível encontrar " + objectIndication);
                Global.AppendFormattedText(rtb, name, Colors.greenHighlight, bold);
                if (origin != "")
                {
                    Global.AppendPlainText(rtb, " dentro da pasta ");
                    Global.AppendFormattedText(rtb, origin, Colors.greenHighlight, bold);
                }
                Global.AppendPlainText(rtb, ".");

                return (paths, rtb.Rtf);
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

            return (paths, "");
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
            sizeModifier = arguments.SizeModifier,
            objectPath = arguments.ObjectPath,
            originPath = arguments.OriginPath,
            destinationPath = arguments.DestinationPath;


            //Definições para arquivo executável (aplicativo)
            if (objectType == "aplicativo")
            {
                name.Replace(" ", "");
                format = "executavel";
            }


            #region---------ERRO: nome inválido (nome, origem ou destino)---------

            //Nome do arquivo inválido
            if (objectType != "pasta" && objectType != "site")
            {
                if (!string.IsNullOrEmpty(name) && !IsValidFileName(name) && objectPath == "")
                {
                    var rtb = new RichTextBox();
                    var bold = new Font(rtb.Font, FontStyle.Bold);
                    Global.AppendFormattedText(rtb, name, Colors.greenHighlight, bold);
                    Global.AppendPlainText(rtb, " é um nome de " + objectType + " inválido.");
                    return rtb.Rtf;
                }
            }


            //Nome da pasta (objeto, origem ou destino) inválido
            char[] invalidPathChars = Path.GetInvalidPathChars();

            if (objectType == "pasta") {
                var rtb = new RichTextBox();
                var bold = new Font(rtb.Font, FontStyle.Bold);

                if (name != "" && objectPath == "" && !IsValidFolderName(name))     //Pasta indicada
                    Global.AppendFormattedText(rtb, name, Colors.greenHighlight, bold);
                else if (origin != "" && originPath == "" && !IsValidFolderName(origin))    //Pasta de origem
                    Global.AppendFormattedText(rtb, origin, Colors.greenHighlight, bold);
                else if (destination != "" && destinationPath == "" && !IsValidFolderName(destination))     //Pasta de destino
                    Global.AppendFormattedText(rtb, destination, Colors.greenHighlight, bold);

                if (rtb.Text != "")
                {
                    Global.AppendPlainText(rtb, " é um nome de pasta inválido.");
                    return rtb.Rtf;
                }
            }

            #endregion----------------------------------------
            

            //Caminho da pasta de origem
            if (!string.IsNullOrEmpty(origin))
            {
                if (originPath == "")
                {
                    //Procurar caminho
                    originPath = (await SearchPaths(origin, true, pathAmount: 1).ConfigureAwait(false)).FirstOrDefault();
                }

                #region---------ERRO: pasta de origem não existe---------

                if (!Directory.Exists(originPath))
                {
                    var rtb = new RichTextBox();
                    var bold = new Font(rtb.Font, FontStyle.Bold);
                    Global.AppendPlainText(rtb, "Não foi possível encontrar a pasta de origem chamada ");
                    Global.AppendFormattedText(rtb, origin, Colors.greenHighlight, bold);
                    Global.AppendPlainText(rtb, ".");
                    return rtb.Rtf;
                }

                #endregion----------------------------------------
            }


            //Caminho da pasta de destino
            if (!string.IsNullOrEmpty(destination))
            {
                if (destinationPath == "")
                {
                    //Procurar caminho
                    destinationPath = (await SearchPaths(destination, true, pathAmount: 1).ConfigureAwait(false)).FirstOrDefault();
                }

                #region---------ERRO: pasta de destino não existe---------

                if (!Directory.Exists(destinationPath))
                {
                    var rtb = new RichTextBox();
                    var bold = new Font(rtb.Font, FontStyle.Bold);
                    Global.AppendPlainText(rtb, "Não foi possível encontrar a pasta de destino chamada ");
                    Global.AppendFormattedText(rtb, destination, Colors.greenHighlight, bold);
                    Global.AppendPlainText(rtb, ".");
                    return rtb.Rtf;
                }

                #endregion----------------------------------------
            }


            //Realizar comando
            (List<string> list, string errorMessage) paths = (new List<string>(), "");
            List<string> objectPathAsList = new List<string>() { objectPath };
            List<string> finalList = objectPathAsList;
            string fileName;

            switch (commandType)
            {
                //Criar
                case "criar":
                    fileName = name + "." + WordGetExtensions(format).FirstOrDefault();
                    if (objectType == "pasta") { return await CriarPasta(name, destinationPath); }
                    if (objectType == "arquivo") { return await CriarArquivo(fileName, destinationPath); }
                    break;


                //Renomear
                case "renomear":
                    //Realizar comando
                    fileName = name + "." + WordGetExtensions(format).FirstOrDefault();
                    if (objectType == "pasta") { return await RenomearPasta(name, newName, originPath); }
                    if (objectType == "arquivo") { return await RenomearArquivo(fileName, newName, originPath); }
                    break;


                //Excluir
                case "excluir":
                    if (objectPath == "")
                    {
                        //Procurar caminhos
                        paths = await GetPaths(objectType, name, format, originPath, amount, size, sizeUnit, sizeModifier).ConfigureAwait(false);
                        finalList = paths.list;
                    }
                    //Erro na busca
                    if (paths.errorMessage != "") return paths.errorMessage;

                    //Realizar comando
                    if (objectType == "pasta") { return await ExcluirPasta(finalList); }
                    if (objectType == "arquivo") { return await ExcluirArquivo(finalList); }
                    break;


                //Mover
                case "mover":
                    if (objectPath == "")
                    {
                        //Procurar caminhos
                        paths = await GetPaths(objectType, name, format, originPath, amount, size, sizeUnit, sizeModifier).ConfigureAwait(false);
                        finalList = paths.list;
                    }
                    //Erro na busca
                    if (paths.errorMessage != "") return paths.errorMessage;

                    //Realizar comando
                    if (objectType == "pasta") { return await MoverPasta(finalList, destinationPath); }
                    if (objectType == "arquivo") { return await MoverArquivo(finalList, destinationPath); }
                    break;


                //Duplicar
                case "duplicar":
                    if (objectPath == "")
                    {
                        //Procurar caminhos
                        paths = await GetPaths(objectType, name, format, originPath, amount, size, sizeUnit, sizeModifier).ConfigureAwait(false);
                        finalList = paths.list;
                    }
                    //Erro na busca
                    if (paths.errorMessage != "") return paths.errorMessage;

                    //Realizar comando
                    if (objectType == "pasta") { return await DuplicarPasta(finalList, destinationPath); }
                    if (objectType == "arquivo") { return await DuplicarArquivo(finalList, destinationPath); }
                    break;


                //Abrir
                case "abrir":
                    //Abrir link
                    if (objectType == "site")
                    {
                        return await AbrirLink(objectPath);
                    }


                    string finalPath = objectPath;

                    if (objectPath == "")
                    {
                        //Procurar caminhos
                        if (objectType == "arquivo" || objectType == "pasta")
                        {
                            paths = await GetPaths(objectType, name, format, originPath, amount, size, sizeUnit, sizeModifier).ConfigureAwait(false);
                        }
                        else
                        {
                            //Prioridades e excessões mais eficientes para procurar aplicativos
                            paths = await GetPaths("arquivo", name, format, originPath, amount, size, sizeUnit, sizeModifier, Global.exePriorities, Global.exeExceptions).ConfigureAwait(false);
                        }

                        finalPath = paths.list.FirstOrDefault();

                        //Erro na busca
                        if (paths.errorMessage != "") return paths.errorMessage;
                    }



                    if (objectType != "pasta")
                    {
                        string processArguments = "";

                        //Abrir lixeira
                        if (name == "lixeira")
                            processArguments = "shell:RecycleBinFolder";

                        //Abrir arquivo / programa
                        return await ExecutarCaminho(finalPath, processArguments);
                    }
                    else
                    {
                        //Abrir explorador de arquivos no caminho da pasta
                        finalPath = Path.Combine(finalPath, "x");   //Qualquer string serve
                        OpenFileExplorer(finalPath, true);
                    }
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


        #region RECONHECIMENTO DE VOZ

        //Filtra palavras indesejadas no texto
        public static string CleanText(string text)
        {
            //Deixar texto minúsculo e remover acentos
            text = text.ToLower();

            //Remover cada uma das palavras indesejadas
            foreach(var word in Comandos.speechIgnoreWords)
            {
                text = text.Replace(word, "");
            }

            return text;
        }

        
        //Corrige as palavras no texto que são comumente confundidas no comando de voz
        public static string CorrectCommonErrors(string command, Dictionary<string, string> corrections)
        {
            foreach (var pair in corrections)
            {
                command = command.Replace(pair.Key, pair.Value);
            }

            return command;
        }


        //Corrige o texto com as palavras mais parecidas
        public static string GetClosestMatch(string inputStr, List<string> hints, int maxDistance = 2)
        {
            inputStr = inputStr.ToLowerInvariant();
            var words = inputStr.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();    //Separa a string em uma lista de palavras
            var corrected = new List<string>();

            //Para cada palavra da string
            int i = 0;
            while (i < words.Count)
            {
                string bestMatch = null;        //Correspondência mais parecida
                int bestLen = 0;                //Número de palavras da possível correspondência
                int bestDist = int.MaxValue;    //Número de edições da melhor correspondência (menor possível)

                //Pular palavra se ela for muito pequena
                if (words[i].Length < 5)
                {
                    corrected.Add(words[i]);
                    i++;
                    continue;
                }


                //Pular palavra se a anterior for uma de nomeação (ex: chamado, de nome)
                if (corrected.Count > 0 && Comandos.namingWords.Contains(corrected[i - 1]))
                {
                    corrected.Add(words[i]);
                    i++;
                    continue;
                }


                //Para cada possivel correspondência
                foreach (var hint in hints)
                {
                    var hintWords = hint.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    //Ignorar caso a correspondência tenha mais palavras que a string
                    if (i + hintWords.Length > words.Count)
                        continue;

                    //Juntar palavras da string para ficar com a mesma quantidade da possível correspondência
                    string segment = string.Join(" ", words.Skip(i).Take(hintWords.Length));
                    int dist = LevenshteinDistance(segment, string.Join(" ", hintWords));   //Quantidade de edições

                    if (dist <= maxDistance && dist < bestDist)
                    {
                        //Definir melhor correspondência
                        bestMatch = hint;
                        bestLen = hintWords.Length;
                        bestDist = dist;
                    }
                }


                //Adicionar correspondência
                if (bestMatch != null)
                {
                    corrected.Add(bestMatch);
                    i += bestLen;
                }
                //Manter palavra original se não encontrar nenhuma correspondência
                else
                {
                    corrected.Add(words[i]);
                    i++;
                }
            }


            //Retornar frase modificada
            return string.Join(" ", corrected);
        }


        //Retorna o número de edições para uma palavra se tornar a outra    (ex: caixa -> baixo  =  2 edições)
        private static int LevenshteinDistance(string s, string t)
        {
            int[,] dp = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++)
                dp[i, 0] = i;

            for (int j = 0; j <= t.Length; j++)
                dp[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    dp[i, j] = Math.Min(
                        Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                        dp[i - 1, j - 1] + cost
                    );
                }
            }

            return dp[s.Length, t.Length];
        }


        public static List<int> PopulateAudioDevices()
        {
            List<int> deviceIds = new List<int>(); ;

            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var cap = WaveIn.GetCapabilities(i);
                deviceIds.Add(i);
            }

            return deviceIds;
        }

        #endregion

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
                //Procurar pasta de origem se não for indicado um caminho como argumento
                if (!Path.IsPathRooted(rootFolder))
                {
                    rootFolder = (await SearchPaths(rootFolder, true)).FirstOrDefault();
                    if (string.IsNullOrEmpty(rootFolder))   //Retornar lista vazia se não encontrar a pasta de origem
                        return resultados;
                }

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
                                //Console.WriteLine($"Acesso negado ao diretório: {atual}");
                                return Array.Empty<string>();
                            }
                        }).ConfigureAwait(false);

                        //Percorrer todas as pastas dentro da pasta atual
                        foreach (var folderPath in subpasta)
                        {
                            //Checar se está na pasta especificada
                            if (!string.IsNullOrEmpty(rootFolder) && !folderPath.Contains(rootFolder))
                                continue;

                            //Checar se a pasta tem o nome correto
                            string actualName = Path.GetFileName(folderPath);
                            if (!string.IsNullOrEmpty(searchName) && !actualName.Contains(searchName, StringComparison.OrdinalIgnoreCase))
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
                                //Console.WriteLine($"Acesso negado ao diretório: {atual}");
                                return Array.Empty<string>();
                            }
                        });

                        //Percorrer todos os arquivos da pasta atual
                        foreach (var filePath in files)
                        {
                            //Parar se o arquivo já tiver sido visitado
                            if (!visitados.Add(filePath)) 
                               continue;

                            //Checar se está na pasta especificada
                            if (!string.IsNullOrEmpty(rootFolder) && !filePath.Contains(rootFolder, StringComparison.OrdinalIgnoreCase))
                                continue;

                            //Checar se o arquivo tem o nome correto
                            string actualName = Path.GetFileName(filePath);


                            if (!string.IsNullOrEmpty(searchName) && !actualName.Contains(searchName, StringComparison.OrdinalIgnoreCase))
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
                    //Console.WriteLine($"Erro: {ex.Message}");
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


        #region RECONHECIMENTO DE VOZ

       // ainda to vendo alguma forma legal de fazer isso aqui dar certo :(

        #endregion


        public static string DriveGetFirst()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    return drive.Name;
                }
            }

            //MessageBox.Show("Nenhum drive disponível encontrado.");
            return "";
        }


        public static string CriarPastaPadrao()
        {
            string defaultFolderPath = Global.driverPath + @"Users\" + Environment.UserName + @"\Documents\Vados";

            try
            {
                if (!Directory.Exists(defaultFolderPath))
                    Directory.CreateDirectory(defaultFolderPath);

                return defaultFolderPath;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Erro ao criar a pasta padrão: " + ex.Message);
                return "";
            }
        }


        public static async Task<string> CriarPasta(string name, string destinationPath)
        {
            try
            {
                //Definir pasta padrão como pasta de destino caso não seja especificada
                if (string.IsNullOrEmpty(destinationPath)) 
                {
                    destinationPath = Global.DefaultFolder;
                }

                //Redefinir nome caso já exista um igual
                string finalName = CriarNome(name, destinationPath);


                string path = Path.Combine(destinationPath, finalName);

                if (!File.Exists(path))
                {
                    //Criar a pasta
                    Directory.CreateDirectory(path);
                    OpenFileExplorer(path, false);
                }
                //---------ERRO: arquivo com mesmo nome---------
                else
                {
                    var rtb = new RichTextBox();
                    var bold = new Font(rtb.Font, FontStyle.Bold);
                    Global.AppendPlainText(rtb, "Não foi possível criar uma pasta chamada ");
                    Global.AppendFormattedText(rtb, name, Colors.greenHighlight, bold);
                    Global.AppendPlainText(rtb, ", pois já existe um arquivo com esse nome.");
                    return rtb.Rtf;
                }
                //----------------------------------------

                return "";
            }

            //---------ERRO: erro não especificado---------
            catch (Exception ex)
            {
                var rtb = new RichTextBox();
                Global.AppendPlainText(rtb, "Não foi possível criar a pasta.\n");
                Global.AppendFormattedText(rtb, ex.Message, Color.Gray, rtb.Font);
                return rtb.Rtf;
            }
            //----------------------------------------
        }


        public static async Task<string> CriarArquivo(string name, string destinationPath)
        {

            try
            {
                //Definir pasta padrão como pasta de destino caso não seja especificada
                if (string.IsNullOrEmpty(destinationPath))
                {
                    destinationPath = Global.DefaultFolder;
                }

                //Redefinir nome caso já exista um igual
                string finalName = CriarNome(name, destinationPath);


                string path = Path.Combine(destinationPath, finalName);

                if (!Directory.Exists(path))
                {
                    //Criar arquivo
                    FileStream fs = File.Create(path);
                    OpenFileExplorer(path, false);
                }
                //---------ERRO: pasta com mesmo nome---------
                else
                {
                    var rtb = new RichTextBox();
                    var bold = new Font(rtb.Font, FontStyle.Bold);
                    Global.AppendPlainText(rtb, "Não foi possível criar um arquivo chamado ");
                    Global.AppendFormattedText(rtb, finalName, Colors.greenHighlight, bold);
                    Global.AppendPlainText(rtb, ", pois já existe uma pasta com esse nome.");
                    return rtb.Rtf;
                }
                //----------------------------------------

                return "";
            }

            //---------ERRO: erro não especificado---------
            catch (Exception ex)
            {
                var rtb = new RichTextBox();
                Global.AppendPlainText(rtb, "Não foi possível criar o arquivo.\n");
                Global.AppendFormattedText(rtb, ex.Message, Color.Gray, rtb.Font);
                return rtb.Rtf;
            }
            //----------------------------------------
        }


        public static async Task<string> ExcluirArquivo(List<string> paths) //exclui arquivo
        {
            try { 
                foreach (string path in paths) {

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

                return "";
            }

            //---------ERRO: erro não especificado---------
            catch (Exception ex)
            {
                var rtb = new RichTextBox();
                Global.AppendPlainText(rtb, "Não foi possível excluir o(s) arquivo(s).\n");
                Global.AppendFormattedText(rtb, ex.Message, Color.Gray, rtb.Font);
                return rtb.Rtf;
            }
            //----------------------------------------
        }

        public static async Task<string> ExcluirPasta(List<string> paths) // exclui pasta
        {
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
                    }
                }

                return "";
            }

            //---------ERRO: erro não especificado---------
            catch (Exception ex)
            {
                var rtb = new RichTextBox();
                Global.AppendPlainText(rtb, "Não foi possível excluir o(s) arquivo(s).\n");
                Global.AppendFormattedText(rtb, ex.Message, Color.Gray, rtb.Font);
                return rtb.Rtf;
            }
            //----------------------------------------
        }


        public static async Task<string> RenomearArquivo(string name, string newName, string originPath)
        {
            //MessageBox.Show(name);
            string path = (await SearchPaths(name, false, rootFolder: originPath)).FirstOrDefault();
            //MessageBox.Show(path);


            //---------ERRO: arquivo não encontrado---------
            if (string.IsNullOrEmpty(path))
            {
                var rtb = new RichTextBox();
                var bold = new Font(rtb.Font, FontStyle.Bold);
                Global.AppendPlainText(rtb, "Não foi possível encontrar o arquivo chamado ");
                Global.AppendFormattedText(rtb, name, Colors.greenHighlight, bold);
                Global.AppendPlainText(rtb, ".");
                return rtb.Rtf;
            }
            //----------------------------------------

            string newPath = Path.Combine(Path.GetDirectoryName(path), newName);

            //Renomear
            if (File.Exists(path))
            {
                File.Move(path, newPath);
            }

            return "";
        }


        public static async Task<string> RenomearPasta(string name, string newName, string originPath)
        {
            //MessageBox.Show(name);
            string path = (await SearchPaths(name, true, rootFolder: originPath)).FirstOrDefault();
            //MessageBox.Show(path);


            //---------ERRO: pasta não encontrada---------
            if (string.IsNullOrEmpty(path))
            {
                var rtb = new RichTextBox();
                var bold = new Font(rtb.Font, FontStyle.Bold);
                Global.AppendPlainText(rtb, "Não foi possível encontrar a pasta chamada ");
                Global.AppendFormattedText(rtb, name, Colors.greenHighlight, bold);
                Global.AppendPlainText(rtb, ".");
                return rtb.Rtf;
            }
            //----------------------------------------

            string newPath = Path.Combine(Path.GetDirectoryName(path) + @"\" + newName);

            if (Directory.Exists(path))
            {
                Directory.Move(path, newPath);
            }

            return "";
        }


        public static async Task<string> MoverPasta(List<string> paths, string destination)
        {
            try
            {
                foreach (string path in paths)
                {
                    string newPath = Path.Combine(destination, Path.GetFileName(path));

                    //---------ERRO: pasta de mesmo nome já existe---------
                    if (Directory.Exists(newPath))
                    {
                        var rtb = new RichTextBox();
                        var bold = new Font(rtb.Font, FontStyle.Bold);
                        Global.AppendPlainText(rtb, "Já existe uma pasta com o nome ");
                        Global.AppendFormattedText(rtb, Path.GetFileName(path), Colors.greenHighlight, bold);
                        Global.AppendPlainText(rtb, " na pasta ");
                        Global.AppendFormattedText(rtb, Path.GetFileName(destination), Colors.greenHighlight, bold);
                        Global.AppendPlainText(rtb, ".");
                        return rtb.Rtf;
                    }
                    //----------------------------------------

                    Directory.Move(path, newPath);
                }

                return "";
            }

            //---------ERRO: erro não especificado---------
            catch (Exception ex)
            {
                var rtb = new RichTextBox();
                Global.AppendPlainText(rtb, "Não foi possível mover a(s) pasta(s).\n");
                Global.AppendFormattedText(rtb, ex.Message, Color.Gray, rtb.Font);
                return rtb.Rtf;
            }
            //----------------------------------------
        }

        public static async Task<string> MoverArquivo(List<string> paths, string destination) 
        {
            try
            {
                foreach (string path in paths)
                {
                    string newPath = Path.Combine(destination, Path.GetFileName(path));

                    //---------ERRO: arquivo de mesmo nome já existe---------
                    if (File.Exists(newPath))
                    {
                        var rtb = new RichTextBox();
                        var bold = new Font(rtb.Font, FontStyle.Bold);
                        Global.AppendPlainText(rtb, "Já existe um arquivo com o nome ");
                        Global.AppendFormattedText(rtb, Path.GetFileName(path), Colors.greenHighlight, bold);
                        Global.AppendPlainText(rtb, " na pasta ");
                        Global.AppendFormattedText(rtb, Path.GetFileName(destination), Colors.greenHighlight, bold);
                        Global.AppendPlainText(rtb, ".");
                        return rtb.Rtf;
                    }
                    //----------------------------------------

                    File.Move(path, newPath);
                }

                return "";
            }

            //---------ERRO: erro não especificado---------
            catch (Exception ex)
            {
                var rtb = new RichTextBox();
                Global.AppendPlainText(rtb, "Não foi possível mover o(s) arquivos(s).\n");
                Global.AppendFormattedText(rtb, ex.Message, Color.Gray, rtb.Font);
                return rtb.Rtf;
            }
            //----------------------------------------
        }


        public static async Task<string> DuplicarPasta(List<string> paths, string destination)
        {
            try
            {
                foreach (string path in paths)
                {
                    //Definir mesma pasta como destino caso não seja especificada
                    if (string.IsNullOrEmpty(destination))
                    {
                        destination = Path.GetDirectoryName(path);
                    }

                    //Definir nome da pasta
                    string folderName = CriarNome(Path.GetFileName(path), destination);
                    string folderPath = Path.Combine(destination, folderName);


                    //---------ERRO: arquivo de mesmo nome já existe---------
                    if (Directory.Exists(folderPath))
                    {
                        var rtb = new RichTextBox();
                        var bold = new Font(rtb.Font, FontStyle.Bold);
                        Global.AppendPlainText(rtb, "Já existe um arquivo com o nome ");
                        Global.AppendFormattedText(rtb, Path.GetFileName(path), Colors.greenHighlight, bold);
                        Global.AppendPlainText(rtb, " na pasta ");
                        Global.AppendFormattedText(rtb, Path.GetFileName(destination), Colors.greenHighlight, bold);
                        Global.AppendPlainText(rtb, ".");
                        return rtb.Rtf;
                    }
                    //----------------------------------------


                    //Criar pasta na nova pasta
                    Directory.CreateDirectory(folderPath);

                    //Duplicar arquivos internos
                    foreach (string file in Directory.GetFiles(path))
                    {
                        string fileName = Path.GetFileName(file);
                        string fileNewPath = Path.Combine(folderPath, fileName);
                        File.Copy(file, fileNewPath, true);
                    }

                    //Duplicar subpastas internas
                    foreach (string subFolder in Directory.GetDirectories(path))
                    {
                        string subFolderName = Path.GetFileName(subFolder);
                        string subFolderPath = Path.Combine(folderPath, subFolderName);
                        var subpastas = new List<string>() { subFolder };
                        DuplicarPasta(subpastas, subFolderPath);
                    }
                }

                return "";
            }

            //---------ERRO: erro não especificado---------
            catch (Exception ex)
            {
                var rtb = new RichTextBox();
                Global.AppendPlainText(rtb, "Não foi possível duplicar a(s) pasta(s).\n");
                Global.AppendFormattedText(rtb, ex.Message, Color.Gray, rtb.Font);
                return rtb.Rtf;
            }
            //----------------------------------------
        }

        public static async Task<string> DuplicarArquivo(List<string> paths, string destination)
        {

            try
            {
                string newPath = "";

                foreach (string path in paths)
                {
                    //Definir mesma pasta como destino caso não seja especificada
                    if (string.IsNullOrEmpty(destination))
                    {
                        destination = Path.GetDirectoryName(path);
                    }

                    //Definir nome do arquivo
                    string fileName = CriarNome(Path.GetFileName(path), destination);
                    newPath = Path.Combine(destination, fileName);


                    //---------ERRO: arquivo de mesmo nome já existe---------
                    if (Directory.Exists(newPath))
                    {
                        var rtb = new RichTextBox();
                        var bold = new Font(rtb.Font, FontStyle.Bold);
                        Global.AppendPlainText(rtb, "Já existe uma pasta com o nome ");
                        Global.AppendFormattedText(rtb, Path.GetFileName(path), Colors.greenHighlight, bold);
                        Global.AppendPlainText(rtb, " na pasta ");
                        Global.AppendFormattedText(rtb, Path.GetFileName(destination), Colors.greenHighlight, bold);
                        Global.AppendPlainText(rtb, ".");
                        return rtb.Rtf;
                    }
                    //----------------------------------------


                    //Mover arquivo
                    File.Copy(path, newPath, false);
                }

                OpenFileExplorer(newPath, false);
                return "";
            }

            //---------ERRO: erro não especificado---------
            catch (Exception ex)
            {
                var rtb = new RichTextBox();
                Global.AppendPlainText(rtb, "Não foi possível duplicar o(s) arquivo(s).\n");
                Global.AppendFormattedText(rtb, ex.Message, Color.Gray, rtb.Font);
                return rtb.Rtf;
            }
            //----------------------------------------
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
                //MessageBox.Show("O programa precisa de permissões de administrador para funcionar corretamente.", "Permissão negada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            //MessageBox.Show("O idioma da interface foi alterado. O computador será reiniciado em 5 segundos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //nao consigo testar isso aqui,meu pc só tem o idioma pt-BR e nao consigo mudar, mas deve funcionar, testem no de vcs se der 


        }


        public static void ReiniciarPC() // reinicia o pc
        {
            Process.Start("shutdown", "/r /t 5");
            Application.Exit();
        }


        public static string CriarNome(string name, string destination)
        {
            int i = 2;
            string newName = name;


            while (Path.Exists(Path.Combine(destination, newName)))
            {
                newName = $"{name} ({i})";
                i++;
            }

            return newName;
        }
        
        public static async Task<string> ExecutarCaminho(string path, string arguments = "")
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();

            try
            {
                //Executar caminho
                processInfo.FileName = path;
                processInfo.Arguments = arguments;
                processInfo.UseShellExecute = true;
                Process.Start(processInfo);
            }

            //---------ERRO: erro não especificado---------
            catch (Exception ex)
            {
                var rtb = new RichTextBox();
                Global.AppendPlainText(rtb, "Não foi possível abrir o arquivo / programa.\n");
                Global.AppendFormattedText(rtb, ex.Message, Color.Gray, rtb.Font);
                return rtb.Rtf;
            }
            //----------------------------------------

            return "";
        }


        public static async Task<string> AbrirLink(string url)
        {
            try
            {
                //Abrir link
                Process.Start("explorer", url);
            }

            //---------ERRO: erro não especificado---------
            catch (Exception ex)
            {
                var rtb = new RichTextBox();
                Global.AppendPlainText(rtb, "Não foi possível abrir o link.\n");
                Global.AppendFormattedText(rtb, ex.Message, Color.Gray, rtb.Font);
                return rtb.Rtf;
            }
            //----------------------------------------

            return "";
        }


        //Testa determinados comandos em sequência
        public static void TestCases()
        {
            string testsPath = Path.Combine(Application.StartupPath, "Vados-Command-Tests.txt");
            MessageBox.Show(testsPath);
            string[] lines = File.ReadAllLines(testsPath);
            RichTextBox rtb = new RichTextBox();

            for (int i = 0; i < lines.Count(); i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                    continue;

                //Remover resultado da frente da string
                string command = lines[i].Substring(4, Math.Min(100 - 4, lines[i].Length - 4)).Trim();
                
                //Obter resultado
                var args = Comandos.CommandGetArguments(command);
                int success = args.success ? 1 : 0;
                //string errorMessage = await Comandos.ExecuteCommand(args.criteria);

                //Escrever resultado na frente da linha
                lines[i] = success.ToString() + " - " + command;


                //Espaçamento
                while (lines[i].Length < 100)
                {
                    lines[i] += " ";
                }


                //Mensagem de confirmação
                string message;

                if (args.success)
                {
                    message = FormMessage.SetConfirmationMessage(rtb, args.criteria);
                }
                else
                {
                    message = FormMessage.SetErrorMessage(rtb, args.criteria);
                }

                lines[i] += message;
            }

            //Reescrever linhas do arquivo
            File.WriteAllLines(testsPath, lines);
            ExecutarCaminho(testsPath);
        }
    }

}
