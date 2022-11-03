using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public List<UIController> LoadedUIs { get; private set; }
        public UIController ActiveUI { get; private set; }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            DontDestroyOnLoad(Instance.gameObject);

            LoadedUIs = new List<UIController>();

            for(int i = 0; i < Data.UI.LoadedUIObjects.Count; i++)
            {
                var _object = GameObject.Instantiate(Data.UI.LoadedUIObjects[i]);
                var controller = _object.GetComponent<UIController>();
                controller.SetParent(transform);
                controller.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            //LoadHUD(DataManager.UI.DefaultUIName);
        }

        public void LoadHUD(string name)
        {
            UnloadHUD();

            foreach(var _controller in LoadedUIs)
            {
                if(Data.UI.GetHUDName(_controller.gameObject) == name)
                {
                    ActiveUI = _controller;
                    break;
                }
            }

            ActiveUI.gameObject.SetActive(true);
            ActiveUI.SetParent(null);
        }

        public void UnloadHUD()
        {
            if(ActiveUI != null)
            {
                ActiveUI.SetParent(transform);
                ActiveUI.gameObject.SetActive(false);
                ActiveUI = null;
            }
        }
    }
}