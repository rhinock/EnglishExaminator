using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Core
{
    [Serializable]
    public class ThreeFormsVerifier
    {
        private List<(string baseForm, string pastSimple, string pastPerfect)> _forms;       
        public List<(string baseForm, string pastSimple, string pastPerfect)> Forms { get => _forms; }

        private List<(string baseForm, string pastSimple, string pastPerfect)> _updatedForms;
        public List<(string baseForm, string pastSimple, string pastPerfect)> UpdatedForms { get => _updatedForms; }

        public (string word, int index, int form) ChosenWord { get; protected set; }

        public bool GameEnd { get => Forms.Count == 0; }

        public int Score { get; protected set; }

        public ThreeFormsVerifier(List<(string baseForm, string pastSimple, string pastPerfect)> forms)
        {
            if (forms == null) throw new ArgumentNullException("ThreeFormsVerifier constructor: Argument cannot be null");

            _forms = forms;
            _updatedForms = new List<(string baseForm, string pastSimple, string pastPerfect)>();
        }

        public string GetRandomWord()
        {
            Random rand = new Random();
            var form = rand.Next(3);
            var index = rand.Next(_forms.Count);

            switch (form)
            {
                case 0:
                    ChosenWord = (_forms[index].baseForm, index, form);
                    return ($"1. { _forms[index].baseForm }");
                case 1:
                    ChosenWord = (_forms[index].pastSimple, index, form);
                    return ($"2. { _forms[index].pastSimple }");
                case 2:
                    ChosenWord = (_forms[index].pastPerfect, index, form);
                    return ($"3. { _forms[index].pastPerfect }");
            }
            return null;
        }

        public (bool rezult, string text) CheckRezults(string input)
        {
            bool rezult = false;

            Regex wordPattern = new Regex(@"[\w/]+");
            MatchCollection matches = wordPattern.Matches(input);
            if (matches.Count != 3)
            {
                return (false, _forms[ChosenWord.index].ToString());
            }
            UpdatedForms.Add((matches[0].Value.ToLower(), matches[1].Value.ToLower(), matches[2].Value.ToLower()));

            if (_forms[ChosenWord.index].Equals(_updatedForms.Last()))
            {
                rezult = true;
                Score++;
            }
            else
                rezult = false;

            var correct = _forms[ChosenWord.index];
            _forms.RemoveAt(ChosenWord.index);
            return (rezult, correct.ToString());
        }
    }
}
