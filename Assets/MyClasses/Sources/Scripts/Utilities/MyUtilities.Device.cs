/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUtilities.Device (version 1.3)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        /// <summary>
        /// Return long edge pixel.
        /// </summary>
        public static float GetDeviceLongEdge()
        {
            return Mathf.Max(Screen.width, Screen.height);
        }

        /// <summary>
        /// Return short edge pixel.
        /// </summary>
        public static float GetDeviceShortEdge()
        {
            return Mathf.Min(Screen.width, Screen.height);
        }

        /// <summary>
        /// Return display aspect ratio.
        /// </summary>
        public static float GetDeviceDisplayAspectRatio()
        {
            return GetDeviceLongEdge() / GetDeviceShortEdge();
        }

        /// <summary>
        /// Return long edge pixel of the safe area.
        /// </summary>
        public static float GetDeviceSafeAreaLongEdge()
        {
            return Mathf.Max(Screen.safeArea.width, Screen.safeArea.height);
        }

        /// <summary>
        /// Return short edge pixel of the safe area.
        /// </summary>
        public static float GetDeviceSafeAreShortEdge()
        {
            return Mathf.Min(Screen.safeArea.width, Screen.safeArea.height);
        }

        /// <summary>
        /// Return safe area ratio.
        /// </summary>
        public static float GetDeviceSafeAreaRatio()
        {
            return GetDeviceSafeAreaLongEdge() / GetDeviceSafeAreShortEdge();
        }

        /// <summary>
        /// Check if the safe area in the center.
        /// </summary>
        public static bool IsDeviceSafeAreaInCenter()
        {
            return !(Screen.safeArea.x == 0 || (Screen.safeArea.x + GetDeviceSafeAreaLongEdge() == GetDeviceLongEdge()));
        }

        /// <summary>
        /// Check if this device has notch.
        /// </summary>
        public static bool IsDeviceNotch()
        {
#if UNITY_EDITOR
            if (GetDeviceSafeAreaLongEdge() < GetDeviceLongEdge())
            {
                return true;
            }
#elif UNITY_IOS
            if (SystemInfo.deviceModel.StartsWith("iPhone1"))
            {
                if (SystemInfo.deviceModel.Equals("iPhone10,6") // iPhone X
                    || !SystemInfo.deviceModel.StartsWith("iPhone11,") // iPhone XS, iPhone XR
                    || !SystemInfo.deviceModel.StartsWith("iPhone12,") // iPhone 11 series, iPhone SE 2nd Gen
                    || !SystemInfo.deviceModel.StartsWith("iPhone13,") // iPhone 12 series
                    || !SystemInfo.deviceModel.StartsWith("iPhone14,")) // iPhone 13 series, iPhone SE 3rd Gen, iPhone 14, iPhone 14 Plus
                {
                    return true;
                }
            }
#else
            if (GetDeviceSafeAreaLongEdge() < GetDeviceLongEdge())
            {
                return true;
            }
#endif
            return false;
        }

        /// <summary>
        /// Check if this device is phone.
        /// </summary>
        public static bool IsPhone()
        {
#if UNITY_IOS
#if UNITY_EDITOR
            return GetDeviceDisplayAspectRatio() >= 1.55f;
#else
            return SystemInfo.deviceModel.Contains("iPhone");
#endif
#elif UNITY_ANDROID
            return GetDeviceDisplayAspectRatio() >= 1.61f;
#else
            return false;
#endif
        }

        /// <summary>
        /// Check if this device is tablet.
        /// </summary>
        public static bool IsTablet()
        {
#if UNITY_IOS
#if UNITY_EDITOR
            return GetDeviceDisplayAspectRatio() < 1.55f;
#else
            return SystemInfo.deviceModel.Contains("iPad");
#endif
#elif UNITY_ANDROID
            return GetDeviceDisplayAspectRatio() < 1.61f;
#else
            return false;
#endif
        }
    }
}