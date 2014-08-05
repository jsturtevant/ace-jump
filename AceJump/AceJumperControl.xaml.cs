namespace AceJump
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Formatting;


    /// <summary>
    /// Interaction logic for AceJumperControl.xaml
    /// </summary>
    public partial class AceJumperControl : Window
    {
        private readonly KeyTypeConverter keyTypeConverter;
        private readonly IWpfTextView view;
        private AceJump aceJump;
        private bool letterHighLightActive;

        public AceJumperControl(IWpfTextView view)
        {
            InitializeComponent();
            this.view = view;
            this.keyTypeConverter = new KeyTypeConverter();
            
            this.CreateAdornmentLayer();
            this.SetJumpBoxLocation();
            
            this.JumpTextBox.Focus();
            this.PreviewKeyDown += new KeyEventHandler(this.HandleKeyPress);
        }

        private void SetJumpBoxLocation()
        {
            // doesn't seem to end up in the right place but close enough for now
            // TODO need to figure out a better way
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
                this.Close();
                return;
            }
            
            char? key = this.keyTypeConverter.ConvertToChar(e.Key);
            if (!key.HasValue)
            {
                return;
            }

            if (this.letterHighLightActive)
            {
                // they have highlighted all letters so they are ready to jump
                SnapshotPoint newCaretPostion = this.aceJump.GetLetterPosition(key.ToString().ToUpper());
                this.view.Caret.MoveTo(newCaretPostion);
                this.Close();
                return;
            }
            else
            {
                this.aceJump.HighlightLetter(key.ToString().ToUpper());
                this.letterHighLightActive = true;
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
