using UnityEngine;
using UnityEngine.Animations;

using NEP.ScoreLab.Core;

namespace NEP.ScoreLab.UI
{
    [RequireComponent(typeof(Animator))]
    public class UIModuleAnimator : MonoBehaviour
    {
        public Animator Animator;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            API.Score.OnScoreAdded += (data) => Animator.Play("score_show");
        }

        private void OnDisable()
        {
            API.Score.OnScoreAdded -= (data) => Animator.Play("score_show");
        }
    }
}
