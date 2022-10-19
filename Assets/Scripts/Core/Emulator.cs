using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.Core
{
    public class Emulator : MonoBehaviour
    {
        public int Score;
        public float Multiplier;

        private void Awake()
        {
            new ScoreTracker();
        }

        private void Update()
        {
            Score = ScoreTracker.Instance.Score;
            Multiplier = ScoreTracker.Instance.Multiplier;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ScoreTracker.Instance.Add(new Data.PackedScore("Test", 100));
            }
        }
    }
}

