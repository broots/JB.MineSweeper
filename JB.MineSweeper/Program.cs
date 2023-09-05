// See https://aka.ms/new-console-template for more information
using JB.MineSweeper;
using JB.MineSweeper.App;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
        .AddJsonFile("config.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection, config);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var game = serviceProvider.GetService<IMineField>();

        if (game != null)
        {
            StartMineSweeperGame(game);
        }

    }

    private static void ConfigureServices(ServiceCollection serviceCollection, IConfiguration config)
    {
        serviceCollection.AddOptions<BoardSettings>().Bind(config.GetSection("BoardSettingOptions"));
        serviceCollection.AddTransient<IMineField, MineFieldGame>();
    }

    private static void StartMineSweeperGame(IMineField game)
    {
        ConsoleKeyInfo cki;

        Console.WriteLine("Press the Escape (Esc) key to quit: \n");
        Console.WriteLine("Use arrow keys to play: \n");
        Console.WriteLine($"You have {game.NumberOfLives} lives.");
        do
        {
            cki = Console.ReadKey();
            try
            {
                var lives = game.NumberOfLives;
                game.ProcessMove(cki.Key);

                if (game.IsEnd)
                {
                    Console.WriteLine($"You had reached the end. You have {game.NumberOfLives} lives left. Final Score: {game.NumberOfMoves}");
                    Console.WriteLine("Press the Escape (Esc) key to quit: \n");
                    continue;
                }

                if (game.NumberOfLives < lives)
                    Console.WriteLine("Hit a mine");

                Console.WriteLine($"Your current position is {game.PlayerPosition.ToBoardCoOrds()}. You have {game.NumberOfLives} lives left. Number of moves: {game.NumberOfMoves}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press the Escape (Esc) key to quit: \n");
            }
        } while (cki.Key != ConsoleKey.Escape);
    }

}
