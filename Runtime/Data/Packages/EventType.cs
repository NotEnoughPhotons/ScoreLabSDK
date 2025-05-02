namespace NEP.ScoreLab.Data
{
    public static class EventType
    {
        public enum ScoreEventType
        {
            Kill,
            Headshot,
            KillInAir,
            WaveCompleted,
            RoundCompleted,
            Crabcest,
            Facehug
        }

        public enum MultiplierEventType
        {
            Kill,
            MidAir,
            Seated,
            SecondWind,
            Ragdolled,
            SwappedAvatars,
            SlowMo
        }
        
        public static string[] ScoreEventTable = new string[]
        {
            "SCORE_KILL",
            "SCORE_HEADSHOT",
            "SCORE_MID_AIR",
            "SCORE_WAVE_COMPLETED",
            "SCORE_ROUND_COMPLETED",
            "SCORE_CRABCEST",
            "SCORE_FACEHUG"
        };

        public static string[] MultiplierEventTable = new string[]
        {
            "MULT_KILL",
            "MULT_MIDAIR",
            "MULT_SEATED",
            "MULT_SECONDWIND",
            "MULT_RAGDOLLED",
            "MULT_SWAPPED_AVATARS",
            "MULT_SLOWMO"
        };
    }
}