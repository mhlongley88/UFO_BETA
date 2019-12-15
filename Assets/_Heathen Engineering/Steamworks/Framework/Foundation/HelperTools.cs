#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using System;
using System.IO;
using UnityEngine;

namespace HeathenEngineering.SteamTools
{
    public static class HelperTools
    {
        public static byte[] FlipImageBufferVertical(int width, int height, byte[] buffer)
        {
            byte[] result = new byte[buffer.Length];

            int xWidth = width * 4;
            int yHeight = height;

            for (int y = 0; y < yHeight; y++)
            {
                for (int x = 0; x < xWidth; x++)
                {
                    result[x + ((yHeight - 1 - y) * xWidth)] = buffer[x + (xWidth * y)];
                }
            }

            return result;
        }

        /// <summary>
        /// Read an image file from disk
        /// </summary>
        /// <param name="path"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static bool LoadImageFromDisk(string path, out Texture2D texture)
        {
            if (File.Exists(path))
            {
                byte[] byteArray = File.ReadAllBytes(path);
                texture = new Texture2D(2, 2);
                return texture.LoadImage(byteArray);
            }
            else
            {
                Debug.LogError("Load Image From Disk called on file [" + path + "] but no such file was found.");
                texture = new Texture2D(2, 2);
                return false;
            }
        }

        /// <summary>
        /// Converts a Linux style time stamp to a DateTime object
        /// </summary>
        /// <param name="nixTime"></param>
        /// <returns></returns>
        public static DateTime ConvertNixDate(uint nixTime)
        {
            var timeStamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return timeStamp.AddSeconds(nixTime);
        }

        /// <summary>
        /// Checks if the 'checkFlag' value is in the 'value'
        /// </summary>
        /// <param name="value"></param>
        /// <param name="checkflag"></param>
        /// <returns></returns>
        public static bool WorkshopItemStateHasFlag(EItemState value, EItemState checkflag)
        {
            return (value & checkflag) == checkflag;
        }

        /// <summary>
        /// Cheks if any of the 'checkflags' values are in the 'value'
        /// </summary>
        /// <param name="value"></param>
        /// <param name="checkflags"></param>
        /// <returns></returns>
        public static bool WorkshopItemStateHasAllFlags(EItemState value, params EItemState[] checkflags)
        {
            foreach (var checkflag in checkflags)
            {
                if ((value & checkflag) != checkflag)
                    return false;
            }
            return true;
        }
    }
}
#endif