﻿using NEP.ScoreLab.Core;
using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.HUD
{
    public class MultiplierModule : Module
    {
        private PackedMultiplier _packedMultiplier { get => (PackedMultiplier)_packedValue; }

        private void Awake()
        {
            if (name == "MultiplierDescriptor")
            {
                ModuleType = UIModuleType.Descriptor;
            }
            else if (name == "Main_Multiplier")
            {
                ModuleType = UIModuleType.Main;
            }
        }

        public override void OnModuleEnable()
        {
            base.OnModuleEnable();

            if (_packedValue == null)
            {
                return;
            }

            if (_packedMultiplier == null)
            {
                return;
            }

            if (ModuleType == UIModuleType.Main)
            {
                SetText(_value, $"{ScoreTracker.Instance.Multiplier.ToString()}x");
            }
            else if (ModuleType == UIModuleType.Descriptor)
            {
                if (PackedValue.Stackable)
                {
                    if (PackedValue.TierEventType != null)
                    {
                        SetText(_title, _packedMultiplier.Name);
                        SetText(_value, $"{_packedMultiplier.Multiplier.ToString()}x");
                    }
                    else
                    {
                        SetText(_title, _packedMultiplier.Name);
                        SetText(_value, $"{_packedMultiplier.AccumulatedMultiplier.ToString()}x");
                    }
                }
                else
                {
                    SetText(_title, _packedMultiplier.Name);
                    SetText(_value, $"{_packedMultiplier.Multiplier.ToString()}x");
                }
            }

            if (_timeBar != null)
            {
                if (_packedMultiplier.Condition != null)
                {
                    _timeBar.gameObject.SetActive(false);
                }
                else
                {
                    _timeBar.gameObject.SetActive(true);
                    SetMaxValueToBar(_timeBar, _packedMultiplier.DecayTime);
                    SetBarValue(_timeBar, _packedMultiplier.DecayTime);
                }
            }
        }

        public override void OnModuleDisable()
        {

        }

        public override void OnUpdate()
        {
            if (_packedMultiplier != null)
            {
                if (_packedMultiplier.condition != null)
                {
                    if (!_packedMultiplier.condition())
                    {
                        UpdateDecay();
                    }
                }
                else
                {
                    UpdateDecay();
                }
            }
            else
            {
                UpdateDecay();
            }

            if (_timeBar != null)
            {
                SetBarValue(_timeBar, _tDecay);
            }
        }
    }
}
