using UnityEngine;
using UnityEngine.UI;

using TMPro;

using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.UI
{
    public class UIMultiplierModule : UIModule
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private Slider _timeBar;

        private Data.PackedValue _packedValue;

        private void Awake()
        {
            Transform titleTran = transform.Find("Title");
            Transform valueTran = transform.Find("Value");
            Transform timerTran = transform.Find("TimeBar");

            if (titleTran != null)
            {
                _title = titleTran.GetComponent<TextMeshProUGUI>();
            }

            if (valueTran != null)
            {
                _value = valueTran.GetComponent<TextMeshProUGUI>();
            }

            if(timerTran != null)
            {
                _timeBar = timerTran.GetComponent<Slider>();
            }
        }

        public void AssignPackedData(PackedValue packedValue)
        {
            this._packedValue = packedValue;
        }

        public override void OnModuleEnable()
        {
            if (_packedValue == null)
            {
                return;
            }

            if (_title != null)
            {
                _title.text = _packedValue.name;
            }

            if (_value != null)
            {
                var packedMultiplier = (PackedMultiplier)_packedValue;
                _value.text = packedMultiplier.multiplier.ToString();
            }

            if(_timeBar != null)
            {
                var packedMultiplier = (PackedMultiplier)_packedValue;

                if(packedMultiplier.timer <= 0f && packedMultiplier.condition != null)
                {
                    _timeBar.gameObject.SetActive(false);
                }
                else
                {
                    _timeBar.gameObject.SetActive(true);
                    _timeBar.maxValue = packedMultiplier.timer;
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

            if(_timeBar != null)
            {
                _timeBar.value = _tDecay;
            }
        }
    }
}
