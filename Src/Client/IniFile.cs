/*
 * Copyright (c) 2024-2025 e1z0. All Rights Reserved.
 * Licensed under the Business Source License 1.1.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

namespace AnotherRTSP
{
    public class IniFile   // revision 11
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);


        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
        }

        public List<string> GetKeys(string category)
        {

            byte[] buffer = new byte[2048];

            GetPrivateProfileSection(category, buffer, 2048, Path);
            String[] tmp = Encoding.ASCII.GetString(buffer).Trim('\0').Split('\0');

            List<string> result = new List<string>();

            foreach (String entry in tmp)
            {
                try
                {
                    result.Add(entry.Substring(0, entry.IndexOf("=")));
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        public string Read(string Key, string Section = null)
        {
   
                var RetVal = new StringBuilder(255);
                GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
                return RetVal.ToString();
            
           
        }

        public string ReadDefault(string Key, string DefaultVal, string Section = null)
        {

            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            if (RetVal.ToString() == null)
                    return DefaultVal;
            try
            {
                return RetVal.ToString();
            }
            catch (Exception)
            {
                return DefaultVal;
            }

        }

        public int ReadIntDefault(string Key, int DefaultVal, string Section = null)
        {

            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            if (RetVal.ToString() == null)
                return DefaultVal;
            try
            {
                return int.Parse(RetVal.ToString());
            }
            catch (Exception)
            {
                return DefaultVal;
            }

        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void WriteInt(string Key, int Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value.ToString(), Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            try
            {
                Write(null, null, Section ?? EXE);
            }
            catch (Exception)
            {
            }
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}
