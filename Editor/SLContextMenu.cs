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
            
            HUD.HUD hud = selection.GetComponent<HUD.HUD>();
            Canvas canvas = null;
            RectTransform rectTransform = null;

            if (hud)
            {
                canvas = hud.GetComponent<Canvas>();
                rectTransform = canvas.GetComponent<RectTransform>();
                
                rectTransform.sizeDelta = Vector2.one;
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.transform.position = Vector3.zero;
                canvas.transform.localPosition = Vector3.zero;
                canvas.transform.localScale = Vector3.one;
                canvas.transform.rotation = Quaternion.identity;
                
                CreateMainScore();
                CreateMainMultiplier();
                return;
            }
            
            hud = selection.gameObject.AddComponent<HUD.HUD>();
            canvas = selection.gameObject.AddComponent<Canvas>();
            rectTransform = canvas.transform as RectTransform;
            
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

            Transform copy = parent.Find("MainScore");
            
            if (copy)
            {
                if (!copy.GetComponent<ScoreModule>())
                {
                    copy.gameObject.AddComponent<ScoreModule>();
                }
                
                return;
            }
            
            GameObject obj = new GameObject();
            obj.name = "MainScore";
            obj.AddComponent<ScoreModule>();
            RectTransform rectTransform = obj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = Vector2.one;
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
            RectTransform rectTransform = obj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = Vector2.one;
            obj.transform.SetParent(parent);
        }
        
        [MenuItem(itemName: "GameObject/ScoreLab/Multiplier/Main Object")]
        public static void CreateMainMultiplier()
        {
            Transform parent = Selection.activeTransform;
            
            Transform copy = parent.Find("MainMultiplier");
            
            if (copy)
            {
                if (!copy.GetComponent<MultiplierModule>())
                {
                    copy.gameObject.AddComponent<MultiplierModule>();
                }
                
                return;
            }
            
            GameObject obj = new GameObject();
            obj.name = "MainMultiplier";
            obj.AddComponent<MultiplierModule>();
            RectTransform rectTransform = obj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = Vector2.one;
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
            RectTransform rectTransform = obj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = Vector2.one;
            obj.transform.SetParent(parent);
        }
    }
}