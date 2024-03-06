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
        public void ServiceStart()
        {
            Logger.WriteLog("Mqtt service Thread is running...");

            var client = new MqttClient(Settings.MqttSettings.Server);

            // register handler to a received message
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            var clientId = Guid.NewGuid().ToString();
            client.Connect(Settings.MqttSettings.ClientID);

            foreach (MqttRulesDefinition rule in Settings.MqttRulesSettings)
            {
                if (rule != null && rule.Topic != "")
                    client.Subscribe(new string[] { rule.Topic },
                new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }
            Settings.MqttServiceRunning = true;

            while (Settings.MqttServiceRunning)
            {
                Thread.Sleep(500);
            }
            Logger.WriteLog("Mqtt service Thread is done.");
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


        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received
            var mqttMessage = Encoding.UTF8.GetString(e.Message);
            var topic = e.Topic;
            var rule = Settings.GetMqttRuleByTopic(e.Topic);
            if (rule != null)
            {
                // raw message
                if (rule.Type == 0)
                {
                    Logger.WriteLog("MQTT alert type: {0}", rule.Type);
                    // log message
                    if (rule.Action == 0)
                    {
                        Logger.WriteLog("MQTT Received message2: {0}", mqttMessage);
                    }
                }
                // json message
                else if (rule.Type == 1)
                {
                    var JsonDict = Tiny.Json.Decode<Dictionary<string, string>>(mqttMessage);


                    // led behaviour
                    if (rule.Action == 2 && JsonDict.ContainsKey(rule.Value1))
                    {
                        bool containsAll = ContainsPropertyWithValue(JsonDict, rule.Value1, rule.Value2);
                        // all true
                        if (containsAll)
                        {
                            LedStateManager.UpdateLedState(rule.Name,1);
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
                Logger.WriteLog("MQTT topic invalid");
            }
          

            }

    }
}
