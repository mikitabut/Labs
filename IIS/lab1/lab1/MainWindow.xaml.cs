using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace lab1
{
    public partial class MainWindow : Window
    {
        public static readonly string CurrentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        public readonly string KnowledgeBasePath = Path.Combine(CurrentDirectory, "Knowledge base.txt");

        public List<Rule> Rules { get; set; } = new List<Rule>();
        public List<int> WrongRuleIndexes { get; set; } = new List<int>();
        public Stack<string> Targets { get; set; } = new Stack<string>();
        public Stack<Property> Context { get; set; } = new Stack<Property>();
        public int CurrentRuleIndex { get; set; } = 1;
        public Property CurrentAnswer = new Property();

        public MainWindow()
        {
            InitializeComponent();
            FillKnowledgeBase();
            Targets.Push(Rules.First().Target.Key);
            CurrentAnswer.Key = Rules.ElementAt(CurrentRuleIndex).Properties.First().Key;
            PropertyKeyLabel.Content = Char.ToUpper(CurrentAnswer.Key[0]) + CurrentAnswer.Key.Substring(1) + ":";
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            bool flag = false;
            CurrentAnswer.Value = InputTextBox.Text;
            Property currentVerifiableProperty = Rules.ElementAt(CurrentRuleIndex).Properties.First(x => x.Key == CurrentAnswer.Key);
            if (String.IsNullOrEmpty(CurrentAnswer.Value))
            {
                Targets.Push(Rules.ElementAt(CurrentRuleIndex).Properties.First(x => x.Key == CurrentAnswer.Key).Key);
            }
            else if (CurrentAnswer.Value == currentVerifiableProperty.Value)
            {
                Context.Push(CurrentAnswer);
                if (Targets.Any())
                {

                }
                else
                {
                    flag = true;
                }
            }
            else
            {
                WrongRuleIndexes.Add(CurrentRuleIndex);
            }

            if (flag)
            {

            }
        }

        private void FillKnowledgeBase()
        {
            string knowledgeBase = File.ReadAllText(KnowledgeBasePath);
            string[] rules = knowledgeBase.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ruleString in rules)
            {
                string[] rows = ruleString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                Rule rule = new Rule();
                foreach (string row in rows)
                {
                    if (row.StartsWith("То"))
                    {
                        string target = row.Substring(2).Trim();
                        string[] keyAndValue = target.Split('-');
                        rule.Target = new Property(keyAndValue[0].Trim(), keyAndValue[1].Trim());
                    }
                    else
                    {
                        string property = row.StartsWith("Если") ? row.Substring(4) : row;
                        property = property.EndsWith("и") ? property.Substring(0, property.Length - 1).Trim() : property;
                        string[] keyAndValue = property.Split('-');
                        rule.Properties.Add(new Property(keyAndValue[0].Trim(), keyAndValue[1].Trim()));
                    }
                }

                Rules.Add(rule);
            }
        }
    }

    public class Rule
    {
        public List<Property> Properties { get; set; } = new List<Property>();
        public Property Target { get; set; }

        public Rule() { }

        public Rule(List<Property> properties, Property target)
        {
            Properties = properties;
            Target = target;
        }
    }

    public class Property
    {
        public string Key { get; set; }
        public string Value { get; set; } // Empty means unknown

        public Property() {}

        public Property(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
