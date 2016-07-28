using System.Linq;
using System.Windows.Input;
using Microsoft.VisualStudio.Text.Editor;

namespace AceJump
{
    public class AceKeyProcessor : KeyProcessor
    {

        private JumpControler jumpControler;

        public AceKeyProcessor(AceJump aceJump)
        {
            this.jumpControler = new JumpControler(aceJump);
        }

        public override void PreviewKeyUp(KeyEventArgs args)
        {
            if (this.jumpControler.Active())
            {
                if (args.Key == Key.Escape || args.Key == Key.Left || args.Key == Key.Right || args.Key == Key.Up || args.Key == Key.Down)
                {
                    this.jumpControler.Close();
                }
            }
            base.KeyDown(args);
        }

        public override void KeyDown(KeyEventArgs args)
        {
            if (Keyboard.IsKeyDown(Key.OemSemicolon) && Keyboard.IsKeyDown(Key.LeftCtrl)
                && Keyboard.IsKeyDown(Key.LeftAlt))
            {
                this.jumpControler.ShowJumpEditor();
                args.Handled = true;
                return;
            }

            string key = KeyUtility.GetKey(args.Key);
            if (!string.IsNullOrEmpty(key))
            {
                bool handled = this.jumpControler.ControlJump(key.FirstOrDefault());

                args.Handled = handled;
            }
        }
    }
}