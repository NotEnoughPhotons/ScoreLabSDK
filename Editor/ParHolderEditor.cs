using NEP.ScoreLab.Data;
using NEP.ScoreLab.SDK;
using UnityEditor;
using UnityEngine;

namespace NEP.ScoreLab.Editor
{
    [CustomEditor(typeof(ParDefinitionHolder))]
    public class ParHolderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ParDefinitionHolder holder = (ParDefinitionHolder)target;
            
            holder.ParDefinition = (ParDefinition)EditorGUILayout.ObjectField("Par Definition", holder.ParDefinition, typeof(ParDefinition), false);

            if (!holder.ParDefinition)
                return;
            
            var so = new SerializedObject(holder);

            if (so == null)
            {
                Debug.LogError("Serialized Par Definition is null");
            }
            
            if (GUILayout.Button("Serialize"))
            {
                so.FindProperty("Data").stringValue = holder.ParDefinition.ToJson().ToString();
                so.ApplyModifiedProperties();
                Debug.Log(so.FindProperty("Data").stringValue);
            }
        }
    }
}
