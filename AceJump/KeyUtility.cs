﻿// Modification of https://editorsamples.codeplex.com/ see history for original
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



namespace AceJump
{
    using System.Windows.Input;

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