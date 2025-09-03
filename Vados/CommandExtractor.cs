using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Vados
{
    public class CommandCriteria
    {
        public string Action = "";          //Tipo de comando
        public string ObjectType = "";      //Tipo de objeto (arquivo / pasta)
        public string ObjectName = "";      //Nome do objeto
        public string ObjectAmount = "";    //Quantidade de objetos (todos, metade)
        public string ObjectNewName = "";   //Novo nome do objeto (ao renomear)
        public string ObjectFormat = "";    //Formato do objeto (pode ser várias extensões)
        public string Origin = "";          //Nome da pasta de origem 
        public string Destination = "";     //Nome da pasta de destino
        public string SizeAmount = "";      //Tamanho (número)
        public string SizeModifier = "";    //Modificador do tamanho (maior, menor)
        public string SizeUnit = "";        //Unidade de tamanho (giga, mega)
    }


    //Retorna os argumentos do comando
    public class CommandParser
    {
        CommandCriteria criteria;
        List<CriteriaExtractor> extractors;

        public CommandParser(CommandCriteria criteria_, List<CriteriaExtractor> extractors_)
        {
            criteria = criteria_;
            extractors = extractors_;
        }

        public CommandCriteria Parse(string command)
        {
            //Extrair cada argumento do comando
            for (int i = 0; i < extractors.Count; i++)
            {
                CriteriaExtractor extractor = extractors[i];

                extractor.Extract(command, criteria);
            }

            return criteria;
        }
    }


    public interface CriteriaExtractor
    {
        void Extract(string command, CommandCriteria criteria);
    }


    //Extrai o tipo de comando
    public class ActionExtractor : CriteriaExtractor
    {
        List<string> actions;
        List<string> starts;

        public ActionExtractor(List<string> starts_, List<string> actions_)
        {
            starts = starts_;
            actions = actions_;
        }

        public void Extract(string command, CommandCriteria criteria)
        {
            string patternStarts = string.Join("|", starts.Select(Regex.Escape));
            string patternAction = string.Join("|", actions.Select(Regex.Escape));
            string pattern = $@"^(({patternStarts})\s+)?({patternAction})";

            //Checar se o padrão está no comando
            var match = Regex.Match(Comandos.RemoveDiacritics(command), pattern, RegexOptions.IgnoreCase);

            //Extrair comando
            if (match.Success)
            {
                string action = match.Groups[3].Value;
                string correctAction = Comandos.WordGetSynonym(action);
                criteria.Action = correctAction;
            }

            string actionStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            //MessageBox.Show("Comando -> " + actionStr);
        }
    }


    //Extrai o objeto (arquivo / pasta), seu nome e seu novo nome
    public class ObjectExtractor : CriteriaExtractor
    {
        List<string> objects;
        List<string> amount;
        List<string> extensions;
        List<string> nominators;
        List<string> stopWords;

        public ObjectExtractor(List<string> amount_, List<string> objects_, List<string> extensions_, List<string> nominators_, List<string> stopWords_ = null)
        {
            amount = amount_;
            objects = objects_;
            extensions = extensions_;
            nominators = nominators_;
            stopWords = stopWords_;

            if (stopWords == null) stopWords = new List<string>();
        }

        public void Extract(string command, CommandCriteria criteria)
        {
            string patternObject = string.Join("|", objects.Select(Regex.Escape));
            string patternAmount = string.Join("|", amount.Select(Regex.Escape));
            string patternExtension = string.Join("|", extensions.Select(Regex.Escape));
            string patternNominator = string.Join("|", nominators.Select(Regex.Escape));
            string patternName = @"?:'([^']+)'|""([^""]+)""|([^'""\s]+)";

            string pattern = $@"\b(({patternAmount})\s+)?({patternObject})(\s+de\s+({patternExtension}))?((\s+({patternNominator}))?\s+({patternName}))?";

            //Checar se o padrão está no comando
            var match = Regex.Match(Comandos.RemoveDiacritics(command), pattern, RegexOptions.IgnoreCase);

            //Extrair argumentos
            if (match.Success)
            {
                //Tipo de objeto
                string obj = Comandos.WordGetSynonym(match.Groups[3].Value);
                criteria.ObjectType = obj;

                //Formato do objeto
                criteria.ObjectFormat = match.Groups[5].Value;

                //Quantidade
                criteria.ObjectAmount = Comandos.WordGetSynonym(match.Groups[2].Value);

                //Nome do objeto
                string name = match.Groups[9].Success ? match.Groups[9].Value :
                              match.Groups[10].Success ? match.Groups[10].Value :
                              match.Groups[11].Value;

                if (stopWords.Contains(name.ToLower())) return; //Checar se o nome não é uma das palavras de parada

                criteria.ObjectName = name;
            }

            string objectStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            //MessageBox.Show("Objeto -> " + objectStr);
        }
    }


    //Extrai o objeto (arquivo / pasta), seu nome e seu novo nome
    public class NewNameExtractor : CriteriaExtractor
    {
        public NewNameExtractor(){}

        public void Extract(string command, CommandCriteria criteria)
        {
            string patternName = @"?:'([^']+)'|""([^""]+)""|([^'""\s]+)";

            string pattern = $@"\b(\s+(para|pra)\s+({patternName}))";

            //Checar se o padrão está no comando
            var match = Regex.Match(Comandos.RemoveDiacritics(command), pattern, RegexOptions.IgnoreCase);

            //Extrair argumentos
            if (match.Success)
            {
                //Novo nome
                criteria.ObjectNewName = match.Groups[3].Success ? match.Groups[3].Value :
                                 match.Groups[4].Success ? match.Groups[4].Value :
                                 match.Groups[5].Value;
            }

            string newNameStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            //MessageBox.Show("Novo nome -> " + newNameStr);
        }
    }


    //Extrai a pasta de origem
    public class OriginExtractor : CriteriaExtractor
    {
        List<string> fromIndicators;
        List<string> folders;
        List<string> nominators;

        public OriginExtractor(List<string> fromIndicators_, List<string> folders_, List<string> nominators_)
        {
            fromIndicators = fromIndicators_;
            folders = folders_;
            nominators = nominators_;
        }

        public void Extract(string command, CommandCriteria criteria)
        {
            string patternFrom = string.Join("|", fromIndicators.Select(Regex.Escape));
            string patternFolder = string.Join("|", folders.Select(Regex.Escape));
            string patternNominator = string.Join("|", nominators.Select(Regex.Escape));
            string patternName = @"?:'([^']+)'|""([^""]+)""|([^'""\s]+)";

            string pattern = $@"\b({patternFrom})\s+({patternFolder})(\s+({patternNominator}))?\s+({patternName})";

            //Checar se o padrão está no comando
            var match = Regex.Match(Comandos.RemoveDiacritics(command), pattern, RegexOptions.IgnoreCase);

            //Extrair argumentos
            if (match.Success)
            {
                criteria.Origin = match.Groups[5].Success ? match.Groups[5].Value :
                                       match.Groups[6].Success ? match.Groups[6].Value :
                                       match.Groups[7].Value; ;
            }

            string originStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            //MessageBox.Show("Origem -> " + originStr);
        }
    }


    //Extrai a pasta de destino
    public class DestinationExtractor : CriteriaExtractor
    {
        List<string> insideIndicators;
        List<string> folders;
        List<string> nominators;

        public DestinationExtractor(List<string> insideIndicators_, List<string> folders_, List<string> nominators_)
        {
            insideIndicators = insideIndicators_;
            folders = folders_;
            nominators = nominators_;
        }

        public void Extract(string command, CommandCriteria criteria)
        {
            string patternInside = string.Join("|", insideIndicators.Select(Regex.Escape));
            string patternFolder = string.Join("|", folders.Select(Regex.Escape));
            string patternNominator = string.Join("|", nominators.Select(Regex.Escape));
            string patternName = @"?:'([^']+)'|""([^""]+)""|([^'""\s]+)";

            string pattern = $@"\b({patternInside})\s+({patternFolder})(\s+({patternNominator}))?\s({patternName})";

            //Checar se o padrão está no comando
            var match = Regex.Match(Comandos.RemoveDiacritics(command), pattern, RegexOptions.IgnoreCase);

            //Extrair argumentos
            if (match.Success)
            {
                criteria.Destination = match.Groups[5].Success ? match.Groups[5].Value :
                                       match.Groups[6].Success ? match.Groups[6].Value :
                                       match.Groups[7].Value; ;
            }

            string destinationStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            //MessageBox.Show("Destino -> " + destinationStr);
        }
    }


    //Extrai o tamanho do arquivo / pasta
    public class SizeExtractor : CriteriaExtractor
    {
        List<string> sizeIndicators;
        List<string> sizeModifiers;
        List<string> sizeUnits;

        public SizeExtractor(List<string> sizeIndicators_, List<string> sizeModifiers_, List<string> sizeUnits_)
        {
            sizeIndicators = sizeIndicators_;
            sizeModifiers = sizeModifiers_;
            sizeUnits = sizeUnits_;
        }

        public void Extract(string command, CommandCriteria criteria)
        {
            string patternIndicator = string.Join("|", sizeIndicators.Select(Regex.Escape));
            string patternModifier = string.Join("|", sizeModifiers.Select(Regex.Escape));
            string patternUnit = string.Join("|", sizeUnits.Select(Regex.Escape));

            string pattern = $@"\b({patternIndicator})?(\s+({patternModifier}))?\s+(\d+)\s+({patternUnit})";

            //Checar se o padrão está no comando
            var match = Regex.Match(Comandos.RemoveDiacritics(command), pattern, RegexOptions.IgnoreCase);

            //Extrair argumentos
            if (match.Success)
            {
                //Tamanho
                criteria.SizeAmount = match.Groups[4].Value;

                //Modificador
                criteria.SizeModifier = Comandos.WordGetSynonym(match.Groups[3].Value);

                //Unidade
                criteria.SizeUnit = Comandos.WordGetSynonym(match.Groups[5].Value);
            }

            string objectStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            //MessageBox.Show("Tamanho -> " + objectStr);
        }
    }
}
