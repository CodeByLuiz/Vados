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
        public int ActionPos = 0;           //Posição da string onde se encontra o tipo de comando
        public string Action = "";          //Tipo de comando
        public string ObjectType = "";      //Tipo de objeto (arquivo / pasta)
        public string ObjectName = "";      //Nome do objeto
        public string ObjectPath = "";      //Caminho do objeto
        public string ObjectAmount = "";    //Quantidade de objetos (todos, metade)
        public string ObjectNewName = "";   //Novo nome do objeto (ao renomear)
        public string ObjectFormat = "";    //Formato do objeto (pode ser várias extensões)
        public string Origin = "";          //Nome da pasta de origem 
        public string OriginPath = "";      //Caminho da pasta de origem 
        public string Destination = "";     //Nome da pasta de destino
        public string DestinationPath = ""; //Caminho da pasta de destino
        public string SizeAmount = "";      //Tamanho (número)
        public string SizeModifier = "";    //Modificador do tamanho (maior, menor)
        public string SizeUnit = "";        //Unidade de tamanho (giga, mega)

        public int PostObjectNameIndex = -1;    //Posição da palavra após o nome do objeto
        public int PostOriginNameIndex = -1;    //Posição da palavra após o nome da pasta de origem
        public int PostNewNameIndex = -1;       //Posição da palavra após o novo nome do objeto
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

        public (CommandCriteria criteria, bool success) Parse(string command)
        {
            //Extrair cada argumento do comando
            for (int i = 0; i < extractors.Count; i++)
            {
                var extractor = extractors[i];
                //MessageBox.Show("extractor " + i.ToString() + " " + extractor.required.ToString());
                bool success = extractor.Extract(command, criteria);

                //Retornar falso se algum critério obrigatório estiver faltando
                if (extractor.required && !success) return (criteria, false);
            }

            return (criteria, true);
        }
    }


    public class Pattern
    {
        public List<string> Values;
        public bool Required;

        public Pattern(List<string> values, bool required = false)
        {
            Values = values ?? new List<string>();
            Required = required;


        }

        //Transforma a lista em um padrão aceito pelo regex, adicionando o ? caso não seja obrigatória
        public string ToPattern()
        {
            if (Values.Count == 0) return string.Empty;

            string joined = string.Join("|", Values.Select(Regex.Escape));
            return $"({joined})";
        }

        //Adiciona um ? no final do padrão caso não seja obrigatório
        public string ToRequired()
        {
            if (Required) return string.Empty;
            return "?";
        }
    }


    public class CriteriaExtractor
    {
        public virtual bool Extract(string command, CommandCriteria criteria) { return true; }
        public bool required = false;
    }


    //Extrai o tipo de comando
    public class ActionExtractor : CriteriaExtractor
    {
        List<string> actions;

        public ActionExtractor(List<string> actions_)
        {
            actions = actions_;
        }

        public override bool Extract(string command, CommandCriteria criteria)
        {
            command = command.ToLower();
            string patternAction = string.Join("|", actions.Select(Regex.Escape));
            string pattern = $@"({patternAction})";

            //Checar se o padrão está no comando
            var match = Regex.Match(Comandos.RemoveDiacritics(command), pattern, RegexOptions.IgnoreCase);

            //Extrair comando
            if (match.Success)
            {
                string action = match.Groups[1].Value;
                string correctAction = Comandos.WordGetSynonym(action);
                criteria.Action = correctAction.ToLower();
                criteria.ActionPos = match.Index + action.Length;
            }

            string actionStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            //MessageBox.Show("Comando -> " + actionStr);
            return match.Success;
        }
    }


    //Extrai o objeto (arquivo / pasta), seu nome e seu novo nome
    public class ObjectExtractor : CriteriaExtractor
    {
        Pattern objects;
        List<string> objectsList;
        Pattern amount;
        Pattern extensions;
        Pattern nominators;
        bool nameIsRequired = false;
        List<string> stopWords;

        public ObjectExtractor((List<string> v, bool r) amount_, (List<string> v, bool r)  objects_, (List<string> v, bool r)  extensions_, (List<string> v, bool r)  nominators_, bool nameIsRequired_, List<string> stopWords_ = null, bool required_ = false)
        {
            //amount_ = (lista, é obrigatório) --> isso para todos
            amount = new Pattern(amount_.v, amount_.r);
            objectsList = objects_.v;
            objects = new Pattern(objects_.v, objects_.r);
            extensions = new Pattern(extensions_.v, extensions_.r);
            nominators = new Pattern(nominators_.v, nominators_.r);
            required = required_;
            nameIsRequired = nameIsRequired_;
            stopWords = stopWords_;

            if (stopWords == null) stopWords = new List<string>();
        }

        public override bool Extract(string command, CommandCriteria criteria)
        {
            command = command.Substring(criteria.ActionPos).ToLower();
            command = Comandos.RemoveDiacritics(command);

            string BuildPattern()
            {
                string nameRequirement = nameIsRequired ? "" : "?";
                string patternName = @"?:'([^']+)'|""([^""]+)""|([^'""\s]+)";
                string pattern = $@"({amount.ToPattern()}\s+){amount.ToRequired()}" +
                                 $@"{objects.ToPattern()}{objects.ToRequired()}" +
                                 $@"(\s+de\s+{extensions.ToPattern()}){extensions.ToRequired()}" +
                                 $@"(\s+{nominators.ToPattern()}){nominators.ToRequired()}";// +
                                 //$@"\s+({patternName})){nameRequirement}";

                return pattern;
            }

            //Checar se o padrão está no comando
            var pattern = BuildPattern();
            var match = Regex.Match(command, pattern, RegexOptions.IgnoreCase);

            //Tipo de objeto
            string obj = Comandos.WordGetSynonym(match.Groups[3].Value);


            #region TENTAR CORRESPONDÊNCIA NOVAMENTE (em casos específicos)

            //Trocar palavras de nomeação caso o tipo de objeto seja site
            if (obj == "site")
            {
                List<string> newNominators = nominators.Values.Concat(Comandos.linkNamingWords).ToList();
                nominators = new Pattern(newNominators, false);
                match = Regex.Match(Comandos.RemoveDiacritics(command), BuildPattern(), RegexOptions.IgnoreCase);
            }

            //Se não encontrar o tipo de objeto, tentar corresponder o nome de outra forma
            if (obj == "")
            {
                objects = new Pattern(objectsList, false);
                nominators = new Pattern(new List<string>() { "o", "a", "os", "as" }, false);
                match = Regex.Match(Comandos.RemoveDiacritics(command), BuildPattern(), RegexOptions.IgnoreCase);
                MessageBox.Show(BuildPattern());
            }

            #endregion


            if (match.Success)
            {
                //Tipo do objeto
                criteria.ObjectType = obj;

                //Formato do objeto
                criteria.ObjectFormat = match.Groups[5].Value;

                //Quantidade
                criteria.ObjectAmount = Comandos.WordGetSynonym(match.Groups[2].Value);

                //Nome do objeto
                string objName = match.Groups[9].Success ? match.Groups[9].Value :
                                match.Groups[10].Success ? match.Groups[10].Value :
                                match.Groups[11].Value;

                //Idenfificar nome do arquivo
                var lastGroup = match.Groups[Global.FindLastGroupIndex(match.Groups)];
                int startIndex = lastGroup.Index + lastGroup.Length;
                bool hasName = criteria.PostObjectNameIndex == -1 || criteria.PostObjectNameIndex > startIndex + 1;

                if (hasName)
                {
                    //Posição de parada do nome
                    int stopIndex = command.Length;
                    if (criteria.PostObjectNameIndex != -1)
                    {
                        stopIndex = criteria.PostObjectNameIndex;
                    }

                    int nameLength = stopIndex - startIndex;
                    objName = command.Substring(startIndex, nameLength).Trim();

                    objName = objName.Replace("\"", "");
                    objName = objName.Replace("\'", "");
                }
                else
                {
                    objName = "";
                }


                //Definir tipo de objeto caso não definido
                if (criteria.ObjectType == "" && criteria.Action == "abrir")
                {
                    criteria.ObjectType = "aplicativo";

                    if (Comandos.allLinkWords.Contains(objName))
                        criteria.ObjectType = "site";

                    //Lixeira
                    if (objName == "lixeira")
                    {
                        criteria.ObjectPath = "explorer.exe";
                    }

                    //Navegador
                    if (objName == "navegador")
                    {
                        criteria.ObjectType = "site";
                        criteria.ObjectPath = "https://";
                    }
                }


                #region CAMINHOS PREDEFINIDOS

                //Palavras associadas à arquivos / pastas específicas
                if (criteria.ObjectType == "pasta")
                {
                    bool isSpecificName = !match.Groups[11].Success || match.Groups[8].Success;

                    //Pasta padrão
                    if (Comandos.defaultFolderWords.Contains(objName) && !isSpecificName)
                    {
                        criteria.ObjectPath = Global.DefaultFolder;
                    }
                }

                //Definir link do site
                if (criteria.ObjectType == "site" && criteria.ObjectPath == "")
                {
                    criteria.ObjectPath = Comandos.WordGetLink(objName);
                }

                #endregion


                //Desconsiderar o nome se não houver o tipo de arquivo
                if (criteria.ObjectType == "")
                    objName = "";

                criteria.ObjectName = objName;

                string objectStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
                MessageBox.Show("Objeto -> " + objectStr);

                //Casos de nome vazio
                if (string.IsNullOrEmpty(objName))
                {
                    if (nameIsRequired) return false;

                    //Retornar falso se não houver nenhuma indicação do objeto (sem formato, quantidade ou tamanho)
                    if (criteria.ObjectFormat == "" && criteria.ObjectAmount == "" && criteria.SizeAmount == "")
                        return false;
                }
            }

            return match.Success;
        }
    }


    //Extrai o objeto (arquivo / pasta), seu nome e seu novo nome
    public class NewNameExtractor : CriteriaExtractor
    {
        public NewNameExtractor(bool required_ = false)
        {
            required = required_;
        }

        public override bool Extract(string command, CommandCriteria criteria)
        {
            command = command.ToLower().Substring(criteria.ActionPos);
            string patternName = @"?:'([^']+)'|""([^""]+)""|([^'""\s]+)";
            string pattern = $@"(\s+(para|pra))";//\s+({patternName}))";

            //Checar se o padrão está no comando
            var match = Regex.Match(Comandos.RemoveDiacritics(command), pattern, RegexOptions.IgnoreCase);

            //Extrair argumentos
            if (match.Success) {
                string newName = "";
                //Idenfificar nome do novo arquivo
                var lastGroup = match.Groups[Global.FindLastGroupIndex(match.Groups)];
                int startIndex = lastGroup.Index + lastGroup.Length;
                bool hasName = criteria.PostNewNameIndex == -1 || criteria.PostNewNameIndex > startIndex + 1;

                if (hasName)
                {
                    //Posição de parada do nome
                    int stopIndex = command.Length;
                    if (criteria.PostNewNameIndex != -1)
                    {
                        stopIndex = criteria.PostNewNameIndex;
                    }

                    int nameLength = stopIndex - startIndex;
                    newName = command.Substring(startIndex, nameLength).Trim();

                    newName = newName.Replace("\"", "");
                    newName = newName.Replace("\'", "");
                }
                else
                {
                    newName = "";
                }

                criteria.ObjectNewName = newName;

                //MessageBox.Show("new name " + criteria.ObjectNewName);

                //Palavra após o nome do objeto
                criteria.PostObjectNameIndex = Global.FindFirstGroupIndex(match.Groups);

                if (criteria.ObjectNewName == "" && required)
                    return false;
            }

            string newNameStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            //MessageBox.Show("Novo nome -> " + newNameStr);
            return match.Success;
        }
    }


    //Extrai a pasta de origem
    public class OriginExtractor : CriteriaExtractor
    {
        List<string> fromIndicators;
        List<string> folders;
        List<string> nominators;

        public OriginExtractor(List<string> fromIndicators_, List<string> folders_, List<string> nominators_, bool required_ = false)
        {
            fromIndicators = fromIndicators_;
            folders = folders_;
            nominators = nominators_;
            required = required_;
        }

        public override bool Extract(string command, CommandCriteria criteria)
        {
            command = command.ToLower().Substring(criteria.ActionPos);
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
                                       match.Groups[7].Value;

                //Idenfificar nome composto sem aspas
                int nameGroup = 7;

                if (match.Groups[nameGroup].Success)
                {
                    //Posição de parada do nome
                    int stopIndex = command.Length;
                    if (criteria.PostOriginNameIndex != -1)
                    {
                        stopIndex = criteria.PostOriginNameIndex;
                    }

                    //MessageBox.Show(criteria.PostOriginNameIndex.ToString());

                    int startIndex = match.Groups[nameGroup].Index;
                    int nameLength = stopIndex - startIndex;
                    criteria.Origin = command.Substring(startIndex, nameLength).Trim();
                }

                //Pasta padrão
                if (Comandos.defaultFolderWords.Contains(criteria.Origin) && match.Groups[7].Success)
                {
                    criteria.OriginPath = Global.DefaultFolder;
                }


                //Palavra após o nome do objeto
                criteria.PostObjectNameIndex = Global.FindFirstGroupIndex(match.Groups);

                //Palavra após o novo nome do objeto
                criteria.PostNewNameIndex = Global.FindFirstGroupIndex(match.Groups);
            }

            string originStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            //MessageBox.Show("Origem -> " + originStr);
            return match.Success;
        }
    }


    //Extrai a pasta de destino
    public class DestinationExtractor : CriteriaExtractor
    {
        List<string> insideIndicators;
        List<string> folders;
        List<string> nominators;

        public DestinationExtractor(List<string> insideIndicators_, List<string> folders_, List<string> nominators_, bool required_ = false)
        {
            insideIndicators = insideIndicators_;
            folders = folders_;
            nominators = nominators_;
            required = required_;
        }

        public override bool Extract(string command, CommandCriteria criteria)
        {
            command = command.ToLower().Substring(criteria.ActionPos);
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
                                       match.Groups[7].Value;

                //Idenfificar nome composto sem aspas
                int nameGroup = 7;

                if (match.Groups[nameGroup].Success)
                {
                    //Posição de parada do nome
                    int stopIndex = command.Length;
                    int startIndex = match.Groups[nameGroup].Index;
                    int nameLength = stopIndex - startIndex;
                    criteria.Destination = command.Substring(startIndex, nameLength).Trim();
                }

                //Pasta padrão
                if (Comandos.defaultFolderWords.Contains(criteria.Destination) && match.Groups[7].Success)
                {
                    criteria.DestinationPath = Global.DefaultFolder;
                }


                //Palavra após o nome do objeto
                int index = Global.FindFirstGroupIndex(match.Groups);
                criteria.PostObjectNameIndex = index;

                //Palavra após o novo nome do objeto
                criteria.PostNewNameIndex = index;

                //Palavra após o nome da pasta de origem
                criteria.PostOriginNameIndex = index;
            }

            string destinationStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            MessageBox.Show("Destino -> " + destinationStr);
            return match.Success;
        }
    }


    //Extrai o tamanho do arquivo / pasta
    public class SizeExtractor : CriteriaExtractor
    {
        List<string> sizeIndicators;
        List<string> sizeModifiers;
        List<string> sizeUnits;

        public SizeExtractor(List<string> sizeIndicators_, List<string> sizeModifiers_, List<string> sizeUnits_, bool required_ = false)
        {
            sizeIndicators = sizeIndicators_;
            sizeModifiers = sizeModifiers_;
            sizeUnits = sizeUnits_;
            required = required_;
        }

        public override bool Extract(string command, CommandCriteria criteria)
        {
            command = command.ToLower().Substring(criteria.ActionPos);
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

                //Palavra após o nome do objeto
                criteria.PostObjectNameIndex = Global.FindFirstGroupIndex(match.Groups);
            }
            

            string objectStr = string.Join(", ", match.Groups.Cast<System.Text.RegularExpressions.Group>().Select((g, i) => $"G{i}:'{g.Value}'"));
            //MessageBox.Show("Tamanho -> " + objectStr);
            return match.Success;
        }
    }
}
