namespace TicTacToe.Core;

public class HeuristicBot : IBotStrategy
{
    /// <summary>
    /// Chooses a move using a simple heuristic strategy:
    /// 1. If winning move exists, take it
    /// 2. If opponent can win next, block it
    /// 3. Prefer center, then corners, then edges
    /// </summary>
    /// <param name="board">The current board state</param>
    /// <returns>The chosen move</returns>
    public Move ChooseMove(Board board)
    {
        var emptyCells = board.GetEmptyCells().ToList();
        if (!emptyCells.Any())
            throw new InvalidOperationException("No empty cells available for move");

        var currentPlayer = board.CurrentPlayer;
        var opponent = currentPlayer == Cell.X ? Cell.O : Cell.X;
        Console.WriteLine($"DEBUG: Current player = {currentPlayer}");
        Console.WriteLine($"DEBUG: Opponent = {opponent}");

        // 1. Check for winning move
        foreach (var (row, col) in emptyCells)
        {
            var testMove = new Move(row, col, currentPlayer);
            var testBoard = board.Apply(testMove);
            if (testBoard.GetStatus() == (currentPlayer == Cell.X ? GameStatus.XWins : GameStatus.OWins))
            {
                return testMove;
            }
        }

        // 2. Check for blocking move (opponent can win next)
        foreach (var (row, col) in emptyCells)
        {
            if (WouldOpponentWin(board, row, col, opponent))
            {
                // Block by taking this position
                return new Move(row, col, currentPlayer);
            }
        }

        // 3. Prefer center, then corners, then edges
        var center = (1, 1);
        if (emptyCells.Contains(center))
        {
            return new Move(center.Item1, center.Item2, currentPlayer);
        }

        var corners = new[] { (0, 0), (0, 2), (2, 0), (2, 2) };
        foreach (var corner in corners)
        {
            if (emptyCells.Contains(corner))
            {
                return new Move(corner.Item1, corner.Item2, currentPlayer);
            }
        }

        // 4. Take any remaining edge position
        var edges = new[] { (0, 1), (1, 0), (1, 2), (2, 1) };
        foreach (var edge in edges)
        {
            if (emptyCells.Contains(edge))
            {
                return new Move(edge.Item1, edge.Item2, currentPlayer);
            }
        }

        // Fallback: take the first available empty cell
        var firstEmpty = emptyCells.First();
        return new Move(firstEmpty.r, firstEmpty.c, currentPlayer);
    }

    /// <summary>
    /// Checks if the opponent would win by placing their mark at the specified position.
    /// </summary>
    /// <param name="board">The current board state</param>
    /// <param name="row">Row coordinate (0-2)</param>
    /// <param name="col">Column coordinate (0-2)</param>
    /// <param name="opponent">The opponent's cell type (X or O)</param>
    /// <returns>True if the opponent would win with this move</returns>
    private static bool WouldOpponentWin(Board board, int row, int col, Cell opponent)
    {
        // Check if this move would complete a winning line for the opponent
        // Check rows
        if (row == 0 && board[1, col] == opponent && board[2, col] == opponent) return true;
        if (row == 1 && board[0, col] == opponent && board[2, col] == opponent) return true;
        if (row == 2 && board[0, col] == opponent && board[1, col] == opponent) return true;
        
        // Check columns
        if (col == 0 && board[row, 1] == opponent && board[row, 2] == opponent) return true;
        if (col == 1 && board[row, 0] == opponent && board[row, 2] == opponent) return true;
        if (col == 2 && board[row, 0] == opponent && board[row, 1] == opponent) return true;
        
        // Check diagonals
        if (row == col) // Main diagonal
        {
            if (row == 0 && board[1, 1] == opponent && board[2, 2] == opponent) return true;
            if (row == 1 && board[0, 0] == opponent && board[2, 2] == opponent) return true;
            if (row == 2 && board[0, 0] == opponent && board[1, 1] == opponent) return true;
        }
        
        if (row + col == 2) // Anti-diagonal
        {
            if (row == 0 && board[1, 1] == opponent && board[2, 0] == opponent) return true;
            if (row == 1 && board[0, 2] == opponent && board[2, 0] == opponent) return true;
            if (row == 2 && board[0, 2] == opponent && board[1, 1] == opponent) return true;
        }
        
        return false;
    }
}
