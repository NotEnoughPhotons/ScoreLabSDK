using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    public class PackedHighScore : PackedValue
    {
        public PackedHighScore(string name, int bestScore) : base(name)
        {
            this.bestScore = bestScore;
        }

        public int bestScore;

        public override void OnValueCreated()
        {

        }
    }
}