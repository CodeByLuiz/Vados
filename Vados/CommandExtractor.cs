using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

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

    public interface CriteriaExtractor
    {
        GroupCollection Extract(string command, CommandCriteria criteria);
    }


    public class ActionExtractor : CriteriaExtractor
    {
        List<string> actions;
        List<string> starts;

        public ActionExtractor(List<string> starts_, List<string> actions_)
        {
            starts = starts_;
            actions = actions_;
        }

        public GroupCollection Extract(string command, CommandCriteria criteria)
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

            return match.Groups;
        }
    }


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

        public GroupCollection Extract(string command, CommandCriteria criteria)
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

            return match.Groups;
        }
    }


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

        public GroupCollection Extract(string command, CommandCriteria criteria)
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

            return match.Groups;
        }
    }
}
