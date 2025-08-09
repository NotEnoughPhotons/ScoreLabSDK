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

            if (holder.ParDefinition != null && GUILayout.Button("Serialize"))
                holder.ParData = holder.ParDefinition.Data;
        }
    }
}
