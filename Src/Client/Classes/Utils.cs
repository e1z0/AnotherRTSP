/*
 * Copyright (c) 2024-2025 e1z0. All Rights Reserved.
 * Licensed under the Business Source License 1.1.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AnotherRTSP.Classes;
using EasyPlayerNetSDK;

namespace AnotherRTSP.Classes
{
    class Utils
    {
        // application exit event
        public static void AppExit()
        {
            if (YmlSettings.IsInit)
            {
                // close all open streams
                foreach (CameraItem localItem in YmlSettings.Data.Cameras)
                {
                    CameraItem item = localItem;
                    if (!item.Disabled)
                    {
                        Camera.Stop(item);
                    }
                }

            }
            try
            {
                PlayerSdk.EasyPlayer_Release();
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Error at AppExit() {0}", ex.StackTrace.ToString());
            }



            Application.DoEvents();

            Application.Exit();
        }

        public static void About()
        {
            try
            {
                var aboutForm = new AnotherRTSP.Forms.AboutForm();
                aboutForm.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("AnotherRTSP v{0}. Copyright (c) 2024-2025 e1z0. All Rights Reserved\nLicensed under MIT License.", YmlSettings.VERSION), "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public static string NormalizeLineEndings(string text, string newline = "\r\n")
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text
                .Replace("\r\n", "\n")   // normalize first to \n
                .Replace("\r", "\n")     // fix any Mac-style \r
                .Replace("\n", newline); // finally normalize to desired style
        }

        public static bool DetectInvalidResolution()
        {
            bool badresolution = false;
            Screen primaryScreen = Screen.PrimaryScreen;
            var aspect = (float)primaryScreen.Bounds.Width / primaryScreen.Bounds.Height;

            switch (primaryScreen.Bounds.Height)
            {
                case 600:
                    badresolution = true;
                    break;
                case 720:
                    badresolution = true;
                    break;
                case 768:
                    badresolution = true;
                    break;
                case 800:
                    badresolution = true;
                    break;
                default:
                    badresolution = false;
                    break;
            }

            Logger.WriteDebug("Detected aspect ratio: {0} working area: {1} bounds: {2} Bad resolution: {3}", aspect, primaryScreen.WorkingArea, primaryScreen.Bounds, badresolution);
            return badresolution;
        }
    }
}
