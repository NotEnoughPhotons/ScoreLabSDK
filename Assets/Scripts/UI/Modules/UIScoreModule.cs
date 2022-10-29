using UnityEngine;
using UnityEngine.UI;

using NEP.ScoreLab.Core;
using NEP.ScoreLab.Data;

using TMPro;

namespace NEP.ScoreLab.UI
{
    public class UIScoreModule : UIModule
    {
        private PackedScore _packedScore { get => (PackedScore)_packedValue; }

        public override void OnModuleEnable()
        {
            base.OnModuleEnable();

            if(_packedValue == null)
            {
                return;
            }

            if(ModuleType == UIModuleType.Main)
            {
                SetText(_value, ScoreTracker.Instance.Score.ToString());
            }
            else if (ModuleType == UIModuleType.Descriptor)
            {
                if (PackedValue.Stackable)
                {
                    if(PackedValue.TierEventType != null)
                    {
                        SetText(_title, _packedScore.Name);
                        SetText(_value, _packedScore.Score);
                    }
                    else
                    {
                        SetText(_title, _packedScore.Name);
                        SetText(_value, _packedScore.AccumulatedScore);
                    }
                }
                else
                {
                    SetText(_title, _packedScore.Name);
                    SetText(_value, _packedScore.Score);
                }
            }
        }

        public override void OnModuleDisable()
        {

        }

        public override void OnUpdate()
        {
            UpdateDecay();
        }
    }
}