using UnityEngine;
using UnityEngine.Animations;

using NEP.ScoreLab.Core;

namespace NEP.ScoreLab.HUD
{
    [AddComponentMenu("ScoreLab/UI Module Animator")]
    [RequireComponent(typeof(Animator))]
    public class ModuleAnimator : MonoBehaviour
    {
        public Animator Animator;

        private Module _module;

        private void Awake()
        {
            _module = GetComponent<Module>();
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

        private void OnModuleEnabled(Module module)
        {
            if (_module != module)
            {
                return;
            }

            if (module.ModuleType == Module.UIModuleType.Main)
            {
                PlayAnimation("main_show");
            }
            else
            {
                PlayAnimation("descriptor_show");
            }
        }

        private void OnModuleDecayed(Module module)
        {
            if (_module != module)
            {
                return;
            }

            if (module.ModuleType == Module.UIModuleType.Main)
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
