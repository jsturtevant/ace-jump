using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace AceJumpPackage
{
    class InputListener : IOleCommandTarget
    {
        IOleCommandTarget nextCommandHandler;
        private readonly IVsTextView _adapter;
        ITextView _textView;
        private AceJumpAdornment _adornment;

        /// <summary>
        /// Add this filter to the chain of Command Filters
        /// </summary>
        internal InputListener(IVsTextView adapter, ITextView textView, AceJumpAdornment adornment)
        {
            _adapter = adapter;
            _textView = textView;
            _adornment = adornment;
        }

        public void AddFilter()
        {
            _adapter.AddCommandFilter(this, out nextCommandHandler);

        }

        public void RemoveFilte()
        {
            _adapter.RemoveCommandFilter(this);
        }

        /// <summary>
        /// Public access to IOleCommandTarget.QueryStatus() function
        /// </summary>
        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return nextCommandHandler.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        /// <summary>
        /// Get user input. 
        /// IOleCommandTarget.Exec() function
        /// </summary>
        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            int hr = VSConstants.S_OK;

            char typedChar;
            if (TryGetTypedChar(pguidCmdGroup, nCmdID, pvaIn, out typedChar))
            {
                _adornment.ClearAdornments();
                _adornment.PlaceAtChar(typedChar);
                return hr;
            }

            hr = nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            return hr;
        }

        /// <summary>
        /// Try to get the keypress value. Returns 0 if attempt fails
        /// </summary>
        /// <param name="typedChar">Outputs the value of the typed char</param>
        /// <returns>Boolean reporting success or failure of operation</returns>
        bool TryGetTypedChar(Guid cmdGroup, uint nCmdID, IntPtr pvaIn, out char typedChar)
        {
            typedChar = char.MinValue;

            if (cmdGroup != VSConstants.VSStd2K || nCmdID != (uint)VSConstants.VSStd2KCmdID.TYPECHAR)
                return false;

            typedChar = (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);
            return true;
        }
    }
}