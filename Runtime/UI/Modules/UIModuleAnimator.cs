using UnityEngine;
using UnityEngine.Animations;

using NEP.ScoreLab.Core;

namespace NEP.ScoreLab.UI
{
    [AddComponentMenu("ScoreLab/UI Module Animator")]
    [RequireComponent(typeof(Animator))]
    public class UIModuleAnimator : MonoBehaviour
    {
        public Animator Animator;

        private UIModule _module;

        private void Awake()
        {
            _module = GetComponent<UIModule>();
            Animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            API.UI.OnModuleEnabled += OnModuleEnabled;
            API.UI.OnModuleDecayed += OnModuleDecayed;
        }

        private void OnDisable()
        {
            API.UI.OnModuleEnabled -= OnModuleEnabled;
            API.UI.OnModuleDecayed -= OnModuleDecayed;
        }

        private void PlayAnimation(string name)
        {
            if(Animator == null)
            {
                return;
            }

            Animator.Play(name, -1, 0f);
        }

        private void OnModuleEnabled(UIModule module)
        {
            if (_module != module)
            {
                return;
            }

            if (module.ModuleType == UIModule.UIModuleType.Main)
            {
                PlayAnimation("main_show");
            }
            else
            {
                PlayAnimation("descriptor_show");
            }
        }

        private void OnModuleDecayed(UIModule module)
        {
            if (_module != module)
            {
                return;
            }

            if (module.ModuleType == UIModule.UIModuleType.Main)
            {
                PlayAnimation("main_hide");
            }
            else
            {
                PlayAnimation("descriptor_hide");
            }
        }
    }
}
