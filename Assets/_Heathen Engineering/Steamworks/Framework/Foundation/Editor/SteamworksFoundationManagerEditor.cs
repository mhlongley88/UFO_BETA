#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using UnityEditor;
using UnityEngine;

namespace HeathenEngineering.SteamTools.Networking
{
    [CustomEditor(typeof(SteamworksFoundationManager))]
    public class SteamworksFoundationManagerEditor : Editor
    {
        private SteamworksFoundationManager pManager;
        private SerializedProperty Settings;
        
        private SerializedProperty DoNotDestroyOnLoad;
        private SerializedProperty OnSteamInitalized;
        private SerializedProperty OnSteamInitalizationError;
        private SerializedProperty OnOverlayActivated;
        private SerializedProperty OnUserStatsRecieved;
        private SerializedProperty OnUserStatsStored;
        private SerializedProperty OnAchievementStored;
        private SerializedProperty OnAvatarLoaded;
        private SerializedProperty OnPersonaStateChanged;
        
        private int tabPage = 0;
        private int appTabPage = 0;
        private int seTab = 0;

        private void OnEnable()
        {
            Settings = serializedObject.FindProperty("Settings");
            
            DoNotDestroyOnLoad = serializedObject.FindProperty("_doNotDistroyOnLoad");
            OnSteamInitalized = serializedObject.FindProperty("OnSteamInitalized");
            OnSteamInitalizationError = serializedObject.FindProperty("OnSteamInitalizationError");
            OnOverlayActivated = serializedObject.FindProperty("OnOverlayActivated");

            OnUserStatsRecieved = serializedObject.FindProperty("OnUserStatsRecieved");
            OnUserStatsStored = serializedObject.FindProperty("OnUserStatsStored");
            OnAchievementStored = serializedObject.FindProperty("OnAchievementStored");
            OnAvatarLoaded = serializedObject.FindProperty("OnAvatarLoaded");
            OnPersonaStateChanged = serializedObject.FindProperty("OnPersonaStateChanged");
        }

