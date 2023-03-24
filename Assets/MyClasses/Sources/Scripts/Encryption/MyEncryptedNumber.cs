/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyEncryptedNumber (version 1.3)
 */

using System;
using UnityEngine;

namespace MyClasses
{
    public class MyEncryptedNumber
    {
        #region ----- Variable -----

        private static int _seed = 616;

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set encryption seed.
        /// </summary>
        public static void SetSeed(int seed)
        {
            _seed = seed;
        }

        /// <summary>
        /// Encrypt an integer.
        /// </summary>
        public static int EncryptInt(int number)
        {
            return ~(number ^ _seed);
        }

        /// <summary>
        /// Decrypt an integer.
        /// </summary>
        public static int DecryptInt(int number)
        {
            return ~number ^ _seed;
        }

        /// <summary>
        /// Encrypt a float.
        /// </summary>
        public static float EncryptFloat(float number)
        {
            float decimalPart = Mathf.Abs(Convert.ToSingle(Convert.ToDecimal(number) % 1));
            int wholePart = (int)number;
            int encryptedWholePart = EncryptInt(wholePart);
            return encryptedWholePart > 0 ? encryptedWholePart + decimalPart : encryptedWholePart - decimalPart;
        }

        /// <summary>
        /// Decrypt a float.
        /// </summary>
        public static float DecryptFloat(float number)
        {
            float decimalPart = Mathf.Abs(Convert.ToSingle(Convert.ToDecimal(number) % 1));
            int wholePart = (int)number;
            int decryptedWholePart = DecryptInt(wholePart);
            return decryptedWholePart > 0 ? decryptedWholePart + decimalPart : decryptedWholePart - decimalPart;
        }

        #endregion
    }
}