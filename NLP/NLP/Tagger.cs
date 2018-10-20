using System;
using System.Collections.Generic;
using java.util;
using edu.stanford.nlp.tagger.maxent;
using System.IO;
using File = System.IO.File;

namespace NLP
{
    public class Tagger
    {
        public static void TagTexts(List<string> files) => files.ForEach(x => TagText(x));

        public static string TagText(string filePath)
        {
            string text = File.ReadAllText(filePath);
            string taggedText = String.Empty;
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\POSTagger\models\wsj-0-18-bidirectional-nodistsim.tagger";
            var tagger = new MaxentTagger(path);
            var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(text)).toArray();

            foreach (ArrayList sentence in sentences)
            {
                var taggedSentence = tagger.tagSentence(sentence).ToString().Trim('[', ']');
                taggedText += taggedSentence + Environment.NewLine;
            }

            File.WriteAllText(MarkAsTagged(filePath, $"_Tagged"), taggedText);
            return taggedText;
        }

        private static string MarkAsTagged(string filePath, string line) =>
            Path.Combine(Path.GetDirectoryName(filePath),
                Path.GetFileNameWithoutExtension(filePath) + line + Path.GetExtension(filePath));
    }
}
