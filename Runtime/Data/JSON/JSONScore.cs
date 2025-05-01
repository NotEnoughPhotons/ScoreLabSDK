using UnityEngine;

using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NEP.ScoreLab.Data
{
    [System.Serializable]
    public struct JSONScore
    {
        public EventType.ScoreEventType EventType;

        public float DecayTime;
        public bool Stackable;
        public JSONAudioParams EventAudio;

        public string Name;
        public int Score;

        public int TierRequirement;

        public JSONScore[] Tiers;

        public string ToJSON()
        {
            string json = string.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;

                    writer.WriteStartObject();
                    WriteJSONScore(writer, this);

                    if (Tiers != null && Tiers.Length > 0)
                    {
                        writer.WritePropertyName("Tiers");
                        writer.WriteStartArray();
                        foreach (var tier in Tiers)
                        {
                            writer.WriteStartObject();
                            WriteJSONScore(writer, tier);
                            writer.WriteEndObject();
                        }

                        writer.WriteEndArray();
                    }

                    writer.WriteEndObject();
                }

                json = sw.ToString();
            }

            return json;
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
            writer.WritePropertyName("Clips");
            writer.WriteStartArray();
            for (int i = 0; i < score.EventAudio.sounds.Length; i++)
            {
                writer.WriteValue(score.EventAudio.sounds[i].name);
            }
            writer.WriteEndArray();
            writer.WritePropertyName("Volume");
            writer.WriteValue(score.EventAudio.volume);
            writer.WritePropertyName("Pitch");
            writer.WriteValue(score.EventAudio.pitch);
            writer.WriteEndObject();
            writer.WritePropertyName("Stackable");
            writer.WriteValue(score.Stackable);
            writer.WritePropertyName("DecayTime");
            writer.WriteValue(score.DecayTime);

            writer.WritePropertyName("EventType");
            writer.WriteValue(Data.EventType.ScoreEventTable[(int)score.EventType]);

            writer.WritePropertyName("TierRequirement");
            writer.WriteValue(score.TierRequirement);
        }
    }
}