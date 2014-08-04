namespace AceJump
{
    using System.Windows.Input;
    using System.ComponentModel.Composition;
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
            return new AceKeyProcessor(wpfTextView);
        }
    }

    public class AceKeyProcessor : KeyProcessor
    {
        private IWpfTextView view;
        private KeyTypeConverter keyTypeConverter;

        public AceKeyProcessor(IWpfTextView wpfTextView)
        {
            this.view = wpfTextView;
            this.keyTypeConverter = new KeyTypeConverter();
        }
      
        public override void KeyDown(KeyEventArgs args)
        {
            char? key = this.keyTypeConverter.ConvertToChar(args.Key);
            if (key.HasValue && key.Value == '+')
            {
               AceJumperControl aceJumperControl = new AceJumperControl(view);
               aceJumperControl.ShowDialog();  
               
               // mark it handled so it doesn't go down to editor
               args.Handled = true;
            }
        }
    }
}
    