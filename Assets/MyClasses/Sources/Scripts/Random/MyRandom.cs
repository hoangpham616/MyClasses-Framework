/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyRandom (version 1.3)
 */

using System.Collections.Generic;

namespace MyClasses
{
    public static class MyRandom
    {
        #region ----- Variable -----

        private static long _evenFactor;
        private static long _oddFactor;
        private static long _firstValue;
        private static long _curValue;
        private static long _count;

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set the seed.
        /// </summary>
        public static void SetSeed(int seed)
        {
            if (seed < 0)
            {
                seed *= -1;
            }
            seed += 1;

            _evenFactor = (seed * 2) + 2;
            _oddFactor = (seed * 3 / 2) + 1;

            _firstValue = 0;
            _curValue = seed % 10;
            _count = 0;

            _NextValue();

            _firstValue = _curValue;
        }

        /// <summary>
        /// Return a integer in range.
        /// </summary>
        public static int Range(int begin, int end)
        {
            long range = end - begin;
            if (range == 0)
            {
                range = 1;
            }

            _NextValue();

            return (int)(_curValue % range) + begin;
        }

        /// <summary>
        /// Return a decimal in range.
        /// </summary>
        public static float Range(float begin, float end)
        {
            float range = end - begin;
            if (range <= 1)
            {
                return begin + (Value01() % range);
            }
            else
            {
                return Range((int)begin, (int)(end - 1)) + Value01();
            }
        }

        /// <summary>
        /// Return a decimal between 0 and 1.
        /// </summary>
        public static float Value01()
        {
            _NextValue();

            return (_curValue % 10000) / 10000f;
        }

        /// <summary>
        /// Randomly choose an element from a list.
        /// </summary>
        public static T Choose<T>(List<T> list)
        {
            if (list != null)
            {
                int count = list.Count;
                if (count > 0)
                {
                    return list[Range(0, count - 1)];
                }
            }

            return default(T);
        }

        /// <summary>
        /// Shuffle a list.
        /// </summary>
        public static void Shuffle<T>(List<T> list)
        {
            for (int i = 0, lastIndex = list.Count - 1; i <= lastIndex; i++)
            {
                MyUtilities.Swap(list, i, Range(0, lastIndex));
            }
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Change value.
        /// </summary>
        private static void _NextValue()
        {
            _curValue = ((_curValue * _oddFactor) + _evenFactor + _count++) % int.MaxValue;

            if (_firstValue == _curValue)
            {
                _curValue = (_curValue + _count) % int.MaxValue;
                _count = 0;
            }
        }

        #endregion
    }
}