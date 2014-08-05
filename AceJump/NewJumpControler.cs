namespace AceJump
{
    public class NewJumpControler
    {
        private IAceJumpAdornment aceJump;

        private bool letterHighLightActive;

        public NewJumpControler(IAceJumpAdornment aceJump)
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

            if (key.HasValue && key.Value == '+')
            {
                this.ShowJumpEditor();
                
                // mark it handled so it doesn't go down to editor
                return true;
            }

            if (this.aceJump != null && this.aceJump.Active)
            {
                if (!key.HasValue)
                {
                    return true;
                }

                if (this.letterHighLightActive)
                {
                    this.JumpCursor(key.Value);
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

        private void JumpCursor(char jumpKey)
        {
            // they have highlighted all letters so they are ready to jump
            this.aceJump.JumpTo(jumpKey.ToString().ToUpper());
            this.letterHighLightActive = false;
            this.aceJump.ClearAdornments();
        }

        private void ShowJumpEditor()
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