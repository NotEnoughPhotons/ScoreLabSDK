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

        public bool Stackable;

        public float DecayTime;
        public float PostDecayTime;

        public virtual void OnValueCreated() { }
        public virtual void OnValueRemoved() { }

        public virtual void OnUpdate() { }
        public virtual void OnUpdateDecay() { }

        public void SetDecayTime(float decayTime)
        {
            _tDecay = decayTime;
        }

        public virtual bool IsActive { get; }

        [UnityEngine.SerializeField] protected float _tDecay;
    }
}

