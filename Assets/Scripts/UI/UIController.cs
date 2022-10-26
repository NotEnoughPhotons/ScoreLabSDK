using UnityEngine;

using NEP.ScoreLab.Core;
using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.UI
{
    public class UIController : MonoBehaviour
    {
        public UIModule ScoreModule { get; set; }
        public UIModule MultiplierModule { get; set; }
        public UIModule HighScoreModule { get; set; }

        public Transform followTarget;

        private Transform _homeParent;

        private void OnEnable()
        {
            API.Score.OnScoreAdded += (data) => UpdateModule(data, ScoreModule);

            API.Multiplier.OnMultiplierAdded += (data) => UpdateModule(data, MultiplierModule);
            API.Multiplier.OnMultiplierRemoved += (data) => UpdateModule(data, MultiplierModule);
        }

        private void OnDisable()
        {
            API.Score.OnScoreAdded -= (data) => UpdateModule(data, ScoreModule);

            API.Multiplier.OnMultiplierAdded -= (data) => UpdateModule(data, MultiplierModule);
            API.Multiplier.OnMultiplierRemoved -= (data) => UpdateModule(data, MultiplierModule);
        }

        private void Update()
        {
            if(followTarget == null)
            {
                return;
            }

            Vector3 move = Vector3.Lerp(transform.position, followTarget.position + followTarget.forward * 3f, 6f * Time.deltaTime);
            Quaternion lookRot = Quaternion.LookRotation(followTarget.forward);

            transform.position = move;
            transform.rotation = lookRot;

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i) != null)
                {
                    //transform.GetChild(i).LookAt(followTarget);
                }
            }
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
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
