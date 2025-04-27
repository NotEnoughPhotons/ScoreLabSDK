using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.HUD
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance { get; private set; }

        public List<HUD> LoadedHUDs { get; private set; }
        public HUD ActiveHUD { get; private set; }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            DontDestroyOnLoad(Instance.gameObject);

            LoadedHUDs = new List<HUD>();

            for(int i = 0; i < DataManager.UI.LoadedUIObjects.Count; i++)
            {
                var _object = GameObject.Instantiate(DataManager.UI.LoadedUIObjects[i]);
                var controller = _object.GetComponent<HUD>();
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

            foreach(var _controller in LoadedHUDs)
            {
                if(DataManager.UI.GetHUDName(_controller.gameObject) == name)
                {
                    ActiveHUD = _controller;
                    break;
                }
            }

            ActiveHUD.gameObject.SetActive(true);
            ActiveHUD.SetParent(null);
        }

        public void UnloadHUD()
        {
            if(ActiveHUD != null)
            {
                ActiveHUD.SetParent(transform);
                ActiveHUD.gameObject.SetActive(false);
                ActiveHUD = null;
            }
        }
    }
}