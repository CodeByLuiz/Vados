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
        public string Action;
        public string ObjectType;
        public string ObjectName;
        public string ObjectNewName;
        public string Destination;
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
        List<string> extensions;
        List<string> nominators;

        public ObjectExtractor(List<string> objects_, List<string> extensions_, List<string> nominators_)
        {
            objects = objects_;
            extensions = extensions_;
            nominators = nominators_;
        }

        public void Extract(string command, CommandCriteria criteria)
        {
            string patternObject = string.Join("|", objects.Select(Regex.Escape));
            string patternExtension = string.Join("|", extensions.Select(Regex.Escape));
            string patternNominator = string.Join("|", nominators.Select(Regex.Escape));
            string patternName = @"?:'([^']+)'|""([^""]+)""|([^'""\s]+)";

            string pattern = $@"\b({patternObject})(\s+de\s+({patternExtension}))?(\s+({patternNominator}))?\s+({patternName})(\s+para\s({patternName}))?";

            //Checar se o padrão está no comando
            var match = Regex.Match(command, pattern, RegexOptions.IgnoreCase);

            //Extrair argumentos
            if (match.Success)
            {
                //Tipo de objeto
                string obj = Comandos.WordGetSynonym(match.Groups[1].Value);
                criteria.ObjectType = obj;

                //Nome do objeto
                string name = match.Groups[6].Success ? match.Groups[6].Value : 
                              match.Groups[7].Success ? match.Groups[7].Value :
                              match.Groups[8].Value;
                var extension = Comandos.WordGetExtensions(name);
                if (extension.Count() != 0)
                {
                    name += "." + extension[0];
                }

                criteria.ObjectName = name;

                //Novo nome
                criteria.ObjectNewName = match.Groups[10].Success ? match.Groups[10].Value :
                                 match.Groups[11].Success ? match.Groups[11].Value :
                                 match.Groups[12].Value;
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
