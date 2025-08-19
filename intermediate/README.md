# START HERE — Intermediate: Tic-Tac-Toe (Engine + CLI, .NET 8)

## Prerequisites
- **.NET 8 SDK** installed and verified with `dotnet --version`

## Project Setup
```bash
cd classic-games/intermediate
dotnet build ./TicTacToe.Core/TicTacToe.Core.csproj
dotnet build ./TicTacToe.Cli/TicTacToe.Cli.csproj
dotnet build ./TicTacToe.Tests/TicTacToe.Tests.csproj
# Tests will fail until you complete the prompts below
```


## Before You Prompt: Explore the Project
Spend 5–10 minutes getting the lay of the land.

1. Build and attempt tests / run:
   ```bash
   dotnet build ./TicTacToe.Core/TicTacToe.Core.csproj
   dotnet build ./TicTacToe.Cli/TicTacToe.Cli.csproj
   dotnet build ./TicTacToe.Tests/TicTacToe.Tests.csproj
   dotnet test  ./TicTacToe.Tests/TicTacToe.Tests.csproj       # failing at first is expected
   dotnet run   --project ./TicTacToe.Cli/TicTacToe.Cli.csproj # will likely fail when hitting NotImplemented
   ```

2. Use **Cursor** to orient yourself:
   - Ask Cursor for a **high‑level architecture summary** of `Board`, `GameStatus`, `IBotStrategy`, and `HeuristicBot`.
   - Request a **diagram or bullet flow** of a typical turn: human move → validate → status check → bot move.
   - Have Cursor **enumerate each `// PROMPT:`** in order and restate what you'll need to implement.


## Using Cursor
Follow the **Prompt Roadmap** below. Implement the engine first, then the bot, then wire up the CLI.

## Prompt Roadmap (do these in this order)
- 1. `TicTacToe.Core/Board.cs` line **14** — PROMPT: Ask Cursor to implement immutable Apply(Move) that returns a NEW Board with the move applied,
- 2. `TicTacToe.Core/Board.cs` line **21** — PROMPT: Ask Cursor to implement GetStatus() that checks rows, cols, diags, and draw.
- 3. `TicTacToe.Core/Board.cs` line **27** — PROMPT: Ask Cursor to implement GetEmptyCells() helper returning IEnumerable<(int r,int c)>.
- 4. `TicTacToe.Cli/Program.cs` line **9** — PROMPT: Ask Cursor to implement Render(board) that draws the 3x3 grid with coordinates.
- 5. `TicTacToe.Cli/Program.cs` line **17** — PROMPT: Ask Cursor to parse input, validate, and apply move; handle errors gracefully.
- 6. `TicTacToe.Cli/Program.cs` line **22** — PROMPT: Ask Cursor to extract GameLoop into a class and add undo/redo as stretch goals.
- 7. `TicTacToe.Core/HeuristicBot.cs` line **5** — PROMPT: Ask Cursor to implement a simple heuristic:
- 8. `TicTacToe.Core/IBotStrategy.cs` line **5** — PROMPT: Ask Cursor to implement ChooseMove(Board board) that selects a legal move for the current player.
- 9. `TicTacToe.Tests/BoardTests.cs` line **18** — PROMPT: Ask Cursor to make Apply(...) work immutably and then make this pass.

## Build, Run, Test
```bash
dotnet test  ./TicTacToe.Tests/TicTacToe.Tests.csproj
dotnet run   --project ./TicTacToe.Cli/TicTacToe.Cli.csproj
```

## Acceptance Criteria
- Immutable engine (`Apply`, `GetStatus`) with tests passing
- Heuristic bot plays legal moves (win/block/center/corners/edges)
- CLI renders board and handles invalid input without crashing
- (Stretch) Extract a `GameLoop` class and add undo/redo

## Closing: Ideate Enhancements with Cursor
When your acceptance criteria are green, take 5–10 minutes to brainstorm **stretch features**.

Use one or more of these prompts in Cursor:
- *"Suggest 10 practical feature improvements for this project, grouped by effort (S/M/L). Explain the user benefit of each."*
- *"Identify weak spots in the current design and propose refactors that improve testability or clarity."*
- *"Pick one Small feature and draft a task checklist with acceptance tests. Then implement it."*
- *"Write a short README 'What I'd Do Next' section summarizing potential upgrades."*

---

## Proposed Refactors for Improved Architecture

### Current Weak Spots

