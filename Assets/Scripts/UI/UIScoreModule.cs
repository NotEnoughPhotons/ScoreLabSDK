using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace NEP.ScoreLab.UI
{
    public class UIScoreModule : UIModule
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private Slider _timeBar;

        private Data.PackedValue _packedValue;

        private void Awake()
        {
            Transform titleTran = transform.Find("Title");
            Transform valueTran = transform.Find("Value");
            Transform timeBarTran = transform.Find("TimeBar");
            
            _title = titleTran?.GetComponent<TextMeshProUGUI>();
            _value = valueTran?.GetComponent<TextMeshProUGUI>();
            _timeBar = timeBarTran?.GetComponent<Slider>();
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
                var packedScore = (Data.PackedScore)_packedValue;
                _value.text = packedScore.score.ToString();
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