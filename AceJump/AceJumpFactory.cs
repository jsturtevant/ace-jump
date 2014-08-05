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
        private KeyTypeConverter keyTypeConverter;
        private NewJumpControler newJumpControler;

        private IWpfTextView view;

        public AceKeyProcessor()
        {
            this.keyTypeConverter = new KeyTypeConverter();
            this.newJumpControler = new NewJumpControler();
        }

        public void SetView(IWpfTextView wpfTextView)
        {   
            this.view = wpfTextView;
        }

        public override void KeyDown(KeyEventArgs args)
        {
            char? jumpKey = this.keyTypeConverter.ConvertToChar(args.Key);

            bool handled = newJumpControler.ControlJump(jumpKey, this.view);
            args.Handled = handled;
        }
    }

    public class NewJumpControler
    {
        private AceJump aceJump;

        private bool letterHighLightActive;

        public bool ControlJump(char? key, IWpfTextView view)
        {
            if (key.HasValue && key.Value == '+')
            {
                if (this.aceJump == null)
                {
                    this.aceJump = new AceJump(view);
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
                return true;
            }

            if (this.aceJump != null && this.aceJump.Active)
            {
                if (!key.HasValue)
                {
                    return true;
                }

                if (this.letterHighLightActive)
                {
                    // they have highlighted all letters so they are ready to jump
                    this.aceJump.JumpTo(key.ToString().ToUpper());
                    this.letterHighLightActive = false;
                    this.aceJump.ClearAdornments();
                    return true;
                }
                else
                {
                    this.aceJump.HighlightLetter(key.ToString().ToUpper());
                    this.letterHighLightActive = true;
                    return true;
                }
            }

            return false;
        }
    }
}
    