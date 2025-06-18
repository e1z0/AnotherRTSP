/*
 * Copyright (c) 2024-2025 e1z0. All Rights Reserved.
 * Licensed under the Business Source License 1.1.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.IO;
using System.Reflection;

namespace AnotherRTSP.Classes
{
    public class LuaScriptItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool RunOnStartup { get; set; }
        public string Source { get; set; }
    }



    public class MqttStack
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientID { get; set; }
        public MqttStack()
        {
            Server = "";
            Port = 1883;
            Username = "";
            Password = "";
            ClientID = "";
        }
    }

    public class MqttRulesDefinition
    {
        public string Uuid { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public int Type { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public int Action { get; set; }
        public MqttRulesDefinition()
        {
            Uuid = Guid.NewGuid().ToString();
            Name = "";
            Topic = "";
            Type = 0;
            Value1 = "";
            Value2 = "";
            Action = 0;
        }
    }

    public class AdvancedSettings
    {
        public bool LedsWindowOnTop { get; set; }
        public bool LedsSoundAlert { get; set; }
        public bool FocusAllWindowsOnClick { get; set; }
        public bool StaticCameraCaption { get; set; }
        public bool DisableCameraCaptions { get; set; }
        public bool AllCamerasWindowsOnTop { get; set; }
        // new settings
        public int ResizeWindowBy { get; set; }
        public AdvancedSettings()
        {
            LedsWindowOnTop = true;
            LedsSoundAlert = false;
            FocusAllWindowsOnClick = true;
            StaticCameraCaption = false;
            DisableCameraCaptions = false;
            AllCamerasWindowsOnTop = false;
            ResizeWindowBy = 5;
        }
    }

    public class YmlSettings
    {
        // dynamic values
        public static string VERSION;
        public static bool LogWindowRunning = false;
        public static bool IsInit = false;
        public static int LedsCount;

        // === SETTINGS PROPERTIES ===
        public bool MqttEnabled { get; set; }
        public bool FirstRun { get; set; }
        public bool Logging { get; set; }
        public string LogPath { get; set; }
        public bool LogWindow { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public int WindowX { get; set; }
        public int WindowY { get; set; }
        public int LogWindowWidth { get; set; }
        public int LogWindowHeight { get; set; }
        public int LogWindowX { get; set; }
        public int LogWindowY { get; set; }
        public int LedWindowX { get; set; }
        public int LedWindowY { get; set; }
        public bool CustomLayout { get; set; }


        public static MqttStack MqttSettings = new MqttStack();
        public static AdvancedSettings Advanced = new AdvancedSettings();


        public MqttStack MQTT { get; set; }
        public AdvancedSettings AdvancedSettings { get; set; }
        public List<CameraItem> Cameras { get; set; }
        public List<MqttRulesDefinition> MQTTRules { get; set; }
        public List<LuaScriptItem> Scripts { get; set; }



        // === PATH TO CONFIG FILE ===
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.yml");
        // === SINGLETON-LIKE GLOBAL INSTANCE ===
        public static YmlSettings Data { get; private set; }

        // === DEFAULT CONSTRUCTOR (sets default values) ===
        public YmlSettings()
        {
            MqttEnabled = false;
            FirstRun = false;
            Logging = false;
            LogWindow = false;
            LogPath = new FileInfo(Assembly.GetExecutingAssembly().GetName().Name + ".log").FullName;
            WindowWidth = 1196;
            WindowHeight = 877;
            WindowX = 305;
            WindowY = 201;
            LogWindowWidth = 616;
            LogWindowHeight = 354;
            LogWindowX = 0;
            LogWindowY = 0;
            LedWindowX = 0;
            LedWindowY = 0;
            CustomLayout = true;
            MQTT = new MqttStack();
            Cameras = new List<CameraItem>();
            MQTTRules = new List<MqttRulesDefinition>();
            AdvancedSettings = new AdvancedSettings();
            Scripts = new List<LuaScriptItem>();
        }

        public static void UpdateCamera(Camera cam)
        {
            cam.UpdateConfigFromForm();
            var existing = Data.Cameras.Find(c => c.Name == cam.Config.Name);
            if (existing != null)
            {
                Data.Cameras.Remove(existing);
            }
            Data.Cameras.Add(cam.Config);
        }

        public static void AddCamera(CameraItem item)
        {
            Data.Cameras.Add(item);
            Save();
        }


        public static void RemoveCamera(string name)
        {
            Data.Cameras.RemoveAll(c => c.Name == name);
            Save();
        }

        // === LOAD METHOD ===
        public static void Load()
        {
            if (!File.Exists(ConfigFilePath))
            {
                Data = new YmlSettings();
                Save(); // save defaults
                return;
            }

            try
            {
                using (var reader = new StreamReader(ConfigFilePath))
                {
                    var deserializer = new Deserializer();
                    Data = deserializer.Deserialize<YmlSettings>(reader);
                }
                if (Data == null)
                    Data = new YmlSettings();
            }
            catch (Exception ex)
            {
                //Logger.WriteLog("Error loading settings: " + ex.Message);
                System.Windows.Forms.MessageBox.Show("Error loading settings: " + ex.Message);
                Data = new YmlSettings();
            }
        }

        // === SAVE METHOD ===
        public static void Save()
        {
            try
            {
                using (var writer = new StreamWriter(ConfigFilePath))
                {
                    var serializer = new Serializer();
                    serializer.Serialize(writer, Data);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error saving settings: " + ex.Message);
                //Logger.WriteLog("Error saving settings: " + ex.Message);
            }
        }
    }
}