1. **Console Coupling**: `GameLoop` tightly coupled to `Console` for I/O
2. **Mixed Responsibilities**: `GameLoop` handles UI, input parsing, state management, and undo/redo
3. **Debug Output in Production**: `Console.WriteLine` statements in `HeuristicBot`
4. **Hard-coded Board Size**: 3x3 board size hard-coded throughout codebase
5. **No State Management Interface**: Direct `Stack<Board>` usage in `GameLoop`
6. **Input Validation Mixed with UI**: Validation logic embedded in `GameLoop`

### Proposed Refactor Roadmap

#### Phase 1: Extract I/O Interface (`IGameIO`)
```csharp
public interface IGameIO
{
    void RenderBoard(Board board);
    string ReadInput(string prompt);
    void WriteMessage(string message);
    void DisplayGameResult(GameStatus status);
}
```
**Benefits**: Enables different UI implementations, improves testability

#### Phase 2: Extract Input Validation (`InputValidator`)
```csharp
public static class InputValidator
{
    public static bool TryParseMove(string input, Cell currentPlayer, 
        out Move? move, out string? errorMessage);
    public static bool IsValidMove(Move move, Board board, 
        out string? errorMessage);
}
```
**Benefits**: Reusable validation logic, clear separation of concerns

#### Phase 3: Extract Game State Management (`IGameStateManager`)
```csharp
public interface IGameStateManager
{
    Board CurrentBoard { get; }
    Board ApplyMove(Move move);
    bool CanUndo { get; }
    bool CanRedo { get; }
    Board Undo();
    Board Redo();
    void Reset();
}
```
**Benefits**: Enables different state management strategies, improves testability

#### Phase 4: Add Logging Interface (`ILogger`)
```csharp
public interface ILogger
{
    void Debug(string message);
    void Info(string message);
    void Warning(string message);
    void Error(string message);
}
```
**Benefits**: Configurable debug output, no console pollution in production

#### Phase 5: Update HeuristicBot with Logging
```csharp
public class HeuristicBot : IBotStrategy
{
    private readonly ILogger _logger;
    
    public HeuristicBot(ILogger? logger = null)
    {
        _logger = logger ?? new ConsoleLogger();
    }
}
```
**Benefits**: Configurable debug output, better debugging control

#### Phase 6: Refactor GameLoop with Dependency Injection
```csharp
public class GameLoop
{
    private readonly IBotStrategy _bot;
    private readonly IGameStateManager _stateManager;
    private readonly IGameIO _gameIO;

    public GameLoop(IBotStrategy bot, IGameIO gameIO, 
        IGameStateManager? stateManager = null)
    {
        _bot = bot;
        _gameIO = gameIO;
        _stateManager = stateManager ?? new GameStateManager();
    }
}
```
**Benefits**: Dependency injection, clear separation of concerns

#### Phase 7: Update Program.cs for Dependency Injection
```csharp
// Create dependencies
var logger = new ConsoleLogger(enableDebug: false);
var bot = new HeuristicBot(logger);
var gameIO = new ConsoleGameIO();
var gameLoop = new GameLoop(bot, gameIO);

// Run the game
gameLoop.Run();
```
**Benefits**: Clear dependency configuration, easy to swap implementations

### New Test Files

1. **InputValidatorTests**: Tests for input parsing and validation
2. **GameStateManagerTests**: Tests for undo/redo functionality

### Benefits of Refactoring

- **Improved Testability**: Each component can be tested independently
- **Better Separation of Concerns**: UI logic separated from game logic
- **Enhanced Maintainability**: Single responsibility for each class
- **Increased Flexibility**: Easy to add new UI implementations
- **Better Debugging**: Configurable debug output, structured logging

### Future Extensions

- **Additional UI Implementations**: GUI, Web-based, Mobile UI
- **Advanced State Management**: Database persistence, network-based, cloud-based
- **Enhanced Logging**: File-based, structured JSON, log aggregation
- **Configuration Management**: JSON-based, environment-based, user preferences

### Implementation Order

1. Create interfaces (`IGameIO`, `IGameStateManager`, `ILogger`)
2. Implement concrete classes (`ConsoleGameIO`, `GameStateManager`, `ConsoleLogger`)
3. Extract InputValidator from GameLoop
4. Update HeuristicBot to use logging
5. Refactor GameLoop to use new interfaces
6. Update Program.cs for dependency injection
7. Add comprehensive tests for new components

### Acceptance Criteria

- [ ] All interfaces created and implemented
- [ ] GameLoop refactored to use new interfaces
- [ ] Input validation extracted and tested
- [ ] Logging integrated throughout application
- [ ] All existing functionality preserved
- [ ] Comprehensive test coverage for new components
- [ ] Dependency injection properly configured

This refactoring transforms the application from a tightly coupled monolith into a well-structured, testable, and extensible system following SOLID principles.
