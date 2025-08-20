using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vados
{
    public class CommandCriteria
    {
        public string Action;
        public string ObjectType;
        public string ObjectName;
        //public string Extension;
        public string Destination;
    }

    public interface CriteriaExtractor
    {
        GroupCollection Extract(string command, CommandCriteria criteria);
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

            string pattern = $@"\b({patternObject})(\s+de\s+({patternExtension}))?(\s+({patternNominator}))?\s('?)([^']+)\6";

            //Checar se o padrão está no comando
            var match = Regex.Match(command, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                string name = match.Groups[1].Value;
                var extension = Comandos.WordGetExtensions(name);
                if (extension.Count() != 0)
                {
                    name += "." + extension[0];
                }
                
                criteria.ObjectType = match.Groups[1].Value;
                //criteria.Extension = match.Groups[3].Value;
                criteria.ObjectName = match.Groups[7].Value;
            }

            return match.Groups;
        }
    }


    public class DetinationExtractor : CriteriaExtractor
    {
        List<string> insideIndicators;
        List<string> folders;
        List<string> nominators;

        public DetinationExtractor(List<string> insideIndicators_, List<string> folders_, List<string> nominators_)
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

            //string pattern = $@"\b({patternObject})(\s+de\s+({patternExtension}))?(\s+({patternNominator}))?\s('?)([^']+)\6";
            string pattern = $@"\b({patternInside})\s+({patternFolder})(\s+({patternNominator}))?\s('?)([^']+)\6";

            //Checar se o padrão está no comando
            var match = Regex.Match(command, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                string name = match.Groups[1].Value;
                var extension = Comandos.WordGetExtensions(name);
                if (extension.Count() != 0)
                {
                    name += "." + extension[0];
                }

                criteria.ObjectType = match.Groups[1].Value;
                criteria.Extension = match.Groups[3].Value;
                criteria.ObjectName = match.Groups[7].Value;
            }

            return match.Groups;
        }
    }
}
