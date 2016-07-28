using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AceJumpPackage.View
{
    /// <summary>
    /// Interaction logic for LetterReference.xaml
    /// </summary>
    public partial class LetterReference : UserControl
    {
        public const int PADDING = 2;

        public LetterReference(string referenceLetter, Rect bounds, double fontRenderingEmSize)
        {
            InitializeComponent();

            this.Content = referenceLetter.ToUpper();
            this.Background = Brushes.GreenYellow;

            // give letters like 'M' and 'W' some room
            this.Width = (bounds.Width * referenceLetter.Length) + (PADDING * 2) + 0;
            this.Height = bounds.Height;
   
            // make it stand out
            this.FontWeight = FontWeights.Bold;

            //
            this.FontSize = fontRenderingEmSize;
        }

        public void UpdateHighlight(string referenceLetter)
        {
            string s = (string)this.Content;
            if (s.StartsWith(referenceLetter.ToUpper()))
                this.Background = Brushes.Yellow;
        }
    }
}
