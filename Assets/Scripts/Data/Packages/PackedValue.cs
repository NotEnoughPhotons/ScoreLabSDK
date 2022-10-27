namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    public class PackedValue
    {
        public enum PackedType
        {
            Base,
            Score,
            Multiplier,
            HighScore,
            Misc
        }

        public PackedValue() { }

        public string Name;

        public virtual PackedType PackedValueType => PackedType.Base;

        public string eventType;
        public string TierEventType;

        public bool Stackable;

        public float DecayTime;
        public float PostDecayTime;

        public PackedValue[] Tiers;
        public PackedValue CurrentTier;

        public virtual bool IsActive { get; }

        [UnityEngine.SerializeField] protected float _tDecay;
        [UnityEngine.SerializeField] private int _tierIndex = 0;

        public virtual void OnValueCreated() { }
        public virtual void OnValueRemoved() { }

        public virtual void OnUpdate() { }
        public virtual void OnUpdateDecay() { }

        public void SetDecayTime(float decayTime)
        {
            _tDecay = decayTime;
        }

        public void NextTier()
        {
            if(Tiers == null)
            {
                return;
            }

            if (_tierIndex >= Tiers.Length)
            {
                _tierIndex = Tiers.Length;
                CurrentTier = Tiers[Tiers.Length - 1];
            }
            else
            {
                CurrentTier = Tiers[_tierIndex++];
            }
        }

        public void ResetTier()
        {
            _tierIndex = 0;
        }
    }
}

