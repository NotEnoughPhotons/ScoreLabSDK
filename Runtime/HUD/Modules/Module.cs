using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.HUD
{
    public class Module : MonoBehaviour
    {
        public enum UIModuleType
        {
            Main,
            Descriptor
        }

        public UIModuleType ModuleType;
    }
}