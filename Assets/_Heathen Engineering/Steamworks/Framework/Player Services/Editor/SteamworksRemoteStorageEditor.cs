#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using UnityEditor;
using UnityEngine;

namespace HeathenEngineering.SteamTools.Networking
{

    [CustomEditor(typeof(SteamworksRemoteStorage))]
    public class SteamworksRemoteStorageEditor : Editor
    {
        private SteamworksRemoteStorage cloud;
        public SerializedProperty FileReadAsyncComplete;
        public SerializedProperty FileWriteAsyncComplete;

        private int tabPage = 0;

        private void OnEnable()
        {
            FileReadAsyncComplete = serializedObject.FindProperty("FileReadAsyncComplete");
            FileWriteAsyncComplete = serializedObject.FindProperty("FileWriteAsyncComplete");
        }

        public override void OnInspectorGUI()
        {
            cloud = target as SteamworksRemoteStorage;

            Rect hRect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("");

            Rect bRect = new Rect(hRect);
            bRect.width = hRect.width / 2f;
            tabPage = GUI.Toggle(bRect, tabPage == 0, "Settings", EditorStyles.toolbarButton) ? 0 : tabPage;
            bRect.x += bRect.width;
            tabPage = GUI.Toggle(bRect, tabPage == 1, "Events", EditorStyles.toolbarButton) ? 1 : tabPage;
            EditorGUILayout.EndHorizontal();

            if (tabPage == 0)
            {
                if (!DropAreaGUI("Drop Data Libraries here to add them"))
                    DrawDataLibList();
            }
            else
            {
                EditorGUILayout.PropertyField(FileReadAsyncComplete);
                EditorGUILayout.PropertyField(FileWriteAsyncComplete);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDataLibList()
        {
            int il = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;
            for (int i = 0; i < cloud.GameDataModel.Count; i++)
            {
                Rect r = EditorGUILayout.GetControlRect();
                var style = EditorStyles.miniButtonLeft;
                Color sC = GUI.backgroundColor;
                GUI.backgroundColor = new Color(sC.r * 1.25f, sC.g * 0.5f, sC.b * 0.5f, sC.a);
                if (GUI.Button(new Rect(r) { x = r.width, width = 20, height = 15 }, "X", EditorStyles.miniButtonLeft))
                {
                    GUI.backgroundColor = sC;
                    cloud.GameDataModel.RemoveAt(i);
                    return;
                }
                else
                {
                    GUI.backgroundColor = sC;
                    if (GUI.Button(new Rect(r) { x = 0, width = 20, height = 15 }, "P", EditorStyles.miniButtonRight))
                    {
                        EditorGUIUtility.PingObject(cloud.GameDataModel[i]);
                    }
                    r.width -= 20;                    
                                           
                    cloud.GameDataModel[i].filePrefix = EditorGUI.TextField(r, cloud.GameDataModel[i].name.Replace(" Steam Data Library ", "").Replace("Steam Data Library ", "").Replace(" Steam Data Library", ""), cloud.GameDataModel[i].filePrefix);
                }
            }
            EditorGUI.indentLevel = il;
        }

        private bool DropAreaGUI(string message)
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
                            if (dragged_object.GetType() == typeof(SteamDataLibrary))
                            {
                                SteamDataLibrary go = dragged_object as SteamDataLibrary;
                                if (!cloud.GameDataModel.Contains(go))
                                {
                                    cloud.GameDataModel.Add(go);
                                    return true;
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