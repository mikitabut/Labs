using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgeJs;

namespace NLP
{
    static class Lemmatizer
    {
        public static string Lemmatize(Word word)
        {
            var cat = GetCategory(word);
            var name = word.Name;
            switch (cat)
            {
                case "N":
                    return LemmatizeNounInterop(name).GetAwaiter().GetResult().ToString();
                case "V":
                    return LemmatizeVerbInterop(name).GetAwaiter().GetResult().ToString();
                case "J":
                    return LemmatizeAdjInterop(name).GetAwaiter().GetResult().ToString();
                default:
                    return word.Name;
            }
        }

        private static string GetCategory(Word word)
        {
            return word.TagsArr
                .Select(x=> x[0].ToString())
                .FirstOrDefault(x=> x == "N" || x == "V" || x == "J");
        }

        private static readonly Func<object, Task<object>> LemmatizeAdjInterop = Edge.Func(@"
                var lemmatize = require( 'wink-lemmatizer' );

                return function (data, callback) {
                    callback(null, lemmatize.adjective(data));
                };");

        private static readonly Func<object, Task<object>> LemmatizeNounInterop = Edge.Func(@"
                var lemmatize = require( 'wink-lemmatizer' );

                return function (data, callback) {
                    callback(null, lemmatize.noun(data));
                };");

        private static readonly Func<object, Task<object>> LemmatizeVerbInterop = Edge.Func(@"
                var lemmatize = require( 'wink-lemmatizer' );

                return function (data, callback) {
                    callback(null, lemmatize.verb(data));
                };");
    }
}
