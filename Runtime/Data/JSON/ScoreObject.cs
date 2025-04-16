using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ScoreObject", menuName = "Not Enough Photons/ScoreLab/Create/Score Object", order = 0)]
    public class ScoreObject : ScriptableObject
    {
        public JSONScore score;
    }
}