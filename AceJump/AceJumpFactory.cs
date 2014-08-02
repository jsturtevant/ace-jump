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
using Microsoft.VisualStudio.TextManager.Interop;

namespace AceJump
{

    #region Adornment Factory
    /// <summary>
    /// Establishes an <see cref="IAdornmentLayer"/> to place the adornment on and exports the <see cref="IWpfTextViewCreationListener"/>
    /// that instantiates the adornment on the event of a <see cref="IWpfTextView"/>'s creation
    /// </summary>
    //[Export(typeof(IWpfTextViewCreationListener))]
    //[ContentType("text")]
    //[TextViewRole(PredefinedTextViewRoles.Document)]
    //internal sealed class PurpleBoxAdornmentFactory : IWpfTextViewCreationListener
    //{
    //    /// <summary>
    //    /// Defines the adornment layer for the scarlet adornment. This layer is ordered 
    //    /// after the selection layer in the Z-order
    //    /// </summary>
       

    //    /// <summary>
    //    /// Instantiates a AceJump manager when a textView is created.
    //    /// </summary>
    //    /// <param name="textView">The <see cref="IWpfTextView"/> upon which the adornment should be placed</param>
    //    public void TextViewCreated(IWpfTextView textView)
    //    {
    //        new AceJump(textView);
    //    }
    //}
    #endregion //Adornment Factory

    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    internal class KeyBindingCommandFilterProvider : IVsTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("AceJump")]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;

        [Import(typeof(IVsEditorAdaptersFactoryService))]
        internal IVsEditorAdaptersFactoryService editorFactory = null;

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            IWpfTextView textView = editorFactory.GetWpfTextView(textViewAdapter);
            if (textView == null)
                return;

            AddCommandFilter(textViewAdapter, new KeybindingCommandFilter(textView));
        }

        void AddCommandFilter(IVsTextView viewAdapter, KeybindingCommandFilter commandFilter)
        {
            if (commandFilter.added == false)
            {
                //get the view adapter from the editor factory
                IOleCommandTarget next;
                int hr = viewAdapter.AddCommandFilter(commandFilter, out next);

                if (hr == VSConstants.S_OK)
                {
                    commandFilter.added = true;
                    //you'll need the next target for Exec and QueryStatus 
                    if (next != null)
                        commandFilter.nextCommand = next;
                }
            }
        }
    }
}
    