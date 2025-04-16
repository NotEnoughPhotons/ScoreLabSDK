using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [CreateAssetMenu(fileName = "HUD Manifest", menuName = "Not Enough Photons/ScoreLab/HUD Manifest")]
    public class HUDManifestObject : ScriptableObject
    {
        public JSONHUDManifest manifest;
    }
}
