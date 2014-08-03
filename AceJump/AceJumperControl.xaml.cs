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

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();


            if (this.aceJump == null)
            {
                this.aceJump = new AceJump(this.view);
            }
            this.aceJump.SetCurrentLetter(e.Key.ToString());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            this.aceJump.ClearAdornment();

            base.OnClosed(e);
        }
    }
}
