using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    public abstract class PackedValue
    {
        public PackedValue(string name)
        {
            this.name = name;
            OnValueCreated();
        }

        public string name;

        public abstract void OnValueCreated();
    }
}

