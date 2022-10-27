using System;

using NEP.ScoreLab.Core;

namespace NEP.ScoreLab.Data
{
    [Serializable]
    public class PackedScore : PackedValue
    {
        public PackedScore() { }

        public override PackedType PackedValueType => PackedType.Score;
        public int Score;
        public int AccumulatedScore;

        public override void OnValueCreated()
        {
            _tDecay = DecayTime;
            AccumulatedScore = Score;
            API.Score.OnScoreAdded?.Invoke(this);
        }

        public override void OnValueRemoved()
        {
            API.Score.OnScoreRemoved?.Invoke(this);
        }

        public override void OnUpdate()
        {
            OnUpdateDecay();
        }

        public override void OnUpdateDecay()
        {
            if (_tDecay <= 0f)
            {
                ScoreTracker.Instance.Remove(this);
            }

            _tDecay -= UnityEngine.Time.deltaTime;
        }
    }
}

