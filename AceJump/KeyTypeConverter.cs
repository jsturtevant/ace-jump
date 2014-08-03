//Microsoft Public License (Ms-PL)
//This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
//1. Definitions
//The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
//A "contribution" is the original software, or any additions or changes to the software.
//A "contributor" is any person that distributes its contribution under this license.
//"Licensed patents" are a contributor's patent claims that read directly on its contribution.
//2. Grant of Rights
//(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
//(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
//3. Conditions and Limitations
//(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
//(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
//(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
//(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
//(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
//https://editorsamples.codeplex.com/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AceJump
{


    internal class KeyTypeConverter : System.Windows.Input.KeyConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
        {
            if (((destinationType != typeof(char)) || (context == null)) || (context.Instance == null))
            {
                return base.CanConvertTo(context, destinationType);
            }

            Key instance = (Key)context.Instance;
            return instance >= Key.None && instance <= Key.OemClear;
        }

        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(char) && value != null)
            {
                string key = base.ConvertToString(value);

                if (!string.IsNullOrEmpty(key) && key.Length == 1)
                {
                    if (char.IsLetter(key, 0))
                    {
                        return IsShiftKeyDown() ? key.ToUpper()[0] : key.ToLower()[0];
                    }
                    else if (char.IsDigit(key, 0) && !IsShiftKeyDown())
                    {
                        return key[0];
                    }
                }

                switch ((Key)value)
                {
                    case Key.Oem4: return IsShiftKeyDown() ? '{' : '[';
                    case Key.Oem6: return IsShiftKeyDown() ? '}' : ']';
                    case Key.Oem5: return IsShiftKeyDown() ? '|' : '\\';
                    case Key.D1: return '!';
                    case Key.D2: return '@';
                    case Key.D3: return '#';
                    case Key.D4: return '$';
                    case Key.D5: return '%';
                    case Key.D6: return '^';
                    case Key.D7: return '&';
                    case Key.D8: return '*';
                    case Key.D9: return '(';
                    case Key.D0: return ')';
                    case Key.Space: return ' ';
                    case Key.OemMinus: return IsShiftKeyDown() ? '_' : '-';
                    case Key.OemPlus: return IsShiftKeyDown() ? '+' : '=';
                    case Key.OemQuestion: return IsShiftKeyDown() ? '?' : '/';
                    case Key.OemSemicolon: return IsShiftKeyDown() ? ':' : ';';
                    case Key.Oem7: return IsShiftKeyDown() ? '"' : '\'';
                    case Key.OemPeriod: return IsShiftKeyDown() ? '>' : '.';
                    case Key.OemComma: return IsShiftKeyDown() ? '<' : ',';
                }

                return null;
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public char? ConvertToChar(Key key)
        {
            object result = ConvertTo(key, typeof(char));
            if (result != null)
            {
                return (char)result;
            }

            return null;
        }

        private bool IsShiftKeyDown()
        {
            return Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftShift);
        }
    }
}