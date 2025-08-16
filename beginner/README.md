# START HERE — Beginner: Number Guess (CLI, .NET 8)

## Prerequisites
- Install **.NET 8 SDK**: https://dotnet.microsoft.com/download
- Verify install:
  ```bash
  dotnet --version
  dotnet --info
  ```

## Project Setup
1. Unzip the repo and open a terminal at the folder:
   ```bash
   cd classic-games/beginner
   ```
2. (Optional sanity) Try to build and run tests. **It will fail** until you complete the prompts below:
   ```bash
   dotnet build ./NumberGuess/NumberGuess.csproj
   dotnet test  ./NumberGuess.Tests/NumberGuess.Tests.csproj
   ```

## How to Use Cursor for this Exercise
- Open the `beginner` folder in **Cursor**.
- Work through the **Prompt Roadmap** below **in order**. For each step:
  1. Open the file at the indicated line.
  2. Copy the prompt text (after “PROMPT:”) and paste it into Cursor.
  3. Let Cursor generate code; review and accept with small edits if needed.
  4. Rebuild after a few steps to check progress.


## Before You Prompt: Explore the Project
Before firing prompts into Cursor, take 5–10 minutes to **see what you’ve got**.

1. Build and try to run:
   ```bash
   dotnet build ./NumberGuess/NumberGuess.csproj
   dotnet test  ./NumberGuess.Tests/NumberGuess.Tests.csproj   # will fail at first
   dotnet run   --project ./NumberGuess/NumberGuess.csproj     # will likely throw NotImplementedException
   ```
   That failure is intentional—it shows you exactly what’s missing.

2. Use **Cursor** to get oriented:
   - Right‑click files and use “Explain” / “Ask” to **summarize `Program.cs`, `Game.cs`, and `ScoreCalculator.cs`**.
   - Ask for a **walkthrough of the program flow**: where input is read, how the secret number is stored, and where scoring will be called.
   - Have Cursor **list the TODOs / PROMPTs** it finds and paraphrase them in plain English.


## Prompt Roadmap (do these in this order)
- 1. `NumberGuess/Game.cs` line **19** — PROMPT: Implement ConfigureRange() to set (_lower,_upper) for Easy(1-50), Normal(1-100), Hard(1-500).
- 2. `NumberGuess/Program.cs` line **7** — PROMPT: Ask Cursor to implement Game.Play() that runs the main loop with input validation,
- 3. `NumberGuess/Program.cs` line **11** — PROMPT: Ask Cursor to add command-line parsing: support --difficulty Easy|Normal|Hard and --seed <int>.
- 4. `NumberGuess.Tests/ScoreCalculatorTests.cs` line **11** — PROMPT: Ask Cursor to implement Calculate() and make these tests pass.
- 5. `NumberGuess/Game.cs` line **35** — PROMPT: Implement ConfigureRange() that validates _difficulty and sets _lower/_upper accordingly.
- 6. `NumberGuess/Game.cs` line **43** — PROMPT: Implement a loop that reads lines from Console, supports 'quit', validates integers,
- 7. `NumberGuess/Game.cs` line **53** — PROMPT: Implement "warm/colder" logic: based on distance between lastGuess and _secret compared
- 8. `NumberGuess/ScoreCalculator.cs` line **5** — PROMPT: Ask Cursor to design a scoring function that rewards fewer attempts and higher difficulty.

## Build, Run, Test
```bash
# Build app and tests
dotnet build ./NumberGuess/NumberGuess.csproj
dotnet build ./NumberGuess.Tests/NumberGuess.Tests.csproj

# Run tests
dotnet test  ./NumberGuess.Tests/NumberGuess.Tests.csproj

# Run the game (after prompts are implemented)
dotnet run   --project ./NumberGuess/NumberGuess.csproj --difficulty Normal
```
> Tip: Add `--seed 42` to make runs deterministic for debugging.

## Acceptance Criteria
- Game starts and accepts input with **Easy/Normal/Hard** ranges
- Warm/colder hints work after the first guess
- Score is computed; first-try is best; tests pass
- Graceful handling of invalid input and `quit`

## Closing: Ideate Enhancements with Cursor
When your acceptance criteria are green, take 5–10 minutes to brainstorm **stretch features**.

Use one or more of these prompts in Cursor:
- *“Suggest 10 practical feature improvements for this project, grouped by effort (S/M/L). Explain the user benefit of each.”*
- *“Identify weak spots in the current design and propose refactors that improve testability or clarity.”*
- *“Pick one Small feature and draft a task checklist with acceptance tests. Then implement it.”*
- *“Write a short README ‘What I’d Do Next’ section summarizing potential upgrades.”*

## What I'd Do Next: Feature Roadmap

### 🚀 Quick Wins (Small Effort - 1-2 hours each)
- **Game Statistics**: Track average attempts, total games, win rate
- **Attempt Limits**: Add `--max-attempts` option for added tension  
- **Sound Effects**: Console beeps for correct/wrong guesses

### 🎯 Engagement Boosters (Medium Effort - 3-6 hours each)
- **High Score System**: Persistent JSON file with top 10 scores
- **Time-Based Scoring**: Include time penalties, add `--timed` mode
- **Multiple Game Modes**: Reverse mode (computer guesses), Battle mode (2 players)
- **Smart Hints**: Request hints with score penalties

### 🌟 Major Expansions (Large Effort - 1-3 days each)
- **Web GUI**: ASP.NET Core/Blazor interface with animations
- **Multiplayer**: Online play, global leaderboards, friend challenges
- **AI Opponent**: Computer players with different strategies and difficulty levels

### 📋 Implementation Priority
1. **Start Small**: Statistics → Attempt Limits → Sound Effects
2. **Build Engagement**: High Scores → Time Scoring → Game Modes  
3. **Scale Up**: Web GUI → Multiplayer → AI Opponents

Each feature builds on the solid foundation while adding new dimensions of challenge and engagement!
