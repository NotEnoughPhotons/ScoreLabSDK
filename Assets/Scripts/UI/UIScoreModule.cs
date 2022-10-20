using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace NEP.ScoreLab.UI
{
    public class UIScoreModule : UIModule
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _value;

        private Data.PackedValue _packedValue;

        private void Awake()
        {
            Transform titleTran = transform.Find("Title");
            Transform valueTran = transform.Find("Value");

            if(titleTran == null)
            {
                return;
            }

            _title = transform.Find("Title").GetComponent<TextMeshProUGUI>();

            if(valueTran == null)
            {
                return;
            }

            _value = transform.Find("Value").GetComponent<TextMeshProUGUI>();
        }

        public void AssignPackedData(Data.PackedValue packedValue)
        {
            this._packedValue = packedValue;
        }

        public override void OnModuleEnable()
        {
            if(_packedValue == null)
            {
                return;
            }

            if(_title != null)
            {
                _title.text = _packedValue.name;
            }

            if(_value != null)
            {
                if(_packedValue is Data.PackedScore packedScore)
                {
                    _value.text = packedScore.score.ToString();
                }
                else if(_packedValue is Data.PackedMultiplier packedMultiplier)
                {
                    _value.text = packedMultiplier.multiplier.ToString();
                }
                else if(_packedValue is Data.PackedHighScore packedHighScore)
                {
                    _value.text = packedHighScore.bestScore.ToString();
                }
            }

            CanDecay(transform.Find("-Persist") == null);
        }

        public override void OnModuleDisable()
        {

        }

        public override void OnUpdate()
        {
            UpdateDecayTimers();
        }
    }
}