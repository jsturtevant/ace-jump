using System;
using System.Collections.Generic;
using System.Linq;

namespace AceJumpPackage.Helpers
{
    /// <summary>
    /// Tracks the location of the letters
    /// </summary>
    public class LetterReferenceDictionary
    {
        private const char START_LETTER = 'A';

        private readonly Dictionary<string, int> dictionary = new Dictionary<string, int>();

        private char currentLetter = START_LETTER;

        private string prefix = string.Empty;

        private readonly int listOffset;

        public LetterReferenceDictionary(int totalLocations)
        {
            var numberOfGroups = (int)Math.Ceiling(totalLocations / (double)26);

            // the offset for the alphabet is number of groups -1
            // if groupgs =1 then a-Z. if groups = 2 then a-y and so on.
            this.listOffset = numberOfGroups - 1;

            // stop the list off set at B
            if (this.listOffset > 24)
            {
                this.listOffset = 24;
            }

        }

        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        public char? OffsetKey
        {
            get
            {
                if (this.listOffset == 0)
                {
                    return null;
                }
                return (char)('Z' - this.listOffset );
            }
        }

        public string LastKey
        {
            get
            {
                return this.dictionary.Last().Key;
            }
        }

        public string AddSpan(int span)
        {
            string key = string.Concat(prefix, currentLetter.ToString());


            if (!string.IsNullOrEmpty(prefix) && prefix.First() < this.OffsetKey.Value)
            {
                return string.Empty;
            }
          
            this.dictionary.Add(key, span);
            this.IncrementKey();

            
            return key;
        }

        private void IncrementKey()
        {
            if (this.listOffset > 0)
            {
                if (this.currentLetter == this.OffsetKey.Value -1  && string.IsNullOrEmpty(this.prefix))
                {
                    // then set prefix
                    this.currentLetter = START_LETTER;
                    this.prefix = "Z";
                    return;
                }
            }

            if (this.currentLetter != 'Z')
            {
                this.currentLetter++;
            }
            else
            {
                // reset
                this.currentLetter = START_LETTER;
              
                // decrement prefix
                if (string.IsNullOrEmpty(this.prefix)) this.prefix = "Z";
                char prefixChar = this.prefix.First();
                prefixChar--;
                this.prefix = (prefixChar).ToString();
            }
        }

        public int GetLetterPosition(string key)
        {
            int span;
            bool success = this.dictionary.TryGetValue(key.ToUpper(), out span);
            if (!success)
                return -1;
            return span;
        }

        public void Reset()
        {
            this.currentLetter = START_LETTER;
            this.dictionary.Clear();
        }
    }
}
