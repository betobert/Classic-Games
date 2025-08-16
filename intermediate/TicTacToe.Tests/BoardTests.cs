using TicTacToe.Core;
using Xunit;
using System;

namespace TicTacToe.Tests;

public class BoardTests
{
    [Fact]
    public void EmptyBoard_IsInProgress()
    {
        var b = new Board();
        Assert.Equal(GameStatus.InProgress, b.GetStatus());
    }

    [Fact]
    public void XCompletesRow_Wins()
    {
        // Arrange a sequence where X wins on top row
        var b = new Board();
        
        // X moves to top-left
        b = b.Apply(new Move(0, 0, Cell.X));
        Assert.Equal(GameStatus.InProgress, b.GetStatus());
        Assert.Equal(Cell.O, b.CurrentPlayer);
        
        // O moves to center
        b = b.Apply(new Move(1, 1, Cell.O));
        Assert.Equal(GameStatus.InProgress, b.GetStatus());
        Assert.Equal(Cell.X, b.CurrentPlayer);
        
        // X moves to top-center
        b = b.Apply(new Move(0, 1, Cell.X));
        Assert.Equal(GameStatus.InProgress, b.GetStatus());
        Assert.Equal(Cell.O, b.CurrentPlayer);
        
        // O moves to bottom-left
        b = b.Apply(new Move(2, 0, Cell.O));
        Assert.Equal(GameStatus.InProgress, b.GetStatus());
        Assert.Equal(Cell.X, b.CurrentPlayer);
        
        // X moves to top-right (completing the top row)
        b = b.Apply(new Move(0, 2, Cell.X));
        
        // Assert X wins
        Assert.Equal(GameStatus.XWins, b.GetStatus());
    }

    [Fact]
    public void Apply_IsImmutable()
    {
        // Arrange
        var originalBoard = new Board();
        
        // Act
        var newBoard = originalBoard.Apply(new Move(0, 0, Cell.X));
        
        // Assert - original board should be unchanged
        Assert.Equal(Cell.Empty, originalBoard[0, 0]);
        Assert.Equal(Cell.X, originalBoard.CurrentPlayer);
        
        // New board should have the move applied
        Assert.Equal(Cell.X, newBoard[0, 0]);
        Assert.Equal(Cell.O, newBoard.CurrentPlayer);
    }

    [Fact]
    public void Apply_InvalidMove_ThrowsException()
    {
        // Arrange
        var board = new Board();
        
        // Act & Assert - trying to move to occupied cell
        board = board.Apply(new Move(0, 0, Cell.X));
        Assert.Throws<ArgumentException>(() => board.Apply(new Move(0, 0, Cell.O)));
    }

    [Fact]
    public void Apply_WrongPlayer_ThrowsException()
    {
        // Arrange
        var board = new Board();
        
        // Act & Assert - O trying to move when it's X's turn
        Assert.Throws<ArgumentException>(() => board.Apply(new Move(0, 0, Cell.O)));
    }

    [Fact]
    public void Apply_OutOfBounds_ThrowsException()
    {
        // Arrange
        var board = new Board();
        
        // Act & Assert - trying to move outside the board
        Assert.Throws<ArgumentException>(() => board.Apply(new Move(3, 0, Cell.X)));
        Assert.Throws<ArgumentException>(() => board.Apply(new Move(0, 3, Cell.X)));
        Assert.Throws<ArgumentException>(() => board.Apply(new Move(-1, 0, Cell.X)));
        Assert.Throws<ArgumentException>(() => board.Apply(new Move(0, -1, Cell.X)));
    }

    [Fact]
    public void OWins_OnDiagonal()
    {
        // Arrange a sequence where O wins on main diagonal
        var b = new Board();
        
        // X moves to top-center
        b = b.Apply(new Move(0, 1, Cell.X));
        
        // O moves to top-left (start of diagonal)
        b = b.Apply(new Move(0, 0, Cell.O));
        
        // X moves to bottom-left
        b = b.Apply(new Move(2, 0, Cell.X));
        
        // O moves to center (middle of diagonal)
        b = b.Apply(new Move(1, 1, Cell.O));
        
        // X moves to top-right
        b = b.Apply(new Move(0, 2, Cell.X));
        
        // O moves to bottom-right (completing diagonal: 0,0 -> 1,1 -> 2,2)
        b = b.Apply(new Move(2, 2, Cell.O));
        
        // Assert O wins
        Assert.Equal(GameStatus.OWins, b.GetStatus());
    }

    [Fact]
    public void GameEndsInDraw()
    {
        // Arrange a sequence that results in a draw
        var b = new Board();
        
        // X | O | X
        // O | X | X
        // O | X | O
        
        b = b.Apply(new Move(0, 0, Cell.X)); // X top-left
        b = b.Apply(new Move(0, 1, Cell.O)); // O top-center
        b = b.Apply(new Move(0, 2, Cell.X)); // X top-right
        b = b.Apply(new Move(1, 0, Cell.O)); // O middle-left
        b = b.Apply(new Move(1, 1, Cell.X)); // X center
        b = b.Apply(new Move(2, 0, Cell.O)); // O bottom-left
        b = b.Apply(new Move(1, 2, Cell.X)); // X middle-right
        b = b.Apply(new Move(2, 2, Cell.O)); // O bottom-right
        b = b.Apply(new Move(2, 1, Cell.X)); // X bottom-center
        
        // Assert draw
        Assert.Equal(GameStatus.Draw, b.GetStatus());
    }
}
