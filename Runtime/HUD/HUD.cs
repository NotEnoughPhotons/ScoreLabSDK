using UnityEngine;

using NEP.ScoreLab.Core;
using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.HUD
{
    public class HUD : MonoBehaviour
    {
        public Module ScoreModule { get; set; }
        public Module MultiplierModule { get; set; }
        public Module HighScoreModule { get; set; }

        public Transform followTarget;

        private void Awake()
        {
            if(transform.Find("Main_Score") != null)
            {
                ScoreModule = transform.Find("Main_Score").GetComponent<ScoreModule>();
            }

            if (transform.Find("Main_Multiplier"))
            {
                MultiplierModule = transform.Find("Main_Multiplier").GetComponent<MultiplierModule>();
            }
        }

        private void OnEnable()
        {
            UpdateModule(null);
            UpdateModule(null);

            API.Value.OnValueAdded += UpdateModule;
            API.Value.OnValueTierReached += UpdateModule;
            API.Value.OnValueAccumulated += UpdateModule;
            API.Value.OnValueRemoved += UpdateModule;
        }

        private void OnDisable()
        {
            API.Value.OnValueAdded -= UpdateModule;
            API.Value.OnValueTierReached -= UpdateModule;
            API.Value.OnValueAccumulated -= UpdateModule;
            API.Value.OnValueRemoved -= UpdateModule;
        }

        private void Start()
        {

        }
        
        // For being attached to a physical point on the body
        private void FixedUpdate()
        {
            if (followTarget == null)
            {
                return;
            }

            Vector3 move = Vector3.Lerp(transform.position, followTarget.position + followTarget.forward * Settings.DistanceToCamera, Settings.MovementSmoothness * Time.deltaTime);
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

        public void SetScoreModule(Module module)
        {
            this.ScoreModule = module;
        }

        public void SetMultiplierModule(Module module)
        {
            this.MultiplierModule = module;
        }

        public void UpdateModule(PackedValue data)
        {
            if (data is PackedScore)
            {
                ScoreModule.AssignPackedData(data);
                ScoreModule.OnModuleEnable();
            }
            else if (data is PackedMultiplier)
            {
                MultiplierModule.AssignPackedData(data);
                MultiplierModule.OnModuleEnable();
            }
        }
    }
}
