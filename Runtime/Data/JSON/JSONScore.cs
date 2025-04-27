using UnityEngine;

namespace NEP.ScoreLab.Data
{
    public struct JSONScore
    {
        public string EventType;
        public string TierEventType;

        public float DecayTime;
        public bool Stackable;
        #if UNITY_EDITOR
        public AudioClip EventAudio;
        #else
        public string EventAudio;
        #endif

        public string Name;
        public int Score;

        public int TierRequirement;

        public JSONScore[] Tiers;
    }
}

