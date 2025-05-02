using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnotherRTSP.Classes
{
    class Mqtt
    {
        public static MqttRulesDefinition GetMqttRuleByTopic(string topic)
        {
            foreach (MqttRulesDefinition rule in YmlSettings.Data.MQTTRules)
            {
                if (topic == rule.Topic)
                    return rule;
            }
            return null;
        }

        public static MqttRulesDefinition NewMqttRule(string name, string topic, int type, string value, string value2, int action)
        {
            var rule = new MqttRulesDefinition();
            rule.Name = name;
            rule.Topic = topic;
            rule.Type = type;
            rule.Value1 = value;
            rule.Value2 = value2;
            rule.Action = action;
            return rule;
        }
    }
}
