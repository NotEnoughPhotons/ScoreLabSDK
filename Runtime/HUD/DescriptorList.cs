using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NEP.ScoreLab.Core;
using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.HUD
{
    [AddComponentMenu("ScoreLab/UI Descriptor List")]
    public class DescriptorList : MonoBehaviour
    {
        public List<Module> ActiveModules;

        public PackedValue.PackedType packedType { get; set; }

        public GameObject modulePrefab { get; set; }
        public int count = 6;

        public List<Module> modules;

        private void Awake()
        {
            ActiveModules = new List<Module>();
        }

        private void OnEnable()
        {
            API.UI.OnModulePostDecayed += (item) => ActiveModules.Remove(item);

#if UNITY_EDITOR
            API.Editor.OnEditorModuleShow += SetEditorModuleActive;
#else
            API.Score.OnScoreAdded += SetModuleActive;
            API.Score.OnScoreAccumulated += SetModuleActive;
            API.Score.OnScoreTierReached += SetModuleActive;

            API.Multiplier.OnMultiplierAdded += SetModuleActive;
            API.Multiplier.OnMultiplierAccumulated += SetModuleActive;
            API.Multiplier.OnMultiplierTierReached += SetModuleActive;
#endif
        }

        private void OnDisable()
        {
            API.UI.OnModulePostDecayed -= (item) => ActiveModules.Remove(item);

#if UNITY_EDITOR
            API.Editor.OnEditorModuleShow -= SetEditorModuleActive;
#else
            API.Score.OnScoreAdded -= SetModuleActive;
            API.Score.OnScoreAccumulated -= SetModuleActive;
            API.Score.OnScoreTierReached -= SetModuleActive;

            API.Multiplier.OnMultiplierAdded -= SetModuleActive;
            API.Multiplier.OnMultiplierAccumulated -= SetModuleActive;
            API.Multiplier.OnMultiplierTierReached -= SetModuleActive;
#endif
        }

        public void SetPackedType(int packedType)
        {
            this.packedType = (PackedValue.PackedType)packedType;
        }

#if UNITY_EDITOR
        internal void SetEditorModuleActive()
        {
            if (modules == null || modules.Count == 0)
            {
                return;
            }

            foreach (var module in modules)
            {
                if (!module.gameObject.activeInHierarchy)
                {
                    module.OnModuleEditorEnable();
                    module.SetDecayTime(Random.Range(1f, 1.5f));
                    module.SetPostDecayTime(0.5f);

                    module.gameObject.SetActive(true);

                    ActiveModules.Add(module);
                    break;
                }
                else
                {
                    if (ActiveModules.Contains(module))
                    {
                        module.OnModuleEditorEnable();
                        module.SetDecayTime(Random.Range(1f, 1.5f));
                        module.SetPostDecayTime(0.5f);
                    }
                }
            }
        }
#endif

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

            foreach(var module in modules)
            {
                if (!module.gameObject.activeInHierarchy)
                {
                    module.AssignPackedData(value);
                    module.SetDecayTime(value.DecayTime);
                    module.SetPostDecayTime(0.5f);

                    module.gameObject.SetActive(true);

                    ActiveModules.Add(module);
                    break;
                }
                else
                {
                    if (ActiveModules.Contains(module))
                    {
                        if(module.PackedValue.eventType == value.eventType)
                        {
                            if (value.Stackable)
                            {
                                module.AssignPackedData(value);
                                module.OnModuleEnable();
                                module.SetDecayTime(value.DecayTime);
                                module.SetPostDecayTime(0.5f);
                                break;
                            }

                            if (value.Tiers != null)
                            {
                                module.AssignPackedData(value);
                                module.OnModuleEnable();
                                module.SetDecayTime(value.DecayTime);
                                module.SetPostDecayTime(0.5f);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}

