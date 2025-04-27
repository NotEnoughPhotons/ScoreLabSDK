using UnityEngine;
using UnityEngine.UI;

using NEP.ScoreLab.Core;
using NEP.ScoreLab.Data;

using TMPro;

namespace NEP.ScoreLab.HUD
{
    [AddComponentMenu("ScoreLab/Modules/Score Module")]
    public class ScoreModule : Module
    {
        private PackedScore _packedScore { get => (PackedScore)_packedValue; }

#if UNITY_EDITOR
        public override void OnModuleEditorEnable()
        {
            base.OnModuleEditorEnable();

            if (_packedValue == null)
            {
                return;
            }

            if (ModuleType == UIModuleType.Main)
            {
                SetText(_value, Random.Range(0, 100));
            }
            else if (ModuleType == UIModuleType.Descriptor)
            {
                SetText(_title, "Placeholder!");
                SetText(_value, Random.Range(0, 100));
            }
        }
#endif

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