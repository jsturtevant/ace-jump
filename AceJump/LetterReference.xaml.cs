using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AceJump
{
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

            // give font some room
            this.Width = bounds.Width + (PADDING * 2);
            this.Height = bounds.Height + (PADDING * 2);
            this.Padding = new Thickness(PADDING);

            // make it stand out
            this.FontWeight = FontWeights.ExtraBold;
        }
    }
}
