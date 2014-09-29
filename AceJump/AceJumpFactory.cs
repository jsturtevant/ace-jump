namespace AceJump
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using System.ComponentModel.Composition;

    using Microsoft.VisualStudio.Shell.Events;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(IKeyProcessorProvider))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    [Name("AceKeyProcessor")]
    internal sealed class AceKeyProcessorProvider : IKeyProcessorProvider
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("AceJump")]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;

        public KeyProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            var aceJump = new AceJump();
            var aceKeyProcessor = new AceKeyProcessor(aceJump);

            aceJump.SetView(wpfTextView);
            return aceKeyProcessor;
        }
    }

    public class AceKeyProcessor : KeyProcessor
    {
        private KeyConverter keyConverter;
        private JumpControler jumpControler;

        public AceKeyProcessor(AceJump aceJump)
        {

            this.jumpControler = new JumpControler(aceJump);
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

    public class KeyUtility
    {
        private static bool IsShiftKey()
        {
            return Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
        }

        public static string GetKey(Key key)
        {
            var keyConverter = new KeyConverter();

            // converts it to string representation.  IE. Key.E = "E" and Key.OemComma = "OemComma"
            string character = keyConverter.ConvertToString(key);
            if (character != null && character.Length == 1)
            {
                if (char.IsLetter(character, 0))
                {
                    return character;
                }

                if (char.IsNumber(character, 0) && !IsShiftKey())
                {
                    return character;
                }
                
                if (char.IsNumber(character, 0) && IsShiftKey())
                {
                    switch (key)
                    {
                        // http://msdn.microsoft.com/en-us/library/system.windows.forms.keys(v=vs.110).aspx
                        case Key.D1:
                            return "!";
                        case Key.D2:
                            return "@";
                        case Key.D3:
                            return "#";
                        case Key.D4:
                            return "$";
                        case Key.D5:
                            return "%";
                        case Key.D6:
                            return "^";
                        case Key.D7:
                            return "&";
                        case Key.D8:
                            return "*";
                        case Key.D9:
                            return "(";
                        case Key.D0:
                            return ")";
                    }
                }
            }

            switch (key)
            {
                // http://msdn.microsoft.com/en-us/library/system.windows.forms.keys(v=vs.110).aspx
                case Key.Oem4: return IsShiftKey() ? "{" : "[";
                case Key.Oem6: return IsShiftKey() ? "}" : "]";
                case Key.Oem5: return IsShiftKey() ? "|" : @"\";
                case Key.OemMinus: return IsShiftKey() ? "_" : "-";
                case Key.OemPlus: return IsShiftKey() ? "+" : "=";
                case Key.OemQuestion: return IsShiftKey() ? "?" : "/";
                case Key.OemSemicolon: return IsShiftKey() ? ":" : ";";
                case Key.Oem7: return IsShiftKey() ? "'" : "\"";
                case Key.OemPeriod: return IsShiftKey() ? ">" : ".";
                case Key.OemComma: return IsShiftKey() ? "<" : ",";
            }

            return string.Empty;
        }
    }
}
