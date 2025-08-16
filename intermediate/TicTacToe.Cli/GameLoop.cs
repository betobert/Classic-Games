using TicTacToe.Core;

namespace TicTacToe.Cli;

public class GameLoop
{
    private readonly IBotStrategy _bot;
    private readonly Stack<Board> _history = new();
    private readonly Stack<Board> _redoStack = new();
    private Board _currentBoard;

    public GameLoop(IBotStrategy bot)
    {
        _bot = bot;
        _currentBoard = new Board();
        _history.Push(_currentBoard);
    }

    public void Run()
    {
        Console.WriteLine("Tic-Tac-Toe — Human (X) vs Bot (O)");
        Console.WriteLine("Commands: 'row,col' to move, 'u' to undo, 'r' to redo, 'q' to quit");

        while (true)
        {
            Render(_currentBoard);

            // Human move
            Console.Write("Enter move (row,col), 'u' to undo, 'r' to redo, or 'q' to quit: ");
            var input = Console.ReadLine();
            
            if (string.IsNullOrEmpty(input)) continue;

            if (string.Equals(input, "q", StringComparison.OrdinalIgnoreCase)) 
                break;

            if (string.Equals(input, "u", StringComparison.OrdinalIgnoreCase))
            {
                Undo();
                continue;
            }

            if (string.Equals(input, "r", StringComparison.OrdinalIgnoreCase))
            {
                Redo();
                continue;
            }

            // Parse input, validate, and apply move
            try
            {
                if (!TryParseAndApplyMove(input))
                    continue;

                // Check game status after human move
                var status = _currentBoard.GetStatus();
                if (status != GameStatus.InProgress)
                {
                    Render(_currentBoard);
                    DisplayGameResult(status);
                    break;
                }

                // Bot's turn
                Console.WriteLine("Bot is thinking...");
                var botMove = _bot.ChooseMove(_currentBoard);
                _currentBoard = _currentBoard.Apply(botMove);
                _history.Push(_currentBoard);
                _redoStack.Clear(); // Clear redo stack when new move is made

                // Check game status after bot move
                status = _currentBoard.GetStatus();
                if (status != GameStatus.InProgress)
                {
                    Render(_currentBoard);
                    DisplayGameResult(status);
                    break;
                }

                // After bot's turn, continue the loop to show the board for human's next turn
                // The current player should now be X (human's turn)
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }

    private bool TryParseAndApplyMove(string input)
    {
        // Parse the input
        var parts = input.Split(',');
        if (parts.Length != 2)
        {
            Console.WriteLine("Error: Please enter coordinates in format 'row,col' (e.g., '2,3')");
            return false;
        }

        // Parse row and column
        if (!int.TryParse(parts[0].Trim(), out int row) || !int.TryParse(parts[1].Trim(), out int col))
        {
            Console.WriteLine("Error: Row and column must be numbers");
            return false;
        }

        // Convert from 1-based user input to 0-based board coordinates
        int boardRow = row - 1;
        int boardCol = col - 1;

        // Validate coordinates are within bounds
        if (boardRow < 0 || boardRow >= 3 || boardCol < 0 || boardCol >= 3)
        {
            Console.WriteLine("Error: Row and column must be between 1 and 3");
            return false;
        }

        // Create and apply the move
        var move = new Move(boardRow, boardCol, _currentBoard.CurrentPlayer);
        _currentBoard = _currentBoard.Apply(move);
        _history.Push(_currentBoard);
        _redoStack.Clear(); // Clear redo stack when new move is made

        return true;
    }

    private void Undo()
    {
        if (_history.Count <= 1)
        {
            Console.WriteLine("Cannot undo: No moves to undo");
            return;
        }

        // Remove current board from history
        _history.Pop();
        
        // Get the previous board
        _currentBoard = _history.Peek();
        
        // Add current board to redo stack
        _redoStack.Push(_currentBoard);
        
        Console.WriteLine("Move undone");
    }

    private void Redo()
    {
        if (_redoStack.Count == 0)
        {
            Console.WriteLine("Cannot redo: No moves to redo");
            return;
        }

        // Get the next board from redo stack
        _currentBoard = _redoStack.Pop();
        
        // Add it back to history
        _history.Push(_currentBoard);
        
        Console.WriteLine("Move redone");
    }

    private static void Render(Board b)
    {
        Console.WriteLine();
        Console.WriteLine("    1   2   3  (columns)");
        Console.WriteLine("  ┌───┬───┬───┐");
        
        for (int row = 0; row < 3; row++)
        {
            Console.Write($"{row + 1} │");
            for (int col = 0; col < 3; col++)
            {
                var cell = b[row, col];
                var display = cell switch
                {
                    Cell.Empty => "   ",
                    Cell.X => " X ",
                    Cell.O => " O ",
                    _ => "   "
                };
                
                Console.Write(display);
                if (col < 2) Console.Write("│");
            }
            Console.WriteLine("│");
            
            if (row < 2)
            {
                Console.WriteLine("  ├───┼───┼───┤");
            }
        }
        
        Console.WriteLine("  └───┴───┴───┘");
        Console.WriteLine("(rows)");
        Console.WriteLine();
        
        // Show current player
        Console.WriteLine($"Current player: {b.CurrentPlayer}");
        Console.WriteLine();
    }

    private static void DisplayGameResult(GameStatus status)
    {
        Console.WriteLine();
        switch (status)
        {
            case GameStatus.XWins:
                Console.WriteLine("🎉 Congratulations! You (X) win!");
                break;
            case GameStatus.OWins:
                Console.WriteLine("🤖 The bot (O) wins!");
                break;
            case GameStatus.Draw:
                Console.WriteLine("🤝 It's a draw!");
                break;
            default:
                Console.WriteLine("Game is still in progress.");
                break;
        }
        Console.WriteLine();
    }
} 
