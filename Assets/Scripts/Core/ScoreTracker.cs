using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.Core
{
    public class ScoreTracker
    {
        public ScoreTracker()
        {
            Initialize();
        }

        public static ScoreTracker Instance { get; private set; }

        public int Score
        {
            get => _score;
        }
        public float Multiplier
        {
            get => _multiplier;
        }

        private int _score;
        private float _multiplier;

        public void Initialize()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        public void Add(PackedValue value)
        {
            value.OnValueCreated();
        }

        public void AddScore(int score)
        {
            _score += score;
        }

        public void AddMultiplier(float multiplier)
        {
            _multiplier += multiplier;
        }
    }
}

