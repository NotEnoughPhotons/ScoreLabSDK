using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using UnityEngine;


namespace NEP.ScoreLab.Data
{
    [CreateAssetMenu(fileName = "HUD Manifest", menuName = "Not Enough Photons/ScoreLab/Announcer Manifest")]
    public class AnnouncerManifestObject : ScriptableObject
    {
        public JSONAnnouncerManifest manifest;
        
        public string ToJSON()
        {
            string json = "";
            using (StringWriter sw = new StringWriter())
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                            
                    writer.WriteStartObject();
                    
                    writer.WritePropertyName("clips");
                    writer.WriteStartArray();
                    
                    for (int i = 0; i < manifest.Clips.Length; i++)
                    {
                        writer.WriteValue(manifest.Clips[i].name);
                    }
                    
                    writer.WriteEndArray();
                            
                    writer.WriteEndObject();
                }

                json = sw.ToString();
            }

            return json;
        }
    }
}