﻿using UnityEngine;
using System.Collections;
using System.ComponentModel;
using Steamworks;
using System.Linq;

public class SteamGameAchievements : MonoBehaviour
{
	public static SteamGameAchievements instance;

	public enum Achievement : int
	{
		UFO_ACHIEVEMENT_1_0,
		UFO_ACHIEVEMENT_1_1,
		UFO_ACHIEVEMENT_1_2,
		UFO_ACHIEVEMENT_1_3,
		UFO_ACHIEVEMENT_1_4,
		UFO_ACHIEVEMENT_1_5,
		UFO_ACHIEVEMENT_1_6,
		UFO_ACHIEVEMENT_1_7,
		UFO_ACHIEVEMENT_1_8,
		UFO_ACHIEVEMENT_1_9,
		UFO_ACHIEVEMENT_1_10,
		UFO_ACHIEVEMENT_1_11,
	};

	private Achievement_t[] m_Achievements = new Achievement_t[] {
		new Achievement_t(Achievement.UFO_ACHIEVEMENT_1_0, "We Come In Peace", "Completed first match"),
		new Achievement_t(Achievement.UFO_ACHIEVEMENT_1_1, "Double Trouble", "Won first Double Match"),
		new Achievement_t(Achievement.UFO_ACHIEVEMENT_1_2, "Annunaki DESTROYED", "Defeated Annunaki Titan!"),
		new Achievement_t(Achievement.UFO_ACHIEVEMENT_1_3, "Robot X DESTROYED", "Beat Tokyo Boss"),
		new Achievement_t(Achievement.UFO_ACHIEVEMENT_1_4, "First Contact", "Played first Online Match"),
		new Achievement_t(Achievement.UFO_ACHIEVEMENT_1_5, "Watch the Skies!", "Played a full 4 Player Local Multiplayer Match"),
		new Achievement_t(Achievement.UFO_ACHIEVEMENT_1_6, "RULER of EARTH", "Completed Single Player Adventure"),
		new Achievement_t(Achievement.UFO_ACHIEVEMENT_1_7, "Mysteries of the Bermuda Triangle : REVEALED", "Match completed on Bonus Level (BRMD)"),
		new Achievement_t(Achievement.UFO_ACHIEVEMENT_1_8, "Destruction Overlord", "Completed 100 Matches"),
		new Achievement_t(Achievement.UFO_ACHIEVEMENT_1_9, "The Conqueror of All Time", "Completed 200 Matches"),
		new Achievement_t(Achievement.UFO_ACHIEVEMENT_1_10, "Online Invader", "Played 50 Online Matches"),
		new Achievement_t(Achievement.UFO_ACHIEVEMENT_1_11, "Across the Universe", "Played as every single character")
	};

	// Our GameID
	private CGameID m_GameID;

	// Did we get the stats from Steam?
	private bool m_bRequestedStats;
	private bool m_bStatsValid;

	// Should we store stats this frame?
	private bool m_bStoreStats;

	protected Callback<UserStatsReceived_t> m_UserStatsReceived;
	protected Callback<UserStatsStored_t> m_UserStatsStored;
	protected Callback<UserAchievementStored_t> m_UserAchievementStored;

