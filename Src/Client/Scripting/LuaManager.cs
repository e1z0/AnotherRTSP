using System;
using System.Collections.Generic;
using System.Linq;
using NLua;
//using KeraLua;
using AnotherRTSP.Classes;
using AnotherRTSP.Services;
using System.Threading;

namespace AnotherRTSP.Scripting
{
    class LuaScriptThread
    {
        public Thread Thread { get; set; }
        public volatile bool Running = true;
        public bool IsRunning()
        {
            return this.Running;
        }
    }

    public static class LuaManager
    {
        private static Lua lua = new Lua();
        public static List<LuaScriptItem> Scripts = new List<LuaScriptItem>();
        // Script Threads
        private static List<LuaScriptThread> scriptThreads = new List<LuaScriptThread>();
        // List of registered MQTT callbacks
        private static Dictionary<string, LuaFunction> mqttSubscriptions = new Dictionary<string, LuaFunction>();
        public static bool Initialized { get; private set; }


        // general functions
        public static void Initialize()
        {
            if (Initialized) return;
            Initialized = true;
            lua.LoadCLRPackage();
            lua["CameraManager"] = new LuaCameraManager();
            //lua["Logger"] = new LuaLogger();
            lua.RegisterFunction("Log", typeof(LuaLogger).GetMethod("WriteLog"));
            lua.RegisterFunction("Sleep", null, typeof(System.Threading.Thread).GetMethod("Sleep", new[] { typeof(int) }));
            lua.RegisterFunction("SubscribeToMqtt", typeof(LuaManager).GetMethod("SubscribeToMqtt"));
            // BallonTip
            lua.RegisterFunction("BalloonTip", typeof(AnotherRTSP.Classes.TrayIconManager).GetMethod("ShowBalloon"));
            // NotificationForm.ShowNotification
            lua.RegisterFunction("Notify", typeof(AnotherRTSP.Forms.NotificationForm).GetMethod("ShowNotification"));
            lua.RegisterFunction("Sound", typeof(SoundManager).GetMethod("PlaySound"));
            LoadScripts();

            // Start all startup scripts
            foreach (var script in Scripts.Where(s => s.RunOnStartup))
            {
                RunScriptThreaded(script);
            }
        }

        public static void Shutdown()
        {
            Logger.WriteLog("[Script] Shutting down all Lua script threads...");
            foreach (var scriptThread in scriptThreads)
            {
                if (scriptThread != null)
                {
                    scriptThread.Running = false;
                }
            }

            // Optionally wait for all threads to really exit
            foreach (var scriptThread in scriptThreads)
            {
                if (scriptThread.Thread != null && scriptThread.Thread.IsAlive)
                {
                    scriptThread.Thread.Join();
                }
            }

            scriptThreads.Clear();
        }

        public static void LoadScripts()
        {
            Scripts = YmlSettings.Data.Scripts ?? new List<LuaScriptItem>();
        }

        public static void SaveScripts()
        {
            YmlSettings.Data.Scripts = Scripts;
            YmlSettings.Save();
        }

        public static void RunScript(string name)
        {
            var script = Scripts.FirstOrDefault(x => x.Name == name);
            if (script != null)
            {
                var scriptThread = new LuaScriptThread();
                scriptThread.Thread = new Thread(() =>
                {
                    try
                    {
                        // Inject Running BEFORE script loads!
                        lua.RegisterFunction("Running", scriptThread, scriptThread.GetType().GetMethod("IsRunning"));
                        lua.DoString(script.Source);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog("[Script] Script error: " + ex.Message);
                    }
                });

                scriptThreads.Add(scriptThread);
                scriptThread.Thread.Start();
            }
        }

        private static void RunScriptThreaded(LuaScriptItem script)
        {
            var scriptThread = new LuaScriptThread();
            scriptThread.Thread = new Thread(() =>
            {
                try
                {
                    // Inject Running BEFORE script loads!
                    lua.RegisterFunction("Running", scriptThread, scriptThread.GetType().GetMethod("IsRunning"));
                    lua.DoString(script.Source);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("[Script] Script error: " + ex.Message);
                }
            });

            scriptThreads.Add(scriptThread);
            scriptThread.Thread.Start();
        }

        public static void AddOrUpdateScript(string name, string source, string description, bool startup)
        {

            var script = Scripts.FirstOrDefault(x => x.Name == name);
            if (script == null)
            {
                Scripts.Add(new LuaScriptItem { Name = name, Source = Utils.NormalizeLineEndings(source, "\n"), Description = description, RunOnStartup = startup });
            }
            else
            {
                script.Source = Utils.NormalizeLineEndings(source, "\n");
                script.Description = description;
                script.RunOnStartup = startup;
            }
            SaveScripts();
        }

        public static void RemoveScript(string name)
        {
            Scripts.RemoveAll(x => x.Name == name);
            SaveScripts();
        }




        public static void SubscribeToMqtt(string topic, string luaCallbackName)
        {
            LuaFunction func = lua[luaCallbackName] as LuaFunction;
            if (func != null)
            {
                if (MqttService.Instance.IsConnected)
                {
                    mqttSubscriptions[topic] = func;
                    MqttService.Instance.SubscribeTopicFromLua(topic);
                    Logger.WriteLog("[Script] Subscribed script to topic: {0}", topic);
                    Logger.WriteLog("[Debug] Current MQTT Lua subscriptions:");
                    foreach (var kvp in mqttSubscriptions)
                    {
                        Logger.WriteLog("[Debug] -> Topic: {0} Function: {1}", kvp.Key, kvp.Value);
                    }
                }
                else
                {
                    Logger.WriteLog("Mqtt is not yet connected!");
                }
            }
            else
            {
                Logger.WriteLog("[Script] Failed to subscribe to topic {0} - callback {1} not found", topic, luaCallbackName);
            }
        }

        public static void HandleIncomingMqtt(string topic, string message)
        {
            LuaFunction func;
            //Logger.WriteLog("[Debug] LuaManager checking topic: {0}", topic);
            if (mqttSubscriptions.TryGetValue(topic, out func))
            {
                try
                {
                    func.Call(message);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("[Script] Error in MQTT callback: " + ex.Message);
                }
            }
        }








    }
    // interfaces
    public class LuaCameraManager
    {
        public void EnableCamera(string name)
        {
            var item = YmlSettings.Data.Cameras.FirstOrDefault(x => x.Name == name);
            if (item != null)
                Camera.EnableCamera(item);
        }

        public void DisableCamera(string name)
        {
            var item = YmlSettings.Data.Cameras.FirstOrDefault(x => x.Name == name);
            if (item != null)
                Camera.DisableCamera(item);
        }
    }

    public static class LuaLogger
    {
        public static void WriteLog(string message)
        {
            Logger.WriteLog("[Script] " + message);
        }
    }





}
