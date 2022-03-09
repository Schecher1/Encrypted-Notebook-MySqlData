using System;
using System.Collections.Generic;

namespace Encrypted_Notebook.Class{
    class SplitManager{
        public static string SplitByteArrayIntoString(byte[] saltArray)
        {
            try{
                string saltString = "";
                for (int i = 1; i <= saltArray.Length; i++)
                {
                    saltString += saltArray[i - 1].ToString() + "§";
                }
                return saltString = saltString.Remove(saltString.Length - 1);
            }
            catch {return null; }
        }
        public static byte[] SplitStringIntoByteArray(string saltString)
        {
            try{
                byte[] saltByteArray;
                string[] saltStringArray = saltString.Split('§');
                List<byte> saltCache = new List<byte>();

                for (int i = 0; i < saltStringArray.Length; i++)
                    saltCache.Add(Convert.ToByte(saltStringArray[i]));

                return saltByteArray = saltCache.ToArray();
            }
            catch { return null; }
        }
    }
}
