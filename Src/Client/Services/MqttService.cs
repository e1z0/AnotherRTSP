using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnotherRTSP.Classes;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using AnotherRTSP.Components;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace AnotherRTSP.Services
{
    class MqttService
    {
        private MqttClient client;
        private Thread mqttThread;

        private void Connect(MqttClient client)
        {
            while (!client.IsConnected)
            {
                if (!Settings.MqttServiceRunning)
                    break;
                try
                {
                    // Attempt to connect
                    var clientId = Guid.NewGuid().ToString();
                    if (Settings.MqttSettings.ClientID != "")
                        clientId = Settings.MqttSettings.ClientID;
                    client.Connect(clientId, Settings.MqttSettings.Username, Settings.MqttSettings.Password);
                    // If connected successfully, subscribe to topics, publish messages, etc.
                    if (client.IsConnected)
                    {
                        Logger.WriteLog("Connected to MQTT server at: {0}:{1}", Settings.MqttSettings.Server, Settings.MqttSettings.Port);
                        foreach (MqttRulesDefinition rule in Settings.MqttRulesSettings)
                        {
                            if (rule != null && rule.Topic != "")
                            {
                                Logger.WriteLog("MQTT Subscribing to topic: {0}", rule.Topic);
                                client.Subscribe(new string[] { rule.Topic },
                            new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle connection error
                    Logger.WriteLog("Unable to connect to MQTT server at: {0}:{1} error: {2}", Settings.MqttSettings.Server, Settings.MqttSettings.Port, ex.Message);
                }

 

                // If connection failed, wait for a while before attempting to reconnect
                if (!client.IsConnected)
                {
                    Thread.Sleep(5000); // Wait for 5 seconds before retrying
                    Logger.WriteLog("MQTT Not connected...");
                }
            }
        }

        private void ServiceStart()
        {
            Logger.WriteLog("Mqtt service Thread is running...");


            client = new MqttClient(Settings.MqttSettings.Server, Settings.MqttSettings.Port, false, null, null, MqttSslProtocols.None);

            // register handler to a received message
            client.MqttMsgPublishReceived += MqttMsgPublishReceived;
            client.ConnectionClosed += OnDisconnect;

            Connect(client);
                
            Settings.MqttServiceRunning = true;

            while (mqttThread.ThreadState == ThreadState.Running)
            {
                Thread.Sleep(500);
            }
            Logger.WriteLog("MQTT Service finished work");
        }

        private void OnDisconnect(object sender, EventArgs e)
        {
            Logger.WriteLog("MQTT Server disconnected...");
            // Attempt to reconnect
            MqttClient cl = (MqttClient)sender;
            if (cl != null)
                Connect(cl);
        }


        public void StartService()
        {
            mqttThread = new Thread(ServiceStart);
            mqttThread.Start();
            if (mqttThread.IsAlive)
                Settings.MqttServiceRunning = true;
        }

        public void StopService()
        {
            mqttThread.Abort();
            Settings.MqttServiceRunning = false;
            if (client != null)
            {
                try
                {
                    client.Disconnect();
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("Unable to disconnect from mqtt server on mqtt thread service end: {0}", ex.StackTrace.ToString());
                }
            }
            Logger.WriteLog("Mqtt service Thread is done.");
        }

        public void WaitForCompletion()
        {
            mqttThread.Join();
        }



        static bool ContainsPropertyWithValue(Dictionary<string, string> dictionary, string propertyName, string propertyValue)
        {
            foreach (var pair in dictionary)
            {
                if (pair.Key == propertyName && pair.Value.ToLower() == propertyValue)
                {
                    return true;
                }
            }
            return false;
        }


        static void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            /* type: 0 raw, 1 json
            * ACTIONS:
            * 0 - Log message
            * 1 - Toast message (notify)
            * 2 - Led behavior
            */

            // handle message received
            var mqttMessage = Encoding.UTF8.GetString(e.Message);
            var topic = e.Topic;
            var rule = Settings.GetMqttRuleByTopic(e.Topic);
            if (rule != null)
            {
                Logger.WriteLog("MQTT debug topic: {0} msg: {1}", e.Topic, mqttMessage);
                // raw message
                if (rule.Type == 0)
                {
                    Logger.WriteLog("MQTT alert type: {0} topic: {1} msg: {2}", rule.Type, e.Topic, mqttMessage);
                    // log message
                    if (rule.Action == 0)
                    {
                        Logger.WriteLog("MQTT Received message: {0}", mqttMessage);
                    }
                    else if (rule.Action == 1)
                    {
                        string msg = "";
                        if (rule.Value2 != "")
                            msg = rule.Value2;
                        else
                            msg = mqttMessage;
                        AnotherRTSP.Components.Toast.showToast(msg);
                    }
                    else if (rule.Action == 2)
                    {
                        if (mqttMessage.Contains(rule.Value1))
                        {
                            LedStateManager.UpdateLedState(rule.Name, 1);
                            if (Settings.Advanced.LedsSoundAlert)
                                SystemSounds.Exclamation.Play();
                        }
                        else
                        {
                            LedStateManager.UpdateLedState(rule.Name, 0);
                        }
                    }

                }
                // json message
                else if (rule.Type == 1)
                {
                    var JsonDict = Tiny.Json.Decode<Dictionary<string, string>>(mqttMessage);

                    if (rule.Action == 0)
                    {
                        Logger.WriteLog("MQTT Received message: {0}", mqttMessage);
                    }

                    if (rule.Action == 1 && JsonDict.ContainsKey(rule.Value1))
                    {
                        bool containsAll = ContainsPropertyWithValue(JsonDict, rule.Value1, rule.Value2);
                        if (containsAll)
                        {
                            AnotherRTSP.Components.Toast.showToast("Activity on object: " + rule.Name);
                        }
                    }
                    // led behaviour
                    else if (rule.Action == 2 && JsonDict.ContainsKey(rule.Value1))
                    {
                        bool containsAll = ContainsPropertyWithValue(JsonDict, rule.Value1, rule.Value2);
                        // all true
                        if (containsAll)
                        {
                            LedStateManager.UpdateLedState(rule.Name, 1);
                            //Logger.WriteLog("LED [{0}] = {1}", rule.Name, 1);
                            if (Settings.Advanced.LedsSoundAlert)
                                SystemSounds.Exclamation.Play();
                        }
                        else
                        {
                            LedStateManager.UpdateLedState(rule.Name, 0);
                            //Logger.WriteLog("LED [{0}] = {1}", rule.Name, 0);

                        }

                    }


                }
            }
            else
            {
                Logger.WriteLog("MQTT topic invalid: " + e.Topic);
            }


        }

    }
}
