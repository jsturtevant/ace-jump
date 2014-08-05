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
        private AceKeyProcessor aceKeyProcessor;
        private AceJump aceJump;

        [Export(typeof(AdornmentLayerDefinition))]
        [Name("AceJump")]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;


        public AceKeyProcessorProvider()
        {
            this.aceJump = new AceJump();
            this.aceKeyProcessor = new AceKeyProcessor(this.aceJump);
        }
        
        public KeyProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            this.aceJump.SetView(wpfTextView);
            return this.aceKeyProcessor;
        }
    }

    public class AceKeyProcessor : KeyProcessor
    {
        private KeyTypeConverter keyTypeConverter;
        private NewJumpControler newJumpControler;

        public AceKeyProcessor(AceJump aceJump)
        {
            this.keyTypeConverter = new KeyTypeConverter();
            this.newJumpControler = new NewJumpControler(aceJump);
        }

        public override void KeyDown(KeyEventArgs args)
        {
            char? jumpKey = this.keyTypeConverter.ConvertToChar(args.Key);

            bool handled = newJumpControler.ControlJump(jumpKey);
            args.Handled = handled;
        }
    }

    public class NewJumpControler
    {
        private AceJump aceJump;

        private bool letterHighLightActive;

        public NewJumpControler(AceJump aceJump)
        {
            this.aceJump = aceJump;
        }

        public bool ControlJump(char? key)
        {
            if (aceJump == null)
            {
                // something didn't get wired up right.  
                return false;
            }

            if (key.HasValue && key.Value == '+')
            {
                this.ShowJumpEditor();
                
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
                    this.JumpCursor(key.Value);
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

        private void JumpCursor(char jumpKey)
        {
            // they have highlighted all letters so they are ready to jump
            this.aceJump.JumpTo(jumpKey.ToString().ToUpper());
            this.letterHighLightActive = false;
            this.aceJump.ClearAdornments();
        }

        private void ShowJumpEditor()
        {
            if (this.aceJump.Active)
            {
                this.aceJump.ClearAdornments();
            }
            else
            {
                this.aceJump.ShowSelector();
            }
        }
    }
}
    