using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    public class JSONHUDManifest
    {
        public string Name;
        public string Author;
        public string Description;
        public string[] Tags;
        public string GUID;
        public Texture2D Logo;
    }
}