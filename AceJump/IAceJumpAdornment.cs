namespace AceJump
{
    public interface IAceJumpAdornment
    {
        bool Active { get; }
        void HighlightLetter(string letter);
        void ClearAdornments();
        void ShowSelector();
        void JumpTo(string key);
    }
}