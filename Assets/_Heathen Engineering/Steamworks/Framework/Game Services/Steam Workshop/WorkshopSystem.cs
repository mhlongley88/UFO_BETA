#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HeathenEngineering.SteamTools
{
    [CreateAssetMenu(menuName = "Steamworks/Workshop System")]
    public class WorkshopSystem : ScriptableObject
    {
        public CallResult<AddAppDependencyResult_t> m_AddAppDependencyResults;
        public CallResult<AddUGCDependencyResult_t> m_AddUGCDependencyResults;
        public CallResult<UserFavoriteItemsListChanged_t> m_UserFavoriteItemsListChanged;
        public CallResult<CreateItemResult_t> m_CreatedItem;
        public CallResult<DeleteItemResult_t> m_DeleteItem;
        public Callback<DownloadItemResult_t> m_DownloadItem;
        public CallResult<GetAppDependenciesResult_t> m_AppDependenciesResult;
        public CallResult<GetUserItemVoteResult_t> m_GetUserItemVoteResult;
        public CallResult<RemoveAppDependencyResult_t> m_RemoveAppDependencyResult;
        public CallResult<RemoveUGCDependencyResult_t> m_RemoveDependencyResult;
        public CallResult<SteamUGCRequestUGCDetailsResult_t> m_SteamUGCRequestUGCDetailsResult;
        public CallResult<SteamUGCQueryCompleted_t> m_SteamUGCQueryCompleted;
        public CallResult<SetUserItemVoteResult_t> m_SetUserItemVoteResult;
        public CallResult<StartPlaytimeTrackingResult_t> m_StartPlaytimeTrackingResult;
        public CallResult<StopPlaytimeTrackingResult_t> m_StopPlaytimeTrackingResult;
        public CallResult<SubmitItemUpdateResult_t> m_SubmitItemUpdateResult;
        public CallResult<RemoteStorageSubscribePublishedFileResult_t> m_RemoteStorageSubscribePublishedFileResult;
        public CallResult<RemoteStorageUnsubscribePublishedFileResult_t> m_RemoteStorageUnsubscribePublishedFileResult;

        #region Events
        /// <summary>
        /// Occures when a UGC item is downloaded
        /// </summary>
        [HideInInspector]
        public UnityWorkshopDownloadedItemResultEvent OnWorkshopItemDownloaded;
        /// <summary>
        /// Occures when a UGC item is created
        /// </summary>
        [HideInInspector]
        public UnityWorkshopItemCreatedEvent OnWorkshopItemCreated;
        /// <summary>
        /// Occures when a UGC item create is requested but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopItemCreatedEvent OnWorkshopItemCreateFailed;
        /// <summary>
        /// Occures when a UGC item is deleted
        /// </summary>
        [HideInInspector]
        public UnityWorkshopItemDeletedEvent OnWorkshopItemDeleted;
        /// <summary>
        /// Occures when a UGC item delete is requested but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopItemDeletedEvent OnWorkshopItemDeleteFailed;
        /// <summary>
        /// Occures when a UGC item favorite flag is changed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopFavoriteItemsListChangedEvent OnWorkshopFavoriteItemsChanged;
        /// <summary>
        /// Occures when a UGC item favorite flag change is requested but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopFavoriteItemsListChangedEvent OnWorkshopFavoriteItemsChangeFailed;
        /// <summary>
        /// Occures when a UGC item add app dependency is called
        /// </summary>
        [HideInInspector]
        public UnityWorkshopAddAppDependencyResultEvent OnWorkshopAddedAppDependency;
        /// <summary>
        /// Occures when a UGC item add app dependency is called but fails
        /// </summary>
        [HideInInspector]
        public UnityWorkshopAddAppDependencyResultEvent OnWorkshopAddAppDependencyFailed;
        /// <summary>
        /// Occures when a UGC item add dependency is called
        /// </summary>
        [HideInInspector]
        public UnityWorkshopAddDependencyResultEvent OnWorkshopAddDependency;
        /// <summary>
        /// Occures when a UGC item add dependency is called but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopAddDependencyResultEvent OnWorkshopAddDependencyFailed;
        /// <summary>
        /// Occures when a UGC item app dependency result is called and recieved
        /// </summary>
        [HideInInspector]
        public UnityWorkshopGetAppDependenciesResultEvent OnWorkshopAppDependenciesResults;
        /// <summary>
        /// Occures when a UGC item app dependency result is called but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopGetAppDependenciesResultEvent OnWorkshopAppDependenciesResultsFailed;
        /// <summary>
        /// Occures when a UGC item vote called and succesful
        /// </summary>
        [HideInInspector]
        public UnityWorkshopGetUserItemVoteResultEvent OnWorkshopUserItemVoteResults;
        /// <summary>
        /// Occures when a UGC item vote called but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopGetUserItemVoteResultEvent OnWorkshopUserItemVoteResultsFailed;
        /// <summary>
        /// Occures when a UGC item remove app dependency is called
        /// </summary>
        [HideInInspector]
        public UnityWorkshopRemoveAppDependencyResultEvent OnWorkshopRemoveAppDependencyResults;
        /// <summary>
        /// Occures when a UGC item remove app dependency is called but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopRemoveAppDependencyResultEvent OnWorkshopRemoveAppDependencyResultsFailed;
        /// <summary>
        /// Occures when a UGC item remove dependency is called
        /// </summary>
        [HideInInspector]
        public UnityWorkshopRemoveUGCDependencyResultEvent OnWorkshopRemoveDependencyResults;
        /// <summary>
        /// Occures when a UGC item remove dependency is called but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopRemoveUGCDependencyResultEvent OnWorkshopRemoveDependencyResultsFailed;
        /// <summary>
        /// Occures when a UGC item details request is called
        /// </summary>
        [HideInInspector]
        public UnityWorkshopSteamUGCRequestUGCDetailsResultEvent OnWorkshopRequestDetailsResults;
        /// <summary>
        /// Occures when a UGC item details is requested but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopSteamUGCRequestUGCDetailsResultEvent OnWorkshopRequestDetailsResultsFailed;
        /// <summary>
        /// Occures when a UGC item query / search is completed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopSteamUGCQueryCompletedEvent OnWorkshopQueryCompelted;
        /// <summary>
        /// Occures when a UGC item query / search is called but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopSteamUGCQueryCompletedEvent OnWorkshopQueryCompeltedFailed;
        /// <summary>
        /// Occures when a UGC item set vote is called and returned
        /// </summary>
        [HideInInspector]
        public UnityWorkshopSetUserItemVoteResultEvent OnWorkshopSetUserItemVoteResult;
        /// <summary>
        /// Occures when a UGC item set vote is called but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopSetUserItemVoteResultEvent OnWorkshopSetUserItemVoteResultFailed;
        /// <summary>
        /// Occures when a UGC item start playtime tracking is called and returned
        /// </summary>
        [HideInInspector]
        public UnityWorkshopStartPlaytimeTrackingResultEvent OnWorkshopStartPlaytimeTrackingResult;
        /// <summary>
        /// Occures when a UGC item start playtime tracking is called but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopStartPlaytimeTrackingResultEvent OnWorkshopStartPlaytimeTrackingResultFailed;
        /// <summary>
        /// Occures when a UGC item stop playtime tracking is called and returned
        /// </summary>
        [HideInInspector]
        public UnityWorkshopStopPlaytimeTrackingResultEvent OnWorkshopStopPlaytimeTrackingResult;
        /// <summary>
        /// Occures when a UGC item stop playtime tracking is called but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopStopPlaytimeTrackingResultEvent OnWorkshopStopPlaytimeTrackingResultFailed;
        /// <summary>
        /// Occures when a UGC item submit item update is called and returned
        /// </summary>
        [HideInInspector]
        public UnityWorkshopSubmitItemUpdateResultEvent OnWorkshopSubmitItemUpdateResult;
        /// <summary>
        /// Occures when a UGC item submit item update is called but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopSubmitItemUpdateResultEvent OnWorkshopSubmitItemUpdateResultFailed;
        /// <summary>
        /// Occures when a UGC subscribe is called and returned
        /// </summary>
        [HideInInspector]
        public UnityWorkshopRemoteStorageSubscribePublishedFileResultEvent OnWorkshopRemoteStorageSubscribeFileResult;
        /// <summary>
        /// Occures when a UGC subscribe is called but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopRemoteStorageSubscribePublishedFileResultEvent OnWorkshopRemoteStorageSubscribeFileResultFailed;
        /// <summary>
        /// Occures when a UGC unsubscribe is called and returned
        /// </summary>
        [HideInInspector]
        public UnityWorkshopRemoteStorageUnsubscribePublishedFileResultEvent OnWorkshopRemoteStorageUnsubscribeFileResult;
        /// <summary>
        /// Occures when a UGC unsubscribe is called but failed
        /// </summary>
        [HideInInspector]
        public UnityWorkshopRemoteStorageUnsubscribePublishedFileResultEvent OnWorkshopRemoteStorageUnsubscribeFileResultFailed;
        #endregion

        #region Workshop System
        private bool UGCReg = false;

        /// <summary>
        /// Registeres the callbacks for the UGC aka Workshop system
        /// Note this is called by the HeathenWorkshopBrowser
        /// </summary>
        public void RegisterWorkshopSystem()
        {
            if (UGCReg)
                return;

            UGCReg = true;
            m_AddAppDependencyResults = CallResult<AddAppDependencyResult_t>.Create(HandleAddAppDependencyResult);
            m_AddUGCDependencyResults = CallResult<AddUGCDependencyResult_t>.Create(HandleAddUGCDependencyResult);
            m_UserFavoriteItemsListChanged = CallResult<UserFavoriteItemsListChanged_t>.Create(HandleUserFavoriteItemsListChanged);
            m_CreatedItem = CallResult<CreateItemResult_t>.Create(HandleCreatedItem);
            m_DeleteItem = CallResult<DeleteItemResult_t>.Create(HandleDeleteItem);
            m_DownloadItem = Callback<DownloadItemResult_t>.Create(HandleDownloadedItem);
            m_AppDependenciesResult = CallResult<GetAppDependenciesResult_t>.Create(HandleGetAppDependenciesResults);
            m_GetUserItemVoteResult = CallResult<GetUserItemVoteResult_t>.Create(HandleGetUserItemVoteResult);
            m_RemoveAppDependencyResult = CallResult<RemoveAppDependencyResult_t>.Create(HandleRemoveAppDependencyResult);
            m_RemoveDependencyResult = CallResult<RemoveUGCDependencyResult_t>.Create(HandleRemoveDependencyResult);
            m_SteamUGCRequestUGCDetailsResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(HandleRequestDetailsResult);
            m_SteamUGCQueryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(HandleQueryCompleted);
            m_SetUserItemVoteResult = CallResult<SetUserItemVoteResult_t>.Create(HandleSetUserItemVoteResult);
            m_StartPlaytimeTrackingResult = CallResult<StartPlaytimeTrackingResult_t>.Create(HandleStartPlaytimeTracking);
            m_StopPlaytimeTrackingResult = CallResult<StopPlaytimeTrackingResult_t>.Create(HandleStopPlaytimeTracking);
            m_SubmitItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create(HandleItemUpdateResult);
            m_RemoteStorageSubscribePublishedFileResult = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create(HandleSubscribeFileResult);
            m_RemoteStorageUnsubscribePublishedFileResult = CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.Create(HandleUnsubscribeFileResult);

            OnWorkshopItemDownloaded = new UnityWorkshopDownloadedItemResultEvent();
            OnWorkshopItemCreated = new UnityWorkshopItemCreatedEvent();
            OnWorkshopItemCreateFailed = new UnityWorkshopItemCreatedEvent();
            OnWorkshopItemDeleted = new UnityWorkshopItemDeletedEvent();
            OnWorkshopItemDeleteFailed = new UnityWorkshopItemDeletedEvent();
            OnWorkshopFavoriteItemsChanged = new UnityWorkshopFavoriteItemsListChangedEvent();
            OnWorkshopFavoriteItemsChangeFailed = new UnityWorkshopFavoriteItemsListChangedEvent();
            OnWorkshopAddedAppDependency = new UnityWorkshopAddAppDependencyResultEvent();
            OnWorkshopAddAppDependencyFailed = new UnityWorkshopAddAppDependencyResultEvent();
            OnWorkshopAddDependency = new UnityWorkshopAddDependencyResultEvent();
            OnWorkshopAddDependencyFailed = new UnityWorkshopAddDependencyResultEvent();
            OnWorkshopAppDependenciesResults = new UnityWorkshopGetAppDependenciesResultEvent();
            OnWorkshopAppDependenciesResultsFailed = new UnityWorkshopGetAppDependenciesResultEvent();
            OnWorkshopUserItemVoteResults = new UnityWorkshopGetUserItemVoteResultEvent();
            OnWorkshopUserItemVoteResultsFailed = new UnityWorkshopGetUserItemVoteResultEvent();
            OnWorkshopRemoveAppDependencyResults = new UnityWorkshopRemoveAppDependencyResultEvent();
            OnWorkshopRemoveAppDependencyResultsFailed = new UnityWorkshopRemoveAppDependencyResultEvent();
            OnWorkshopRemoveDependencyResults = new UnityWorkshopRemoveUGCDependencyResultEvent();
            OnWorkshopRemoveDependencyResultsFailed = new UnityWorkshopRemoveUGCDependencyResultEvent();
            OnWorkshopRequestDetailsResults = new UnityWorkshopSteamUGCRequestUGCDetailsResultEvent();
            OnWorkshopRequestDetailsResultsFailed = new UnityWorkshopSteamUGCRequestUGCDetailsResultEvent();
            OnWorkshopQueryCompelted = new UnityWorkshopSteamUGCQueryCompletedEvent();
            OnWorkshopQueryCompeltedFailed = new UnityWorkshopSteamUGCQueryCompletedEvent();
            OnWorkshopSetUserItemVoteResult = new UnityWorkshopSetUserItemVoteResultEvent();
            OnWorkshopSetUserItemVoteResultFailed = new UnityWorkshopSetUserItemVoteResultEvent();
            OnWorkshopStartPlaytimeTrackingResult = new UnityWorkshopStartPlaytimeTrackingResultEvent();
            OnWorkshopStartPlaytimeTrackingResultFailed = new UnityWorkshopStartPlaytimeTrackingResultEvent();
            OnWorkshopStopPlaytimeTrackingResult = new UnityWorkshopStopPlaytimeTrackingResultEvent();
            OnWorkshopStopPlaytimeTrackingResultFailed = new UnityWorkshopStopPlaytimeTrackingResultEvent();
            OnWorkshopSubmitItemUpdateResult = new UnityWorkshopSubmitItemUpdateResultEvent();
            OnWorkshopSubmitItemUpdateResultFailed = new UnityWorkshopSubmitItemUpdateResultEvent();
            OnWorkshopRemoteStorageSubscribeFileResult = new UnityWorkshopRemoteStorageSubscribePublishedFileResultEvent();
            OnWorkshopRemoteStorageSubscribeFileResultFailed = new UnityWorkshopRemoteStorageSubscribePublishedFileResultEvent();
            OnWorkshopRemoteStorageUnsubscribeFileResult = new UnityWorkshopRemoteStorageUnsubscribePublishedFileResultEvent();
            OnWorkshopRemoteStorageUnsubscribeFileResultFailed = new UnityWorkshopRemoteStorageUnsubscribePublishedFileResultEvent();
        }

        /// <summary>
        /// Adds a dependency between the given item and the appid. This list of dependencies can be retrieved by calling GetAppDependencies. This is a soft-dependency that is displayed on the web. It is up to the application to determine whether the item can actually be used or not.
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="appId"></param>
        public void WorkshopAddAppDependency(PublishedFileId_t fileId, AppId_t appId)
        {
            var call = SteamUGC.AddAppDependency(fileId, appId);
            m_AddAppDependencyResults.Set(call, HandleAddAppDependencyResult);
        }

        /// <summary>
        /// Adds a workshop item as a dependency to the specified item. If the nParentPublishedFileID item is of type k_EWorkshopFileTypeCollection, than the nChildPublishedFileID is simply added to that collection. Otherwise, the dependency is a soft one that is displayed on the web and can be retrieved via the ISteamUGC API using a combination of the m_unNumChildren member variable of the SteamUGCDetails_t struct and GetQueryUGCChildren.
        /// </summary>
        /// <param name="parentFileId"></param>
        /// <param name="childFileId"></param>
        public void WorkshopAddDependency(PublishedFileId_t parentFileId, PublishedFileId_t childFileId)
        {
            var call = SteamUGC.AddDependency(parentFileId, childFileId);
            m_AddUGCDependencyResults.Set(call, HandleAddUGCDependencyResult);
        }

        /// <summary>
        /// Adds a excluded tag to a pending UGC Query. This will only return UGC without the specified tag.
        /// </summary>
        /// <param name="handle">The UGC query handle to customize.</param>
        /// <param name="tagName">The tag that must NOT be attached to the UGC to receive it.</param>
        /// <returns>true upon success. false if the UGC query handle is invalid, if the UGC query handle is from CreateQueryUGCDetailsRequest, or tagName was NULL.</returns>
        /// <remarks>This must be set before you send a UGC Query handle using SendQueryUGCRequest.</remarks>
        public bool WorkshopAddExcludedTag(UGCQueryHandle_t handle, string tagName)
        {
            return SteamUGC.AddExcludedTag(handle, tagName);
        }

        /// <summary>
        /// Adds a key-value tag pair to an item. Keys can map to multiple different values (1-to-many relationship).
        /// Key names are restricted to alpha-numeric characters and the '_' character.
        /// Both keys and values cannot exceed 255 characters in length.
        /// Key-value tags are searchable by exact match only.
        /// </summary>
        /// <param name="handle">The workshop item update handle to customize.</param>
        /// <param name="key">The key to set on the item.</param>
        /// <param name="value">A value to map to the key.</param>
        /// <returns></returns>
        public bool WorkshopAddItemKeyValueTag(UGCUpdateHandle_t handle, string key, string value)
        {
            return SteamUGC.AddItemKeyValueTag(handle, key, value);
        }

        /// <summary>
        /// Adds an additional preview file for the item.
        /// Then the format of the image should be one that both the web and the application(if necessary) can render, and must be under 1MB.Suggested formats include JPG, PNG and GIF.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="previewFile"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool WorkshopAddItemPreviewFile(UGCUpdateHandle_t handle, string previewFile, EItemPreviewType type)
        {
            return SteamUGC.AddItemPreviewFile(handle, previewFile, type);
        }

        /// <summary>
        /// Adds an additional video preview from YouTube for the item.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="videoId">The YouTube video ID ... e.g jHgZh4GV9G0</param>
        /// <returns></returns>
        public bool WorkshopAddItemPreviewVideo(UGCUpdateHandle_t handle, string videoId)
        {
            return SteamUGC.AddItemPreviewVideo(handle, videoId);
        }

        /// <summary>
        /// Adds workshop item to the users favorite list
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="fileId"></param>
        public void WorkshopAddItemToFavorites(AppId_t appId, PublishedFileId_t fileId)
        {
            var call = SteamUGC.AddItemToFavorites(appId, fileId);
            m_UserFavoriteItemsListChanged.Set(call, HandleUserFavoriteItemsListChanged);
        }

        /// <summary>
        /// Adds a required key-value tag to a pending UGC Query. This will only return workshop items that have a key = pKey and a value = pValue.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WorkshopAddRequiredKeyValueTag(UGCQueryHandle_t handle, string key, string value)
        {
            return SteamUGC.AddRequiredKeyValueTag(handle, key, value);
        }

        /// <summary>
        /// Adds a required tag to a pending UGC Query. This will only return UGC with the specified tag.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public bool WorkshopAddRequiredTag(UGCQueryHandle_t handle, string tagName)
        {
            return SteamUGC.AddRequiredTag(handle, tagName);
        }

        /// <summary>
        /// Creates an empty workshop Item
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="type"></param>
        public void WorkshopCreateItem(AppId_t appId, EWorkshopFileType type)
        {
            var call = SteamUGC.CreateItem(appId, type);
            m_CreatedItem.Set(call, HandleCreatedItem);
        }

        /// <summary>
        /// Query for all matching UGC. You can use this to list all of the available UGC for your app.
        /// You must release the handle returned by this function by calling WorkshopReleaseQueryRequest when you are done with it!
        /// </summary>
        /// <param name="queryType"></param>
        /// <param name="matchingFileType"></param>
        /// <param name="creatorAppId"></param>
        /// <param name="consumerAppId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public UGCQueryHandle_t WorkshopCreateQueryAllRequest(EUGCQuery queryType, EUGCMatchingUGCType matchingFileType, AppId_t creatorAppId, AppId_t consumerAppId, uint page)
        {
            return SteamUGC.CreateQueryAllUGCRequest(queryType, matchingFileType, creatorAppId, consumerAppId, page);
        }

        /// <summary>
        /// Query for the details of specific workshop items
        /// You must release the handle returned by this function by calling WorkshopReleaseQueryRequest when you are done with it!
        /// </summary>
        /// <param name="fileIds">The list of workshop items to get the details for.</param>
        /// <param name="count">The number of items in the list</param>
        /// <returns></returns>
        public UGCQueryHandle_t WorkshopCreateQueryDetailsRequest(PublishedFileId_t[] fileIds)
        {
            return SteamUGC.CreateQueryUGCDetailsRequest(fileIds, (uint)fileIds.GetLength(0));
        }

        /// <summary>
        /// Query for the details of specific workshop items
        /// You must release the handle returned by this function by calling WorkshopReleaseQueryRequest when you are done with it!
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        public UGCQueryHandle_t WorkshopCreateQueryDetailsRequest(List<PublishedFileId_t> fileIds)
        {
            //Specifc to a List this would be most common with Unity developers
            return SteamUGC.CreateQueryUGCDetailsRequest(fileIds.ToArray(), (uint)fileIds.Count);
        }

        /// <summary>
        /// Query for the details of specific workshop items
        /// You must release the handle returned by this function by calling WorkshopReleaseQueryRequest when you are done with it!
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        public UGCQueryHandle_t WorkshopCreateQueryDetailsRequest(IEnumerable<PublishedFileId_t> fileIds)
        {
            //If not an array and not a list but it is some collection of IEnumerable then create a list for it and use that ... not efficent but useful and should be used infrequently
            List<PublishedFileId_t> list = new List<PublishedFileId_t>(fileIds);
            return SteamUGC.CreateQueryUGCDetailsRequest(list.ToArray(), (uint)list.Count);
        }

        /// <summary>
        /// Query UGC associated with a user. You can use this to list the UGC the user is subscribed to amongst other things.
        /// You must release the handle returned by this function by calling WorkshopReleaseQueryRequest when you are done with it!
        /// </summary>
        /// <param name="accountId">The Account ID to query the UGC for. You can use CSteamID.GetAccountID to get the Account ID from a Steam ID.</param>
        /// <param name="listType">Used to specify the type of list to get.</param>
        /// <param name="matchingType">Used to specify the type of UGC queried for.</param>
        /// <param name="sortOrder">Used to specify the order that the list will be sorted in.</param>
        /// <param name="creatorAppId">This should contain the App ID of the app where the item was created. This may be different than nConsumerAppID if your item creation tool is a seperate App ID.</param>
        /// <param name="consumerAppId">This should contain the App ID for the current game or application. Do not pass the App ID of the workshop item creation tool if that is a separate App ID!</param>
        /// <param name="page">The page number of the results to receive. This should start at 1 on the first call.</param>
        /// <returns></returns>
        public UGCQueryHandle_t WorkshopCreateQueryUserRequest(AccountID_t accountId, EUserUGCList listType, EUGCMatchingUGCType matchingType, EUserUGCListSortOrder sortOrder, AppId_t creatorAppId, AppId_t consumerAppId, uint page)
        {
            return SteamUGC.CreateQueryUserUGCRequest(accountId, listType, matchingType, sortOrder, creatorAppId, consumerAppId, page);
        }

        /// <summary>
        /// Frees a UGC query
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool WorkshopReleaseQueryRequest(UGCQueryHandle_t handle)
        {
            return SteamUGC.ReleaseQueryUGCRequest(handle);
        }

        /// <summary>
        /// Requests delete of a UGC item
        /// </summary>
        /// <param name="fileId"></param>
        public void WorkshopDeleteItem(PublishedFileId_t fileId)
        {
            var call = SteamUGC.DeleteItem(fileId);
            m_DeleteItem.Set(call, HandleDeleteItem);
        }

        /// <summary>
        /// Request download of a UGC item
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="setHighPriority"></param>
        /// <returns></returns>
        public bool WorkshopDownloadItem(PublishedFileId_t fileId, bool setHighPriority)
        {
            return SteamUGC.DownloadItem(fileId, setHighPriority);
        }

        /// <summary>
        /// Request the app dependencies of a UGC item
        /// </summary>
        /// <param name="fileId"></param>
        public void WorkshopGetAppDependencies(PublishedFileId_t fileId)
        {
            var call = SteamUGC.GetAppDependencies(fileId);
            m_AppDependenciesResult.Set(call, HandleGetAppDependenciesResults);
        }

        /// <summary>
        /// Request the download informaiton of a UGC item
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="completion"></param>
        /// <returns></returns>
        public bool WorkshopGetItemDownloadInfo(PublishedFileId_t fileId, out float completion)
        {
            ulong current;
            ulong total;
            var result = SteamUGC.GetItemDownloadInfo(fileId, out current, out total);
            if (result)
                completion = Convert.ToSingle(current / (double)total);
            else
                completion = 0;
            return result;
        }

        /// <summary>
        /// Request the installation informaiton of a UGC item
        /// </summary>
        /// <param name="fileId">The item to check</param>
        /// <param name="sizeOnDisk">The size of the item on the disk</param>
        /// <param name="folderPath">The path of the item on the disk</param>
        /// <param name="folderSize">The size of folder path</param>
        /// <param name="timeStamp">The date time stamp of the item</param>
        /// <returns></returns>
        public bool WorkshopGetItemInstallInfo(PublishedFileId_t fileId, out ulong sizeOnDisk, out string folderPath, uint folderSize, out DateTime timeStamp)
        {
            uint iTimeStamp;
            var result = SteamUGC.GetItemInstallInfo(fileId, out sizeOnDisk, out folderPath, folderSize, out iTimeStamp);
            timeStamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            timeStamp = timeStamp.AddSeconds(iTimeStamp);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>Item State flags, use with WorkshopItemStateHasFlag and WorkshopItemStateHasAllFlags</returns>
        public EItemState WorkshopGetItemState(PublishedFileId_t fileId)
        {
            return (EItemState)SteamUGC.GetItemState(fileId);
        }

        /// <summary>
        /// Checks if the 'checkFlag' value is in the 'value'
        /// </summary>
        /// <param name="value"></param>
        /// <param name="checkflag"></param>
        /// <returns></returns>
        public bool WorkshopItemStateHasFlag(EItemState value, EItemState checkflag)
        {
            return (value & checkflag) == checkflag;
        }

        /// <summary>
        /// Cheks if any of the 'checkflags' values are in the 'value'
        /// </summary>
        /// <param name="value"></param>
        /// <param name="checkflags"></param>
        /// <returns></returns>
        public bool WorkshopItemStateHasAllFlags(EItemState value, params EItemState[] checkflags)
        {
            foreach (var checkflag in checkflags)
            {
                if ((value & checkflag) != checkflag)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Requests the progress of a UGC item update request
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="completion"></param>
        /// <returns></returns>
        public EItemUpdateStatus WorkshopGetItemUpdateProgress(UGCUpdateHandle_t handle, out float completion)
        {
            ulong current;
            ulong total;
            var result = SteamUGC.GetItemUpdateProgress(handle, out current, out total);
            if (result != EItemUpdateStatus.k_EItemUpdateStatusInvalid)
                completion = Convert.ToSingle(current / (double)total);
            else
                completion = 0;
            return result;
        }

        /// <summary>
        /// Returns the number of subscribed UGC items
        /// </summary>
        /// <returns></returns>
        public uint WorkshopGetNumSubscribedItems()
        {
            return SteamUGC.GetNumSubscribedItems();
        }

        /// <summary>
        /// Request an additional preview for a UGC item
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="previewIndex"></param>
        /// <param name="urlOrVideoId"></param>
        /// <param name="urlOrVideoSize"></param>
        /// <param name="fileName"></param>
        /// <param name="fileNameSize"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool WorkshopGetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string urlOrVideoId, uint urlOrVideoSize, string fileName, uint fileNameSize, out EItemPreviewType type)
        {
            return SteamUGC.GetQueryUGCAdditionalPreview(handle, index, previewIndex, out urlOrVideoId, urlOrVideoSize, out fileName, fileNameSize, out type);
        }

        /// <summary>
        /// Request the child items of a given UGC item
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="fileIds"></param>
        /// <param name="maxEntries"></param>
        /// <returns></returns>
        public bool WorkshopGetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] fileIds, uint maxEntries)
        {
            return SteamUGC.GetQueryUGCChildren(handle, index, fileIds, maxEntries);
        }

        /// <summary>
        /// Request the key value tag of a UGC item
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="keyValueTagIndex"></param>
        /// <param name="key"></param>
        /// <param name="keySize"></param>
        /// <param name="value"></param>
        /// <param name="valueSize"></param>
        /// <returns></returns>
        public bool WorkshopGeQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string key, uint keySize, string value, uint valueSize)
        {
            return SteamUGC.GetQueryUGCKeyValueTag(handle, index, keyValueTagIndex, out key, keySize, out value, valueSize);
        }

        /// <summary>
        /// Request the metadata of a UGC item
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="metadata"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool WorkshopGetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string metadata, uint size)
        {
            return SteamUGC.GetQueryUGCMetadata(handle, index, out metadata, size);
        }

        /// <summary>
        /// Request the number of previews of a UGC item
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public uint WorkshopGetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
        {
            return SteamUGC.GetQueryUGCNumAdditionalPreviews(handle, index);
        }

        /// <summary>
        /// Request the number of key value tags for a UGC item
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public uint WorkshopGetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
        {
            return SteamUGC.GetQueryUGCNumKeyValueTags(handle, index);
        }

        /// <summary>
        /// Get the preview URL of a UGC item
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="URL"></param>
        /// <param name="urlSize"></param>
        /// <returns></returns>
        public bool WorkshopGetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string URL, uint urlSize)
        {
            return SteamUGC.GetQueryUGCPreviewURL(handle, index, out URL, urlSize);
        }

        /// <summary>
        /// Fetch the results of a UGC query
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public bool WorkshopGetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t details)
        {
            return SteamUGC.GetQueryUGCResult(handle, index, out details);
        }

        /// <summary>
        /// Fetch the statistics of a UGC query
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="statType"></param>
        /// <param name="statValue"></param>
        /// <returns></returns>
        public bool WorkshopGetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic statType, out ulong statValue)
        {
            return SteamUGC.GetQueryUGCStatistic(handle, index, statType, out statValue);
        }

        /// <summary>
        /// Get the file IDs of all subscribed UGC items up to the array size
        /// </summary>
        /// <param name="fileIDs"></param>
        /// <param name="maxEntries"></param>
        /// <returns></returns>
        public uint WorkshopGetSubscribedItems(PublishedFileId_t[] fileIDs, uint maxEntries)
        {
            return SteamUGC.GetSubscribedItems(fileIDs, maxEntries);
        }

        /// <summary>
        /// Get the item vote value of a UGC item
        /// </summary>
        /// <param name="fileId"></param>
        public void WorkshopGetUserItemVote(PublishedFileId_t fileId)
        {
            var call = SteamUGC.GetUserItemVote(fileId);
            m_GetUserItemVoteResult.Set(call, HandleGetUserItemVoteResult);
        }

        /// <summary>
        /// Request the removal of app dependency from a UGC item
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="appId"></param>
        public void WorkshopRemoveAppDependency(PublishedFileId_t fileId, AppId_t appId)
        {
            var call = SteamUGC.RemoveAppDependency(fileId, appId);
            m_RemoveAppDependencyResult.Set(call, HandleRemoveAppDependencyResult);
        }

        /// <summary>
        /// Request the removal of a dependency from a UGC item
        /// </summary>
        /// <param name="parentFileId"></param>
        /// <param name="childFileId"></param>
        public void WorkshopRemoveDependency(PublishedFileId_t parentFileId, PublishedFileId_t childFileId)
        {
            var call = SteamUGC.RemoveDependency(parentFileId, childFileId);
            m_RemoveDependencyResult.Set(call, HandleRemoveDependencyResult);
        }

        /// <summary>
        /// Removes the UGC item from user favorites
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="fileId"></param>
        public void WorkshopRemoveItemFromFavorites(AppId_t appId, PublishedFileId_t fileId)
        {
            var call = SteamUGC.RemoveItemFromFavorites(appId, fileId);
            m_UserFavoriteItemsListChanged.Set(call, HandleUserFavoriteItemsListChanged);
        }

        /// <summary>
        /// Remove UGC item key value tags
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool WorkshopRemoveItemKeyValueTags(UGCUpdateHandle_t handle, string key)
        {
            return SteamUGC.RemoveItemKeyValueTags(handle, key);
        }

        /// <summary>
        /// Removes UGC item preview
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool WorkshopRemoveItemPreview(UGCUpdateHandle_t handle, uint index)
        {
            return SteamUGC.RemoveItemPreview(handle, index);
        }

        /// <summary>
        /// Requests details of a UGC item
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="maxAgeSeconds"></param>
        public void WorkshopRequestDetails(PublishedFileId_t fileId, uint maxAgeSeconds)
        {
            var call = SteamUGC.RequestUGCDetails(fileId, maxAgeSeconds);
            m_SteamUGCRequestUGCDetailsResult.Set(call, HandleRequestDetailsResult);
        }

        /// <summary>
        /// Sends a UGC query
        /// </summary>
        /// <param name="handle"></param>
        public void WorkshopSendQueryUGCRequest(UGCQueryHandle_t handle)
        {
            var call = SteamUGC.SendQueryUGCRequest(handle);
            m_SteamUGCQueryCompleted.Set(call, HandleQueryCompleted);
        }

        /// <summary>
        /// Set allow cached responce
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="maxAgeSeconds"></param>
        /// <returns></returns>
        public bool WorkshopSetAllowCachedResponse(UGCQueryHandle_t handle, uint maxAgeSeconds)
        {
            return SteamUGC.SetAllowCachedResponse(handle, maxAgeSeconds);
        }

        /// <summary>
        /// Set cloud file name filter
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool WorkshopSetCloudFileNameFilter(UGCQueryHandle_t handle, string fileName)
        {
            return SteamUGC.SetCloudFileNameFilter(handle, fileName);
        }

        /// <summary>
        /// Set item content path
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public bool WorkshopSetItemContent(UGCUpdateHandle_t handle, string folder)
        {
            return SteamUGC.SetItemContent(handle, folder);
        }

        /// <summary>
        /// Set item description
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool WorkshopSetItemDescription(UGCUpdateHandle_t handle, string description)
        {
            return SteamUGC.SetItemDescription(handle, description);
        }

        /// <summary>
        /// Set item metadata
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public bool WorkshopSetItemMetadata(UGCUpdateHandle_t handle, string metadata)
        {
            return SteamUGC.SetItemMetadata(handle, metadata);
        }

        /// <summary>
        /// Set item preview
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="previewFile"></param>
        /// <returns></returns>
        public bool WorkshopSetItemPreview(UGCUpdateHandle_t handle, string previewFile)
        {
            return SteamUGC.SetItemPreview(handle, previewFile);
        }

        /// <summary>
        /// Set item tags
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public bool WorkshopSetItemTags(UGCUpdateHandle_t handle, List<string> tags)
        {
            return SteamUGC.SetItemTags(handle, tags);
        }

        /// <summary>
        /// Set item title
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool WorkshopSetItemTitle(UGCUpdateHandle_t handle, string title)
        {
            return SteamUGC.SetItemTitle(handle, title);
        }

        /// <summary>
        /// Set item update language
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public bool WorkshopSetItemUpdateLanguage(UGCUpdateHandle_t handle, string language)
        {
            return SteamUGC.SetItemUpdateLanguage(handle, language);
        }

        /// <summary>
        /// Set item visibility
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="visibility"></param>
        /// <returns></returns>
        public bool WorkshopSetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility visibility)
        {
            return SteamUGC.SetItemVisibility(handle, visibility);
        }

        /// <summary>
        /// Set item langauge
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public bool WorkshopSetLanguage(UGCQueryHandle_t handle, string language)
        {
            return SteamUGC.SetLanguage(handle, language);
        }

        /// <summary>
        /// Set match any tag
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="anyTag"></param>
        /// <returns></returns>
        public bool WorkshopSetMatchAnyTag(UGCQueryHandle_t handle, bool anyTag)
        {
            return SteamUGC.SetMatchAnyTag(handle, anyTag);
        }

        /// <summary>
        /// Set ranked by trend days
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public bool WorkshopSetRankedByTrendDays(UGCQueryHandle_t handle, uint days)
        {
            return SteamUGC.SetRankedByTrendDays(handle, days);
        }

        /// <summary>
        /// Set return additional previews
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="additionalPreviews"></param>
        /// <returns></returns>
        public bool WorkshopSetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool additionalPreviews)
        {
            return SteamUGC.SetReturnAdditionalPreviews(handle, additionalPreviews);
        }

        /// <summary>
        /// Set return childre
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="returnChildren"></param>
        /// <returns></returns>
        public bool WorkshopSetReturnChildren(UGCQueryHandle_t handle, bool returnChildren)
        {
            return SteamUGC.SetReturnChildren(handle, returnChildren);
        }

        /// <summary>
        /// Set return key value tags
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public bool WorkshopSetReturnKeyValueTags(UGCQueryHandle_t handle, bool tags)
        {
            return SteamUGC.SetReturnKeyValueTags(handle, tags);
        }

        /// <summary>
        /// SEt return long description
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="longDescription"></param>
        /// <returns></returns>
        public bool WorkshopSetReturnLongDescription(UGCQueryHandle_t handle, bool longDescription)
        {
            return SteamUGC.SetReturnLongDescription(handle, longDescription);
        }

        /// <summary>
        /// Set return metadata
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public bool WorkshopSetReturnMetadata(UGCQueryHandle_t handle, bool metadata)
        {
            return SteamUGC.SetReturnMetadata(handle, metadata);
        }

        /// <summary>
        /// Set return IDs only
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="onlyIds"></param>
        /// <returns></returns>
        public bool WorkshopSetReturnOnlyIDs(UGCQueryHandle_t handle, bool onlyIds)
        {
            return SteamUGC.SetReturnOnlyIDs(handle, onlyIds);
        }

        /// <summary>
        /// Set return playtime stats
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public bool WorkshopSetReturnPlaytimeStats(UGCQueryHandle_t handle, uint days)
        {
            return SteamUGC.SetReturnPlaytimeStats(handle, days);
        }

        /// <summary>
        /// Set return total only
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="totalOnly"></param>
        /// <returns></returns>
        public bool WorkshopSetReturnTotalOnly(UGCQueryHandle_t handle, bool totalOnly)
        {
            return SteamUGC.SetReturnTotalOnly(handle, totalOnly);
        }

        /// <summary>
        /// Set search text
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool WorkshopSetSearchText(UGCQueryHandle_t handle, string text)
        {
            return SteamUGC.SetSearchText(handle, text);
        }

        /// <summary>
        /// Set user item vote
        /// </summary>
        /// <param name="fileID"></param>
        /// <param name="voteUp"></param>
        public void WorkshopSetUserItemVote(PublishedFileId_t fileID, bool voteUp)
        {
            var call = SteamUGC.SetUserItemVote(fileID, voteUp);
            m_SetUserItemVoteResult.Set(call, HandleSetUserItemVoteResult);
        }

        /// <summary>
        /// Start item update
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="fileID"></param>
        /// <returns></returns>
        public UGCUpdateHandle_t WorkshopStartItemUpdate(AppId_t appId, PublishedFileId_t fileID)
        {
            return SteamUGC.StartItemUpdate(appId, fileID);
        }

        /// <summary>
        /// Start playtime tracking
        /// </summary>
        /// <param name="fileIds"></param>
        /// <param name="count"></param>
        public void WorkshopStartPlaytimeTracking(PublishedFileId_t[] fileIds, uint count)
        {
            var call = SteamUGC.StartPlaytimeTracking(fileIds, count);
            m_StartPlaytimeTrackingResult.Set(call, HandleStartPlaytimeTracking);
        }

        /// <summary>
        /// Stop playtime tracking
        /// </summary>
        /// <param name="fileIds"></param>
        /// <param name="count"></param>
        public void WorkshopStopPlaytimeTracking(PublishedFileId_t[] fileIds, uint count)
        {
            var call = SteamUGC.StopPlaytimeTracking(fileIds, count);
            m_StopPlaytimeTrackingResult.Set(call, HandleStopPlaytimeTracking);
        }

        /// <summary>
        /// stop playtime tracking for all items
        /// </summary>
        public void WorkshopStopPlaytimeTrackingForAllItems()
        {
            var call = SteamUGC.StopPlaytimeTrackingForAllItems();
            m_StopPlaytimeTrackingResult.Set(call, HandleStopPlaytimeTracking);
        }

        /// <summary>
        /// Submit item update
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="changeNote"></param>
        public void WorkshopSubmitItemUpdate(UGCUpdateHandle_t handle, string changeNote)
        {
            var call = SteamUGC.SubmitItemUpdate(handle, changeNote);
            m_SubmitItemUpdateResult.Set(call, HandleItemUpdateResult);
        }

        /// <summary>
        /// Subscribe to item
        /// </summary>
        /// <param name="fileId"></param>
        public void WorkshopSubscribeItem(PublishedFileId_t fileId)
        {
            var call = SteamUGC.SubscribeItem(fileId);
            m_RemoteStorageSubscribePublishedFileResult.Set(call, HandleSubscribeFileResult);
        }

        /// <summary>
        /// Suspend downloads
        /// </summary>
        /// <param name="suspend"></param>
        public void WorkshopSuspendDownloads(bool suspend)
        {
            SteamUGC.SuspendDownloads(suspend);
        }

        /// <summary>
        /// Unsubscribe to item
        /// </summary>
        /// <param name="fileId"></param>
        public void WorkshopUnsubscribeItem(PublishedFileId_t fileId)
        {
            var call = SteamUGC.UnsubscribeItem(fileId);
            m_RemoteStorageUnsubscribePublishedFileResult.Set(call, HandleUnsubscribeFileResult);
        }

        /// <summary>
        /// Update item preview file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool WorkshopUpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string file)
        {
            return SteamUGC.UpdateItemPreviewFile(handle, index, file);
        }

        /// <summary>
        /// Update item preview video
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public bool WorkshopUpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string videoId)
        {
            return SteamUGC.UpdateItemPreviewVideo(handle, index, videoId);
        }

        #region callbacks
        private void HandleAddUGCDependencyResult(AddUGCDependencyResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopAddDependency.Invoke(param);
            else
                OnWorkshopAddDependencyFailed.Invoke(param);
        }

        private void HandleAddAppDependencyResult(AddAppDependencyResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopAddedAppDependency.Invoke(param);
            else
                OnWorkshopAddAppDependencyFailed.Invoke(param);
        }

        private void HandleUserFavoriteItemsListChanged(UserFavoriteItemsListChanged_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopFavoriteItemsChanged.Invoke(param);
            else
                OnWorkshopFavoriteItemsChangeFailed.Invoke(param);
        }

        private void HandleCreatedItem(CreateItemResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopItemCreated.Invoke(param);
            else
                OnWorkshopItemCreateFailed.Invoke(param);
        }

        private void HandleDeleteItem(DeleteItemResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopItemDeleted.Invoke(param);
            else
                OnWorkshopItemDeleteFailed.Invoke(param);
        }

        private void HandleDownloadedItem(DownloadItemResult_t param)
        {
            OnWorkshopItemDownloaded.Invoke(param);
        }

        private void HandleGetAppDependenciesResults(GetAppDependenciesResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopAppDependenciesResults.Invoke(param);
            else
                OnWorkshopAppDependenciesResultsFailed.Invoke(param);
        }

        private void HandleGetUserItemVoteResult(GetUserItemVoteResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopUserItemVoteResults.Invoke(param);
            else
                OnWorkshopUserItemVoteResultsFailed.Invoke(param);
        }

        private void HandleRemoveAppDependencyResult(RemoveAppDependencyResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopRemoveAppDependencyResults.Invoke(param);
            else
                OnWorkshopRemoveAppDependencyResultsFailed.Invoke(param);
        }

        private void HandleRemoveDependencyResult(RemoveUGCDependencyResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopRemoveDependencyResults.Invoke(param);
            else
                OnWorkshopRemoveDependencyResultsFailed.Invoke(param);
        }

        private void HandleRequestDetailsResult(SteamUGCRequestUGCDetailsResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopRequestDetailsResults.Invoke(param);
            else
                OnWorkshopRequestDetailsResultsFailed.Invoke(param);
        }

        private void HandleQueryCompleted(SteamUGCQueryCompleted_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopQueryCompelted.Invoke(param);
            else
                OnWorkshopQueryCompeltedFailed.Invoke(param);
        }

        private void HandleSetUserItemVoteResult(SetUserItemVoteResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopSetUserItemVoteResult.Invoke(param);
            else
                OnWorkshopSetUserItemVoteResultFailed.Invoke(param);
        }

        private void HandleStartPlaytimeTracking(StartPlaytimeTrackingResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopStartPlaytimeTrackingResult.Invoke(param);
            else
                OnWorkshopStartPlaytimeTrackingResultFailed.Invoke(param);
        }

        private void HandleStopPlaytimeTracking(StopPlaytimeTrackingResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopStopPlaytimeTrackingResult.Invoke(param);
            else
                OnWorkshopStopPlaytimeTrackingResultFailed.Invoke(param);
        }

        private void HandleUnsubscribeFileResult(RemoteStorageUnsubscribePublishedFileResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopRemoteStorageUnsubscribeFileResult.Invoke(param);
            else
                OnWorkshopRemoteStorageUnsubscribeFileResultFailed.Invoke(param);
        }

        private void HandleSubscribeFileResult(RemoteStorageSubscribePublishedFileResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopRemoteStorageSubscribeFileResult.Invoke(param);
            else
                OnWorkshopRemoteStorageSubscribeFileResultFailed.Invoke(param);
        }

        private void HandleItemUpdateResult(SubmitItemUpdateResult_t param, bool bIOFailure)
        {
            if (!bIOFailure)
                OnWorkshopSubmitItemUpdateResult.Invoke(param);
            else
                OnWorkshopSubmitItemUpdateResultFailed.Invoke(param);
        }
        #endregion

        #endregion
    }
}
#endif