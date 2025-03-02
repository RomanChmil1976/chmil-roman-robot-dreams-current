using System;
using UnityEngine;

namespace Lesson5
{
    public class MoversController : MonoBehaviour
    {
        [SerializeField] private SimplexMover[] simplexMovers;

        private GUIStyle _labelStyle;
        private GUIStyle _buttonStyle;
        
        private Vector2 _scrollPosition;
        
        private void Start()
        {
            for (int i = 0; i < simplexMovers.Length; ++i)
            {
                simplexMovers[i].Init();
            }
        }

        private void OnGUI()
        {
            if (_labelStyle == null)
            {
                _labelStyle = new GUIStyle(GUI.skin.label);
                _labelStyle.alignment = TextAnchor.MiddleCenter;
            }

            if (_buttonStyle == null)
            {
                _buttonStyle = new GUIStyle(GUI.skin.button);
                _buttonStyle.padding = new RectOffset(12, 12, 0, 0);
            }
            
            GUILayout.BeginArea(new Rect(16, 16, 200, 200));
                GUI.Box(new Rect(0, 0, 200, 200), string.Empty);
                    GUILayout.Label("Movers controller", _labelStyle);
                    _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, true);
                        for (int i = 0; i < simplexMovers.Length; ++i)
                        {
                            SimplexMover simplexMover = simplexMovers[i];
                            GUI.enabled = !simplexMover.enabled;
                            if (GUILayout.Button(simplexMover.name, _buttonStyle))
                            {
                                PlayMover(simplexMover);
                            }
                        }
                        GUI.enabled = true;
                    GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void PlayMover(SimplexMover simplexMover)
        {
            simplexMover.enabled = true;
        }
    }
}