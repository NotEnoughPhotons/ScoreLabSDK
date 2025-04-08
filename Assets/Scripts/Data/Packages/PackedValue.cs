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

        public UnityEngine.AudioClip EventAudio;

        public bool Stackable;

        public float DecayTime;
        public float PostDecayTime;

        public PackedValue[] Tiers;

        public PackedValue CurrentTier
        {
            get
            {
                if(_tierIndex == Tiers.Length)
                {
                    _tierIndex = Tiers.Length;
                    return Tiers[_tierIndex];
                }
                else
                {
                    return Tiers[_tierIndex];
                }
            }
        }

        public int TierIndex { get => _tierIndex; }
        public int TierRequirementIndex { get => _tierRequirementIndex; }
        public int TierRequirement;

        public virtual bool IsActive { get; }

        [UnityEngine.SerializeField] protected float _tDecay;
        [UnityEngine.SerializeField] private int _tierIndex = 0;
        [UnityEngine.SerializeField] private int _tierRequirementIndex = 0;

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
            if(_tierRequirementIndex < TierRequirement && _tierIndex < Tiers.Length)
            {
                _tierRequirementIndex++;
            }
            else if(_tierRequirementIndex == TierRequirement)
            {
                _tierRequirementIndex = TierRequirement;

                if(_tierIndex + 1 < Tiers.Length)
                {
                    _tierIndex++;
                }
            }
        }

        protected void ResetTier()
        {
            _tierIndex = 0;
        }
    }
}

