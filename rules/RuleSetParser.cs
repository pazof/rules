// // RuleSetParser.cs
// /*
// Paul Schneider paul@pschneider.fr 03/10/2020 17:28 20202020 10 3
// */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace rules
{
    public class RuleSetParser
    {
        Parser parser;
        Scanner scanner;
        DefinitionSet definitions = new DefinitionSet();
        RuleSet rules = new RuleSetDefault(false);
        public RuleSetParser(bool defaultingToAllow)
        {
            rules = new RuleSetDefault(defaultingToAllow) ;
        }

        void initParser()
        {
            parser.Definitions = definitions;
            parser.Rules = rules;
        }

        public void ParseFile(string inputFileName)
        {
            scanner = new Scanner(inputFileName);
            parser = new Parser(scanner);
            initParser();
            UseParser();
        }

        public void Parse(Stream input)
        {
            scanner = new Scanner(input);
            parser = new Parser(scanner);
            initParser();
            UseParser();
        }

        public void Parse(string input)
        {
            var memStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            scanner = new Scanner(memStream);
            parser = new Parser(scanner);
            initParser();
            UseParser();
        }

        void UseParser()
        {
            parser.Parse();
            if (parser.errors.count > 0)
                throw new Exception("Parsing failed ");
        }

        public void Reset()
        {
            definitions.Clear();
            rules.Clear();
        }

        public DefinitionSet Definitions { get => definitions; }
        public RuleSet Rules { get => rules; }
    }
}
