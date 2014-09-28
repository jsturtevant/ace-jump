namespace AceJump
{
    using System;
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
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("AceJump")]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;

        public KeyProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            var aceJump = new AceJump();
            var aceKeyProcessor = new AceKeyProcessor(aceJump);

            aceJump.SetView(wpfTextView);
            return aceKeyProcessor;
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
            if (Keyboard.IsKeyDown(Key.OemSemicolon) && 
                Keyboard.IsKeyDown(Key.LeftCtrl) && 
                Keyboard.IsKeyDown(Key.LeftAlt))
            {
                this.jumpControler.ShowJumpEditor();
                args.Handled = true;
                return;
            }
            
            char? jumpKey = this.keyTypeConverter.ConvertToChar(args.Key);

            bool handled = this.jumpControler.ControlJump(jumpKey);
            args.Handled = handled;
        }
    }
}
    