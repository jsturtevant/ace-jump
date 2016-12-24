namespace AceJumpPackage.Interfaces
{
    public interface IAceJumpAdornment
    {
        bool Active { get; }

        char? OffsetKey { get;  }

        void HighlightLetter(string letter);
        void UpdateLetter(string ch);
        void ClearAdornments();
        void ShowSelector();
        void JumpTo(string key);
    }
}