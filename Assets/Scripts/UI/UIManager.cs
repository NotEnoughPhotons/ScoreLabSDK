using UnityEngine;

using NEP.ScoreLab.Core;
using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.UI
{
    public class UIManager : MonoBehaviour
    {
        public UIModule ScoreModule;
        public UIModule MultiplierModule;
        public UIModule HighScoreModule;

        private void Awake()
        {
            API.Score.OnScoreAdded += (data) => UpdateModule(data, ScoreModule);

            API.Multiplier.OnMultiplierAdded += (data) => UpdateModule(data, MultiplierModule);
            API.Multiplier.OnMultiplierRemoved += (data) => UpdateModule(data, MultiplierModule);
        }

        public void UpdateModule(PackedValue data, UIModule module)
        {
            if(module == null)
            {
                return;
            }

            module.AssignPackedData(data);
            module.OnModuleEnable();
        }
    }
}
