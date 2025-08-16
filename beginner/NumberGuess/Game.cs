namespace NumberGuess;

public class Game
{
    private readonly string _difficulty;
    private readonly Random _random;
    private int _secret;
    private int _lower;
    private int _upper;
    private bool _running;
    private int _attempts;
    private int _bestScore = int.MaxValue;
    private int _previousDistance = -1;

    public Game(string difficulty, int? seed = null)
    {
        _difficulty = difficulty;
        _random = seed.HasValue ? new Random(seed.Value) : new Random();
        // PROMPT: Implement ConfigureRange() to set (_lower,_upper) for Easy(1-50), Normal(1-100), Hard(1-500).
        ConfigureRange();
        ResetSecret();
    }

    private void ResetSecret()
    {
        _secret = _random.Next(_lower, _upper + 1);
        _attempts = 0;
        _running = true;
    }

    private void ConfigureRange()
    {
        switch (_difficulty.ToLower())
        {
            case "easy":
                _lower = 1;
                _upper = 50;
                break;
            case "normal":
                _lower = 1;
                _upper = 100;
                break;
            case "hard":
                _lower = 1;
                _upper = 500;
                break;
            default:
                Console.WriteLine($"Unknown difficulty '{_difficulty}'. Defaulting to Normal difficulty.");
                _lower = 1;
                _upper = 100;
                break;
        }
    }

    public void Play()
    {
        Console.WriteLine($"\n🎯 Welcome to Number Guess! ({_difficulty} mode)");
        Console.WriteLine($"I'm thinking of a number between {_lower} and {_upper}.");
        Console.WriteLine("Type 'quit' to exit the game.\n");

        while (_running)
        {
            Console.Write($"Guess #{_attempts + 1}: ");
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("❌ Please enter a number or 'quit'.");
                continue;
            }

            if (input == "quit")
            {
                Console.WriteLine("👋 Thanks for playing! Goodbye!");
                _running = false;
                return;
            }

            if (!int.TryParse(input, out int guess))
            {
                Console.WriteLine("❌ Please enter a valid number or 'quit'.");
                continue;
            }

            if (guess < _lower || guess > _upper)
            {
                Console.WriteLine($"❌ Please enter a number between {_lower} and {_upper}.");
                continue;
            }

            _attempts++;

            if (guess == _secret)
            {
                HandleCorrectGuess();
            }
            else
            {
                string hint = GetHint(guess);
                Console.WriteLine($"💡 {hint}");
            }
        }
    }

    private void HandleCorrectGuess()
    {
        int rangeSize = _upper - _lower + 1;
        int score = ScoreCalculator.Calculate(_attempts, rangeSize);
        
        Console.WriteLine($"\n🎉 Congratulations! You found the number in {_attempts} attempt(s)!");
        Console.WriteLine($"📊 Your score: {score} (lower is better)");
        
        if (score < _bestScore)
        {
            Console.WriteLine("🏆 New best score!");
            _bestScore = score;
        }
        else if (_bestScore != int.MaxValue)
        {
            Console.WriteLine($"🥈 Best score so far: {_bestScore}");
        }

        Console.Write("\n🔄 Play again? (y/n): ");
        string? response = Console.ReadLine()?.Trim().ToLower();
        
        if (response == "y" || response == "yes")
        {
            Console.WriteLine("\n" + new string('-', 40));
            ResetSecret();
            _previousDistance = -1; // Reset for new game
        }
        else
        {
            Console.WriteLine("👋 Thanks for playing! Goodbye!");
            _running = false;
        }
    }

    private string GetHint(int lastGuess)
    {
        int currentDistance = Math.Abs(lastGuess - _secret);
        
        if (_previousDistance == -1)
        {
            // First guess - just provide higher/lower hint
            _previousDistance = currentDistance;
            return lastGuess < _secret ? "higher" : "lower";
        }
        
        // Compare with previous distance for warm/colder hint
        string distanceHint = currentDistance < _previousDistance ? "warmer" : 
                             currentDistance > _previousDistance ? "colder" : "same distance";
        
        _previousDistance = currentDistance;
        
        return lastGuess < _secret ? $"higher ({distanceHint})" : $"lower ({distanceHint})";
    }
}
