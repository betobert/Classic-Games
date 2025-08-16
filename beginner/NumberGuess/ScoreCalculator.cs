namespace NumberGuess;

public static class ScoreCalculator
{
    // PROMPT: Ask Cursor to design a scoring function that rewards fewer attempts and higher difficulty.
    // Suggested: base = rangeSize / 10; score = Math.Max(1, base - attempts + 1). Lower is better (like golf).
    public static int Calculate(int attempts, int rangeSize)
    {
        // Base score increases with difficulty (larger range = higher difficulty)
        int baseScore = rangeSize / 10;
        
        // Penalty increases exponentially with attempts to strongly reward efficiency
        int attemptPenalty = attempts * attempts;
        
        // Final score: base score + attempt penalty (lower is better)
        int score = Math.Max(1, baseScore + attemptPenalty);
        
        return score;
    }
}
