using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.UI
{
    public class UIModuleAnimator : MonoBehaviour
    {
        public Animator Animator;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }
    }
}
