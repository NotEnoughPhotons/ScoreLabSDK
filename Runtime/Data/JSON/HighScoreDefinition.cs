using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "High Score Definition", menuName="Not Enough Photons/ScoreLab/New High Score Definition", order = 20)]
    public class HighScoreDefinition : ScriptableObject
    {
        [System.Serializable]
        public class GradeDefinition
        {
            public string Grade => _grade;
            public int Threshold => _threshold;
        
            [SerializeField] private string _grade;
            [SerializeField] private int _threshold;

            public JObject ToJSON()
            {
                JObject result = null;

                using (JTokenWriter writer = new JTokenWriter())
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("grade");
                    writer.WriteValue(_grade);
                    writer.WritePropertyName("threshold");
                    writer.WriteValue(_threshold);
                    writer.WriteEndObject();

                    result = writer.Token as JObject;
                }
                
                return result;
            }
        }
        
        public string Barcode => _barcode;
        public GradeDefinition[] Grades => _grades;
        public int Score => _score;
        public bool IsBaseGame => _isBaseGame;
        
        [SerializeField] private string _barcode;
        [SerializeField] private GradeDefinition[] _grades;
        [SerializeField] private int _score;
        [SerializeField] private bool _isBaseGame;

        public JObject ToJson()
        {
            JObject result = null;

            using (JTokenWriter writer = new JTokenWriter())
            {
                writer.Formatting = Formatting.Indented;
                    
                writer.WriteStartObject();
                writer.WritePropertyName("barcode");
                writer.WriteValue(_barcode);
                if (_grades != null)
                {
                    writer.WritePropertyName("grades");
                    writer.WriteStartArray();
                    JArray array = writer.CurrentToken as JArray;
                    for (int i = 0; i < _grades.Length; i++)
                    {
                        JObject grade = _grades[i].ToJSON();
                        array.Add(grade);
                    }
                    writer.WriteEndArray();
                }
                else
                {
                    writer.WritePropertyName("score");
                    writer.WriteValue(_score);
                }
                writer.WriteEndObject();

                result = writer.Token as JObject;
            }

            return result;
        }
    }
}