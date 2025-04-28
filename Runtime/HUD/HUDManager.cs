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

        }

        public void LoadHUD(string name)
        {

        }

        public void UnloadHUD()
        {

        }
    }
}