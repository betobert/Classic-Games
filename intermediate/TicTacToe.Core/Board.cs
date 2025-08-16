namespace TicTacToe.Core;

public enum Cell { Empty, X, O }
public enum GameStatus { InProgress, XWins, OWins, Draw }

public record Move(int Row, int Col, Cell Player);

public class Board
{
    private readonly Cell[,] _cells = new Cell[3,3];
    public Cell this[int r, int c] => _cells[r,c];
    public Cell CurrentPlayer { get; private set; } = Cell.X;

    // Public constructor for creating new boards
    public Board()
    {
        // Initialize with empty cells and X as first player
    }

    // Private constructor for creating copies
    private Board(Cell[,] cells, Cell currentPlayer)
    {
        _cells = new Cell[3, 3];
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                _cells[r, c] = cells[r, c];
            }
        }
        CurrentPlayer = currentPlayer;
    }

    /// <summary>
    /// Applies a move to the board and returns a new immutable board instance.
    /// Validates the move is legal and flips the current player.
    /// </summary>
    /// <param name="move">The move to apply</param>
    /// <returns>A new board with the move applied</returns>
    /// <exception cref="ArgumentException">Thrown when move is invalid (out of bounds, cell occupied, wrong player)</exception>
    public Board Apply(Move move)
    {
        // Validate move coordinates are within bounds
        if (move.Row < 0 || move.Row >= 3 || move.Col < 0 || move.Col >= 3)
        {
            throw new ArgumentException($"Move coordinates ({move.Row}, {move.Col}) are out of bounds. Must be 0-2.");
        }

        // Validate the cell is empty
        if (_cells[move.Row, move.Col] != Cell.Empty)
        {
            throw new ArgumentException($"Cell at ({move.Row}, {move.Col}) is already occupied by {_cells[move.Row, move.Col]}.");
        }

        // Validate the move is for the current player
        if (move.Player != CurrentPlayer)
        {
            throw new ArgumentException($"Move is for {move.Player} but current player is {CurrentPlayer}.");
        }

        // Create a new board with copied state
        var newBoard = new Board(_cells, CurrentPlayer);
        
        // Apply the move to the new board
        newBoard._cells[move.Row, move.Col] = move.Player;
        
        // Flip the current player
        newBoard.CurrentPlayer = CurrentPlayer == Cell.X ? Cell.O : Cell.X;
        
        return newBoard;
    }

    /// <summary>
    /// Determines the current game status by checking all winning combinations and draw conditions.
    /// Checks rows, columns, diagonals for winners, and determines if game is a draw or still in progress.
    /// </summary>
    /// <returns>The current game status</returns>
    public GameStatus GetStatus()
    {
        // Check rows for winner
        for (int row = 0; row < 3; row++)
        {
            if (_cells[row, 0] != Cell.Empty && 
                _cells[row, 0] == _cells[row, 1] && 
                _cells[row, 1] == _cells[row, 2])
            {
                return _cells[row, 0] == Cell.X ? GameStatus.XWins : GameStatus.OWins;
            }
        }

        // Check columns for winner
        for (int col = 0; col < 3; col++)
        {
            if (_cells[0, col] != Cell.Empty && 
                _cells[0, col] == _cells[1, col] && 
                _cells[1, col] == _cells[2, col])
            {
                return _cells[0, col] == Cell.X ? GameStatus.XWins : GameStatus.OWins;
            }
        }

        // Check main diagonal (top-left to bottom-right)
        if (_cells[0, 0] != Cell.Empty && 
            _cells[0, 0] == _cells[1, 1] && 
            _cells[1, 1] == _cells[2, 2])
        {
            return _cells[0, 0] == Cell.X ? GameStatus.XWins : GameStatus.OWins;
        }

        // Check anti-diagonal (top-right to bottom-left)
        if (_cells[0, 2] != Cell.Empty && 
            _cells[0, 2] == _cells[1, 1] && 
            _cells[1, 1] == _cells[2, 0])
        {
            return _cells[0, 2] == Cell.X ? GameStatus.XWins : GameStatus.OWins;
        }

        // Check for draw (all cells filled)
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (_cells[row, col] == Cell.Empty)
                {
                    return GameStatus.InProgress;
                }
            }
        }

        // If we get here, all cells are filled and no winner
        return GameStatus.Draw;
    }

    /// <summary>
    /// Returns the coordinates of all empty cells on the board.
    /// Useful for AI strategies to find available moves.
    /// </summary>
    /// <returns>Enumerable of (row, column) tuples for empty cells</returns>
    public IEnumerable<(int r, int c)> GetEmptyCells()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (_cells[row, col] == Cell.Empty)
                {
                    yield return (row, col);
                }
            }
        }
    }
}
