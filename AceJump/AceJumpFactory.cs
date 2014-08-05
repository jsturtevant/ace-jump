namespace AceJump
{
    using System.Windows.Input;
    using System.ComponentModel.Composition;

    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(IKeyProcessorProvider))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    [Name("AceKeyProcessor")]
    internal sealed class AceKeyProcessorProvider : IKeyProcessorProvider
    {
        public AceKeyProcessorProvider()
        {
            this.aceKeyProcessor = new AceKeyProcessor();
        }

        [Export(typeof(AdornmentLayerDefinition))]
        [Name("AceJump")]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;

        private AceKeyProcessor aceKeyProcessor;

        public KeyProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            this.aceKeyProcessor.SetView(wpfTextView);
            return this.aceKeyProcessor;
        }
    }

    public class AceKeyProcessor : KeyProcessor
    {
        private IWpfTextView view;
        private KeyTypeConverter keyTypeConverter;
        private AceJump aceJump;

        private bool letterHighLightActive;

        public AceKeyProcessor()
        {
            this.keyTypeConverter = new KeyTypeConverter();
        }

        public void SetView(IWpfTextView wpfTextView)
        {   
            this.view = wpfTextView;
        }

        public override void KeyDown(KeyEventArgs args)
        {
            char? key = this.keyTypeConverter.ConvertToChar(args.Key);
            if (key.HasValue && key.Value == '+')
            {
                if (this.aceJump == null)
                {
                    this.aceJump = new AceJump(this.view);
                    this.aceJump.ShowSelector();
                }
                else if (!this.aceJump.Active)
                {
                    this.aceJump.ShowSelector();
                }
                else if (this.aceJump.Active)
                {
                    this.aceJump.ClearAdornments();
                }

                // mark it handled so it doesn't go down to editor
               args.Handled = true;
               return;
            }

            if (this.aceJump != null && this.aceJump.Active)
            {
                if (!key.HasValue)
                {
                    args.Handled = true;
                    return;
                }

                if (this.letterHighLightActive)
                {
                    // they have highlighted all letters so they are ready to jump
                    SnapshotPoint newCaretPostion = this.aceJump.GetLetterPosition(key.ToString().ToUpper());
                    this.view.Caret.MoveTo(newCaretPostion);
                    args.Handled = true;
                    this.letterHighLightActive = false;
                    this.aceJump.ClearAdornments();
                    return;
                }
                else
                {
                    this.aceJump.HighlightLetter(key.ToString().ToUpper());
                    this.letterHighLightActive = true;
                    args.Handled = true;
                    return;
                }
            }
        }
    }
}
    