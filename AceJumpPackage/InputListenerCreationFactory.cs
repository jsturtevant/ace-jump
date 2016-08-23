//------------------------------------------------------------------------------
// <copyright file="InputListenerCreationFactory.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
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
    internal sealed class InputListenerCreationFactory : IVsTextViewCreationListener
    {
        public static InputListenerCreationFactory Instance { get; private set; }
        public InputListener InputListener { get; private set; }

        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService = null;

        public InputListenerCreationFactory()
        {
        }


        #region IVsTextViewCreationListener


        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            var textView = AdapterService.GetWpfTextView(textViewAdapter);
            Instance = this;
            try
            {
//                var adornment = textView.Properties.GetProperty<AceJumpAdornment>(nameof(AceJumpAdornment));
                var adornment = textView.Properties.GetProperty<AceJumpAdornment>(nameof(AceJumpAdornment));
                InputListener = new InputListener(textViewAdapter, textView, adornment);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }            // The adornment will listen to any event that changes the layout (text changes, scrolling, etc)

        }

        #endregion
    }
}
