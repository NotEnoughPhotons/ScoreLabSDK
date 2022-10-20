using System;

using NEP.ScoreLab.Data;

public static class API
{
    public static class Score
    {
        public static Action<PackedScore> OnScoreAdded;
    }

    public static class Multiplier
    {
        public static Action<PackedMultiplier> OnMultiplierAdded;
        public static Action<PackedMultiplier> OnMultiplierRemoved;

        public static Action<PackedMultiplier> OnMultiplierTimeBegin;
        public static Action<PackedMultiplier> OnMultiplierTimeExpired;
    }

    public static class HighScore
    {
        public static Action<PackedHighScore> OnHighScoreReached;
        public static Action<PackedHighScore> OnHighScoreUpdated;
        public static Action<PackedHighScore> OnHighScoreLoaded;
        public static Action<PackedHighScore> OnHighScoreSaved;
    }

    public static class GameConditions
    {
        public static Func<bool> IsPlayerMoving = new Func<bool>(() => true);
        public static Func<bool> IsPlayerInAir = new Func<bool>(() => true);
    }
}
