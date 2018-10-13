using DynamicSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CorrectionConsoleApp
{
    public  class SpellCorrector
    {

        Dictionary<string, int> DWORDS;
        const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";

        public SpellCorrector()
        {

            DWORDS = this.Train(this.Words(System.IO.File.ReadAllText("dictionary.txt")));
        }
        List<string> Words(string text)
        {

            return Regex.Matches(text.ToLower(), "[a-z]+").ToList();
        }
        Dictionary<string, int> Train(List<string> features)
        {

            var model = new Dictionary<string, int>();
            foreach (var f in features)
                model[f] = model.Get(f, 0) + 1;
            return model;
        }
        class StringPair
        {

            public string a, b;
        }
        List<string> Edits1(string word)
        {

            var splits = DS.Range(word.Length + 1).Map(i => new StringPair { a = word.Slice(0, i), b = word.Slice(i) });
            var deletes = splits.Map(s => s.a + s.b.Slice(1));
            var transposes = splits.Map(s => s.b.Length > 1 ? s.a + s.b[1] + s.b[0] + s.b.Slice(2) : "**");

            var replaces = new List<string>();
            foreach (var s in splits)
                foreach (var c in ALPHABET)
                    if (s.b.Length > 0)
                        replaces.Add(s.a + c + s.b.Slice(1, -1));

            var inserts = new List<string>();
            foreach (var s in splits)
                foreach (var c in ALPHABET)
                    inserts.Add(s.a + c + s.b);

            return deletes.Add(transposes).Add(replaces).Add(inserts);
        }
        List<string> KnownEdits2(string word)
        {

            var l = new List<string>();
            foreach (var e1 in this.Edits1(word))
                foreach (var e2 in Edits1(e1))
                    if (this.DWORDS.ContainsKey(e2))
                        l.Add(e2);
            return l;
        }
        List<string> Known(List<string> words)
        {

            return words.Filter(w => this.DWORDS.ContainsKey(w));
        }
        public List<string> Candidates(string word)

        {
            var candidateWords = this.Known(DS.List(word));
            if (candidateWords.Count == 0)
                candidateWords = this.Known(this.Edits1(word));
            if (candidateWords.Count == 0)
                candidateWords = this.KnownEdits2(word);
            if (candidateWords.Count == 0)
                candidateWords = DS.List(word);

            return candidateWords.Distinct().ToList();
        }
        public string Correct(string word)

        {
            return this.DWORDS.Max(Candidates(word));
        }
    }
}
