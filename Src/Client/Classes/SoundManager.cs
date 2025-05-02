using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Media;

namespace AnotherRTSP.Classes
{
    public static class SoundManager
    {
        private static readonly string[] CommonExtensions = { ".wav", ".mp3", ".ogg", ".flac", ".m4a" };

        public static void PlaySound(string filename)
        {
            try
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string soundsDirectory = Path.Combine(appDirectory, "sounds");

                // Check if filename already has an extension
                string ext = Path.GetExtension(filename).ToLowerInvariant();
                string fullPath = Path.Combine(soundsDirectory, filename);

                if (string.IsNullOrEmpty(ext))
                {
                    // Try with common extensions
                    bool found = false;
                    foreach (var extension in CommonExtensions)
                    {
                        string tryPath = Path.Combine(soundsDirectory, filename + extension);
                        if (File.Exists(tryPath))
                        {
                            fullPath = tryPath;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        Logger.WriteLog("[Sound] Sound file not found with common extensions: " + filename);
                        return;
                    }
                }
                else
                {
                    // If extension given, check directly
                    if (!File.Exists(fullPath))
                    {
                        Logger.WriteLog("[Sound] Sound file not found: " + fullPath);
                        return;
                    }
                }

                // Now play the found sound
                string foundExt = Path.GetExtension(fullPath).ToLowerInvariant();

                if (foundExt == ".wav")
                {
                    new Thread(() =>
                    {
                        try
                        {
                            using (SoundPlayer player = new SoundPlayer(fullPath))
                            {
                                player.Load();
                                player.PlaySync();
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLog("[Sound] Error playing WAV: " + ex.Message);
                        }
                    }) { IsBackground = true }.Start();
                }
                else
                {
                    Logger.WriteLog("[Sound] Error playing media file: " + filename + " it may not be supported");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("[Sound] General sound error: " + ex.Message);
            }
        }
    }
}
