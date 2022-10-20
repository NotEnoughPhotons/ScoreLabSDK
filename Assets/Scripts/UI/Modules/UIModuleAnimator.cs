using UnityEngine;
using UnityEngine.Animations;

using NEP.ScoreLab.Core;

namespace NEP.ScoreLab.UI
{
    [RequireComponent(typeof(Animator))]
    public class UIModuleAnimator : MonoBehaviour
    {
        public Animator Animator;

        private UIModule _module;

        private void Awake()
        {
            _module = GetComponent<UIModule>();
            Animator = GetComponent<Animator>();

            if(_module is UIScoreModule)
            {
                _module.OnModuleEnabled += () => PlayAnimation("score_descriptor_show");
                _module.OnModuleDecayed += () => PlayAnimation("score_descriptor_hide");
            }
            
            if(_module is UIMultiplierModule)
            {
                _module.OnModuleEnabled += () => PlayAnimation("mult_descriptor_show");
                _module.OnModuleDecayed += () => PlayAnimation("mult_descriptor_hide");
            }
        }

        private void PlayAnimation(string name)
        {
            if(Animator == null)
            {
                return;
            }
            
            Animator.Play(name);
        }
    }
}
