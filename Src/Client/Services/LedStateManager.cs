/*
 * Copyright (c) 2024-2025 e1z0. All Rights Reserved.
 * Licensed under the Business Source License 1.1.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnotherRTSP.Classes;

namespace AnotherRTSP.Services
{
    public class LedStateManager
    {
        // Define ledStates dictionary
        public static Dictionary<string, int> ledStates = new Dictionary<string, int>();

        // Define event for led state change
        public static event EventHandler LedStateChanged;

        // Method to update ledStates and raise the event
        public static void UpdateLedState(string key, int value)
        {
            ledStates[key] = value;
            // Raise the event to notify subscribers about the change in ledStates
            OnLedStateChanged(EventArgs.Empty);
        }

        // Method to raise LedStateChanged event
        private static void OnLedStateChanged(EventArgs e)
        {
            LedStateChanged.Invoke(null, e);
        }
    }
}
