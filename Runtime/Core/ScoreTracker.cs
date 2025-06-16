namespace NEP.ScoreLab.Core
{
    public static class ScoreTracker
    {
        public static int Score
        {
            get => _score;
        }

        public static float Multiplier
        {
            get => _multiplier;
        }

        private static int _score;
        private static float _multiplier;
    }
}

