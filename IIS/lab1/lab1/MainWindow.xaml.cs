using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly string CurrentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        public readonly string KnowledgeBasePath = Path.Combine(CurrentDirectory, "Knowledge base.txt");

        public List<Rule> Rules { get; set; } = new List<Rule>();

        public MainWindow() => InitializeComponent();

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            FillKnowledgeBase();

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
                        rule.Target = new KeyValuePair<string, string>(keyAndValue[0].Trim(), keyAndValue[1].Trim());
                    }
                    else
                    {
                        string property = row.StartsWith("Если") ? row.Substring(4) : row;
                        property = property.EndsWith("и") ? property.Substring(0, property.Length - 1).Trim() : property;
                        string[] keyAndValue = property.Split('-');
                        rule.Properties.Add(keyAndValue[0].Trim(), keyAndValue[1].Trim());
                    }
                }

                Rules.Add(rule);
            }
        }
    }

    public class Rule
    {
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
        public KeyValuePair<string, string> Target { get; set; }

        public Rule() { }

        public Rule(Dictionary<string, string> properties, KeyValuePair<string, string> target)
        {
            Properties = properties;
            Target = target;
        }
    }
}
