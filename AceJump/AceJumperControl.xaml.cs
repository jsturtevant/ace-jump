using System;
using System.Windows.Input;

namespace AceJump
{
    using  System.Windows;

    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Formatting;


    /// <summary>
    /// Interaction logic for AceJumperControl.xaml
    /// </summary>
    public partial class AceJumperControl : Window
    {
        private IWpfTextView view;

        private AceJump aceJump;

        private bool letterdrawn;

        public AceJumperControl(IWpfTextView view)
        {
            InitializeComponent();
            this.JumpBox.Focus();
            this.view = view;

            TextBounds characterBounds = view.TextViewLines.GetCharacterBounds(view.Caret.Position.BufferPosition);
            Point point = new Point(view.ViewportTop + characterBounds.Top, view.ViewportLeft + characterBounds.Right);
            Point pointToScreen = view.VisualElement.PointToScreen(point);

            this.Top = pointToScreen.X;
            this.Left = pointToScreen.Y;

            this.PreviewKeyDown += new KeyEventHandler(this.HandleKeyPress);

        }


        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
                return;
            }


            if (this.aceJump == null)
            {
                this.aceJump = new AceJump(this.view);
            }

            if (!this.letterdrawn)
            {
                this.aceJump.SetCurrentLetter(e.Key.ToString());
                letterdrawn = true;
            }
            else
            {
                SnapshotPoint newCaretPostion = this.aceJump.GetLetterPosition(e.Key.ToString().ToUpper());
                this.view.Caret.MoveTo(newCaretPostion);
                this.Close();
                return;
            }
            
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.aceJump != null)
            {
                this.aceJump.ClearAdornment();
            }


            base.OnClosed(e);
        }
    }
}
