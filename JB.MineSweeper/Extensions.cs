using JB.MineSweeper.App;

namespace JB.MineSweeper
{
    public static class Extensions
    {
        public static string ToBoardCoOrds(this XYCoordinates xYCoordinates)
        {
            var xLetter = ((ConsoleKey)(xYCoordinates.X + 65));

            return $"{xLetter}{xYCoordinates.Y}";
        }
    }
}
