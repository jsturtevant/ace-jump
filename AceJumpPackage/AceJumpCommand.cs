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
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

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

            _jumpControler = new JumpControler(AceJump);

            InputListenerCreationFactory.Instance.InputListener.KeyPressed += InputListenerOnKeyPressed;
        }

        private bool isSecondLetter = false;

        private void InputListenerOnKeyPressed(object sender, KeyPressEventArgs keyPressEventArgs)
        {
            if (_jumpControler.ControlJump(keyPressEventArgs.KeyChar))
            {
                InputListenerCreationFactory.Instance.InputListener.RemoveFilter();
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

        public static AceJump.AceJump AceJump { get; set; }

        private JumpControler _jumpControler;

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
            if (_jumpControler.Active())
                return;

            Debug.WriteLine("AceJumpCommand.cs | MenuItemCallback | Getting input listener ready");
            _jumpControler.ShowJumpEditor();
            InputListenerCreationFactory.Instance.InputListener.AddFilter();
        }
    }
}
