using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace EasyPlayerNetSDK
{
    public class PlayerSdk
    {
        #region data structure

        /// <summary>
        /// Encoding format
        /// </summary>
        public enum RENDER_FORMAT
        {

            /// DISPLAY_FORMAT_YV12 -> 842094169
            DISPLAY_FORMAT_YV12 = 842094169,

            /// DISPLAY_FORMAT_YUY2 -> 844715353
            DISPLAY_FORMAT_YUY2 = 844715353,

            /// DISPLAY_FORMAT_UYVY -> 1498831189
            DISPLAY_FORMAT_UYVY = 1498831189,

            /// DISPLAY_FORMAT_A8R8G8B8 -> 21
            DISPLAY_FORMAT_A8R8G8B8 = 21,

            /// DISPLAY_FORMAT_X8R8G8B8 -> 22
            DISPLAY_FORMAT_X8R8G8B8 = 22,

            /// DISPLAY_FORMAT_RGB565 -> 23
            DISPLAY_FORMAT_RGB565 = 23,

            /// DISPLAY_FORMAT_RGB555 -> 25
            DISPLAY_FORMAT_RGB555 = 25,

            /// DISPLAY_FORMAT_RGB24_GDI -> 26
            DISPLAY_FORMAT_RGB24_GDI = 26,
        }

        /// <summary>
        /// Frame structure information
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct EASY_FRAME_INFO
        {
            public uint codec;                  /* Audio and video format */

            public uint type;                   /* Video frame type */
            public byte fps;                    /* Video frame rate */
            public ushort width;               /* video width */
            public ushort height;              /* video height */

            public uint reserved1;         /* Reserve parameter 1 */
            public uint reserved2;         /* Reserve parameter 2 */

            public uint sample_rate;       /* Audio sample rate */
            public uint channels;          /* Number of audio channels */
            public uint bits_per_sample;        /* Audio sampling accuracy */

            public uint length;                /* Audio and video frame size */
            public uint timestamp_usec;        /* timestamp, subtle */
            public uint timestamp_sec;          /* timestamp seconds */

            public float bitrate;                       /* bitrate */
            public float losspacket;                    /* Packet loss rate */
        }

        /// <summary>
        /// coordinate
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct Point
        {
            /// LONG->int
            public int x;

            /// LONG->int
            public int y;
        }

        /// <summary>
        /// Pixel information
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct tagRECT
        {
            public int left;

            public int top;

            public int right;

            public int bottom;
        }

        #endregion

        /// <summary>
        /// Stream callback
        /// </summary>
        /// <param name="channelId">Channel ID, EasyPlayer_OpenStream function return value. Channel ID.</param>
        /// <param name="userPtr">channel pointer.</param>
        /// <param name="_frameType">Frame data type.</param>
        /// <param name="pBuf">data pointer.</param>
        /// <param name="_frameInfo">frame data structure.</param>
        /// <returns>System.Int32.</returns>
        public delegate int MediaSourceCallBack(int _channelId, IntPtr _channelPtr, int _frameType, IntPtr pBuf, ref EASY_FRAME_INFO _frameInfo);

        /// <summary>
        /// EasyPlayer initialization.
        /// </summary>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_Init@@YAHPAD@Z")]
        public static extern int EasyPlayer_Init(string key = "6D75724D7A4969576B5A7341706B56666F4C705A3065314659584E35556C525455454E73615756756443356C6547556A567778576F502F522F32566863336B3D");

        /// <summary>
        /// Resource release.
        /// </summary>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "?EasyPlayer_Release@@YAXXZ")]
        public static extern void EasyPlayer_Release();

        /// <summary>
        /// Start streaming.
        /// </summary>
        /// <param name="url">Media address.</param>
        /// <param name="hWnd">window handle.</param>
        /// <param name="renderFormat">Encoding format.</param>
        /// <param name="rtpovertcp">The transmission mode of the pull stream, 0=udp, 1=tcp.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="userPtr">User defined pointer.</param>
        /// <param name="callback">Data callback.</param>
        /// <param name="bHardDecode">Hardware decoding 1=yes, 0=no.</param>
        /// <param name="startTime">Playback start time, fill in null for live stream.</param>
        /// <param name="endTime">Playback end time, the live stream is filled in null.</param>
        /// <param name="fScale">Playback magnification, live stream is invalid.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_OpenStream@@YAHPBDPAUHWND__@@W4__RENDER_FORMAT@@H00P6GHHPAHHPADPAUtagEASY_FRAME_INFO@@@ZPAX_N44M@Z")]
        public static extern int EasyPlayer_OpenStream(string url, IntPtr hWnd, RENDER_FORMAT renderFormat, int rtpovertcp, string username, string password, MediaSourceCallBack callback, IntPtr userPtr, bool bHardDecode = true, string startTime = null, string endTime = null, float fScale = 1.0f);

        /// <summary>
        /// Easies the player_ close stream.
        /// </summary>
        /// <param name="channelId">Channel ID, EasyPlayer_OpenStream function return value.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_CloseStream@@YAXH@Z")]
        public static extern int EasyPlayer_CloseStream(int channelId);

        /// <summary>
        /// Set the number of buffered frames for the current stream playback.
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <param name="cache">Number of cached video frames.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_SetFrameCache@@YAHHH@Z")]
        public static extern int EasyPlayer_SetFrameCache(int channelId, int cache);

        /// <summary>
        /// The player displays proportionally.
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <param name="shownToScale">0=Display the entire window area, 1=Display proportionally.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_SetShownToScale@@YAHHH@Z")]
        public static extern int EasyPlayer_SetShownToScale(int channelId, int shownToScale);

        /// <summary>
        /// Set decoding type
        /// </summary>
        /// <param name="channelId">Channel ID, EasyPlayer_OpenStream function return value.</param>
        /// <param name="decodeKeyframeOnly">0=decode all frames, 1=decode only keyframes.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_SetDecodeType@@YAHHH@Z")]
        public static extern int EasyPlayer_SetDecodeType(int channelId, int decodeKeyframeOnly);

        /// <summary>
        /// Set the rendering area when the video is displayed.
        /// </summary>
        /// <param name="channelId">Channel ID, EasyPlayer_OpenStream function return value.</param>
        /// <param name="lpSrcRect">Set the rectangular structure of the rendering area.</param>
        /// <returns>System.Int32.</returns>
        public static int EasyPlayer_SetRenderRect(int channelId, Rect lpSrcRect)
        {
            tagRECT rect = new tagRECT { left = (int)lpSrcRect.X, bottom = (int)lpSrcRect.Height, right = (int)lpSrcRect.Width, top = (int)lpSrcRect.Y };
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rect));
            Marshal.StructureToPtr(rect, ptr, true);
            int ret = EasyPlayer_SetRenderRect(channelId, ptr);
            Marshal.FreeHGlobal(ptr);
            return 0;
        }

        /// <summary>
        /// Set the rendering area when the video is displayed.
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <param name="lpSrcRect">Sets the rectangular structure of the rendering area.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_SetRenderRect@@YAHHPAUtagRECT@@@Z")]
        private static extern int EasyPlayer_SetRenderRect(int channelId, IntPtr lpSrcRect);

        /// <summary>
        /// Set whether to display code stream information.
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <param name="show">0=Do not display, 1=Display</param>
        /// <returns></returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_ShowStatisticalInfo@@YAHHH@Z")]
        public static extern int EasyPlayer_ShowStatisticalInfo(int channelId, int show);

        /// <summary>
        /// Start playing audio.
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_PlaySound@@YAHH@Z")]
        public static extern int EasyPlayer_PlaySound(int channelId);

        /// <summary>
        /// Stop playing audio.
        /// </summary>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_StopSound@@YAHXZ")]
        public static extern int EasyPlayer_StopSound();

        /// <summary>
        /// Screenshots, currently [2017/11/29] only supports screenshots in YUV2 encoding format
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <param name="shotPath">shotPath folder in the default program path.</param>
        /// <returns></returns>
        public static int EasyPlayer_PicShot(int channelId, string shotPath = null)
        {
            int ret = -99;
            string path = shotPath ?? System.AppDomain.CurrentDomain.BaseDirectory + "shotPath\\";

            if (!System.IO.Directory.Exists(path))//Create the file folder if it does not exist　　             　　                
                System.IO.Directory.CreateDirectory(path);//Create this folder

            ret = EasyPlayer_SetManuPicShotPath(channelId, path);
            ret = EasyPlayer_StartManuPicShot(channelId);
            System.Threading.Thread.Sleep(200);
            ret = EasyPlayer_StopManuPicShot(channelId);
            return ret;
        }

        /// <summary>
        /// Easies the player_ set manu pic shot path.
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <param name="shotPath">The shot path.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_SetManuPicShotPath@@YAHHPBD@Z")]
        private static extern int EasyPlayer_SetManuPicShotPath(int channelId, string shotPath);

        /// <summary>
        /// Easies the player_ start manu pic shot.
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_StartManuPicShot@@YAHH@Z")]
        private static extern int EasyPlayer_StartManuPicShot(int channelId);

        /// <summary>
        /// Easies the player_ stop manu pic shot.
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_StopManuPicShot@@YAHH@Z")]
        private static extern int EasyPlayer_StopManuPicShot(int channelId);

        /// <summary>
        /// Audio and video data recording, the recording format is MP4.
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <param name="recordPath">Default program path.</param>
        /// <returns></returns>
        public static int EasyPlayer_StartManuRecording(int channelId, string recordPath = null)
        {
            int ret = -99;
            string path = recordPath ?? System.AppDomain.CurrentDomain.BaseDirectory + "record\\";

            if (!System.IO.Directory.Exists(path))//Create the file folder if it does not exist　　             　　                
                System.IO.Directory.CreateDirectory(path);//Create this folder
            ret = EasyPlayer_SetManuRecordPath(channelId, path);
            ret = EasyPlayer_StartManuRecording(channelId);
            return ret;
        }

        /// <summary>
        /// Audio and video data recording, the recording format is MP4.
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_StartManuRecording@@YAHH@Z")]
        private static extern int EasyPlayer_StartManuRecording(int channelId);

        /// <summary>
        /// Easies the player_ set manu record path.
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <param name="recordPath">The record path.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_SetManuRecordPath@@YAHHPBD@Z")]
        private static extern int EasyPlayer_SetManuRecordPath(int channelId, string recordPath);

        /// <summary>
        /// Stop recording.
        /// </summary>
        /// <param name="channelId">Channel ID, return value of EasyPlayer_OpenStream function.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(@"Lib\libEasyPlayer-RTSP.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?EasyPlayer_StopManuRecording@@YAHH@Z")]
        public static extern int EasyPlayer_StopManuRecording(int channelId);
    }
}
