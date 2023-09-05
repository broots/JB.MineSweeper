namespace JB.MineSweeper.App
{
    public interface IMineField
    {
        XYCoordinates PlayerPosition { get; }
        int NumberOfLives { get; }
        int NumberOfMoves { get; }
        bool IsEnd { get; }
        void ProcessMove(ConsoleKey input);
    }
}
