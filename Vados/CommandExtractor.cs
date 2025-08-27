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

namespace Vados
{
    public class CommandCriteria
    {
        public string Action = "";           //Tipo de comando
        public string ObjectType = "";       //Tipo de objeto (arquivo / pasta)
        public string ObjectName = "";       //Nome do objeto
        public string ObjectAmount = "";     //Quantidade de objetos ("todos")
        public string ObjectNewName = "";    //Novo nome do objeto (ao renomear)
        public string Destination = "";      //Nome da pasta de destino
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
            var match = Regex.Match(command, pattern, RegexOptions.IgnoreCase);

            //Extrair comando
            if (match.Success)
            {
                string action = match.Groups[3].Value;
                string correctAction = Comandos.WordGetSynonym(action);
                criteria.Action = correctAction;
            }

            string actionStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            MessageBox.Show("Comando -> " + actionStr);
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

            string pattern = $@"\b(({patternAmount})\s+)?({patternObject})(\s+de\s+({patternExtension}))?((\s+({patternNominator}))?\s+({patternName}))?(\s+para\s({patternName}))?";

            //Checar se o padrão está no comando
            var match = Regex.Match(command, pattern, RegexOptions.IgnoreCase);

            //Extrair argumentos
            if (match.Success)
            {
                //Tipo de objeto
                string obj = Comandos.WordGetSynonym(match.Groups[3].Value);
                criteria.ObjectType = obj;

                //Novo nome
                criteria.ObjectNewName = match.Groups[13].Success ? match.Groups[13].Value :
                                 match.Groups[14].Success ? match.Groups[14].Value :
                                 match.Groups[15].Value;

                //Quantidade
                criteria.ObjectAmount = Comandos.WordGetSynonym(match.Groups[2].Value);

                //Nome do objeto
                string name = match.Groups[9].Success ? match.Groups[9].Value :
                              match.Groups[10].Success ? match.Groups[10].Value :
                              match.Groups[11].Value;

                if (stopWords.Contains(name.ToLower())) return; //Checar se o nome não é uma das palavras de parada

                var extension = Comandos.WordGetExtensions(match.Groups[5].Value);
                if (extension.Count() != 0)
                {
                    //Adicionar a extensão ao nome
                    name += "." + extension[0];
                }

                criteria.ObjectName = name;
            }

            string objectStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            MessageBox.Show("Objeto -> " + objectStr);
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
            var match = Regex.Match(command, pattern, RegexOptions.IgnoreCase);

            //Extrair argumentos
            if (match.Success)
            {
                criteria.Destination = match.Groups[5].Success ? match.Groups[5].Value :
                                       match.Groups[6].Success ? match.Groups[6].Value :
                                       match.Groups[7].Value; ;
            }

            string destinationStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            MessageBox.Show("Destino -> " + destinationStr);
        }
    }
}
