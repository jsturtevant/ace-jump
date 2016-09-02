//------------------------------------------------------------------------------
// <copyright file="AceJumpCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using AceJump;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace AceJumpPackage
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class AceJumpCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("20929872-63f6-48df-bc23-04798035c26f");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="AceJumpCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private AceJumpCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        private bool isSecondLetter = false;

        private void InputListenerOnKeyPressed(object sender, KeyPressEventArgs keyPressEventArgs)
        {
            if (_jumpControler.ControlJump(keyPressEventArgs.KeyChar))
            {
                _input.KeyPressed -= InputListenerOnKeyPressed;
                _input.RemoveFilter();
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static AceJumpCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        private JumpControler _jumpControler;
        private InputListener _input;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new AceJumpCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            if (_jumpControler!= null &&  _jumpControler.Active())
                return;

            var txtMgr = (IVsTextManager)Package.GetGlobalService(typeof(SVsTextManager));
            IVsTextView view;
            txtMgr.GetActiveView(1, null, out view);

            if (view == null)
            {
                Debug.WriteLine("AceJumpCommand.cs | MenuItemCallback | could not retrieve current view");
                return;
            }


            _jumpControler?.Close();
            var textView = GetWpfTextView(view);
            var ace = new AceJump.AceJump();
            ace.SetView(textView);

            _input = new InputListener(view,textView);

            _jumpControler = new JumpControler(ace);

            Debug.WriteLine("AceJumpCommand.cs | MenuItemCallback | Getting input listener ready");
            _jumpControler.ShowJumpEditor();
            _input.AddFilter();
            _input.KeyPressed += InputListenerOnKeyPressed;
        }

        private IWpfTextView GetWpfTextView(IVsTextView vTextView)
        {
            IWpfTextView view = null;
            IVsUserData userData = vTextView as IVsUserData;

            if (null != userData)
            {
                IWpfTextViewHost viewHost;
                object holder;
                Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
                userData.GetData(ref guidViewHost, out holder);
                viewHost = (IWpfTextViewHost)holder;
                view = viewHost.TextView;
            }

            return view;
        }
    }
}
