using AceJumpPackage.Interfaces;

namespace AceJump
{
    using System;

    public class JumpControler
    {
        private IAceJumpAdornment aceJump;

        private bool letterHighLightActive;

        private char previouskeypress;

        private bool offsetKeyPressed = false;

        public JumpControler(IAceJumpAdornment aceJump)
        {
            this.aceJump = aceJump;
        }

        /// <summary>
        /// Controls the jump.
        /// </summary>
        /// <param name="key">The pressed key.</param>
        /// <returns><c>true</c> if we moved the cursor (i.e. we jumped) or aborted else <c>false</c></returns>
        /// <exception cref="ArgumentNullException">aceJump is not set</exception>
        public bool ControlJump(char? key)
        {
            if (this.aceJump == null)
            {
                // something didn't get wired up right.  
                throw new ArgumentNullException("aceJump is not set");
            }

            if (this.aceJump != null && this.aceJump.Active)
            {
                if (!key.HasValue)
                {
                    return false;
                }

                if(key == '\0')
                {
                    Close();
                    return true;
                }


                if (this.letterHighLightActive)
                {
                    if (Char.ToUpper(key.Value) >= this.aceJump.OffsetKey &&
                         !this.offsetKeyPressed)
                    {
                        this.offsetKeyPressed = true;
                        this.previouskeypress = key.Value;
                        this.aceJump.UpdateLetter(key.Value.ToString());
                        return false;
                    }

                    if (this.offsetKeyPressed)
                    {
                        this.JumpCursor(string.Format("{0}{1}", this.previouskeypress, key.Value));
                    }
                    else
                    {
                        this.JumpCursor(key.Value.ToString());
                    }
                    return true;
                }
                this.aceJump.HighlightLetter(key.ToString().ToUpper());

                this.letterHighLightActive = true;
                return false;
            }

            return false;
        }

        private void JumpCursor(string jumpKey)
        {
            // they have highlighted all letters so they are ready to jump
            this.aceJump.JumpTo(jumpKey.ToUpper());
            this.Close();
        }

        public bool Active()
        {
            return this.aceJump.Active;
        }

        public void Close()
        {
            this.letterHighLightActive = false;
            this.offsetKeyPressed = false;
            this.aceJump.ClearAdornments();
        }

        public void ShowJumpEditor()
        {
            if (this.aceJump.Active)
            {
                this.Close();
            }
            else
            {
                this.aceJump.ShowSelector();
            }
        }
    }
}