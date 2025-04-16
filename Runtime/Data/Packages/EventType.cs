namespace NEP.ScoreLab.Data
{
    public static class EventType
    {
        public static readonly string None = "None";

        public static class Score
        {
            public static readonly string Kill = "SCORE_KILL";
            public static readonly string Test = "SCORE_TEST";
        }

        public static class Mult
        {
            public static readonly string Kill = "MULT_KILL";
            public static readonly string Test = "MULT_TEST";
            public static readonly string Test2 = "MULT_TEST2";
            public static readonly string Slowdown = "MULT_SLOWDOWN";
        }
    }
}