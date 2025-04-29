using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    public struct JSONScore
    {
        public EventType.ScoreEventType EventType;

        public float DecayTime;
        public bool Stackable;
        public JSONAudioParams EventAudio;

        public string Name;
        public int Score;

        public int TierRequirement;

        public JSONScore[] Tiers;
    }
}

