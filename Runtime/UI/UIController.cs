using UnityEngine;

using NEP.ScoreLab.Core;
using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.UI
{
    [UnityEngine.ExecuteAlways]
    [AddComponentMenu("ScoreLab/UI Controller")]
    public class UIController : MonoBehaviour
    {
        public UIModule ScoreModule { get; set; }
        public UIModule MultiplierModule { get; set; }
        public UIModule HighScoreModule { get; set; }

        public Transform followTarget;

        public float Distance;
        public float Lerp;

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

            Vector3 move = Vector3.Lerp(transform.position, followTarget.position + followTarget.forward * Distance, Lerp * Time.deltaTime);
            Quaternion lookRot = Quaternion.LookRotation(followTarget.forward);

            transform.position = move;
            transform.rotation = lookRot;
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR || UNITY_EDITOR_64
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
#endif
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
