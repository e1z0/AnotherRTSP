/*
 * Copyright (c) 2024-2025 e1z0. All Rights Reserved.
 * Licensed under the Business Source License 1.1.
 */


using System;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using AnotherRTSP.Classes;
using AnotherRTSP.Scripting;
using System.Collections.Generic;

namespace AnotherRTSP.Services
{
    class MqttService
    {
        private static MqttService _instance = new MqttService();
        public static MqttService Instance
        {
            get { return _instance; }
        }

        public static List<string> LuaDynamicSubscriptions = new List<string>();
        private MqttClient client;
        private Thread mqttThread;
        private volatile bool running = false;
        private volatile bool connected = false;

        private MqttService() { }

        public bool IsConnected
        {
            get { return client != null && client.IsConnected; }
        }

        public void StartService()
        {
            if (mqttThread != null && mqttThread.IsAlive)
                return;

            running = true;
            mqttThread = new Thread(ServiceThread);
            mqttThread.Start();

            Logger.WriteLog("[MQTT] Service thread started.");
        }


        public void StopService()
        {
            running = false;

            if (client != null)
            {
                try
                {
                    client.MqttMsgPublishReceived -= MqttMsgPublishReceived;
                    client.ConnectionClosed -= OnConnectionClosed;
                }
                catch { }

                if (client.IsConnected)
                {
                    try
                    {
                        client.Disconnect();
                        Logger.WriteLog("[MQTT] Client disconnected.");
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog("[MQTT] Error during client disconnect: {0}", ex.Message);
                    }
                }
                client = null;
            }

            if (mqttThread != null && mqttThread.IsAlive)
            {
                Logger.WriteLog("[MQTT] Waiting for service thread to exit...");
                bool exited = mqttThread.Join(5000); // Wait max 5 seconds
                if (!exited)
                {
                    Logger.WriteLog("[MQTT] WARNING: MQTT thread did not exit in time. Application may hang.");
                }
                else
                {
                    Logger.WriteLog("[MQTT] MQTT service thread stopped cleanly.");
                }
            }
        }

        private void ServiceThread()
        {
            while (running)
            {
                try
                {
                    Logger.WriteLog("[MQTT] Trying to connect to broker...");

                    client = new MqttClient(YmlSettings.Data.MQTT.Server, YmlSettings.Data.MQTT.Port, false, null, null, MqttSslProtocols.None);

                    client.MqttMsgPublishReceived += MqttMsgPublishReceived;
                    client.ConnectionClosed += OnConnectionClosed;

                    string clientId = !string.IsNullOrEmpty(YmlSettings.Data.MQTT.ClientID) ? YmlSettings.Data.MQTT.ClientID : Guid.NewGuid().ToString();
                    client.Connect(clientId, YmlSettings.Data.MQTT.Username, YmlSettings.Data.MQTT.Password);

                    if (client.IsConnected)
                    {
                        Logger.WriteLog("[MQTT] Connected to broker at {0}:{1}", YmlSettings.Data.MQTT.Server, YmlSettings.Data.MQTT.Port);
                        connected = true;
                        // After YML topics subscribed
                        foreach (var luaTopic in LuaDynamicSubscriptions)
                        {
                            try
                            {
                                Logger.WriteLog("[MQTT] Re-subscribing Lua dynamic topic: " + luaTopic);
                                client.Subscribe(new string[] { luaTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteLog("[MQTT] Error re-subscribing Lua topic {0}: {1}", luaTopic, ex.Message);
                            }
                        }

                        while (running && client.IsConnected)
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("[MQTT] Connection failed: {0}", ex.Message);
                }

                if (client != null && client.IsConnected)
                {
                    try
                    {
                        client.Disconnect();
                        Logger.WriteLog("[MQTT] Client disconnected.");
                    }
                    catch { }
                }

                client = null;
                connected = false;

                if (running)
                {
                    Logger.WriteLog("[MQTT] Waiting 2 seconds before reconnect...");
                    Thread.Sleep(2000);
                }
            }

            Logger.WriteLog("[MQTT] Service thread exiting...");
        }

        private void OnConnectionClosed(object sender, EventArgs e)
        {
            Logger.WriteLog("[MQTT] Connection to broker closed.");
        }

        private static void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var topic = e.Topic;
            var payload = Encoding.UTF8.GetString(e.Message);
            LuaManager.HandleIncomingMqtt(topic, payload);
        }

        public void SubscribeTopicFromLua(string topic)
        {
            if (client == null)
            {
                Logger.WriteLog("[MQTT ERROR] Cannot subscribe: client is null.");
                return;
            }

            if (!client.IsConnected)
            {
                Logger.WriteLog("[MQTT ERROR] Cannot subscribe: client not connected.");
                return;
            }

            try
            {
                client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                Logger.WriteLog("[MQTT] Subscribed to topic: {0}", topic);
                if (!LuaDynamicSubscriptions.Contains(topic))
                {
                    LuaDynamicSubscriptions.Add(topic);
                    Logger.WriteLog("[MQTT] Tracked Lua dynamic subscription: " + topic);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("[MQTT] Subscription error for {0}: {1}", topic, ex.Message);
            }
        }
    }
}