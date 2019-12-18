#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using HeathenEngineering.Scriptable;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HeathenEngineering.SteamTools
{
    public class SteamDataFileList : MonoBehaviour
    {
        public SteamDataLibrary Library;
        public SteamDataFileRecord RecordPrefab;
        public RectTransform Container;
        public BoolReference RemovePrefix = new BoolReference(true);
        public StringReference DateDisplayFormat = new StringReference("F");
        [Header("Events")]
        public UnityEvent SelectionChanged;
        public SteamDataFile Active
        {
            get
            {
                return Library.activeFile;
            }
        }
        private SteamDataFileAddress? s_SelectedFile;
        public SteamDataFileAddress? SelectedFile
        {
            get
            {
                return s_SelectedFile;
            }
            set
            {
                if(s_SelectedFile.HasValue && value.HasValue)
                {
                    if (s_SelectedFile.Value != value.Value)
                    {
                        s_SelectedFile = value;
                        SelectionChanged.Invoke();
                    }
                }
                else if (s_SelectedFile.HasValue != value.HasValue)
                {
                    s_SelectedFile = value;
                    SelectionChanged.Invoke();
                }
            }
        }

        /// <summary>
        /// Updates the list from the library values sorted on the time stamp of the record
        /// </summary>
        public void Refresh()
        {
            SteamworksRemoteStorage.Instance.RefreshDataFilesIndex();
            var temp = new List<GameObject>();
            foreach(Transform child in Container)
            {
                temp.Add(child.gameObject);
            }

            while(temp.Count > 0)
            {
                var t = temp[0];
                temp.Remove(t);
                Destroy(t);
            }

            Library.availableFiles.Sort((p1, p2) => { return p1.UtcTimestamp.CompareTo(p2.UtcTimestamp); });
            Library.availableFiles.Reverse();

            foreach (var address in Library.availableFiles)
            {
                var go = Instantiate(RecordPrefab.gameObject, Container);
                var r = go.GetComponent<SteamDataFileRecord>();
                r.parentList = this;
                r.Address = address;
                if (RemovePrefix.Value && address.fileName.StartsWith(Library.filePrefix))
                    r.FileName.text = address.fileName.Substring(Library.filePrefix.Length);
                else
                    r.FileName.text = address.fileName;
                r.Timestamp.text = address.LocalTimestamp.ToString(DateDisplayFormat, Thread.CurrentThread.CurrentCulture);
            }
        }

        public SteamDataFileAddress? GetLatest()
        {
            if (Library.availableFiles.Count > 0)
                return Library.availableFiles[0];
            else
                return null;
        }

        public void ClearSelected()
        {
            SelectedFile = null;
        }

        public void Select(SteamDataFileAddress address)
        {
            SelectedFile = address;
        }

        public void SelectLatest()
        {
            SelectedFile = GetLatest();
        }

        public void LoadSelected()
        {
            if (SelectedFile.HasValue)
                Library.Load(SelectedFile.Value);
        }

        public void LoadSelectedAsync()
        {
            if (SelectedFile.HasValue)
                Library.LoadAsync(SelectedFile.Value);
        }

        public void DeleteSelected()
        {
            if (SelectedFile.HasValue)
                SteamworksRemoteStorage.Instance.FileDelete(SelectedFile.Value);

            Refresh();
        }

        public void ForgetSelected()
        {
            if (SelectedFile.HasValue)
                SteamworksRemoteStorage.Instance.FileForget(SelectedFile.Value);
        }

        public void SaveActive()
        {
            if (SelectedFile.HasValue)
            {
                Library.Save();
                Refresh();
            }
            else
            {
                Debug.LogWarning("[SteamDataFileList.SaveActive] Attempted to save the active file but no file is active.");
            }
        }

        public void SaveAs(string fileName)
        {
            Library.SaveAs(fileName);
            Refresh();

            SelectLatest();
        }

        public void SaveAs(InputField fileName)
        {
            if (fileName == null || string.IsNullOrEmpty(fileName.text))
            {
                Debug.LogWarning("[SteamDataFileList.SaveAs] Attempted to SaveAs but was not provided with a file name ... will attempt to save the active file instead.");
                SaveActive();
            }
            else
            {
                Library.SaveAs(fileName.text);
                Refresh();

                SelectLatest();
            }
        }

        public void SaveActiveAsync()
        {
            if (SelectedFile.HasValue)
                Library.SaveAsync();
        }

        public void SaveAsAsync(string fileName)
        {
            Library.SaveAsAsync(fileName);

            string fName = fileName.StartsWith(Library.filePrefix) ? fileName : Library.filePrefix + fileName;

            if (Library.availableFiles.Exists(p => p.fileName == fName))
                SelectedFile = Library.availableFiles.First(p => p.fileName == fName);
        }

        public void SaveAsAsync(InputField fileName)
        {
            Library.SaveAsAsync(fileName.text);

            string fName = fileName.text.StartsWith(Library.filePrefix) ? fileName.text : Library.filePrefix + fileName.text;

            if (Library.availableFiles.Exists(p => p.fileName == fName))
                SelectedFile = Library.availableFiles.First(p => p.fileName == fName);
        }
    }
}
#endif