#if UNITY_EDITOR
using System.IO;
using System.Security.Policy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                    writer.WriteValue(manifest.Description);

                    if (manifest.Tags != null && manifest.Tags.Length > 0)
                    {
                        writer.WritePropertyName("tags");
                        writer.WriteStartArray();
                        for (int i = 0; i < manifest.Tags.Length; i++)
                        {
                            writer.WriteValue(manifest.Tags[i]);
                        }
                        writer.WriteEndArray();
                    }
                    
                    writer.WritePropertyName("guid");
                    writer.WriteValue(manifest.GUID);
                            
                    writer.WriteEndObject();
                }

                json = sw.ToString();
            }

            return json;
        }

        public void SetGUID(string guid)
        {
            manifest.GUID = guid;
        }
    }
}
#endif