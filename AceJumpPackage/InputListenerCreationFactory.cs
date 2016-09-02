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
        public InputListener InputListener { get; private set; }

        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService = null;

        public InputListenerCreationFactory()
        {
        }


        #region IVsTextViewCreationListener


        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
//            var textView = AdapterService.GetWpfTextView(textViewAdapter);
//            try
//            {
//                InputListener = new InputListener(textViewAdapter, textView);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                throw;
//            }

        }

        #endregion
    }
}
