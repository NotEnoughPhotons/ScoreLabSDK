using UnityEngine;
using NEP.ScoreLab.SDK;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    public class ParData
    {
        public string Barcode => _barcode;
        public GradeDefinition[] Grades => _grades;
        public int Score => _score;
        public bool IsBaseGame => _isBaseGame;
        
        [SerializeField] private string _barcode;
        [SerializeField] private GradeDefinition[] _grades;
        [SerializeField] private int _score;
        [SerializeField] private bool _isBaseGame;
    }
}