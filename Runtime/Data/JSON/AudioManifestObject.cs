using System.IO;

using Newtonsoft.Json;

using UnityEngine;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Audio Manifest", menuName = "Not Enough Photons/ScoreLab/Audio Manifest")]
    public class AudioManifestObject : ScriptableObject
    {
        public JSONAudioManifest manifest;
        
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