using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.UI
{
    public class UIDescriptorList : MonoBehaviour
    {
        public GameObject modulePrefab;
        public int count;

        public List<UIModule> modules;

        private void Awake()
        {
            modules = new List<UIModule>();

            if(modulePrefab == null)
            {
                return;
            }

            for(int i = 0; i < count; i++)
            {
                var module = GameObject.Instantiate(modulePrefab.gameObject, transform);
                modules.Add(module.GetComponent<UIModule>());
                module.SetActive(false);
            }
        }

        private void OnEnable()
        {
            API.Score.OnScoreAdded += SetScoreModuleActive;
            API.Multiplier.OnMultiplierAdded += SetMultiplierModuleActive;
        }

        private void OnDisable()
        {
            API.Score.OnScoreAdded -= SetScoreModuleActive;
            API.Multiplier.OnMultiplierAdded -= SetMultiplierModuleActive;
        }

        public void SetScoreModuleActive(Data.PackedValue packedValue)
        {
            if(modules == null || modules.Count == 0)
            {
                return;
            }

            for(int i = 0; i < modules.Count; i++)
            {
                if (!modules[i].gameObject.activeInHierarchy)
                {
                    UIScoreModule scoreModule = (UIScoreModule)modules[i];
                    scoreModule.AssignPackedData(packedValue);

                    scoreModule.SetDecayTime(5f);
                    scoreModule.SetPostDecayTime(0.5f);

                    modules[i].gameObject.SetActive(true);
                    return;
                }
            }
        }

        public void SetMultiplierModuleActive(Data.PackedValue packedValue)
        {
            if (modules == null || modules.Count == 0)
            {
                return;
            }

            for (int i = 0; i < modules.Count; i++)
            {
                if (!modules[i].gameObject.activeInHierarchy)
                {
                    Data.PackedMultiplier packedMult = (Data.PackedMultiplier)packedValue;
                    UIMultiplierModule multiplierModule = (UIMultiplierModule)modules[i];

                    multiplierModule.AssignPackedData(packedValue);

                    multiplierModule.SetDecayTime(packedMult.timer);
                    multiplierModule.SetPostDecayTime(0.5f);

                    modules[i].gameObject.SetActive(true);
                    return;
                }
            }
        }

    }
}

