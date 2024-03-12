#define DEBUG


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using AnotherRTSP.Classes;
using AnotherRTSP.Services;



namespace AnotherRTSP
{
    public class MqttStack
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientID { get; set; }
    }

    public class MqttRulesDefinition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public int Type { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public int Action { get; set; }
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
    } 

    public static class Settings
    {
        public static string VERSION;
        public static string camerasSection = "Cameras";
        public static bool FirstRun = false;
        public static bool MqttServiceRunning = false;
        public static bool LogWindowRunning = false;
        public static int LedsCount;
        public static string LogPath = "";
        public static int WindowWidth;
        public static int WindowHeight;
        public static int WindowX;
        public static int WindowY;
        public static int LogWindowWidth;
        public static int LogWindowHeight;
        public static int LogWindowX;
        public static int LogWindowY;
        public static int LedWindowX;
        public static int LedWindowY;
        public static int CustomLayout;
        public static int MqttEnabled;
        public static MqttStack MqttSettings = new MqttStack();
        public static MqttRulesDefinition[] MqttRulesSettings = new MqttRulesDefinition[90];
        public static AdvancedSettings Advanced = new AdvancedSettings();
        public static int Logging;
        public static int LogWindow;

        public static List<Camera> Cameras = new List<Camera>();
        public static MqttRulesDefinition NewMqttRule(int id, string name, string topic, int type, string value, string value2, int action)
        {
            var rule = new MqttRulesDefinition();
            rule.Id = id;
            rule.Name = name;
            rule.Topic = topic;
            rule.Type = type;
            rule.Value1 = value;
            rule.Value2 = value2;
            rule.Action = action;
            return rule;
        }

        public static void Save()
        {
            var ini = new IniFile();
            ini.WriteInt("WindowWidth", WindowWidth, "General");
            ini.WriteInt("WindowHeight", WindowHeight, "General");
            ini.WriteInt("WindowX", WindowX, "General");
            ini.WriteInt("WindowY", WindowY, "General");
            ini.WriteInt("CustomLayout", CustomLayout, "General");
            ini.WriteInt("MqttStack", MqttEnabled, "General");
            ini.WriteInt("Logging", Logging, "General");
            ini.WriteInt("LogWindow", LogWindow, "General");

            // log window
            ini.WriteInt("LogWindowWidth", LogWindowWidth, "General");
            ini.WriteInt("LogWindowHeight", LogWindowHeight, "General");
            ini.WriteInt("LogWindowX", LogWindowX, "General");
            ini.WriteInt("LogWindowY", LogWindowY, "General");
            // Led window
            ini.WriteInt("LedWindowX", LedWindowX, "General");
            ini.WriteInt("LedWindowY", LedWindowY, "General");

            // Advanced settings
            ini.WriteInt("LedsWindowOnTop", Advanced.LedsWindowOnTop ? 1 : 0,"Advanced");
            ini.WriteInt("LedsSoundAlert", Advanced.LedsSoundAlert ? 1 : 0, "Advanced");
            ini.WriteInt("FocusAllWindowsOnClick", Advanced.FocusAllWindowsOnClick ? 1 : 0, "Advanced");
            ini.WriteInt("StaticCameraCaption", Advanced.StaticCameraCaption ? 1 : 0, "Advanced");
            ini.WriteInt("DisableCameraCaptions", Advanced.DisableCameraCaptions ? 1 : 0, "Advanced");
            ini.WriteInt("AllCamerasWindowsOnTop", Advanced.AllCamerasWindowsOnTop ? 1 : 0, "Advanced");

            // save mqtt settings
            if (MqttEnabled > 0)
            {
                try
                {
                    if (Settings.MqttSettings.Server != "")
                        ini.Write("Server", Settings.MqttSettings.Server, "MQTT");
                    if (Settings.MqttSettings.Port > 0)
                        ini.WriteInt("Port", Settings.MqttSettings.Port, "MQTT");
                    if (Settings.MqttSettings.Username != "")
                        ini.Write("Username", Settings.MqttSettings.Username, "MQTT");
                    if (Settings.MqttSettings.Password != "")
                        ini.Write("Password", Settings.MqttSettings.Password, "MQTT");
                    if (Settings.MqttSettings.ClientID != "")
                        ini.Write("ClientID", Settings.MqttSettings.ClientID, "MQTT");
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("Exception [function Save() mqtt settings]: {0}", ex.ToString());
                }
                // save mqtt rules
                try
                {
                    ini.DeleteSection("MQTTRules");
                    foreach (MqttRulesDefinition rule in MqttRulesSettings)
                    {
                        if (rule != null)
                        {
                            Logger.WriteLog("Got mqtt rule: " + rule.Topic);
                            var ruleval = String.Format("{0}::{1}::{2}::{3}::{4}::{5}", rule.Name, rule.Topic, rule.Type, rule.Value1, rule.Value2, rule.Action);
                            ini.Write(rule.Id.ToString(), ruleval, "MQTTRules");
                        }
                    }

                }
                catch (Exception ex)
                {
                    Logger.WriteLog("Exception [function Save() mqtt rules]: {0}", ex.ToString());
                }
            }

            //foreach (KeyValuePair<string, Camera> cam in Settings.Cameras)
            foreach (Camera cam in Cameras)
            {
                ini.WriteInt("WWidth", cam.WWidth, cam.Name);
                ini.WriteInt("WHeight", cam.WHeight, cam.Name);
                ini.WriteInt("WX", cam.WX, cam.Name);
                ini.WriteInt("WY", cam.WY, cam.Name);
                ini.WriteInt("Disabled", cam.Disabled ? 1 : 0, cam.Name);
            }
            Logger.WriteLog("Program settings have been saved!");
        }

        public static void SetFormDetails(string name, int ww, int wh, int x, int y)
        {
            var cams = Cameras;
            foreach (Camera cam in Cameras) 
            //foreach (KeyValuePair<string, Camera> cam in cams)
            {
                if (cam.Name == name)
                {
                    cam.WHeight = wh;
                    cam.WWidth = ww;
                    cam.WX = x;
                    cam.WY = y;
                }
            }
            Cameras = cams;
        }

        public static MqttRulesDefinition GetMqttRuleByTopic(string topic)
        {
            foreach (MqttRulesDefinition rule in Settings.MqttRulesSettings)
            {
                if (topic == rule.Topic)
                    return rule;
            }
            return null;
        }

   
        public static void GetCams()
        {
            var ini = new IniFile();
            //Dictionary<string, Camera> Cams = new Dictionary<string, Camera>();
            var cameraNames = ini.GetKeys(camerasSection);
            foreach (string camName in cameraNames)
            {
                var camUrl = ini.ReadDefault(camName, "", camerasSection);
                if (camUrl != "")
                {
                    //var camobj = new Camera();
                    string url = camUrl;
                    int wwidth = ini.ReadIntDefault("WWidth", 240, camName);
                    int wheight = ini.ReadIntDefault("WHeight", 180, camName);
                    int wx = ini.ReadIntDefault("WX", 0, camName);
                    int wy = ini.ReadIntDefault("WY", 0, camName);
                    bool disabled = ini.ReadIntDefault("Disabled", 0, camName) != 0;
                    Camera newcam = new Camera(camName,wwidth,wheight,wx,wy,url,disabled);
                    Cameras.Add(newcam);

                    //Cams.Add(camName, camobj);
                }
            }
            //Cameras = Cams;
        }

        public static void OverrideCamsList()
        {
            // delete ini section cameras
            var ini = new IniFile();
            ini.DeleteSection("Cameras");
            foreach (Camera cam in Cameras)
            {
                ini.Write(cam.Name, cam.Url, "Cameras");
                // FIXME need to save more settings here for the cam details
            }
        }

        public static void ShowOrActivateForm<T>() where T : System.Windows.Forms.Form, new()
        {
            // Check if the form is already open
            foreach (System.Windows.Forms.Form form in System.Windows.Forms.Application.OpenForms)
            {
                if (form is T)
                {
                    // If the form is already open, activate it
                    form.Activate();
                    return;
                }
            }

            // If the form is not already open, create a new instance and show it
            T newForm = new T();
            newForm.Show();
        }

        public static void DeactivateForm<T>() where T : System.Windows.Forms.Form, new()
        {
            // Check if the form is already open
            foreach (System.Windows.Forms.Form form in System.Windows.Forms.Application.OpenForms)
            {
                if (form is T)
                {
                    // If the form is already open, activate it
                    form.Close();
                    return;
                }
            }
        }

        public static void Load()
        {
            LogPath = new FileInfo(Assembly.GetExecutingAssembly().GetName().Name + ".log").FullName;
            var IniFl = new FileInfo(Assembly.GetExecutingAssembly().GetName().Name + ".ini").FullName;
            if (!File.Exists(IniFl))
            {
                // first run
                FirstRun = true;
                ShowOrActivateForm<SettingsForm>();
            }


            var ini = new IniFile();
            // general settings
            try
            {
                WindowWidth = ini.ReadIntDefault("WindowWidth", 810, "General");
                WindowHeight = ini.ReadIntDefault("WindowHeight", 610, "General");
                WindowX = ini.ReadIntDefault("WindowX", 0, "General");
                WindowY = ini.ReadIntDefault("WindowY", 0, "General");
                CustomLayout = ini.ReadIntDefault("CustomLayout", 0, "General");
                MqttEnabled = ini.ReadIntDefault("MqttStack", 0, "General");
                Logging = ini.ReadIntDefault("Logging", 0, "General");
                LogWindow = ini.ReadIntDefault("LogWindow", 0, "General");
                // log window settings
                LogWindowWidth = ini.ReadIntDefault("LogWindowWidth", 0, "General");
                LogWindowHeight = ini.ReadIntDefault("LogWindowHeight", 0, "General");
                LogWindowX = ini.ReadIntDefault("LogWindowX", 0, "General");
                LogWindowY = ini.ReadIntDefault("LogWindowY", 0, "General");
                // led window settings
                LedWindowX = ini.ReadIntDefault("LedWindowX", 0, "General");
                LedWindowY = ini.ReadIntDefault("LedWindowY", 0, "General");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Exception [function Load(): general settings]: {0}", ex.ToString());
            }
            // advanced settings
            try
            {
                Advanced.LedsWindowOnTop = ini.ReadIntDefault("LedsWindowOnTop", 0, "Advanced") != 0;
                Advanced.LedsSoundAlert = ini.ReadIntDefault("LedsSoundAlert", 0, "Advanced") != 0;
                Advanced.FocusAllWindowsOnClick = ini.ReadIntDefault("FocusAllWindowsOnClick", 0, "Advanced") != 0;
                Advanced.StaticCameraCaption = ini.ReadIntDefault("StaticCameraCaption", 0, "Advanced") != 0;
                Advanced.DisableCameraCaptions = ini.ReadIntDefault("DisableCameraCaptions", 0, "Advanced") != 0;
                Advanced.AllCamerasWindowsOnTop = ini.ReadIntDefault("AllCamerasWindowsOnTop", 0, "Advanced") != 0;
                Advanced.ResizeWindowBy = ini.ReadIntDefault("ResizeWindowBy", 1, "Advanced");
            }
            catch (Exception ex)
            {
                // nowhere to put the output :D
                Logger.WriteLog("Exception [function Load(): advanced settings]: {0}", ex.ToString());
            }
#if DEBUG
            Logging = 1;
#endif

            // enable logging support if enabled

            if (Logging > 0)
            {
                Logger.WriteLog("Program started. Logging enabled!");
                if (LogWindow > 0)
                {
                    ShowOrActivateForm<LogForm>();
                }
            }

            try
            {
                GetCams();
                if (Cameras.Count <= 0)
                    ShowOrActivateForm<SettingsForm>();
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Exception [function Load: GetCams() call]: {0}", ex.ToString());
                ShowOrActivateForm<SettingsForm>();
            }



            if (MqttEnabled > 0)
            {
                try
                {
                    var clientIdRand = Guid.NewGuid().ToString();
                    Settings.MqttSettings.Server = ini.ReadDefault("Server", "127.0.0.1", "MQTT");
                    Settings.MqttSettings.Port = ini.ReadIntDefault("Port", 1883, "MQTT");
                    Settings.MqttSettings.Username = ini.ReadDefault("Username", "user", "MQTT");
                    Settings.MqttSettings.Password = ini.ReadDefault("Password", "pass", "MQTT");
                    Settings.MqttSettings.ClientID = ini.ReadDefault("ClientID", clientIdRand, "MQTT");
   
                    // load mqtt rules
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("Exception [function Load(): mqtt settings]: {0}", ex.ToString());
                    ShowOrActivateForm<SettingsForm>();

                    //if (!SettingsShowing)
                    //    settingsfrm.Show();
                }
                try
                {
                    var mqttrules = ini.GetKeys("MQTTRules");
                    var cnt = 0;
                    foreach (string ruleid in mqttrules)
                    {
                        var ruleval = ini.ReadDefault(ruleid, "", "MQTTRules");
                        if (ruleval != "")
                        {
                            string[] RuleParts = ruleval.Split(new string[] { "::" }, StringSplitOptions.None);
                            if (RuleParts[5] == "2")
                            {
                                LedsCount++;
                                //ledStates[RuleParts[0]] = 0;
                                LedStateManager.ledStates[RuleParts[0]] = 0;
                            }
                            var rule = Settings.NewMqttRule(int.Parse(ruleid), RuleParts[0], RuleParts[1], int.Parse(RuleParts[2]), RuleParts[3], RuleParts[4], int.Parse(RuleParts[5]));
                            MqttRulesSettings[cnt] = rule;
                        }
                        cnt++;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("Exception [function Load(): mqtt rules]: {0}", ex.ToString());
                }
            }


        }

    }
}
