using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    public class PackedScore : PackedValue
    {
        public PackedScore(string name, int score) : base(name)
        {
            this.score = score;
        }

        public int score;

        public override void OnValueCreated()
        {
            Core.ScoreTracker.Instance.AddScore(score);
        }
    }
}

