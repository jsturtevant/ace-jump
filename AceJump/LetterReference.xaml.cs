namespace AceJump
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for LetterReference.xaml
    /// </summary>
    public partial class LetterReference : UserControl
    {
        public const int PADDING = 5;

        public LetterReference(string referenceLetter, Rect bounds)
        {
            InitializeComponent();

            this.Content = referenceLetter.ToUpper();
            this.Background = Brushes.GreenYellow;

            // give letter some room
            this.Width = (bounds.Width * referenceLetter.Length) + (PADDING * 2);
            this.Height = bounds.Height + (PADDING * 2);
            this.Padding = new Thickness(PADDING);

            // make it stand out
            this.FontWeight = FontWeights.ExtraBold;
        }
    }
}
