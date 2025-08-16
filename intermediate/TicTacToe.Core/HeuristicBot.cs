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
            var testMove = new Move(row, col, opponent);
            var testBoard = board.Apply(testMove);
            if (testBoard.GetStatus() == (opponent == Cell.X ? GameStatus.XWins : GameStatus.OWins))
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
}