	private void Awake()
	{
		if (instance)
		{
			if (instance != this)
				Destroy(gameObject);

			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	void OnEnable()
	{
		if (!SteamManager.Initialized)
			return;

		// Cache the GameID for use in the Callbacks
		m_GameID = new CGameID(SteamUtils.GetAppID());

		m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
		m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
		m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

		// These need to be reset to get the stats upon an Assembly reload in the Editor.
		m_bRequestedStats = false;
		m_bStatsValid = false;
	}

	private void Update()
	{
		if (!SteamManager.Initialized)
			return;

		if (!m_bRequestedStats)
		{
			// Is Steam Loaded? if no, can't get stats, done
			if (!SteamManager.Initialized)
			{
				m_bRequestedStats = true;
				return;
			}

			// If yes, request our stats
			bool bSuccess = SteamUserStats.RequestCurrentStats();

			// This function should only return false if we weren't logged in, and we already checked that.
			// But handle it being false again anyway, just ask again later.
			m_bRequestedStats = bSuccess;
		}

		if (!m_bStatsValid)
			return;

		// Get info from sources

		// Evaluate achievements
		foreach (Achievement_t achievement in m_Achievements)
		{
			if (achievement.m_bAchieved)
				continue;
		}

		//Store stats in the Steam database if necessary
		if (m_bStoreStats)
		{
			bool bSuccess = SteamUserStats.StoreStats();
			m_bStoreStats = !bSuccess;
		}
	}

	public void UnlockAchievement(Achievement achievement)
	{
		var a = m_Achievements.FirstOrDefault(it => it.m_eAchievementID == achievement);
		if (a != null)
			UnlockAchievement(a);
	}

	private void UnlockAchievement(Achievement_t achievement)
	{
		if (achievement.m_bAchieved) return;

		achievement.m_bAchieved = true;

		// the icon may change once it's unlocked
		//achievement.m_iIconImage = 0;

		// mark it down
		SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());

		// Store stats end of frame
		m_bStoreStats = true;
	}

	private void OnUserStatsReceived(UserStatsReceived_t pCallback)
	{
		if (!SteamManager.Initialized)
			return;

		// we may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID)
		{
			if (EResult.k_EResultOK == pCallback.m_eResult)
			{
				Debug.Log("Received stats and achievements from Steam\n");

				m_bStatsValid = true;

				// load achievements
				foreach (Achievement_t ach in m_Achievements)
				{
					bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);
					if (ret)
					{
						ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
						ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
					}
					else
					{
						Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
					}
				}

				// load stats
				//SteamUserStats.GetStat("NumGames", out m_nTotalGamesPlayed);
				//SteamUserStats.GetStat("NumWins", out m_nTotalNumWins);
				//SteamUserStats.GetStat("NumLosses", out m_nTotalNumLosses);
				//SteamUserStats.GetStat("FeetTraveled", out m_flTotalFeetTraveled);
				//SteamUserStats.GetStat("MaxFeetTraveled", out m_flMaxFeetTraveled);
				//SteamUserStats.GetStat("AverageSpeed", out m_flAverageSpeed);
			}
			else
			{
				Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: Our stats data was stored!
	//-----------------------------------------------------------------------------
	private void OnUserStatsStored(UserStatsStored_t pCallback)
	{
		// we may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID)
		{
			if (EResult.k_EResultOK == pCallback.m_eResult)
			{
				Debug.Log("StoreStats - success");
			}
			else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
			{
				// One or more stats we set broke a constraint. They've been reverted,
				// and we should re-iterate the values now to keep in sync.
				Debug.Log("StoreStats - some failed to validate");
				// Fake up a callback here so that we re-load the values.
				UserStatsReceived_t callback = new UserStatsReceived_t();
				callback.m_eResult = EResult.k_EResultOK;
				callback.m_nGameID = (ulong)m_GameID;
				OnUserStatsReceived(callback);
			}
			else
			{
				Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: An achievement was stored
	//-----------------------------------------------------------------------------
	private void OnAchievementStored(UserAchievementStored_t pCallback)
	{
		// We may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID)
		{
			if (0 == pCallback.m_nMaxProgress)
			{
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
			}
			else
			{
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
			}
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: Display the user's stats and achievements
	//-----------------------------------------------------------------------------
	public void Render()
	{
		if (!SteamManager.Initialized)
		{
			GUILayout.Label("Steamworks not Initialized");
			return;
		}

		GUILayout.BeginArea(new Rect(Screen.width - 300, 0, 300, 800));
		foreach (Achievement_t ach in m_Achievements)
		{
			GUILayout.Label(ach.m_eAchievementID.ToString());
			GUILayout.Label(ach.m_strName + " - " + ach.m_strDescription);
			GUILayout.Label("Achieved: " + ach.m_bAchieved);
			GUILayout.Space(20);
		}

		// FOR TESTING PURPOSES ONLY!
		if (GUILayout.Button("RESET STATS AND ACHIEVEMENTS"))
		{
			SteamUserStats.ResetAllStats(true);
			SteamUserStats.RequestCurrentStats();

			//OnGameStateChange(EClientGameState.k_EClientGameActive);
		}
		GUILayout.EndArea();
	}

	private class Achievement_t
	{
		public Achievement m_eAchievementID;
		public string m_strName;
		public string m_strDescription;
		public bool m_bAchieved;

		/// <summary>
		/// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
		/// </summary>
		/// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
		/// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
		/// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
		public Achievement_t(Achievement achievementID, string name, string desc)
		{
			m_eAchievementID = achievementID;
			m_strName = name;
			m_strDescription = desc;
			m_bAchieved = false;
		}
	}
}