using Microsoft.Extensions.Options;

namespace JB.MineSweeper.App
{
    public class MineFieldGame : IMineField
    {
        private XYCoordinates _playerPosition = new XYCoordinates { X = 0, Y = 0 };
        private int _numberOfLives;
        private int _numberOfMoves;

        private readonly XYCoordinates _boardCoordinates;
        private Random _random = new();
        private List<XYCoordinates> _mineCoordinates = new();

        public XYCoordinates PlayerPosition => _playerPosition;
        public int NumberOfLives => _numberOfLives;
        public int NumberOfMoves => _numberOfMoves;

        public bool IsEnd => _playerPosition.X == _boardCoordinates.X && _playerPosition.Y == _boardCoordinates.Y;

        public MineFieldGame(IOptions<BoardSettings> options)
        {
            var boardSettings = options.Value;
            _boardCoordinates = new XYCoordinates { X = boardSettings.BoardWidth, Y = boardSettings.BoardHeight };
            _numberOfLives = boardSettings.MaxLives;
            SetupMines(boardSettings.NumberOfMines);
        }

        public void ProcessMove(ConsoleKey input)
        {
            if (_numberOfLives == 0)
                throw new ArgumentException("No player Lives left");

            if (IsEnd) return;

            _numberOfMoves++;

            if ((_playerPosition.Y == _boardCoordinates.Y && input == ConsoleKey.UpArrow) || (_playerPosition.Y == 0 && input == ConsoleKey.DownArrow))
                return;

            if ((_playerPosition.X == _boardCoordinates.X && input == ConsoleKey.RightArrow) || (_playerPosition.X == 0 && input == ConsoleKey.LeftArrow))
                return;

            switch (input)
            {
                case ConsoleKey.UpArrow:
                    _playerPosition.Y++;
                    break;
                case ConsoleKey.DownArrow:
                    _playerPosition.Y--;
                    break;
                case ConsoleKey.LeftArrow:
                    _playerPosition.X--;
                    break;
                case ConsoleKey.RightArrow:
                    _playerPosition.X++;
                    break;
                default:
                throw new ArgumentException("Invalid Input");
            }

            if (_mineCoordinates.Any(m => m.X == _playerPosition.X && m.Y == _playerPosition.Y))
                _numberOfLives--;
        }

        private void SetupMines(int numberOfMines)
        {
            var mineXMax = _boardCoordinates.X - 1;
            var mineYMax = _boardCoordinates.Y - 1;

            if (mineXMax < 1 && mineYMax < 1) return;

            var mineXMin = mineXMax == 0 ? 0 : 1;
            var mineYMin = mineYMax == 0 ? 0 : 1;

            for (var i = 0; i < numberOfMines; i++)
            {
                var x = _random.Next(mineXMin, mineXMax);
                var y = _random.Next(mineYMin, mineYMax);

                _mineCoordinates.Add(new XYCoordinates { X = x, Y = y });
            }
        }
    }
}
