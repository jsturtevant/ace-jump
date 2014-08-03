using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;

namespace AceJump
{
    using System.Net.Mime;
    using System.Windows;
    using System.Windows.Input;

    [Export(typeof(IKeyProcessorProvider))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    [Name("Default Key Processor")]
   
    internal sealed class AceKeyProcessorProvider : IKeyProcessorProvider
    {

        [Export(typeof(AdornmentLayerDefinition))]
        [Name("AceJump")]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;

        [ImportingConstructor]
        internal AceKeyProcessorProvider()
        {
         
        }

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

                args.Handled = true;
            }
        }
    }
}
    