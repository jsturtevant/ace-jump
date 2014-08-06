namespace AceJump
{
    using System.ComponentModel;

    using Microsoft.VisualStudio.Text;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Tracks the location of the letters
    /// </summary>
    public class LetterReferenceDictionary
    {
        private const char START_LETTER = 'A';
        readonly Dictionary<string, int> dictionary = new Dictionary<string, int>();
        private char currentLetter = START_LETTER;
        private string prefix = string.Empty;


        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        public string AddSpan(int span)
        {
            string key = string.Concat(prefix, currentLetter.ToString());
            this.dictionary.Add(key, span);

            this.IncrementKey();
            return key;
        }

        private void IncrementKey()
        {
            // might need to rethink algorithm at some point
            if (this.currentLetter != 'Z')
            {
                this.currentLetter++;
            }
            else
            {
                //reset
                this.currentLetter = START_LETTER;

                if (string.IsNullOrEmpty(this.prefix))
                {
                    //initialize prefix for key
                    this.prefix = "X";
                }
                else
                {
                    //increment prefix
                    char prefixChar = this.prefix.First();
                    prefixChar++;
                    this.prefix = (prefixChar).ToString();
                }
            }
        }

        public int GetLetterPosition(string key)
        {
            int span;
            this.dictionary.TryGetValue(key, out span);
            return span;
        }

        public void Reset()
        {
            this.currentLetter = START_LETTER;
            this.dictionary.Clear();
        }

        public static LetterReferenceDictionary CreateJumps(List<int> foundKeyLocations, int cursorlocation)
        {
            // ignore current location
            var lessThanCursor = new Stack<int>( foundKeyLocations.Where(l => l < cursorlocation));
            var moreThanCursor = new Stack<int>( foundKeyLocations.Where(l => l > cursorlocation).OrderByDescending(i => i));

            LetterReferenceDictionary letterReferenceDictionary = new LetterReferenceDictionary();

            for (var i = 0; i< foundKeyLocations.Count(); i++)
            {
                if (lessThanCursor.Any())
                {
                    letterReferenceDictionary.AddSpan(lessThanCursor.Pop());
                }
                if (moreThanCursor.Any())
                {
                    letterReferenceDictionary.AddSpan(moreThanCursor.Pop());
                }
            }


            return letterReferenceDictionary;
        }
    }
}
