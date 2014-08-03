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
    using System.ComponentModel;
    using System.Windows.Interop;
    using  System.Windows;
   // using EnvDTE;
    using Microsoft.VisualStudio.PlatformUI;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Formatting;

    using MyAddin1;

    /// <summary>
    /// Interaction logic for AceJumperControl.xaml
    /// </summary>
    public partial class AceJumperControl : Window
    {
        private IWpfTextView view;

        private AceJump aceJump;

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

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

        }


        private void HandleEsc(object sender, KeyEventArgs e)
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
            this.aceJump.SetCurrentLetter(e.Key.ToString());
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
