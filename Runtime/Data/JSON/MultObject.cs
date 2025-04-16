using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "MultiplierObject", menuName = "Not Enough Photons/ScoreLab/Multiplier Object", order = 0)]
    public class MultObject : ScriptableObject
    {
        public JSONMult multiplier;
    }
}