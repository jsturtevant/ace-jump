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

        public bool ControlJump(char? key)
        {
            if (this.aceJump == null)
            {
                // something didn't get wired up right.  
                return false;
            }

            if (this.aceJump != null && this.aceJump.Active)
            {
                if (!key.HasValue)
                {
                    return true;
                }

                if (this.letterHighLightActive)
                {
                    if (Char.ToUpper(key.Value) >= this.aceJump.OffsetKey &&
                         !this.offsetKeyPressed)
                    {
                        this.offsetKeyPressed = true;
                        this.previouskeypress = key.Value;
                        return true;
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
                else
                {
                    this.aceJump.HighlightLetter(key.ToString().ToUpper());
                    this.letterHighLightActive = true;
                    return true;
                }
            }

            return false;
        }

        private void JumpCursor(string jumpKey)
        {
            // they have highlighted all letters so they are ready to jump
            this.aceJump.JumpTo(jumpKey.ToUpper());
            this.letterHighLightActive = false;
            this.offsetKeyPressed = false;
            this.aceJump.ClearAdornments();
        }

        public void ShowJumpEditor()
        {
            if (this.aceJump.Active)
            {
                this.aceJump.ClearAdornments();
            }
            else
            {
                this.aceJump.ShowSelector();
            }
        }
    }
}