        public override void OnInspectorGUI()
        { 
            pManager = target as SteamworksFoundationManager;

            EditorGUILayout.PropertyField(Settings);

            if (pManager.Settings == null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Assign a Steam Settings object to get started!");
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Create a new Steam Settings object by right clicking in your Project panel and selecting [Create] > [Steamworks] > [Steam Settings]", MessageType.Info);
            }
            else
            {
                Rect hRect = EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("");

                Rect bRect = new Rect(hRect);
                bRect.width = hRect.width / 3f;
                tabPage = GUI.Toggle(bRect, tabPage == 0, "App & Overlay", EditorStyles.toolbarButton) ? 0 : tabPage;
                bRect.x += bRect.width;
                tabPage = GUI.Toggle(bRect, tabPage == 1, "Stats", EditorStyles.toolbarButton) ? 1 : tabPage;
                bRect.x += bRect.width;
                tabPage = GUI.Toggle(bRect, tabPage == 2, "Achievements", EditorStyles.toolbarButton) ? 2 : tabPage;
                EditorGUILayout.EndHorizontal();
                //EditorGUILayout.Space();

                if (tabPage == 0)
                {
                    DrawAppOverlayData(pManager);
                }
                else if (tabPage == 1)
                {
                    if (pManager.Settings != null)
                    {
                        if (!StatsDropAreaGUI("Drop Stats here to add them", pManager))
                            DrawStatsList(pManager);
                    }
                }
                else if (tabPage == 2)
                {
                    hRect = EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("");
                    bRect = new Rect(hRect);
                    bRect.width = hRect.width / 2f;
                    seTab = GUI.Toggle(bRect, seTab == 0, "Settings", EditorStyles.toolbarButton) ? 0 : seTab;
                    bRect.x += bRect.width;
                    seTab = GUI.Toggle(bRect, seTab == 1, "Events", EditorStyles.toolbarButton) ? 1 : seTab;
                    EditorGUILayout.EndHorizontal();

                    if (seTab == 0)
                    {
                        if (pManager.Settings != null)
                        {
                            if (!AchievementsDropAreaGUI("Drop Achievements here to add them", pManager))
                                DrawAchievementList(pManager);
                        }
                    }
                    else
                    {
                        if (pManager.Settings != null)
                        {
                            EditorGUILayout.PropertyField(OnUserStatsRecieved);
                            EditorGUILayout.PropertyField(OnUserStatsStored);
                            EditorGUILayout.PropertyField(OnAchievementStored);
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Requires Steam Settings");
                        }
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSteamUserData(SteamworksFoundationManager pManager)
        {
            if(pManager.Settings == null)
            {
                EditorGUILayout.HelpBox("Requires Steam Settings", MessageType.Info);
                return;
            }

            if(pManager.Settings.UserData == null)
            {
                EditorGUILayout.HelpBox("Requires you reference a Steam User Data object in your Steam Settings", MessageType.Info);
                return;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Steam Id");
            EditorGUILayout.LabelField(pManager != null ? pManager.Settings.UserData.SteamId.m_SteamID.ToString() : "unknown");
            EditorGUILayout.EndHorizontal();

            if (pManager.Settings.UserData.State == Steamworks.EPersonaState.k_EPersonaStateAway)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Status");
                EditorGUILayout.LabelField("Away");
                EditorGUILayout.EndHorizontal();
            }
            else if (pManager.Settings.UserData.State == Steamworks.EPersonaState.k_EPersonaStateBusy)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Status");
                EditorGUILayout.LabelField("Busy");
                EditorGUILayout.EndHorizontal();
            }
            else if (pManager.Settings.UserData.State == Steamworks.EPersonaState.k_EPersonaStateLookingToPlay)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Status");
                EditorGUILayout.LabelField("Looking to Play");
                EditorGUILayout.EndHorizontal();
            }
            else if (pManager.Settings.UserData.State == Steamworks.EPersonaState.k_EPersonaStateLookingToTrade)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Status");
                EditorGUILayout.LabelField("Looking to Trade");
                EditorGUILayout.EndHorizontal();
            }
            else if (pManager.Settings.UserData.State == Steamworks.EPersonaState.k_EPersonaStateMax)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Status");
                EditorGUILayout.LabelField("Max");
                EditorGUILayout.EndHorizontal();
            }
            else if (pManager.Settings.UserData.State == Steamworks.EPersonaState.k_EPersonaStateOffline)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Status");
                EditorGUILayout.LabelField("Offline");
                EditorGUILayout.EndHorizontal();
            }
            else if (pManager.Settings.UserData.State == Steamworks.EPersonaState.k_EPersonaStateOnline)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Status");
                EditorGUILayout.LabelField("Online");
                EditorGUILayout.EndHorizontal();
            }
            else if (pManager.Settings.UserData.State == Steamworks.EPersonaState.k_EPersonaStateSnooze)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Status");
                EditorGUILayout.LabelField("Snooze");
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Status");
                EditorGUILayout.LabelField("unknown");
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Steam Name");
            EditorGUILayout.LabelField(pManager != null && !string.IsNullOrEmpty(pManager.Settings.UserData.DisplayName) ? pManager.Settings.UserData.DisplayName : "unknown");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Avatar");
            Rect iRect = EditorGUILayout.GetControlRect(true, 150);
            EditorGUILayout.EndHorizontal();

            //iRect.y += iRect.height;
            iRect.width = 150;
            //iRect.height = 150;

            EditorGUILayout.Space();

