using NumberGuess;

// Parse command-line arguments
string difficulty = "Normal";
int? seed = null;

for (int i = 0; i < args.Length; i++)
{
    switch (args[i].ToLower())
    {
        case "--difficulty":
        case "-d":
            if (i + 1 < args.Length)
            {
                string difficultyArg = args[i + 1];
                if (difficultyArg.Equals("easy", StringComparison.OrdinalIgnoreCase) ||
                    difficultyArg.Equals("normal", StringComparison.OrdinalIgnoreCase) ||
                    difficultyArg.Equals("hard", StringComparison.OrdinalIgnoreCase))
                {
                    difficulty = difficultyArg;
                    i++; // Skip the next argument since we consumed it
                }
                else
                {
                    Console.WriteLine($"❌ Invalid difficulty: {difficultyArg}. Valid options: Easy, Normal, Hard");
                    Console.WriteLine("Usage: NumberGuess [--difficulty Easy|Normal|Hard] [--seed <number>]");
                    return;
                }
            }
            else
            {
                Console.WriteLine("❌ --difficulty requires a value: Easy, Normal, or Hard");
                Console.WriteLine("Usage: NumberGuess [--difficulty Easy|Normal|Hard] [--seed <number>]");
                return;
            }
            break;

        case "--seed":
        case "-s":
            if (i + 1 < args.Length)
            {
                if (int.TryParse(args[i + 1], out int seedValue))
                {
                    seed = seedValue;
                    i++; // Skip the next argument since we consumed it
                }
                else
                {
                    Console.WriteLine($"❌ Invalid seed value: {args[i + 1]}. Must be a valid integer.");
                    Console.WriteLine("Usage: NumberGuess [--difficulty Easy|Normal|Hard] [--seed <number>]");
                    return;
                }
            }
            else
            {
                Console.WriteLine("❌ --seed requires a numeric value");
                Console.WriteLine("Usage: NumberGuess [--difficulty Easy|Normal|Hard] [--seed <number>]");
                return;
            }
            break;

        case "--help":
        case "-h":
        case "-?":
            ShowHelp();
            return;

        default:
            Console.WriteLine($"❌ Unknown argument: {args[i]}");
            Console.WriteLine("Usage: NumberGuess [--difficulty Easy|Normal|Hard] [--seed <number>]");
            Console.WriteLine("Use --help for more information.");
            return;
    }
}

Console.WriteLine($"Number Guess — Difficulty: {difficulty}" + (seed.HasValue ? $", Seed: {seed.Value}" : ""));
var game = new Game(difficulty, seed);
game.Play();

static void ShowHelp()
{
    Console.WriteLine("Number Guess Game");
    Console.WriteLine();
    Console.WriteLine("Usage: NumberGuess [options]");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  --difficulty, -d <level>    Set difficulty level (Easy, Normal, Hard)");
    Console.WriteLine("  --seed, -s <number>         Set random seed for reproducible games");
    Console.WriteLine("  --help, -h, -?              Show this help message");
    Console.WriteLine();
    Console.WriteLine("Examples:");
    Console.WriteLine("  NumberGuess                    # Play with Normal difficulty");
    Console.WriteLine("  NumberGuess --difficulty Hard  # Play with Hard difficulty");
    Console.WriteLine("  NumberGuess -d Easy -s 42     # Play Easy mode with seed 42");
}
