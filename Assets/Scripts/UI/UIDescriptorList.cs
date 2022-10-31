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

            API.Value.OnValueAdded += SetModuleActive;
            API.Value.OnValueAccumulated += SetModuleActive;
            API.Value.OnValueTierReached += SetModuleActive;
        }

        private void OnDisable()
        {
            API.UI.OnModulePostDecayed -= (item) => ActiveModules.Remove(item);

            API.Value.OnValueAdded -= SetModuleActive;
            API.Value.OnValueAccumulated -= SetModuleActive;
            API.Value.OnValueTierReached -= SetModuleActive;
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

