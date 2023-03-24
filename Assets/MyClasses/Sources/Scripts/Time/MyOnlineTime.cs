/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyOnlineTime (version 1.2)
 */

using System;

namespace MyClasses
{
    public class MyOnlineTime
    {
        #region ----- Variable -----

        private static long _offsetUnixTime = 0;

        #endregion

        #region ----- Property -----

        /// <summary>
        /// Return the current unix timestamp in milliseconds.
        /// </summary>
        public static long CurrentUnixTime
        {
            get { return MyLocalTime.CurrentUnixTime + _offsetUnixTime; }
        }

        /// <summary>
        /// Return the current DateTime.
        /// </summary>
        public static DateTime CurrentDateTime
        {
            get { return MyLocalTime.GetTimeSinceUnixEpoch(CurrentUnixTime); }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Sync with online time.
        /// </summary>
        /// <param name="onlineUnixTime">milliseconds</param>
        public static void SyncOnlineTime(long onlineUnixTime)
        {
            _offsetUnixTime = onlineUnixTime - MyLocalTime.CurrentUnixTime;
        }

        #endregion
    }
}