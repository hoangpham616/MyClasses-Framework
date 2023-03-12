/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUtilities.Convert (version 1.5)
 */

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        private static StringBuilder mStringBuilerConvert;
        private static StringBuilder mStringBuilerConvert2;

        #region ----- From Array -----

        /// <summary>
        /// Convert bool array to string.
        /// </summary>
        /// <param name="mergeChar">a delimiter to merge an array of int into one string</param>
        public static string ConvertBoolArrayToString(bool[] content, char mergeChar = ',')
        {
            if (content == null)
            {
                return null;
            }

            if (mStringBuilerConvert == null)
            {
                mStringBuilerConvert = new StringBuilder();
            }

            mStringBuilerConvert.Length = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (i > 0)
                {
                    mStringBuilerConvert.Append(mergeChar);
                }
                mStringBuilerConvert.Append(content[i]);
            }
            return mStringBuilerConvert.ToString();
        }

        /// <summary>
        /// Convert bool two-directional array to string.
        /// </summary>
        /// <param name="mergeChar">a delimiter to merge a two-directional array of bool into one string</param>
        public static string ConvertBoolTwoDirectionalArrayToString(bool[][] content, char mergeChar1 = '|', char mergeChar2 = ',')
        {
            if (content == null)
            {
                return null;
            }

            if (mStringBuilerConvert2 == null)
            {
                mStringBuilerConvert2 = new StringBuilder();
            }

            mStringBuilerConvert2.Length = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (i > 0)
                {
                    mStringBuilerConvert2.Append(mergeChar1);
                }
                mStringBuilerConvert2.Append(ConvertBoolArrayToString(content[i], mergeChar2));
            }
            return mStringBuilerConvert2.ToString();
        }

        /// <summary>
        /// Convert bool two-directional array to string.
        /// </summary>
        /// <param name="mergeChar">a delimiter to merge a two-directional array of int into one string</param>
        public static string ConvertIntTwoDirectionalArrayToString(bool[][] content, char mergeChar1 = '|', char mergeChar2 = ',')
        {
            if (content == null)
            {
                return null;
            }

            if (mStringBuilerConvert2 == null)
            {
                mStringBuilerConvert2 = new StringBuilder();
            }

            mStringBuilerConvert2.Length = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (i > 0)
                {
                    mStringBuilerConvert2.Append(mergeChar1);
                }
                mStringBuilerConvert2.Append(ConvertBoolArrayToString(content[i], mergeChar2));
            }
            return mStringBuilerConvert2.ToString();
        }

        /// <summary>
        /// Convert int array to string.
        /// </summary>
        /// <param name="mergeChar">a delimiter to merge an array of int into one string</param>
        public static string ConvertIntArrayToString(int[] content, char mergeChar = ',')
        {
            if (content == null)
            {
                return null;
            }

            if (mStringBuilerConvert == null)
            {
                mStringBuilerConvert = new StringBuilder();
            }

            mStringBuilerConvert.Length = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (i > 0)
                {
                    mStringBuilerConvert.Append(mergeChar);
                }
                mStringBuilerConvert.Append(content[i]);
            }
            return mStringBuilerConvert.ToString();
        }

        /// <summary>
        /// Convert int two-directional array to string.
        /// </summary>
        /// <param name="mergeChar">a delimiter to merge a two-directional array of int into one string</param>
        public static string ConvertIntTwoDirectionalArrayToString(int[][] content, char mergeChar1 = '|', char mergeChar2 = ',')
        {
            if (content == null)
            {
                return null;
            }

            if (mStringBuilerConvert2 == null)
            {
                mStringBuilerConvert2 = new StringBuilder();
            }

            mStringBuilerConvert2.Length = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (i > 0)
                {
                    mStringBuilerConvert2.Append(mergeChar1);
                }
                mStringBuilerConvert2.Append(ConvertIntArrayToString(content[i], mergeChar2));
            }
            return mStringBuilerConvert2.ToString();
        }

        /// <summary>
        /// Convert float array to string.
        /// </summary>
        /// <param name="mergeChar">a delimiter to merge an array of float into one string</param>
        public static string ConvertFloatArrayToString(float[] content, char mergeChar)
        {
            if (content == null)
            {
                return null;
            }

            if (mStringBuilerConvert == null)
            {
                mStringBuilerConvert = new StringBuilder();
            }

            mStringBuilerConvert.Length = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (i > 0)
                {
                    mStringBuilerConvert.Append(mergeChar);
                }
                mStringBuilerConvert.Append(content[i]);
            }
            return mStringBuilerConvert.ToString();
        }

        /// <summary>
        /// Convert float two-directional array to string.
        /// </summary>
        /// <param name="mergeChar">a delimiter to merge a two-directional array of float into one string</param>
        public static string ConvertFloatTwoDirectionalArrayToString(float[][] content, char mergeChar1 = '|', char mergeChar2 = ',')
        {
            if (content == null)
            {
                return null;
            }

            if (mStringBuilerConvert2 == null)
            {
                mStringBuilerConvert2 = new StringBuilder();
            }

            mStringBuilerConvert2.Length = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (i > 0)
                {
                    mStringBuilerConvert2.Append(mergeChar1);
                }
                mStringBuilerConvert2.Append(ConvertFloatArrayToString(content[i], mergeChar2));
            }
            return mStringBuilerConvert2.ToString();
        }

        #endregion

        #region ----- From Dictionary -----

        /// <summary>
        /// Convert dictionary(string, object) to dictionnary(int, int).
        /// </summary>
        public static Dictionary<int, int> ConvertDictStringObjectToDictIntInt(Dictionary<string, object> dict)
        {
            Dictionary<int, int> resultDict = new Dictionary<int, int>();
            foreach (var item in dict)
            {
                resultDict[int.Parse(item.Key)] = int.Parse(item.Value.ToString());
            }
            return resultDict;
        }

        /// <summary>
        /// Convert dictionary(string, object) to dictionnary(string, int).
        /// </summary>
        public static Dictionary<string, int> ConvertDictStringObjectToDictStringInt(Dictionary<string, object> dict)
        {
            Dictionary<string, int> resultDict = new Dictionary<string, int>();
            foreach (var item in dict)
            {
                resultDict[item.Key] = int.Parse(item.Value.ToString());
            }
            return resultDict;
        }


        /// <summary>
        /// Convert dictionary(string, object) to 2 int arrays.
        /// </summary>
        public static bool ConvertDictStringObjectTo2IntArrays(Dictionary<string, object> dict, out int[] keys, out int[] values)
        {
            if (dict == null)
            {
                keys = null;
                values = null;
                return false;
            }

            keys = new int[dict.Keys.Count];
            values = new int[keys.Length];
            int index = 0;
            foreach (var item in dict)
            {
                keys[index] = int.Parse(item.Key);
                values[index] = int.Parse(item.Value.ToString());
            }
            return true;
        }

        #endregion

        #region ----- From List -----

        /// <summary>
        /// Convert list object to bool array.
        /// </summary>
        public static bool[] ConvertListObjectToBoolArray(List<object> list)
        {
            if (list == null)
            {
                return null;
            }

            bool[] values = new bool[list.Count];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = bool.Parse(list[i].ToString());
            }
            return values;
        }

        /// <summary>
        /// Convert list object to int array.
        /// </summary>
        public static int[] ConvertListObjectToIntArray(List<object> list)
        {
            if (list == null)
            {
                return null;
            }

            int[] values = new int[list.Count];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = int.Parse(list[i].ToString());
            }
            return values;
        }

        /// <summary>
        /// Convert list object to float array.
        /// </summary>
        public static float[] ConvertListObjectToFloatArray(List<object> list)
        {
            if (list == null)
            {
                return null;
            }

            float[] values = new float[list.Count];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = float.Parse(list[i].ToString());
            }
            return values;
        }

        /// <summary>
        /// Convert list object to double array.
        /// </summary>
        public static double[] ConvertListObjectToDoubleArray(List<object> list)
        {
            if (list == null)
            {
                return null;
            }

            double[] values = new double[list.Count];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = double.Parse(list[i].ToString());
            }
            return values;
        }

        /// <summary>
        /// Convert list object to string array.
        /// </summary>
        public static string[] ConvertListObjectToStringArray(List<object> list)
        {
            if (list == null)
            {
                return null;
            }

            string[] values = new string[list.Count];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = list[i].ToString();
            }
            return values;
        }

        #endregion

        #region ----- From String -----

        /// <summary>
        /// Convert string to bool array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into an array of bool</param>
        public static bool[] ConvertStringToBoolArray(string content, char splitChar = ',')
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues = content.Split(splitChar);
            bool[] boolValues = new bool[content.Length > 0 ? stringValues.Length : 0];
            for (int i = 0; i < boolValues.Length; i++)
            {
                boolValues[i] = bool.Parse(stringValues[i]);
            }
            return boolValues;
        }

        /// <summary>
        /// Convert string to bool two-directional array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into a two-directional array of bool</param>
        public static bool[][] ConvertStringToBoolTwoDirectionalArray(string content, char splitChar1 = '|', char splitChar2 = ',')
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues1 = content.Split(splitChar1);
            bool[][] boolValues = new bool[content.Length > 0 ? stringValues1.Length : 0][];
            for (int i = 0; i < boolValues.Length; i++)
            {
                boolValues[i] = ConvertStringToBoolArray(stringValues1[i], splitChar2);
            }
            return boolValues;
        }

        /// <summary>
        /// Convert string to byte array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into an array of byte</param>
        public static byte[] ConvertStringToByteArray(string content, char splitChar)
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues = content.Split(splitChar);
            byte[] byteValues = new byte[content.Length > 0 ? stringValues.Length : 0];
            for (int i = 0; i < byteValues.Length; i++)
            {
                byteValues[i] = byte.Parse(stringValues[i]);
            }
            return byteValues;
        }

        /// <summary>
        /// Convert string to sbyte array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into an array of sbyte</param>
        public static sbyte[] ConvertStringToSByteArray(string content, char splitChar)
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues = content.Split(splitChar);
            sbyte[] ushortValues = new sbyte[content.Length > 0 ? stringValues.Length : 0];
            for (int i = 0; i < ushortValues.Length; i++)
            {
                ushortValues[i] = sbyte.Parse(stringValues[i]);
            }
            return ushortValues;
        }

        /// <summary>
        /// Convert string to short array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into an array of short</param>
        public static short[] ConvertStringToShortArray(string content, char splitChar)
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues = content.Split(splitChar);
            short[] ushortValues = new short[content.Length > 0 ? stringValues.Length : 0];
            for (int i = 0; i < ushortValues.Length; i++)
            {
                ushortValues[i] = short.Parse(stringValues[i]);
            }
            return ushortValues;
        }

        /// <summary>
        /// Convert string to ushort array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into an array of ushort</param>
        public static ushort[] ConvertStringToUShortArray(string content, char splitChar)
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues = content.Split(splitChar);
            ushort[] ushortValues = new ushort[content.Length > 0 ? stringValues.Length : 0];
            for (int i = 0; i < ushortValues.Length; i++)
            {
                ushortValues[i] = ushort.Parse(stringValues[i]);
            }
            return ushortValues;
        }

        /// <summary>
        /// Convert string to int array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into an array of int</param>
        public static int[] ConvertStringToIntArray(string content, char splitChar = ',')
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues = content.Split(splitChar);
            int[] intValues = new int[content.Length > 0 ? stringValues.Length : 0];
            for (int i = 0; i < intValues.Length; i++)
            {
                intValues[i] = int.Parse(stringValues[i]);
            }
            return intValues;
        }

        /// <summary>
        /// Convert string to int two-directional array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into a two-directional array of int</param>
        public static int[][] ConvertStringToIntTwoDirectionalArray(string content, char splitChar1 = '|', char splitChar2 = ',')
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues1 = content.Split(splitChar1);
            int[][] intValues = new int[content.Length > 0 ? stringValues1.Length : 0][];
            for (int i = 0; i < intValues.Length; i++)
            {
                intValues[i] = ConvertStringToIntArray(stringValues1[i], splitChar2);
            }
            return intValues;
        }

        /// <summary>
        /// Convert string to uint array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into an array of uint</param>
        public static uint[] ConvertStringToUIntArray(string content, char splitChar)
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues = content.Split(splitChar);
            uint[] intValues = new uint[content.Length > 0 ? stringValues.Length : 0];
            for (int i = 0; i < intValues.Length; i++)
            {
                intValues[i] = uint.Parse(stringValues[i]);
            }
            return intValues;
        }

        /// <summary>
        /// Convert string to float array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into an array of float</param>
        public static float[] ConvertStringToFloatArray(string content, char splitChar = ',')
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues = content.Split(splitChar);
            float[] intValues = new float[content.Length > 0 ? stringValues.Length : 0];
            for (int i = 0; i < intValues.Length; i++)
            {
                intValues[i] = float.Parse(stringValues[i]);
            }
            return intValues;
        }

        /// <summary>
        /// Convert string to float two-directional array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into a two-directional array of float</param>
        public static float[][] ConvertStringToFloatTwoDirectionalArray(string content, char splitChar1 = '|', char splitChar2 = ',')
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues1 = content.Split(splitChar1);
            float[][] intValues = new float[content.Length > 0 ? stringValues1.Length : 0][];
            for (int i = 0; i < intValues.Length; i++)
            {
                intValues[i] = ConvertStringToFloatArray(stringValues1[i], splitChar2);
            }
            return intValues;
        }

        /// <summary>
        /// Convert string to double array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into an array of double</param>
        public static double[] ConvertStringToDoubleArray(string content, char splitChar)
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues = content.Split(splitChar);
            double[] intValues = new double[content.Length > 0 ? stringValues.Length : 0];
            for (int i = 0; i < intValues.Length; i++)
            {
                intValues[i] = double.Parse(stringValues[i]);
            }
            return intValues;
        }

        /// <summary>
        /// Convert string to Vector2.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into values in Vector2</param>
        public static Vector2 ConvertStringToVector2(string content, char splitChar)
        {
            string[] stringValues = content.Split(splitChar);
            Vector2 v = Vector2.zero;
            v.x = float.Parse(stringValues[0]);
            v.y = float.Parse(stringValues[1]);
            return v;
        }

        /// <summary>
        /// Convert string to Vector2 array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into an array of Vector2</param>
        /// <param name="splitChar2">a delimiter to divide the string into values in Vector2</param>
        public static Vector2[] ConvertStringToArrayVector2(string content, char splitChar, char splitChar2)
        {
            if (content == null)
            {
                return null;
            }

            string[] stringValues = content.Split(splitChar);
            Vector2[] vectorValues = new Vector2[content.Length > 0 ? stringValues.Length : 0];
            for (int i = 0; i < vectorValues.Length; i++)
            {
                string[] temp = stringValues[i].Split(splitChar2);
                Vector2 v = Vector2.zero;
                v.x = float.Parse(temp[0]);
                v.y = float.Parse(temp[1]);
                vectorValues[i] = v;
            }
            return vectorValues;
        }

        /// <summary>
        /// Convert string to Vector3.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into values in Vector3</param>
        public static Vector3 ConvertStringToVector3(string content, char splitChar)
        {
            string[] stringValues = content.Split(splitChar);
            Vector3 v = Vector3.zero;
            v.x = float.Parse(stringValues[0]);
            v.y = float.Parse(stringValues[1]);
            v.z = float.Parse(stringValues[2]);
            return v;
        }

        /// <summary>
        /// Convert string to Vector3 array.
        /// </summary>
        /// <param name="splitChar">a delimiter to divide the string into an array of Vector3</param>
        /// <param name="splitChar2">a delimiter to divide the string into values in Vector3</param>
        public static Vector3[] ConvertStringToVector3Array(string content, char splitChar, char splitChar2)
        {
            string[] stringValues = content.Split(splitChar);
            Vector3[] vectorValues = new Vector3[content.Length > 0 ? stringValues.Length : 0];
            for (int i = 0; i < vectorValues.Length; i++)
            {
                string[] temp = stringValues[i].Split(splitChar2);
                Vector3 v = Vector3.zero;
                v.x = float.Parse(temp[0]);
                v.y = float.Parse(temp[1]);
                v.z = float.Parse(temp[2]);
                vectorValues[i] = v;
            }
            return vectorValues;
        }

        /// <summary>
        /// Convert string to Stream.
        /// </summary>
        public static Stream ConvertStringToStream(string content)
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            streamWriter.Write(content);
            streamWriter.Flush();
            memoryStream.Position = 0;
            return memoryStream;
        }

        #endregion

        #region ----- From Vector -----

        /// <summary>
        /// Convert Vector2 to string.
        /// </summary>
        /// <param name="mergeChar">a delimiter to merge values of vector into one string</param>
        public static string ConvertVector2ToString(Vector3 vector2, char mergeChar)
        {
            if (mStringBuilerConvert == null)
            {
                mStringBuilerConvert = new StringBuilder();
            }

            mStringBuilerConvert.Length = 0;
            mStringBuilerConvert.Append(vector2.x);
            mStringBuilerConvert.Append(mergeChar);
            mStringBuilerConvert.Append(vector2.y);
            return mStringBuilerConvert.ToString();
        }

        /// <summary>
        /// Convert Vector3 to string.
        /// </summary>
        /// <param name="mergeChar">a delimiter to merge values of vector into one string</param>
        public static string ConvertVector3ToString(Vector3 vector3, char mergeChar)
        {
            if (mStringBuilerConvert == null)
            {
                mStringBuilerConvert = new StringBuilder();
            }

            mStringBuilerConvert.Length = 0;
            mStringBuilerConvert.Append(vector3.x);
            mStringBuilerConvert.Append(mergeChar);
            mStringBuilerConvert.Append(vector3.y);
            mStringBuilerConvert.Append(mergeChar);
            mStringBuilerConvert.Append(vector3.z);
            return mStringBuilerConvert.ToString();
        }

        #endregion
    }
}