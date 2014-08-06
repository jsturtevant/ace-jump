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
        private JumpControler jumpControler;

        public AceKeyProcessor(AceJump aceJump)
        {
            this.keyTypeConverter = new KeyTypeConverter();
            this.jumpControler = new JumpControler(aceJump);
        }

        public override void KeyDown(KeyEventArgs args)
        {
            char? jumpKey = this.keyTypeConverter.ConvertToChar(args.Key);

            bool handled = this.jumpControler.ControlJump(jumpKey);
            args.Handled = handled;
        }
    }
}
    