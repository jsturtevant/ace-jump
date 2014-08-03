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

    //[Export(typeof(IVsTextViewCreationListener))]
    //[ContentType("text")]
    //[TextViewRole(PredefinedTextViewRoles.Editable)]
    //internal class KeyBindingCommandFilterProvider : IVsTextViewCreationListener
    //{
    //    [Export(typeof(AdornmentLayerDefinition))]
    //    [Name("AceJump")]
    //    [Order(After = PredefinedAdornmentLayers.Caret)]
    //    public AdornmentLayerDefinition editorAdornmentLayer = null;

    //    [Import(typeof(IVsEditorAdaptersFactoryService))]
    //    internal IVsEditorAdaptersFactoryService editorFactory = null;

    //    [Import]
    //    internal SVsServiceProvider ServiceProvider = null;

    //    public void VsTextViewCreated(IVsTextView textViewAdapter)
    //    {
    //        IWpfTextView textView = editorFactory.GetWpfTextView(textViewAdapter);
    //        if (textView == null)
    //            return;

    //        KeybindingCommandFilter keybindingCommandFilter = new KeybindingCommandFilter(textView);
    //        textView.Properties.GetOrCreateSingletonProperty<KeybindingCommandFilter>(() => keybindingCommandFilter);

    //        DTE dte = (DTE)ServiceProvider.GetService(typeof(DTE));
    //        AddCommandFilter(textViewAdapter, keybindingCommandFilter);
    //    }

    //    void AddCommandFilter(IVsTextView viewAdapter, KeybindingCommandFilter commandFilter)
    //    {
    //        if (commandFilter.added == false)
    //        {
    //            //get the view adapter from the editor factory
    //            IOleCommandTarget next;
    //            int hr = viewAdapter.AddCommandFilter(commandFilter, out next);

    //            if (hr == VSConstants.S_OK)
    //            {
    //                commandFilter.added = true;
    //                //you'll need the next target for Exec and QueryStatus 
    //                if (next != null)
    //                    commandFilter.nextCommand = next;
    //            }
    //        }
    //    }
    //}

    [Export(typeof(IKeyProcessorProvider))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    [Name("Default Key Processor")]
   
    internal sealed class DefaultKeyProcessorProvider : IKeyProcessorProvider
    {

        [Export(typeof(AdornmentLayerDefinition))]
        [Name("AceJump")]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;
    
        [ImportingConstructor]
        internal DefaultKeyProcessorProvider()
        {
         
        }

        public KeyProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            return new VimKeyProcessor(wpfTextView);
        }
    }

    public class VimKeyProcessor : KeyProcessor
    {
        private IWpfTextView view;

        private KeybindingCommandFilter adornment;

        private KeyTypeConverter keyTypeConverter;

        public VimKeyProcessor(IWpfTextView wpfTextView)
        {
            this.view = wpfTextView;
            this.adornment = adornment;

            this.keyTypeConverter = new KeyTypeConverter();
        }

      
        public override void KeyDown(KeyEventArgs args)
        {
             char? key = this.keyTypeConverter.ConvertToChar(args.Key);
            if (key.HasValue && key.Value == '+')
            {
                AceJumperControl aceJumperControl = new AceJumperControl(this.view);
                aceJumperControl.ShowDialog();

                args.Handled = true;
            }

            base.KeyDown(args);
        }
    }
}
    