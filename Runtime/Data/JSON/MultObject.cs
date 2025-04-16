using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "MultObject", menuName = "Not Enough Photons/ScoreLab/Create/Mult Object", order = 0)]
    public class MultObject : ScriptableObject
    {
        public JSONMult multiplier;
    }
}