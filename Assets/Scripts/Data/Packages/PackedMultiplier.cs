using System;
using UnityEngine;

using NEP.ScoreLab.Core;

namespace NEP.ScoreLab.Data
{
    [Serializable]
    public class PackedMultiplier : PackedValue
    {
        public PackedMultiplier() { }

        public override PackedType PackedValueType => PackedType.Multiplier;
        public float Multiplier;
        public float AccumulatedMultiplier;
        public float Elapsed { get => base._tDecay; }
        public string Condition;
        public Func<bool> condition { get; private set; }

        private bool _timed;
        private bool _timeBegin;

        public override void OnValueCreated()
        {
            if (DecayTime != 0f)
            {
                _timed = true;
            }

            condition = API.GameConditions.GetCondition(Condition);
            AccumulatedMultiplier = Multiplier;
            _tDecay = DecayTime;
            API.Multiplier.OnMultiplierAdded?.Invoke(this);
        }

        public override void OnValueRemoved()
        {
            
            _timeBegin = false;

            API.Multiplier.OnMultiplierRemoved?.Invoke(this);
        }

        public override void OnUpdate()
        {
            if (condition != null)
            {
                if (!condition())
                {
                    ScoreTracker.Instance.Remove(this);
                }
            }

            OnUpdateDecay();
        }

        public override void OnUpdateDecay()
        {
            base.OnUpdateDecay();

            if (_timed)
            {
                if (!_timeBegin)
                {
                    API.Multiplier.OnMultiplierTimeBegin?.Invoke(this);
                    _timeBegin = true;
                }

                if (_tDecay <= 0f)
                {
                    API.Multiplier.OnMultiplierTimeExpired?.Invoke(this);
                    _tDecay = DecayTime;
                    ScoreTracker.Instance.Remove(this);
                }

                _tDecay -= Time.deltaTime;
            }
        }
    }
}