using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceJump
{
    using System.Runtime.InteropServices;

    using Microsoft.VisualStudio.Text;

    public class LetterReferenceDictionary
    {
        Dictionary<string, SnapshotSpan> dictionary  = new Dictionary<string, SnapshotSpan>();

        private char currentLetter = 'A';


        public string AddSpan(SnapshotSpan span)
        {
            string key = currentLetter.ToString();
            this.dictionary.Add(key, span);
            currentLetter++;
            return key;
        }

        public SnapshotPoint GetLetterPosition(string key)
        {
            SnapshotSpan span;
            this.dictionary.TryGetValue(key, out span);

            return span.Start;
        }
    }
}
