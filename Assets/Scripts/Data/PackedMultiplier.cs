using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    public class PackedMultiplier : PackedValue
    {
        public PackedMultiplier(string name) : base(name)
        {

        }

        public float multiplier;

        public override void OnValueCreated()
        {
            throw new System.NotImplementedException();
        }
    }
}