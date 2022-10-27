using System.Collections.Generic;

using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.Core
{
    public class ScoreTracker
    {
        public ScoreTracker() => Initialize();

        public static ScoreTracker Instance { get; private set; }

        public List<PackedValue> ActiveValues { get; private set; }

        public int Score
        {
            get => _score;
        }
        public int ScoreDifference
        {
            get => _scoreDifference;
        }
        public int LastScore
        {
            get => _lastScore;
        }
        public float Multiplier
        {
            get => _multiplier;
        }

        private int _score = 0;
        private int _scoreDifference = 0;
        private int _lastScore = 0;
        private float _multiplier = 1f;

        private float _baseMultiplier = 1f;

        public void Initialize()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            ActiveValues = new List<PackedValue>();
        }

        public void Update()
        {
            for(int i = 0; i < ActiveValues.Count; i++)
            {
                ActiveValues[i].OnUpdate();
            }
        }

        public void Add(string eventType)
        {
            var packedValue = DataManager.PackedValues.Get(eventType);

            if(packedValue.PackedValueType == PackedValue.PackedType.Score)
            {
                var value = DataManager.PackedValues.Get<PackedScore>(eventType);
                Add(value);
            }
            else if(packedValue.PackedValueType == PackedValue.PackedType.Multiplier)
            {
                var value = DataManager.PackedValues.Get<PackedMultiplier>(eventType);
                Add(value);
            }
        }

        public void Add(PackedValue value)
        {
            if (value.PackedValueType == PackedValue.PackedType.Score)
            {
                PackedScore score = value as PackedScore;

                if (CheckDuplicate(score))
                {
                    var duplicate = GetClone<PackedScore>(score);

                    if (score.Stackable && score.Tiers != null)
                    {
                        duplicate.NextTier();
                        PackedScore tier = (PackedScore)duplicate.CurrentTier;

                        score.Name = tier.Name;
                        score.Score = tier.Score;
                        score.DecayTime = tier.DecayTime;

                        AddScore(score.Score);
                        duplicate.SetDecayTime(score.DecayTime);
                        OnScoreValueAccumulated(score);
                    }
                    else if (score.Stackable)
                    {
                        AddScore(score.Score);
                        OnScoreValueAccumulated(duplicate);
                    }
                    else
                    {
                        AddScore(score.Score);
                        ActiveValues.Add(score);
                    }
                }
                else
                {
                    AddScore(score.Score);
                    ActiveValues.Add(score);
                }

                score.OnValueCreated();
            }
            else if (value.PackedValueType == PackedValue.PackedType.Multiplier)
            {
                PackedMultiplier mult = value as PackedMultiplier;

                if (CheckDuplicate(mult))
                {
                    var duplicate = GetClone<PackedMultiplier>(mult);

                    if (mult.Stackable && mult.Tiers != null)
                    {
                        duplicate.NextTier();
                        PackedMultiplier tier = (PackedMultiplier)duplicate.CurrentTier;

                        mult.Name = tier.Name;
                        mult.Multiplier = tier.Multiplier;
                        mult.DecayTime = tier.DecayTime;
                        mult.Condition = tier.Condition;

                        AddMultiplier(mult.Multiplier);
                        duplicate.SetDecayTime(mult.DecayTime);
                        OnMultiplierValueAccumulated(mult);
                    }
                    else if (mult.Stackable)
                    {
                        AddMultiplier(mult.Multiplier);
                        OnMultiplierValueAccumulated(duplicate);
                    }
                    else
                    {
                        AddMultiplier(mult.Multiplier);
                        ActiveValues.Add(mult);
                    }
                }
                else
                {
                    AddMultiplier(mult.Multiplier);
                    ActiveValues.Add(mult);
                }

                mult.OnValueCreated();
            }
        }

        public void Remove(PackedValue value)
        {
            if(value.PackedValueType == PackedValue.PackedType.Score)
            {
                ActiveValues.Remove(value);

                value.OnValueRemoved();
            }
            else if (value.PackedValueType == PackedValue.PackedType.Multiplier)
            {
                ActiveValues.Remove(value);
                PackedMultiplier mult = value as PackedMultiplier;
                RemoveMultiplier(mult.AccumulatedMultiplier);

                value.OnValueRemoved();
            }
        }

        public void AddScore(int score)
        {
            _lastScore = _score;
            _score += UnityEngine.Mathf.RoundToInt(score * _multiplier);
            _scoreDifference = _score - _lastScore;
        }

        public void AddMultiplier(float multiplier)
        {
            _multiplier += multiplier;
        }

        public void RemoveMultiplier(float multiplier)
        {
            if(_multiplier < _baseMultiplier)
            {
                _multiplier = _baseMultiplier;
            }

            _multiplier -= multiplier;
        }

        public bool CheckDuplicate(PackedValue value)
        {
            foreach(var val in ActiveValues)
            {
                if(val.eventType == value.eventType)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnScoreValueAccumulated(PackedScore value)
        {
            value.SetDecayTime(value.DecayTime);
            value.AccumulatedScore += value.Score;
            API.Score.OnScoreAccumulated?.Invoke(value);
        }

        private void OnMultiplierValueAccumulated(PackedMultiplier value)
        {
            value.SetDecayTime(value.DecayTime);
            value.AccumulatedMultiplier += value.Multiplier;
            API.Multiplier.OnMultiplierAccumulated?.Invoke(value);
        }

        private T GetClone<T>(PackedValue value) where T : PackedValue
        {
            return (T)ActiveValues.Find((match) => match.eventType == value.eventType);
        }
    }
}