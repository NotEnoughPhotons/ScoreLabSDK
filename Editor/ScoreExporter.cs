#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NEP.ScoreLab.Data;

namespace NEP.ScoreLab.Editor
{
    public class ScoreExporter : EditorWindow
    {
        private ScoreObject m_scoreObject;
        private string fileName;
        
        [MenuItem("Not Enough Photons/ScoreLab/Score Editor", false)]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(ScoreExporter));
            window.titleContent = new GUIContent("Score Editor");
        }

        private void OnGUI()
        {
            GUILayout.Label("Fields");
            fileName = EditorGUILayout.TextField("File Name", fileName);
            m_scoreObject = EditorGUILayout.ObjectField(m_scoreObject, typeof(ScoreObject), false) as ScoreObject;

            if (m_scoreObject)
            {
                if (GUILayout.Button("Export"))
                {
                    string path = $"Data/Not Enough Photons/ScoreLab/Data/Score/{fileName}.json";
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Application.dataPath, path)))
                    {
                        using (JsonWriter writer = new JsonTextWriter(sw))
                        {
                            JSONScore score = m_scoreObject.score;
                            writer.Formatting = Formatting.Indented;
                            
                            writer.WriteStartObject();
                            WriteJSONScore(writer, score);

                            if (score.Tiers != null && score.Tiers.Length > 0)
                            {
                                writer.WritePropertyName("Tiers");
                                writer.WriteStartArray();
                                foreach (var tier in score.Tiers)
                                {
                                    writer.WriteStartObject();
                                    WriteJSONScore(writer, tier);
                                    writer.WriteEndObject();
                                }
                                writer.WriteEndArray();
                            }
                            
                            writer.WriteEndObject();
                        }
                    }
                }
            }
        }

        private void WriteJSONScore(JsonWriter writer, JSONScore score)
        {
            if (score.Name != string.Empty)
            {
                writer.WritePropertyName("Name");
                writer.WriteValue(score.Name);
            }
            
            writer.WritePropertyName("Score");
            writer.WriteValue(score.Score);
            
            writer.WritePropertyName("EventAudio");
            writer.WriteStartObject();
            writer.WritePropertyName("Clip");
            writer.WriteValue(score.EventAudio.sound != null ? score.EventAudio.sound.name : null);
            writer.WritePropertyName("Volume");
            writer.WriteValue(score.EventAudio.volume);
            writer.WritePropertyName("Pitch");
            writer.WriteValue(score.EventAudio.pitch);
            writer.WriteEndObject();
            writer.WritePropertyName("Stackable");
            writer.WriteValue(score.Stackable);
            writer.WritePropertyName("DecayTime");
            writer.WriteValue(score.DecayTime);

            if (score.EventType != string.Empty)
            {
                writer.WritePropertyName("EventType");
                writer.WriteValue(score.EventType);
            }

            if (score.TierEventType != string.Empty)
            {
                writer.WritePropertyName("TierEventType");
                writer.WriteValue(score.TierEventType);
            }
            
            writer.WritePropertyName("TierRequirement");
            writer.WriteValue(score.TierRequirement);
        }
    }
}
#endif