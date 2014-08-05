namespace AceJump
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Formatting;


    /// <summary>
    /// Interaction logic for AceJumperControl.xaml
    /// </summary>
    public partial class AceJumperControl : UserControl
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
       //     this.Top = screenpoint.X;
         //   this.Left = screenpoint.Y;
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            
        }

      
    }
}
