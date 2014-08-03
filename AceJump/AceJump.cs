using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;

namespace AceJump
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using EnvDTE;

    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Formatting;

    /// <summary>
    /// Adornment class that draws a square box in the top right hand corner of the viewport
    /// </summary>
    class AceJump
    {
        IAdornmentLayer _layer;
        IWpfTextView _view;
        Brush _brush;
        Pen _pen;

        private string letter;

        private LetterReferenceDictionary letterLocationSpans;

        public AceJump(IWpfTextView view)
        {
            _view = view;
            _layer = view.GetAdornmentLayer("AceJump");

            //Create the pen and brush to color the box behind the a's
            Brush brush = new SolidColorBrush(Color.FromArgb(0x20, 0x00, 0x00, 0xff));
            brush.Freeze();
            Brush penBrush = new SolidColorBrush(Colors.Red);
            penBrush.Freeze();
            Pen pen = new Pen(penBrush, 0.5);
            pen.Freeze();

            _brush = brush;
            _pen = pen;

            letterLocationSpans = new LetterReferenceDictionary();
        }

        public void SetCurrentLetter(string letter)
        {
            this.letter = letter;

            foreach (var line in this._view.TextViewLines)
            {
                this.CreateVisuals(line);
            }

        }

        /// <summary>
        /// Within the given line add the scarlet box behind the a
        /// </summary>
        private void CreateVisuals(ITextViewLine line)
        {
            //grab a reference to the lines in the current TextView 
            IWpfTextViewLineCollection textViewLines = _view.TextViewLines;
            int start = line.Start;
            int end = line.End;

            //Loop through each character, and place a box around any a 
            for (int i = start; (i < end); ++i)
            {
                if (_view.TextSnapshot[i].ToString().ToLower() == this.letter.First().ToString().ToLower())
                {
                    SnapshotSpan span = new SnapshotSpan(_view.TextSnapshot, Span.FromBounds(i, i + 1));
                    string key = this.letterLocationSpans.AddSpan(span);

                    Geometry g = textViewLines.GetMarkerGeometry(span);
                    if (g != null)
                    {
                        GeometryDrawing drawing = new GeometryDrawing(_brush, _pen, g);
                        drawing.Freeze();

                        DrawingImage drawingImage = new DrawingImage(drawing);
                        drawingImage.Freeze();

                        Image image = new Image();
                        image.Source = drawingImage;

                        //Align the image with the top of the bounds of the text geometry
                        Canvas.SetLeft(image, g.Bounds.Left);
                        Canvas.SetTop(image, g.Bounds.Top);

                        _layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, image, null);
                        
                        LetterReference letterReference = new LetterReference(key);

                        //Align the image with the top of the bounds of the text geometry
                        Canvas.SetLeft(letterReference, g.Bounds.Left);
                        Canvas.SetTop(letterReference, g.Bounds.Top + _view.LineHeight);
                        _layer.AddAdornment(AdornmentPositioningBehavior.TextRelative,span,null,letterReference,null);
                    }
                }
            }
        }

        public void ClearAdornment()
        {
            this.letter = string.Empty;
            _layer.RemoveAllAdornments();
        }
    }
}
