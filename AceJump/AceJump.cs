namespace AceJump
{
    using System.Linq;
    using System.Net.Mime;

    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Formatting;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Microsoft.VisualStudio.Text.Editor;

    class AceJump
    {
        private readonly IAdornmentLayer aceLayer;
        private readonly IWpfTextView textView;
        private string letter;
        private readonly LetterReferenceDictionary letterLocationSpans;

        private bool active;

        public AceJump(IWpfTextView textView)
        {
            this.textView = textView;
            this.aceLayer = textView.GetAdornmentLayer("AceJump");

            letterLocationSpans = new LetterReferenceDictionary();
        }

        public bool Active
        {
            get
            {
                return this.active;
            }
            private set
            {
                this.active = value;
            }
        }

        public void HighlightLetter(string letter)
        {
            this.letter = letter;
            foreach (var line in this.textView.TextViewLines)
            {
                this.CreateVisualsForLetter(line);
            }
        }

        private void CreateVisualsForLetter(ITextViewLine line)
        {
            //grab a reference to the lines in the current TextView 
            IWpfTextViewLineCollection textViewLines = this.textView.TextViewLines;
            int start = line.Start;
            int end = line.End;

            //Loop through each character, and place a box over item 
            for (int i = start; (i < end); ++i)
            {
                if (this.textView.TextSnapshot[i].ToString().ToLower() == this.letter.First().ToString().ToLower())
                {
                    var span = new SnapshotSpan(this.textView.TextSnapshot, Span.FromBounds(i, i + 1));

                    Geometry g = textViewLines.GetMarkerGeometry(span);
                    if (g != null)
                    {
                        // save the location of this letter to jump to later
                        string key = this.letterLocationSpans.AddSpan(span);

                        // Align the image with the top of the bounds of the text geometry
                        var letterReference = new LetterReference(key, g.Bounds);
                        Canvas.SetLeft(letterReference, g.Bounds.Left);
                        Canvas.SetTop(letterReference, g.Bounds.Top);

                        this.aceLayer.AddAdornment(AdornmentPositioningBehavior.TextRelative,span,null,letterReference,null);
                    }
                }
            }
        }

        public void ClearAdornments()
        {
            this.Active = false;
            this.letter = string.Empty;
            this.letterLocationSpans.Reset();
            this.aceLayer.RemoveAllAdornments();
        }

        public SnapshotPoint GetLetterPosition(string key)
        {
          return this.letterLocationSpans.GetLetterPosition(key).Start;
        }

        public void ShowSelector()
        {
            int snapshotPoint = textView.Caret.Position.BufferPosition;

            if (textView.Caret.ContainingTextViewLine.End.Position == snapshotPoint)
            {
                //caret is at end of line back it up one
                snapshotPoint = snapshotPoint - 1;
            }

            SnapshotPoint point = new SnapshotPoint(textView.TextSnapshot, snapshotPoint);
            SnapshotPoint point2 = new SnapshotPoint(textView.TextSnapshot, snapshotPoint+1);

            var span = new SnapshotSpan(point, point2);
            // Align the image with the top of the bounds of the text geometry
             Geometry g = textView.TextViewLines.GetMarkerGeometry(span);
            if (g != null)
            {
                AceJumperControl aceJumperControl = new AceJumperControl(this.textView);
                Canvas.SetLeft(aceJumperControl, g.Bounds.Left);
                Canvas.SetTop(aceJumperControl, g.Bounds.Top);

                this.aceLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, span, null, aceJumperControl, null);
                this.Active = true;
            }
        }

        public void JumpTo(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            SnapshotPoint newCaretPostion = this.GetLetterPosition(key.ToUpper());
            bool isValidPoint = newCaretPostion.Snapshot == textView.TextSnapshot && 
                                newCaretPostion.Position >= 0 && 
                                newCaretPostion.Position <= textView.TextSnapshot.Length;

            if (isValidPoint)
            {
                this.textView.Caret.MoveTo(newCaretPostion);                
            }
        }
    }
}
