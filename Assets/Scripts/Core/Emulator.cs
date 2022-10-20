using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.Core
{
    public class Emulator : MonoBehaviour
    {
        public int Score;
        public float Multiplier;
        public List<Data.PackedMultiplier> ActiveMultipliers;
        public bool testCondition = true;

        public Func<bool> TestFunc;

        private void Awake()
        {
            new ScoreTracker();
            TestFunc = new Func<bool>(() => testCondition);
        }

        private void Update()
        {
            ScoreTracker.Instance.Update();

            Score = ScoreTracker.Instance.Score;
            Multiplier = ScoreTracker.Instance.Multiplier;
            ActiveMultipliers = ScoreTracker.Instance.ActiveMultipliers;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ScoreTracker.Instance.Add(new Data.PackedMultiplier("Test", 1f, 5f));
            }
        }
    }
}

