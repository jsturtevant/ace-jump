using System;
using System.Windows.Input;

namespace AceJump
{
    using  System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

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
        private bool letterSelectionActive;

        public AceJumperControl(IWpfTextView view)
        {
            InitializeComponent();

            this.view = view;
            
            this.CreateAdornmentLayer();
            this.SetJumpBoxLocation();
            this.JumpBox.Focus();
            
            this.PreviewKeyDown += new KeyEventHandler(this.HandleKeyPress);
        }

        private void SetJumpBoxLocation()
        {
            // doesn seem to end me up in the right place but close enough
            // need to figure out a better way
            TextBounds characterBounds = view.TextViewLines.GetCharacterBounds(view.Caret.Position.BufferPosition);
            Point point = new Point(view.ViewportTop + characterBounds.Top, view.ViewportLeft + characterBounds.Left);
            Point screenpoint = view.VisualElement.PointToScreen(point);
            this.Top = screenpoint.X;
            this.Left = screenpoint.Y;
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
                return;
            }

            if (!this.letterSelectionActive)
            {
                this.aceJump.SetCurrentLetter(e.Key.ToString());
                this.letterSelectionActive = true;
            }
            else
            {
                SnapshotPoint newCaretPostion = this.aceJump.GetLetterPosition(e.Key.ToString().ToUpper());
                this.view.Caret.MoveTo(newCaretPostion);
                this.Close();
                return;
            }
        }

        private void CreateAdornmentLayer()
        {
            if (this.aceJump == null)
            {
                // create the adornment layer
                this.aceJump = new AceJump(this.view);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.aceJump != null)
            {
                this.aceJump.ClearAdornments();
            }

            base.OnClosed(e);
        }
    }
}
