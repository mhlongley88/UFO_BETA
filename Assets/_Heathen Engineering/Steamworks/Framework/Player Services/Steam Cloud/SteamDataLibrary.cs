#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using System;
using System.Collections.Generic;
using System.Linq;
using HeathenEngineering.Scriptable;
using UnityEngine;

namespace HeathenEngineering.SteamTools
{
    /// <summary>
    /// Defines the relationship between a Steam Data File and a given Data Library
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "Library Variables/Steam Game Data Library")]
    public class SteamDataLibrary : DataLibraryVariable
    {
        public string filePrefix;
        [HideInInspector]
        public SteamDataFile activeFile;
        [HideInInspector]
        public List<SteamDataFileAddress> availableFiles = new List<SteamDataFileAddress>();

        /// <summary>
        /// Saves the current library data to the current active file
        /// </summary>
        /// <returns></returns>
        public void Save()
        {
            activeFile.linkedLibrary = this;
            SteamworksRemoteStorage.Instance.FileWrite(activeFile);
        }
        
        /// <summary>
        /// Saves the file with a new name.
        /// Note that if the provided file name does not start with the filePrefix defined then it will be added
        /// </summary>
        /// <param name="fileName">The name to save as
        /// Note that if the provided file name does not start with the filePrefix defined then it will be added</param>
        /// <returns></returns>
        public void SaveAs(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            if (!fileName.StartsWith(filePrefix))
                fileName = filePrefix + fileName;

            var result = SteamworksRemoteStorage.Instance.FileWrite(fileName, this);

            if(result)
            {
                Debug.Log("[SteamDataLibrary.SaveAs] Saved '" + fileName + "' successfully.");
            }
            else
            {
                Debug.LogWarning("[SteamDataLibrary.SaveAs] Failed to save '" + fileName + "' to Steam Remote Storage.\nPlease consult https://partner.steamgames.com/doc/api/ISteamRemoteStorage#FileWrite for more information.");
            }
        }

        /// <summary>
        /// Saves the current library data to the current active file
        /// </summary>
        /// <returns></returns>
        public void SaveAsync()
        {
            activeFile.linkedLibrary = this;
            SteamworksRemoteStorage.Instance.FileWriteAsync(activeFile);
        }

        /// <summary>
        /// Saves the file with a new name.
        /// Note that if the provided file name does not start with the filePrefix defined then it will be added
        /// </summary>
        /// <param name="fileName">The name to save as
        /// Note that if the provided file name does not start with the filePrefix defined then it will be added</param>
        /// <returns></returns>
        public void SaveAsAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            if (!fileName.StartsWith(filePrefix))
                fileName = filePrefix + fileName;

            SteamworksRemoteStorage.Instance.FileWriteAsync(fileName, this);
        }

        /// <summary>
        /// Loads the data for the current active file if any
        /// Note that this will overwrite the data current stored in the library
        /// </summary>
        /// <returns>True if the operation completed, false if skiped such as for a blank active file</returns>
        public void Load()
        {
            if (activeFile != null)
            {
                SteamworksRemoteStorage.Instance.FileRead(activeFile.address);
            }
        }

        /// <summary>
        /// Loads the data for the current active file if any
        /// Note that this will overwrite the data current stored in the library
        /// </summary>
        /// <returns>True if the operation completed, false if skiped such as for a blank active file</returns>
        public void LoadAsync()
        {
            if (activeFile != null)
            {
                SteamworksRemoteStorage.Instance.FileReadAsync(activeFile.address);
            }
        }

        /// <summary>
        /// Loads the data from a given address
        /// Note that the load operation will only establish the result as the active data if its prefix matches
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public void Load(SteamDataFileAddress address)
        {
            if (!string.IsNullOrEmpty(address.fileName) && address.fileName.StartsWith(filePrefix))
            {
                SteamworksRemoteStorage.Instance.FileRead(address);
            }
        }

        /// <summary>
        /// Loads the data from a given address
        /// Note that the load operation will only establish the result as the active data if its prefix matches
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public void LoadAsync(SteamDataFileAddress address)
        {
            if (!string.IsNullOrEmpty(address.fileName) && address.fileName.StartsWith(filePrefix))
            {
                SteamworksRemoteStorage.Instance.FileReadAsync(address);
            }
        }

        /// <summary>
        /// Loads the data from a given address
        /// Note that the load operation will only establish the result as the active data if its prefix matches
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public void Load(int availableFileIndex)
        {
            if (availableFileIndex >= 0 && availableFileIndex < availableFiles.Count)
            {
                Load(availableFiles[availableFileIndex]);
            }
        }

        /// <summary>
        /// Loads the data from a given address
        /// Note that the load operation will only establish the result as the active data if its prefix matches
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public void LoadAsync(int availableFileIndex)
        {
            if (availableFileIndex >= 0 && availableFileIndex < availableFiles.Count)
            {
                LoadAsync(availableFiles[availableFileIndex]);
            }
        }
    }
}
#endif