namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    public struct JSONScore
    {
        public string Name;
        public int Score;
        
        public UnityEngine.AudioClip EventAudio;
        
        public bool Stackable;
        
        public float DecayTime;
        
        public string EventType;
        public string TierEventType;
        
        public int TierRequirement;

        public JSONScore[] Tiers;
    }
}

