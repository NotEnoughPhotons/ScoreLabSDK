using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NEP.ScoreLab.Core;
using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.UI
{
    public class UIDescriptorList : MonoBehaviour
    {
        public List<UIModule> ActiveModules;

        public PackedValue.PackedType packedType { get; set; }

        public GameObject modulePrefab { get; set; }
        public int count = 6;

        public List<UIModule> modules;

        private void Awake()
        {
            ActiveModules = new List<UIModule>();
        }

        private void OnEnable()
        {
            API.UI.OnModulePostDecayed += (item) => ActiveModules.Remove(item);

            API.Score.OnScoreAdded += SetModuleActive;
            API.Score.OnScoreAccumulated += SetModuleActive;
            API.Score.OnScoreTierReached += SetModuleActive;

            API.Multiplier.OnMultiplierAdded += SetModuleActive;
            API.Multiplier.OnMultiplierAccumulated += SetModuleActive;
            API.Multiplier.OnMultiplierTierReached += SetModuleActive;
        }

        private void OnDisable()
        {
            API.UI.OnModulePostDecayed -= (item) => ActiveModules.Remove(item);

            API.Score.OnScoreAdded -= SetModuleActive;
            API.Score.OnScoreAccumulated -= SetModuleActive;
            API.Score.OnScoreTierReached -= SetModuleActive;

            API.Multiplier.OnMultiplierAdded -= SetModuleActive;
            API.Multiplier.OnMultiplierAccumulated -= SetModuleActive;
            API.Multiplier.OnMultiplierTierReached -= SetModuleActive;
        }

        public void SetPackedType(int packedType)
        {
            this.packedType = (PackedValue.PackedType)packedType;
        }

        public void SetModuleActive(PackedValue value)
        {
            if (modules == null || modules.Count == 0)
            {
                return;
            }

            if (value.PackedValueType != packedType)
            {
                return;
            }

            foreach (var module in modules)
            {
                if (!ActiveModules.Contains(module))
                {
                    InitializeInactiveModule(value, module);
                    break;
                }
                else
                {
                    InitializeActiveModule(value, module);
                    break;
                }
            }
        }

        public void InitializeInactiveModule(PackedValue value, UIModule module)
        {
            module.AssignPackedData(value);

            module.SetDecayTime(value.DecayTime);
            module.SetPostDecayTime(0.5f);
            module.gameObject.SetActive(true);

            ActiveModules.Add(module);
        }

        public void InitializeActiveModule(PackedValue value, UIModule module)
        {
            if (value.Stackable)
            {
                module.OnModuleEnable();
                module.SetDecayTime(value.DecayTime);
                module.SetPostDecayTime(0.5f);
            }

            if (value.Tiers != null)
            {
                module.AssignPackedData(value);
                module.OnModuleEnable();
                module.SetDecayTime(value.DecayTime);
                module.SetPostDecayTime(0.5f);
            }
        }
    }
}

