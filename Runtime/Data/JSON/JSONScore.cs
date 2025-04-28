using UnityEngine;

namespace NEP.ScoreLab.Data
{
    public struct JSONScore
    {
        public string EventType;
        public string TierEventType;

        public float DecayTime;
        public bool Stackable;
        public JSONAudioParams EventAudio;

        public string Name;
        public int Score;

        public int TierRequirement;

        public JSONScore[] Tiers;
    }
}

