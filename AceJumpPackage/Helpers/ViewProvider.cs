using System;
using System.Diagnostics;
using AceJumpPackage.Interfaces;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace AceJumpPackage.Helpers
{
    public class ViewProvider : IViewProvider
    {
        private readonly IVsTextManager myTextManager;

        public ViewProvider(IVsTextManager textManager)
        {
            myTextManager = textManager;
        }

        public IVsTextView GetActiveView()
        {
            IVsTextView view;
            myTextManager.GetActiveView(1, null, out view);

            if (view == null)
            {
                Debug.WriteLine("AceJumpCommand.cs | MenuItemCallback | could not retrieve current view");
                return null;
            }

            return view;
        }

        public IWpfTextView GetActiveWpfView(IVsTextView view = null)
        {
            if (view == null)
                view = GetActiveView();

            if (view == null)
                return null;

            // ReSharper disable once SuspiciousTypeConversion.Global
            return getWpfView(view as IVsUserData);
        }

        private IWpfTextView getWpfView(IVsUserData userData)
        {
            IWpfTextView view = null;

            if (null != userData)
            {
                IWpfTextViewHost viewHost;
                object holder;
                Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
                userData.GetData(ref guidViewHost, out holder);
                viewHost = (IWpfTextViewHost) holder;
                view = viewHost.TextView;
            }

            return view;
        }
    }
}
