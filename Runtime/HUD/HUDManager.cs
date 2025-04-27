using UnityEngine;

using System.Collections.Generic;

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
            Instance = this;
        }

        private void Start()
        {
            LoadHUD("Tabloid");
        }

        public void LoadHUD(string name)
        {
            UnloadHUD();
            foreach (var _controller in LoadedHUDs)
            {
                if (DataManager.UI.GetHUDName(_controller.gameObject) == name)
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