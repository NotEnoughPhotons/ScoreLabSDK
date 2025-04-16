namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    public struct JSONMult
    {
        public string Name;
        public float Multiplier;

        public bool Stackable;
        
        public float DecayTime;

        public string EventType;
        public string TierEventType;
        
        public string Condition;

        public int TierRequirement;

        public JSONMult[] Tiers;
    }
}