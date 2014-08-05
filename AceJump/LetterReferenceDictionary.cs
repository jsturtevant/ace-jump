namespace AceJump
{
    using Microsoft.VisualStudio.Text;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Tracks the location of the letters
    /// </summary>
    public class LetterReferenceDictionary
    {
        private const char START_LETTER = 'A';
        readonly Dictionary<string, SnapshotSpan> dictionary = new Dictionary<string, SnapshotSpan>();
        private char currentLetter = START_LETTER;
        private string prefix = string.Empty;


        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        public string AddSpan(SnapshotSpan span)
        {
            string key = string.Concat(prefix, currentLetter.ToString());
            this.dictionary.Add(key, span);

            this.IncrementKey();
            return key;
        }

        private void IncrementKey()
        {
            // might need to rethink algorithm at some point
            // but 26*26 = 676 character on one screen which seems sufficient for now.
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
                    this.prefix = "A";
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

        public SnapshotSpan GetLetterPosition(string key)
        {
            SnapshotSpan span;
            this.dictionary.TryGetValue(key, out span);
            return span;
        }

        public void Reset()
        {
            this.currentLetter = START_LETTER;
            this.dictionary.Clear();
        }
    }
}
