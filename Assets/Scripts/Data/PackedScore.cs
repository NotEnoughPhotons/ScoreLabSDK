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

        public override PackedType packedType => PackedType.Score;
        public int score;

        public override void OnValueCreated()
        {
            Core.ScoreTracker.Instance.AddScore(score);
            API.Score.OnScoreAdded?.Invoke(this);
        }

        public override void OnValueRemoved()
        {
            return;
        }
    }
}

