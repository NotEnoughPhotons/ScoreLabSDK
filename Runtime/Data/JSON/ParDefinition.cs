using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Par Definition", menuName="Not Enough Photons/ScoreLab/New Par Definition", order = 20)]
    public class ParDefinition : ScriptableObject
    {
        public ParData Data { get; private set; }

        public JToken ToJson()
        {
            JToken result = null;

            using (JTokenWriter writer = new JTokenWriter())
            {
                writer.Formatting = Formatting.Indented;
                    
                writer.WritePropertyName(Data.Barcode);
                writer.WriteStartObject();
                if (Data.Grades != null)
                {
                    writer.WritePropertyName("grades");
                    writer.WriteStartArray();
                    JArray array = writer.CurrentToken as JArray;
                    for (int i = 0; i < Data.Grades.Length; i++)
                    {
                        JObject grade = Data.Grades[i].ToJSON();
                        array.Add(grade);
                    }
                    writer.WriteEndArray();
                }
                else
                {
                    writer.WritePropertyName("score");
                    writer.WriteValue(Data.Score);
                }
                writer.WriteEndObject();
                
                result = writer.Token;
            }

            return result;
        }
    }
}