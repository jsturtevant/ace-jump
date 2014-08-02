namespace AceJump
{
    using System;
    using System.Runtime.InteropServices;

    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Text.Editor;

    internal class KeybindingCommandFilter : IOleCommandTarget
    {
        private IWpfTextView textView;
        private bool adorned;
        internal IOleCommandTarget nextCommand;

        internal bool added;

        public KeybindingCommandFilter(IWpfTextView textView)
        {
            this.textView = textView;
            this.adorned = false;
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return this.nextCommand.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (this.adorned == false)
            {
                char typedChar = char.MinValue;

                if (pguidCmdGroup == VSConstants.VSStd2K && nCmdID == (uint)VSConstants.VSStd2KCmdID.TYPECHAR)
                {
                    typedChar = (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);
                    if (typedChar.Equals('+'))
                    {
                        new AceJump(this.textView);
                        this.adorned = true;
                    }
                }
            }
            return this.nextCommand.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }
    }
}