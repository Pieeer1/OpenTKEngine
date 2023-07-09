namespace OpenTKEngine.Enums
{
    [Flags]
    public enum InputFlags
    {
        None = 0,
        Player = 1 << 0,
        Menu = 1 << 1,
        Chat = 1 << 2,

        All = Player | Menu | Chat
    }
}
