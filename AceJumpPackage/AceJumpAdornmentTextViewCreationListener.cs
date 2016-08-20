//------------------------------------------------------------------------------
// <copyright file="AceJumpAdornmentTextViewCreationListener.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace AceJumpPackage
{
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    internal sealed class AceJumpAdornmentTextViewCreationListener : IVsTextViewCreationListener
    {
        public static AceJumpAdornmentTextViewCreationListener Instance { get; private set; }
        public InputListener InputListener { get; private set; }

        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService = null;

        public AceJumpAdornmentTextViewCreationListener()
        {
        }


        #region IVsTextViewCreationListener


        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);
            Instance = this;
            // The adornment will listen to any event that changes the layout (text changes, scrolling, etc)
            //            new AceJumpAdornment(textView);
            InputListener = new InputListener(textViewAdapter, textView);

            textView.Properties.GetOrCreateSingletonProperty(() => InputListener);
        }




        #endregion
    }
}
