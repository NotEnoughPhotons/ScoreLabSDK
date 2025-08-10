using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Par Definition", menuName="Not Enough Photons/ScoreLab/New Par Definition", order = 20)]
    public class ParDefinition : ScriptableObject
    {
        public ParData Data => m_data;

        [SerializeField] private ParData m_data;

        public JToken ToJson()
        {
            JToken result = null;

            using (JTokenWriter writer = new JTokenWriter())
            {
                writer.Formatting = Formatting.Indented;
                
                writer.WriteStartObject();
                writer.WritePropertyName("barcode");
                writer.WriteValue(m_data.Barcode);
                if (m_data.Grades != null)
                {
                    writer.WritePropertyName("grades");
                    writer.WriteStartArray();
                    JArray array = writer.CurrentToken as JArray;
                    for (int i = 0; i < m_data.Grades.Length; i++)
                    {
                        JObject grade = m_data.Grades[i].ToJSON();
                        array.Add(grade);
                    }
                    writer.WriteEndArray();
                }
                else
                {
                    writer.WritePropertyName("score");
                    writer.WriteValue(m_data.Score);
                }
                writer.WriteEndObject();
                
                result = writer.Token;
            }

            return result;
        }
    }
}