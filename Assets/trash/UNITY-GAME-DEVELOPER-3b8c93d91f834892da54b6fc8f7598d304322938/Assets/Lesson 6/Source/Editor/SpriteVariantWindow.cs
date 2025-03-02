using UnityEditor;
using UnityEngine;

namespace Lesson6.Editor
{
    public class SpriteVariantWindow : EditorWindow
    {
        [MenuItem("Tools/Sprite Variant Window")]
        private static void OpenWindow()
        {
            GetWindow<SpriteVariantWindow>("Sprite Variant Window").Show();
        }

        [SerializeField] private Sprite _sprite;
        
        private UnityEditor.Editor _selfEditor;

        private void OnEnable()
        {
            _selfEditor = UnityEditor.Editor.CreateEditor(this);
        }

        private void OnGUI()
        {
            _selfEditor.DrawDefaultInspector();

            if (GUILayout.Button("Create Sprite Variant"))
            {
                Sprite sprite = Instantiate(_sprite);
                string path =
                    EditorUtility.SaveFilePanelInProject("Save Sprite", sprite.name, "asset", "Save sprite variant");
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }
                AssetDatabase.CreateAsset(sprite, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void OnDisable()
        {
            DestroyImmediate(_selfEditor);
        }
    }
}