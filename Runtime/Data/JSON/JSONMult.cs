using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NEP.ScoreLab.Data
{
    public struct JSONMult
    {
        public EventType.MultiplierEventType EventType;

        public float DecayTime;
        public bool Stackable;

        public string Name;
        public float Multiplier;
        public string Condition;

        public int TierRequirement;

        public JSONMult[] Tiers;
        
        public string ToJSON()
        {
            string json = string.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;

                    writer.WriteStartObject();
                    WriteJSONMult(writer, this);

                    if (Tiers != null && Tiers.Length > 0)
                    {
                        writer.WritePropertyName("Tiers");
                        writer.WriteStartArray();
                        foreach (var tier in Tiers)
                        {
                            writer.WriteStartObject();
                            WriteJSONMult(writer, tier);
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

        private void WriteJSONMult(JsonWriter writer, JSONMult mult)
        {
            if (mult.Name != string.Empty)
            {
                writer.WritePropertyName("Name");
                writer.WriteValue(mult.Name);
            }

            writer.WritePropertyName("Multiplier");
            writer.WriteValue(mult.Multiplier);

            //writer.WritePropertyName("EventAudio");
            //writer.WriteStartObject();
            //writer.WritePropertyName("Clip");
            //writer.WriteValue(mult.EventAudio.sound != null ? mult.EventAudio.sound.name : null);
            //writer.WritePropertyName("Volume");
            //writer.WriteValue(mult.EventAudio.volume);
            //writer.WritePropertyName("Pitch");
            //writer.WriteValue(mult.EventAudio.pitch);
            //writer.WriteEndObject();
            writer.WritePropertyName("Stackable");
            writer.WriteValue(mult.Stackable);
            writer.WritePropertyName("DecayTime");
            writer.WriteValue(mult.DecayTime);

            writer.WritePropertyName("EventType");
            writer.WriteValue(Data.EventType.ScoreEventTable[(int)mult.EventType]);

            writer.WritePropertyName("TierRequirement");
            writer.WriteValue(mult.TierRequirement);
        }
    }
}