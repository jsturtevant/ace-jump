//------------------------------------------------------------------------------
// <copyright file="AceJumpCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using AceJump;
using AceJumpPackage.Interfaces;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace AceJumpPackage
{
    /// <summary>
    /// Command handler
    /// </summary>
    public sealed class AceJumpCommand
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
        private readonly Package myPackage;

        private readonly ICommandExecutorService myCommandExecutorService;
        private readonly IViewProvider myViewProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AceJumpCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandExecutor"></param>
        /// <param name="viewProvider"></param>
        private AceJumpCommand(IServiceProvider package, ICommandExecutorService commandExecutor, IViewProvider viewProvider)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (commandExecutor == null)
            {
                throw new ArgumentNullException(nameof(commandExecutor));
            }

            if (viewProvider == null)
            {
                throw new ArgumentNullException(nameof(viewProvider));
            }

            this._serviceProvider = package;
            this.myCommandExecutorService = commandExecutor;
            myViewProvider = viewProvider;

            OleMenuCommandService commandService = this._serviceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

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
        private IServiceProvider _serviceProvider;

        private JumpControler _jumpControler;
        private InputListener _input;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandExecutor">command executor, not null. Needed to turn off VsVim during command execution</param>
        /// <param name="viewProvider">service to get current view</param>
        public static void Initialize(IServiceProvider package, ICommandExecutorService commandExecutor, IViewProvider viewProvider)
        {
            Instance = new AceJumpCommand(package, commandExecutor, viewProvider);
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
            if (_jumpControler != null && _jumpControler.Active())
                return;

            IVsTextView view = myViewProvider.GetActiveView();
            if (view == null)
            {
                Debug.WriteLine("AceJumpCommand.cs | MenuItemCallback | could not retrieve current view");
                return;
            }

            var textView = myViewProvider.GetActiveWpfView(view);

            _jumpControler?.Close();
            var ace = new AceJump.AceJump();
            ace.SetView(textView);

            _jumpControler = new JumpControler(ace);
            _jumpControler.ShowJumpEditor();

            CreateInputListener(view, textView);
        }

        private void CreateInputListener(IVsTextView view, IWpfTextView textView)
        {
            _input = new InputListener(view, textView);
            _input.AddFilter();
            _input.KeyPressed += InputListenerOnKeyPressed;
        }
    }
}
