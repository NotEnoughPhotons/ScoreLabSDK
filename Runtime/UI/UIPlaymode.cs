using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.UI
{
    [RequireComponent(typeof(UIController))]
    public class UIPlaymode : MonoBehaviour
    {
        [SerializeField] private KeyCode _createScoreCode = KeyCode.Alpha1;
        [SerializeField] private KeyCode _createMultiplierCode = KeyCode.Alpha2;

        private UIController _uiController;

        private void Awake()
        {
            _uiController = GetComponent<UIController>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_createScoreCode))
            {
                Core.API.Editor.OnEditorModuleShow?.Invoke();
            }

            if (Input.GetKeyDown(_createMultiplierCode))
            {

            }
        }
    }
}
