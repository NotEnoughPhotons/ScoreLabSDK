using System;

using NEP.ScoreLab.Data;

public static class API
{
    public static class Score
    {
        public static event Action<PackedScore> OnScoreAdded;
        public static event Action<PackedScore> OnScoreRemoved;
    }

    public static class Multiplier
    {
        public static event Action<PackedMultiplier> OnMultiplierAdded;
        public static event Action<PackedMultiplier> OnMultiplierRemoved;

        public static event Action<PackedMultiplier> OnMultiplierTimeBegin;
        public static event Action<PackedMultiplier> OnMultiplierTimeExpired;
    }

    public static class HighScore
    {

    }
}
