using UnityEngine;
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
            Add(DataManager.PackedValues.Get(eventType));
        }

        public void Add(PackedValue value)
        {
            if (value.PackedValueType == PackedValue.PackedType.Score)
            {
                SetPackedScore(DataManager.PackedValues.GetScore(value.eventType));
            }
            else if (value.PackedValueType == PackedValue.PackedType.Multiplier)
            {
                SetPackedMultiplier(DataManager.PackedValues.GetMultiplier(value.eventType));
            }
        }

        public void Remove(PackedValue value)
        {
            if(value.PackedValueType == PackedValue.PackedType.Score)
            {
                ActiveValues.Remove(value);

                value.OnValueRemoved();

                API.Score.OnScoreRemoved?.Invoke((PackedScore)value);
            }
            else if (value.PackedValueType == PackedValue.PackedType.Multiplier)
            {
                ActiveValues.Remove(value);
                PackedMultiplier mult = value as PackedMultiplier;
                RemoveMultiplier(mult.AccumulatedMultiplier);

                value.OnValueRemoved();

                API.Multiplier.OnMultiplierRemoved?.Invoke((PackedMultiplier)value);
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

        private void SetPackedScore(PackedScore score)
        {
            if(score == null)
            {
                return;
            }

            if (!CheckDuplicate(score))
            {
                InitializeValue(score);
                AddScore(score.Score);
                ActiveValues.Add(score);

                API.Score.OnScoreAdded?.Invoke(score);
            }
            else
            {
                if(score.Tiers != null)
                {
                    PackedScore tier = score.NextTier() as PackedScore;
                    PackedScore copy = CopyFromScoreTier(score, tier);

                    API.Score.OnScoreTierReached?.Invoke(copy);
                }
                else if (score.Stackable)
                {
                    AddScore(score.Score);
                    score.SetDecayTime(score.DecayTime);
                    score.AccumulatedScore += score.Score;

                    API.Score.OnScoreAccumulated?.Invoke(score);
                }
                else
                {
                    PackedScore copy = CopyFromScore(score);
                    InitializeValue(copy);
                    AddScore(copy.Score);
                    ActiveValues.Add(copy);

                    API.Score.OnScoreAdded?.Invoke(copy);
                }
            }
        }

        private void SetPackedMultiplier(PackedMultiplier multiplier)
        {
            if (multiplier == null)
            {
                return;
            }

            if (!CheckDuplicate(multiplier))
            {
                InitializeValue(multiplier);
                AddMultiplier(multiplier.Multiplier);
                ActiveValues.Add(multiplier);

                API.Multiplier.OnMultiplierAdded?.Invoke(multiplier);
            }
            else
            {
                if (multiplier.Tiers != null)
                {
                    PackedMultiplier tier = multiplier.NextTier() as PackedMultiplier;
                    PackedMultiplier copy = CopyFromMultTier(multiplier, tier);

                    API.Multiplier.OnMultiplierTierReached?.Invoke(copy);
                }
                else if (multiplier.Stackable)
                {
                    AddMultiplier(multiplier.Multiplier);
                    multiplier.SetDecayTime(multiplier.DecayTime);
                    multiplier.AccumulatedMultiplier += multiplier.Multiplier;

                    API.Multiplier.OnMultiplierAccumulated?.Invoke(multiplier);
                }
                else
                {
                    PackedMultiplier copy = CopyFromMult(multiplier);
                    InitializeValue(copy);
                    AddMultiplier(copy.Multiplier);
                    ActiveValues.Add(copy);

                    API.Multiplier.OnMultiplierAdded?.Invoke(copy);
                }
            }
        }

        private PackedScore CopyFromScoreTier(PackedScore original, PackedScore tier)
        {
            original.eventType = tier.eventType;
            original.Name = tier.Name;
            original.Score = tier.Score;
            original.DecayTime = tier.DecayTime;
            original.SetDecayTime(original.DecayTime);
            original.EventAudio = tier.EventAudio;

            return original;
        }

        private PackedMultiplier CopyFromMultTier(PackedMultiplier original, PackedMultiplier tier)
        {
            original.eventType = tier.eventType;
            original.Name = tier.Name;
            original.Multiplier = tier.Multiplier;
            original.SetDecayTime(tier.DecayTime);
            original.EventAudio = tier.EventAudio;
            original.Condition = tier.Condition;

            return original;
        }

        private PackedScore CopyFromScore(PackedScore original)
        {
            PackedScore score = new PackedScore()
            {
                eventType = original.eventType,
                Name = original.Name,
                Score = original.Score,
                EventAudio = original.EventAudio,
                DecayTime = original.DecayTime
            };

            return score;
        }

        private PackedMultiplier CopyFromMult(PackedMultiplier original)
        {
            PackedMultiplier score = new PackedMultiplier()
            {
                eventType = original.eventType,
                Name = original.Name,
                Multiplier = original.Multiplier,
                EventAudio = original.EventAudio,
                DecayTime = original.DecayTime,
                Condition = original.Condition
            };

            return score;
        }

        private void InitializeValue(PackedValue value)
        {
            value.OnValueCreated();
            value.SetDecayTime(value.DecayTime);
        }

        private T GetClone<T>(PackedValue value) where T : PackedValue
        {
            return (T)ActiveValues.Find((match) => match.eventType == value.eventType);
        }
    }
}