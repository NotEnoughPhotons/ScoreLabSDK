using UnityEngine;
using Newtonsoft.Json.Linq;

namespace NEP.ScoreLab.Data
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
}