            if (pManager.Settings.UserData.Avatar != null)
            {
                EditorGUI.DrawPreviewTexture(iRect, pManager.Settings.UserData.Avatar);
            }
            else
            {
                EditorGUI.DrawRect(iRect, Color.black);
            }
        }

        private void DrawAppOverlayData(SteamworksFoundationManager pManager)
        {
            Rect hRect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("");

            Rect bRect = new Rect(hRect);
            bRect.width = hRect.width / 2f;
            seTab = GUI.Toggle(bRect, seTab == 0, "Settings", EditorStyles.toolbarButton) ? 0 : seTab;
            bRect.x += bRect.width;
            seTab = GUI.Toggle(bRect, seTab == 1, "Events", EditorStyles.toolbarButton) ? 1 : seTab;
            EditorGUILayout.EndHorizontal();

            if (seTab == 0)
            {
                EditorGUILayout.PropertyField(DoNotDestroyOnLoad);
                EditorGUILayout.BeginHorizontal();
                if (pManager.Settings != null)
                {
                    var v = System.Convert.ToUInt32(EditorGUILayout.IntField("Steam App Id", System.Convert.ToInt32(pManager.Settings.ApplicationId.m_AppId)));
                    if (v != pManager.Settings.ApplicationId.m_AppId)
                    {
                        pManager.Settings.ApplicationId.m_AppId = v;
                        EditorUtility.SetDirty(pManager.Settings);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Notification Settings");
                if (pManager.Settings == null)
                {
                    EditorGUILayout.LabelField("Requires Steam Settings");
                }
                else
                {
                    int cSelected = (int)pManager.Settings.NotificationPosition;

                    EditorGUILayout.BeginVertical();
                    cSelected = EditorGUILayout.Popup(cSelected, new string[] { "Top Left", "Top Right", "Bottom Left", "Bottom Right" });

                    var v = EditorGUILayout.Vector2IntField(GUIContent.none, pManager.Settings.NotificationInset);
                    if(pManager.Settings.NotificationInset != v)
                    {
                        pManager.Settings.NotificationInset = v;
                        EditorUtility.SetDirty(pManager.Settings);
                    }
                    EditorGUILayout.EndVertical();

                    if (pManager.Settings.NotificationPosition != (ENotificationPosition)cSelected)
                    {
                        pManager.Settings.NotificationPosition = (ENotificationPosition)cSelected;
                        EditorUtility.SetDirty(pManager.Settings);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                hRect = EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("");

                bRect = new Rect(hRect);
                bRect.width = hRect.width / 3f;
                appTabPage = GUI.Toggle(bRect, appTabPage == 0, "Application", EditorStyles.toolbarButton) ? 0 : appTabPage;
                bRect.x += bRect.width;
                appTabPage = GUI.Toggle(bRect, appTabPage == 1, "Overlay", EditorStyles.toolbarButton) ? 1 : appTabPage;
                bRect.x += bRect.width;
                appTabPage = GUI.Toggle(bRect, appTabPage == 2, "Friends", EditorStyles.toolbarButton) ? 2 : appTabPage;
                EditorGUILayout.EndHorizontal();

                if (appTabPage == 0)
                {
                    EditorGUILayout.PropertyField(OnSteamInitalized);
                    EditorGUILayout.PropertyField(OnSteamInitalizationError);
                }
                else if (appTabPage == 1)
                {
                    if (pManager.Settings != null)
                    {
                        EditorGUILayout.PropertyField(OnOverlayActivated);
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Requires Steam Settings");
                    }
                }
                else
                {
                    if (pManager.Settings != null)
                    {
                        EditorGUILayout.PropertyField(OnAvatarLoaded);
                        EditorGUILayout.PropertyField(OnPersonaStateChanged);
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Requires Steam Settings");
                    }
                }
            }
        }

        private void DrawStatsList(SteamworksFoundationManager pManager)
        {
            int il = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;
            for (int i = 0; i < pManager.Settings.stats.Count; i++)
            {
                Rect r = EditorGUILayout.GetControlRect();
                var style = EditorStyles.miniButtonLeft;
                Color sC = GUI.backgroundColor;
                GUI.backgroundColor = new Color(sC.r * 1.25f, sC.g * 0.5f, sC.b * 0.5f, sC.a);
                if (GUI.Button(new Rect(r) { x = r.width, width = 20, height = 15 }, "X", EditorStyles.miniButtonLeft))
                {
                    GUI.backgroundColor = sC;
                    pManager.Settings.stats.RemoveAt(i);
                    EditorUtility.SetDirty(pManager.Settings);
                    return;
                }
                else
                {
                    GUI.backgroundColor = sC;
                    if (GUI.Button(new Rect(r) { x = 0, width = 20, height = 15 }, "P", EditorStyles.miniButtonRight))
                    {
                        EditorGUIUtility.PingObject(pManager.Settings.stats[i]);
                    }
                    r.width -= 20;
                    var iFWidth = r.width * 0.75f;
                    var dFWidth = r.width * 0.25f;
                    r.width = iFWidth;
                    var v = EditorGUI.TextField(r, pManager.Settings.stats[i].name.Replace(" Float Stat Data", "").Replace(" Int Stat Data", "").Replace("Float Stat Data ", "").Replace("Int Stat Data ", ""), pManager.Settings.stats[i].statName);
                    if (v != pManager.Settings.stats[i].statName)
                    {
                        pManager.Settings.stats[i].statName = v;
                        EditorUtility.SetDirty(pManager.Settings.stats[i]);
                    }
                    r.x += r.width;
                    r.width = dFWidth;
                    EditorGUI.LabelField(r, pManager.Settings.stats[i].DataType == SteamStatData.StatDataType.Float ? pManager.Settings.stats[i].GetFloatValue().ToString("0.####") : pManager.Settings.stats[i].GetIntValue().ToString());
                }
            }
            EditorGUI.indentLevel = il;
        }

        private void DrawAchievementList(SteamworksFoundationManager pManager)
        {
            int il = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;
            for (int i = 0; i < pManager.Settings.achievements.Count; i++)
            {
                Rect r = EditorGUILayout.GetControlRect();
                var style = EditorStyles.miniButtonLeft;
                Color sC = GUI.backgroundColor;
                GUI.backgroundColor = new Color(sC.r * 1.25f, sC.g * 0.5f, sC.b * 0.5f, sC.a);
                if (GUI.Button(new Rect(r) { x = r.width, width = 20, height = 15 }, "X", EditorStyles.miniButtonLeft))
                {
                    GUI.backgroundColor = sC;
                    pManager.Settings.achievements.RemoveAt(i);
                    EditorUtility.SetDirty(pManager.Settings);
                    return;
                }
                else
                {
                    GUI.backgroundColor = sC;
                    if (GUI.Button(new Rect(r) { x = 0, width = 20, height = 15 }, "P", EditorStyles.miniButtonRight))
                    {
                        EditorGUIUtility.PingObject(pManager.Settings.achievements[i]);
                    }
                    r.width -= 20;
                    var v = EditorGUI.TextField(r, pManager.Settings.achievements[i].name.Replace("Steam Achievement Data ", ""), pManager.Settings.achievements[i].achievementId);
                    if (v != pManager.Settings.achievements[i].achievementId)
                    {
                        pManager.Settings.achievements[i].achievementId = v;
                        EditorUtility.SetDirty(pManager.Settings);
                    }
                }
            }
            EditorGUI.indentLevel = il;
        }
        
        private bool AchievementsDropAreaGUI(string message, SteamworksFoundationManager pManager)
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
                            if (dragged_object.GetType() == typeof(SteamAchievementData))
                            {
                                SteamAchievementData go = dragged_object as SteamAchievementData;
                                if (!pManager.Settings.achievements.Exists(p => p == go))
                                {
                                    pManager.Settings.achievements.Add(go);
                                    EditorUtility.SetDirty(pManager.Settings);
                                    return true;
                                }
                            }
                        }
                    }
                    break;
            }

            return false;
        }

        private bool StatsDropAreaGUI(string message, SteamworksFoundationManager pManager)
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
                            if (dragged_object.GetType() == typeof(SteamFloatStatData) || dragged_object.GetType() == typeof(SteamIntStatData))
                            {
                                SteamStatData go = dragged_object as SteamStatData;
                                if (!pManager.Settings.stats.Exists(p => p == go))
                                {
                                    pManager.Settings.stats.Add(go);
                                    EditorUtility.SetDirty(pManager.Settings);
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