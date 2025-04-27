using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.HUD
{
    [RequireComponent(typeof(HUD))]
    public class HUDPlaymode : MonoBehaviour
    {
        [SerializeField] private KeyCode _createScoreCode = KeyCode.Alpha1;
        [SerializeField] private KeyCode _createMultiplierCode = KeyCode.Alpha2;

        private HUD _hud;

        private void Awake()
        {
            _hud = GetComponent<HUD>();
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(_createScoreCode))
            {
                // Core.API.Editor.OnEditorModuleShow?.Invoke();
            }

            if (Input.GetKeyDown(_createMultiplierCode))
            {

            }
            #endif
        }
    }
}
