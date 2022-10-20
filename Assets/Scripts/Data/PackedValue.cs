using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    public abstract class PackedValue
    {
        public PackedValue(string name)
        {
            this.name = name;
        }

        public string name;

        public abstract void OnValueCreated();
        public abstract void OnValueRemoved();

        public virtual void OnUpdate() { }
        public virtual bool IsActive { get; }
    }
}

