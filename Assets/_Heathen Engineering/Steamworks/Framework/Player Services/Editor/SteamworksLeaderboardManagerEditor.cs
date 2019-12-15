#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using UnityEditor;
using UnityEngine;

namespace HeathenEngineering.SteamTools.Networking
{
    [CustomEditor(typeof(SteamworksLeaderboardManager))]
    public class SteamworksLeaderboardManagerEditor : Editor
    {
        private SerializedProperty LeaderboardRankChanged;
        private SerializedProperty LeaderboardRankLoaded;
        private SerializedProperty LeaderboardNewHighRank;

        private int seTab = 0;

        private void OnEnable()
        {
            LeaderboardRankChanged = serializedObject.FindProperty("LeaderboardRankChanged");
            LeaderboardRankLoaded = serializedObject.FindProperty("LeaderboardRankLoaded");
            LeaderboardNewHighRank = serializedObject.FindProperty("LeaderboardNewHighRank");
        }

        public override void OnInspectorGUI()
        {
            var pManager = target as SteamworksLeaderboardManager;

            var hRect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("");
            var bRect = new Rect(hRect);
            bRect.width = hRect.width / 2f;
            seTab = GUI.Toggle(bRect, seTab == 0, "Settings", EditorStyles.toolbarButton) ? 0 : seTab;
            bRect.x += bRect.width;
            seTab = GUI.Toggle(bRect, seTab == 1, "Events", EditorStyles.toolbarButton) ? 1 : seTab;
            EditorGUILayout.EndHorizontal();

            if (seTab == 0)
            {
                if (!LeaderboardDropAreaGUI("Drop Leaderboards here to add them", pManager))
                    DrawLeaderboardList(pManager);
            }
            else
            {
                EditorGUILayout.PropertyField(LeaderboardRankChanged);
                EditorGUILayout.PropertyField(LeaderboardRankLoaded);
                EditorGUILayout.PropertyField(LeaderboardNewHighRank);
            }

            serializedObject.ApplyModifiedProperties();
        }


        private void DrawLeaderboardList(SteamworksLeaderboardManager pManager)
        {
            if (pManager.Leaderboards == null)
                pManager.Leaderboards = new System.Collections.Generic.List<HeathenSteamLeaderboardData>();

            int il = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;
            for (int i = 0; i < pManager.Leaderboards.Count; i++)
            {
                Rect r = EditorGUILayout.GetControlRect();
                var style = EditorStyles.miniButtonLeft;
                Color sC = GUI.backgroundColor;
                GUI.backgroundColor = new Color(sC.r * 1.25f, sC.g * 0.5f, sC.b * 0.5f, sC.a);
                if (GUI.Button(new Rect(r) { x = r.width, width = 20, height = 15 }, "X", EditorStyles.miniButtonLeft))
                {
                    GUI.backgroundColor = sC;
                    pManager.Leaderboards.RemoveAt(i);
                    return;
                }
                else
                {
                    GUI.backgroundColor = sC;
                    if (GUI.Button(new Rect(r) { x = 0, width = 20, height = 15 }, "P", EditorStyles.miniButtonRight))
                    {
                        EditorGUIUtility.PingObject(pManager.Leaderboards[i]);
                    }
                    r.width -= 20;
                    pManager.Leaderboards[i].leaderboardName = EditorGUI.TextField(r, pManager.Leaderboards[i].name, pManager.Leaderboards[i].leaderboardName);
                }
            }
            EditorGUI.indentLevel = il;
        }

        private bool LeaderboardDropAreaGUI(string message, SteamworksLeaderboardManager pManager)
        {
            Event evt = Event.current;
            Rect drop_area = GUILayoutUtility.GetRect(0.0f, 60.0f, GUILayout.ExpandWidth(true));
            GUI.Box(drop_area, "\n\n" + message, EditorStyles.helpBox);

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!drop_area.Contains(evt.mousePosition))
                        return false;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (UnityEngine.Object dragged_object in DragAndDrop.objectReferences)
                        {
                            // Do On Drag Stuff here
                            if (dragged_object.GetType() == typeof(HeathenSteamLeaderboardData))
                            {
                                HeathenSteamLeaderboardData go = dragged_object as HeathenSteamLeaderboardData;
                                if (go != null)
                                {
                                    if (!pManager.Leaderboards.Exists(p => p == go))
                                    {
                                        pManager.Leaderboards.Add(go);
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

            return false;
        }
    }
}
#endif