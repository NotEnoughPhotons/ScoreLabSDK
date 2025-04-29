#if UNITY_EDITOR
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [CreateAssetMenu(fileName = "HUD Manifest", menuName = "Not Enough Photons/ScoreLab/HUD Manifest")]
    public class HUDManifestObject : ScriptableObject
    {
        public JSONHUDManifest manifest;

        public string ToJSON()
        {
            string json = "";
            using (StringWriter sw = new StringWriter())
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                            
                    writer.WriteStartObject();
                    
                    writer.WritePropertyName("name");
                    writer.WriteValue(manifest.Name);
                    writer.WritePropertyName("author");
                    writer.WriteValue(manifest.Author);
                    writer.WritePropertyName("description");
                            
                    writer.WriteEndObject();
                }

                json = sw.ToString();
            }

            return json;
        }
    }
}
#endif