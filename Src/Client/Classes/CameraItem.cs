using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnotherRTSP.Classes
{
    public class CameraItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Disabled { get; set; }
        public int WWidth { get; set; }
        public int WHeight { get; set; }
        public int WX { get; set; }
        public int WY { get; set; }
        public bool isTCP { get; set; }
        public bool HardDecode { get; set; }

        public CameraItem()
        {
            Name = "Camera";
            Url = "rtsp://<camera>";
            Disabled = false;
            WWidth = 300;
            WHeight = 200;
            WX = 100;
            WY = 100;
            isTCP = true;
            HardDecode = false;
        }
    }
}
