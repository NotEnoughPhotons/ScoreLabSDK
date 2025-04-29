namespace NEP.ScoreLab.Data
{
    public struct JSONMult
    {
        public EventType.MultiplierEventType EventType;

        public float DecayTime;
        public bool Stackable;

        public string Name;
        public float Multiplier;
        public string Condition;

        public int TierRequirement;

        public JSONMult[] Tiers;
    }
}