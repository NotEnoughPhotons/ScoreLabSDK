using UnityEngine;

namespace NEP.ScoreLab.HUD
{
    [RequireComponent(typeof(Canvas))]
    public class HUD : MonoBehaviour
    {
        public Module ScoreModule { get; set; }
        public Module MultiplierModule { get; set; }
        public Module HighScoreModule { get; set; }

        public Transform followTarget;

        public float distanceToCamera;
        public float movementSmoothness;

        private void Awake()
        {
            if(transform.Find("MainScore") != null)
            {
                ScoreModule = transform.Find("MainScore").GetComponent<ScoreModule>();
            }

            if (transform.Find("Main_Multiplier"))
            {
                MultiplierModule = transform.Find("MainMultiplier").GetComponent<MultiplierModule>();
            }
        }
        
        // For being attached to a physical point on the body
        private void FixedUpdate()
        {
            if (followTarget == null)
            {
                return;
            }

            Vector3 move = Vector3.Lerp(transform.position, followTarget.position + followTarget.forward * distanceToCamera, movementSmoothness * Time.deltaTime);
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
    }
}
