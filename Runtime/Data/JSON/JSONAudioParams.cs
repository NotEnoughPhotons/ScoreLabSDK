using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    public class JSONAudioParams
    {
        public AudioClip[] sounds;
        
        #if UNITY_EDITOR
        [Range(0f, 1f)]
        #endif
        public float volume = 1f;
        
        #if UNITY_EDITOR
        [Range(0f, 2f)]
        #endif
        public float pitch = 1f;
    }
}