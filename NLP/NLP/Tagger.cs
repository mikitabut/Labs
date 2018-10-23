using edu.stanford.nlp.tagger.maxent;
using java.util;
using System;
using System.Collections.Generic;
using System.IO;

namespace NLP
{
    public class Tagger
    {
        public static void TagTexts(List<Text> texts) => texts.ForEach(x => TagText(x.Path));

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

            File.WriteAllText(filePath, taggedText);
            return taggedText;
        }
    }
}
