using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace JB.MineSweeper.App.Tests
{
    [TestClass()]
    public class MineFieldGameTests
    {
        private IMineField? _game;
        private readonly Mock<IOptions<BoardSettings>> _boardOptions;

        public MineFieldGameTests()
        {
            _boardOptions = new Mock<IOptions<BoardSettings>>();
        }

        [TestMethod()]
        public void MaxLivesSet_Test()
        {
            _boardOptions.Setup(o => o.Value).Returns(new BoardSettings { MaxLives = 5 });
            _game = new MineFieldGame(_boardOptions.Object);

            Assert.IsTrue(_game.NumberOfLives == 5);
        }

        [TestMethod()]
        public void MineHit_Test()
        {
            var board = new BoardSettings { BoardHeight = 2, BoardWidth = 1, MaxLives = 1, NumberOfMines = 1 };

            _boardOptions.Setup(o => o.Value).Returns(board);
            _game = new MineFieldGame(_boardOptions.Object);

            _game.ProcessMove(ConsoleKey.UpArrow);

            Assert.IsTrue(_game.NumberOfLives == 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "No player Lives left")]
        public void NoLives_Test()
        {
            var board = new BoardSettings { BoardHeight = 2, BoardWidth = 1, MaxLives = 0, NumberOfMines = 1 };

            _boardOptions.Setup(o => o.Value).Returns(board);
            _game = new MineFieldGame(_boardOptions.Object);

            _game.ProcessMove(ConsoleKey.UpArrow);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Invalid Input")]
        public void InvalidInput_Test()
        {
            var board = new BoardSettings { BoardHeight = 2, BoardWidth = 1, MaxLives = 1, NumberOfMines = 1 };

            _boardOptions.Setup(o => o.Value).Returns(board);
            _game = new MineFieldGame(_boardOptions.Object);

            _game.ProcessMove(ConsoleKey.C);
        }

        [TestMethod()]
        public void IsEnd_Test()
        {
            var board = new BoardSettings { BoardHeight = 1, BoardWidth = 0, MaxLives = 1, NumberOfMines = 0 };

            _boardOptions.Setup(o => o.Value).Returns(board);
            _game = new MineFieldGame(_boardOptions.Object);

            _game.ProcessMove(ConsoleKey.UpArrow);

            Assert.IsTrue(_game.IsEnd);
        }

        [TestMethod()]
        public void NumberOfMoves_Test()
        {
            var board = new BoardSettings { BoardHeight = 5, BoardWidth = 5, MaxLives = 5, NumberOfMines = 2 };

            _boardOptions.Setup(o => o.Value).Returns(board);
            _game = new MineFieldGame(_boardOptions.Object);

            _game.ProcessMove(ConsoleKey.UpArrow);
            _game.ProcessMove(ConsoleKey.LeftArrow);
            _game.ProcessMove(ConsoleKey.DownArrow);
            _game.ProcessMove(ConsoleKey.RightArrow);

            Assert.IsTrue(_game.NumberOfMoves == 4);
        }
    }
}