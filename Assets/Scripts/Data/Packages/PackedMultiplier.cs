using System;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    public class PackedMultiplier : PackedValue
    {
        public PackedMultiplier(string name, float multiplier) : base(name)
        {
            this.name = name;
            this.multiplier = multiplier;
            _timed = false;
        }

        public PackedMultiplier(string name, float multiplier, float timer) : base(name)
        {
            this.name = name;
            this.multiplier = multiplier;
            this.timer = timer;
            _timed = true;
        }

        public PackedMultiplier(string name, float multiplier, Func<bool> condition) : base(name)
        {
            this.name = name;
            this.multiplier = multiplier;
            this.condition = condition;
        }

        public override PackedType packedType => PackedType.Multiplier;
        public float multiplier;
        public float timer;
        public float elapsed;
        public Func<bool> condition;

        private bool _timed;
        private bool _timeBegin;

        public override void OnValueCreated()
        {
            Core.ScoreTracker.Instance.AddMultiplier(multiplier);
            Core.ScoreTracker.Instance.ActiveMultipliers.Add(this);

            API.Multiplier.OnMultiplierAdded?.Invoke(this);
        }

        public override void OnValueRemoved()
        {
            Core.ScoreTracker.Instance.RemoveMultiplier(multiplier);
            Core.ScoreTracker.Instance.ActiveMultipliers.Remove(this);

            API.Multiplier.OnMultiplierRemoved?.Invoke(this);
        }

        public override void OnUpdate()
        {
            if (_timed)
            {
                if (!_timeBegin)
                {
                    API.Multiplier.OnMultiplierTimeBegin?.Invoke(this);
                    _timeBegin = true;
                }

                elapsed += Time.deltaTime;

                if (elapsed > timer)
                {
                    API.Multiplier.OnMultiplierTimeExpired?.Invoke(this);
                    Core.ScoreTracker.Instance.Remove(this);
                    elapsed = 0f;
                }
            }

            if(condition != null)
            {
                if (!condition())
                {
                    OnValueRemoved();
                }
            }
        }
    }
}