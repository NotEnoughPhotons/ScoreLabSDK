using NEP.ScoreLab.HUD;
using UnityEditor;
using UnityEngine;

namespace NEP.ScoreLab.Editor
{
    public static class SLContextMenu
    {
        [MenuItem(itemName: "GameObject/ScoreLab/Setup HUD")]
        public static void SetupHUD()
        {
            Transform selection = Selection.activeTransform;
            selection.gameObject.AddComponent<HUD.HUD>();
            Canvas canvas = selection.gameObject.AddComponent<Canvas>();
            RectTransform rectTransform = canvas.transform as RectTransform;

            rectTransform.sizeDelta = Vector2.one;
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.transform.position = Vector3.zero;
            canvas.transform.localPosition = Vector3.zero;
            canvas.transform.localScale = Vector3.one;
            canvas.transform.rotation = Quaternion.identity;
            
            CreateMainScore();
            CreateMainMultiplier();
        }
        
        [MenuItem(itemName: "GameObject/ScoreLab/Score/Main Object")]
        public static void CreateMainScore()
        {
            Transform parent = Selection.activeTransform;
            GameObject obj = new GameObject();
            obj.name = "Main_Score";
            obj.AddComponent<ScoreModule>();
            obj.transform.SetParent(parent);
        }
        
        [MenuItem(itemName: "GameObject/ScoreLab/Score/Descriptor Object")]
        public static void CreateDescriptorScore()
        {
            Transform parent = Selection.activeTransform;

            if (parent.GetComponent<DescriptorList>() == null)
            {
                EditorUtility.DisplayDialog("ScoreLab SDK - Error", "Cannot assign descriptor to something that doesn't have a list!", "OK");
                return;
            }
            
            GameObject obj = new GameObject();
            obj.name = "ScoreDescriptor";
            obj.AddComponent<ScoreModule>();
            obj.transform.SetParent(parent);
        }
        
        [MenuItem(itemName: "GameObject/ScoreLab/Multiplier/Main Object")]
        public static void CreateMainMultiplier()
        {
            Transform parent = Selection.activeTransform;
            GameObject obj = new GameObject();
            obj.name = "Main_Multiplier";
            obj.AddComponent<ScoreModule>();
            obj.transform.SetParent(parent);
        }
        
        [MenuItem(itemName: "GameObject/ScoreLab/Multiplier/Descriptor Object")]
        public static void CreateDescriptorMultiplier()
        {
            Transform parent = Selection.activeTransform;
            
            if (parent.GetComponent<DescriptorList>() == null)
            {
                EditorUtility.DisplayDialog("ScoreLab SDK - Error", "Cannot assign descriptor to something that doesn't have a list!", "OK");
                return;
            }
            
            GameObject obj = new GameObject();
            obj.name = "MultiplierDescriptor";
            obj.AddComponent<MultiplierModule>();
            obj.transform.SetParent(parent);
        }
    }
}