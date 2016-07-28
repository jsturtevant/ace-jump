﻿namespace AceJump
{
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
            var aceJump = new AceJump();
            var aceKeyProcessor = new AceKeyProcessor(aceJump);

            aceJump.SetView(wpfTextView);
            return aceKeyProcessor;
        }
    }
}
    