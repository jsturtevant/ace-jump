using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace AceJumpPackage.Interfaces
{
    public interface IViewProvider
    {
        IVsTextView GetActiveView();
        IWpfTextView GetActiveWpfView(IVsTextView view = null);
    }
}