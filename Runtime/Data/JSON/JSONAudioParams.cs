using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    public struct JSONAudioParams
    {
        public AudioClip sound;
        
        #if UNITY_EDITOR
        [Range(0f, 1f)]
        #endif
        public float volume;
        
        #if UNITY_EDITOR
        [Range(0f, 10f)]
        #endif
        public float pitch;
    }
}