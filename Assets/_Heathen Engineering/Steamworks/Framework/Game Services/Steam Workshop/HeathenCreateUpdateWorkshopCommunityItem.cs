#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamTools
{
    /// <summary>
    /// Handler for Workshop items
    /// This object can be used to simplify create, update, delete and display of Workshop Items
    /// </summary>
    [Serializable]
    public class HeathenCreateUpdateWorkshopCommunityItem
    {
        public AppId_t TargetApp;
        public PublishedFileId_t FileId;
        public SteamUserData Author;
        public EWorkshopFileType FileType = EWorkshopFileType.k_EWorkshopFileTypeCommunity;
        public string Title;
        public string Description;
        public ERemoteStoragePublishedFileVisibility Visibility = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate;
        public List<string> Tags = new List<string>();
        /// <summary>
        /// This should be set to a valid folder path in which the content you wish to upload can be found
        /// </summary>
        public string ContentLocation;
        public Texture2D previewImage;
        /// <summary>
        /// This should be set to a valid file path for the preview image you wish to upload
        /// </summary>
        public string PreviewImageLocation;

        public UnityWorkshopItemCreatedEvent Created = new UnityWorkshopItemCreatedEvent();
        public UnityWorkshopItemCreatedEvent CreateFailed = new UnityWorkshopItemCreatedEvent();
        public UnityWorkshopSubmitItemUpdateResultEvent Updated = new UnityWorkshopSubmitItemUpdateResultEvent();
        public UnityWorkshopSubmitItemUpdateResultEvent UpdateFailed = new UnityWorkshopSubmitItemUpdateResultEvent();

        public CallResult<CreateItemResult_t> m_CreatedItem;
        public CallResult<SubmitItemUpdateResult_t> m_SubmitItemUpdateResult;
        public CallResult<RemoteStorageDownloadUGCResult_t> m_RemoteStorageDownloadUGCResult;

        public bool HasFileId
        {
            get { return FileId != PublishedFileId_t.Invalid; }
        }
        public bool HasAppId
        {
            get { return TargetApp != AppId_t.Invalid; }
        }

        #region Internals
        private bool processingCreateAndUpdate = false;
        private string processingChangeNote = "";
        /// <summary>
        /// WARNING used for internal and debug only
        /// </summary>
        public UGCUpdateHandle_t updateHandle;
        #endregion

        public HeathenCreateUpdateWorkshopCommunityItem(AppId_t targetApp)
        {
            TargetApp = targetApp;
            m_SubmitItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create(HandleItemUpdated);
            m_CreatedItem = CallResult<CreateItemResult_t>.Create(HandleItemCreate);
            m_RemoteStorageDownloadUGCResult = CallResult<RemoteStorageDownloadUGCResult_t>.Create(HandleUGCDownload);
        }

        public HeathenCreateUpdateWorkshopCommunityItem(SteamUGCDetails_t itemDetails)
        {
            m_SubmitItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create(HandleItemUpdated);
            m_CreatedItem = CallResult<CreateItemResult_t>.Create(HandleItemCreate);
            m_RemoteStorageDownloadUGCResult = CallResult<RemoteStorageDownloadUGCResult_t>.Create(HandleUGCDownload);

            TargetApp = itemDetails.m_nConsumerAppID;
            FileId = itemDetails.m_nPublishedFileId;
            Title = itemDetails.m_rgchTitle;
            Description = itemDetails.m_rgchDescription;
            Visibility = itemDetails.m_eVisibility;
            Author = SteamworksFoundationManager._GetUserData(itemDetails.m_ulSteamIDOwner);
            var previewCall = SteamRemoteStorage.UGCDownload(itemDetails.m_hPreviewFile, 1);
            m_RemoteStorageDownloadUGCResult.Set(previewCall, HandleUGCDownload);            
        }

        /// <summary>
        /// Creates a new file with the currently applied information
        /// </summary>
        /// <param name="changeNote"></param>
        /// <returns>Returns instantly, true indicates request submited, false indicates an error in create process, note that the file is created empty and update will happen asynchroniously</returns>
        public bool CreateAndUpdate(string changeNote)
        {
            if (TargetApp == AppId_t.Invalid)
            {
                Debug.LogError("HeathenWorkshopItem|CreateAndUpdate ... Create operation aborted, the current AppId is invalid.");
                return false;
            }

            if (string.IsNullOrEmpty(Title))
            {
                Debug.LogError("HeathenWorkshopItem|CreateAndUpdate ... operation aborted, Title is null or empty and must have a value.");
                return false;
            }

            if(string.IsNullOrEmpty(ContentLocation))
            {
                Debug.LogError("HeathenWorkshopItem|CreateAndUpdate ... operation aborted, Content location is null or empty and must have a value.");
                return false;
            }

            if (string.IsNullOrEmpty(PreviewImageLocation))
            {
                Debug.LogError("HeathenWorkshopItem|CreateAndUpdate ... operation aborted, Preview image location is null or empty and must have a value.");
                return false;
            }

            processingChangeNote = changeNote;
            processingCreateAndUpdate = true;

            var call = SteamUGC.CreateItem(TargetApp, FileType);
            m_CreatedItem.Set(call, HandleItemCreate);

            return true;
        }
        
        /// <summary>
        /// Starts and Item Update process updating all standard fields of the item
        /// </summary>
        /// <param name="changeNote">The change note to be applied to the update entry</param>
        /// <returns>True if update was submitted without error, false otherwise</returns>
        public bool Update(string changeNote)
        {
            if(TargetApp == AppId_t.Invalid)
            {
                Debug.LogError("HeathenWorkshopItem|Update ... Update operation aborted, the current AppId is invalid.");
                return false;
            }

            if (FileId == PublishedFileId_t.Invalid)
            {
                Debug.LogError("HeathenWorkshopItem|Update ... Update operation aborted, the current FileId is invalid.");
                return false;
            }

            if (!Directory.Exists(ContentLocation))
            {
                Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item content location, [" + ContentLocation + "] does not exist, this must be a valid folder path.");
                return false;
            }

            if (!File.Exists(PreviewImageLocation))
            {
                Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item preview, [" + PreviewImageLocation + "] does not exist, this must be a valid file path.");
                return false;
            }

            updateHandle = SteamUGC.StartItemUpdate(TargetApp, FileId);

            if(!SteamUGC.SetItemTitle(updateHandle, Title))
            {
                Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item title, item has not been updated.");
                return false;
            }

            if (!SteamUGC.SetItemDescription(updateHandle, Description))
            {
                Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item description, item has not been updated.");
                return false;
            }

            if (!SteamUGC.SetItemVisibility(updateHandle, Visibility))
            {
                Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item visibility, item has not been updated.");
                return false;
            }

            if (!SteamUGC.SetItemTags(updateHandle, Tags))
            {
                Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item tags, item has not been updated.");
                return false;
            }

            if (!SteamUGC.SetItemContent(updateHandle, ContentLocation))
            {
                Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item content location, item has not been updated.");
                return false;
            }

            if (!SteamUGC.SetItemPreview(updateHandle, PreviewImageLocation))
            {
                Debug.LogError("HeathenWorkshopItem|Update ... Failed to update item preview, item has not been updated.");
                return false;
            }

            var call = SteamUGC.SubmitItemUpdate(updateHandle, changeNote);            
            m_SubmitItemUpdateResult.Set(call, HandleItemUpdated);

            return true;
        }

        /// <summary>
        /// Fetch the status of the current update operation
        /// Note that this depends on a valid updateHandle
        /// </summary>
        /// <param name="bytesProcessed"></param>
        /// <param name="bytesTotal"></param>
        /// <returns></returns>
        public EItemUpdateStatus GetItemUpdateProgress(out ulong bytesProcessed, out ulong bytesTotal)
        {
            if (updateHandle != UGCUpdateHandle_t.Invalid)
            {
                return SteamUGC.GetItemUpdateProgress(updateHandle, out bytesProcessed, out bytesTotal);
            }
            else
            {
                bytesProcessed = 0;
                bytesTotal = 0;
                return EItemUpdateStatus.k_EItemUpdateStatusInvalid;
            }
        }

        #region Handlers
        private void HandleItemUpdated(SubmitItemUpdateResult_t param, bool bIOFailure)
        {
            if (bIOFailure)
                UpdateFailed.Invoke(param);
            else
                Updated.Invoke(param);
        }

        private void HandleItemCreate(CreateItemResult_t param, bool bIOFailure)
        {
            if (bIOFailure)
                CreateFailed.Invoke(param);
            else
            {
                Author = SteamworksFoundationManager._GetUserData(SteamUser.GetSteamID());
                FileId = param.m_nPublishedFileId;
                Created.Invoke(param);
            }

            if (processingCreateAndUpdate)
            {
                processingCreateAndUpdate = false;
                Update(processingChangeNote);
                processingChangeNote = string.Empty;
            }
        }

        private void HandleUGCDownload(RemoteStorageDownloadUGCResult_t param, bool bIOFailure)
        {
            //TODO: we shoudl probably setup a unique handler for each type of file .. at the moment we are assuming we only ever load the preview image
            PreviewImageLocation = param.m_pchFileName;
            Texture2D image;
            if (HelperTools.LoadImageFromDisk(param.m_pchFileName, out image))
            {
                previewImage = image;
            }
            else
            {
                Debug.LogError("Failed to load preview image (" + param.m_pchFileName + ") from disk!");
            }
        }
        #endregion


    }
}
#endif