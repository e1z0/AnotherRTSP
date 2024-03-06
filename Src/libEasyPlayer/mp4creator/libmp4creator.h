/*
	Copyright (c) 2013-2014 EasyDarwin.ORG.  All rights reserved.
	Github: https://github.com/EasyDarwin
	WEChat: EasyDarwin
	Website: http://www.EasyDarwin.org
*/
#ifndef __LIB_MP4_CREATOR_H__
#define __LIB_MP4_CREATOR_H__


#define LIB_MP4CREATOR_API __declspec(dllexport)


#ifndef MEDIA_TYPE_VIDEO
#define MEDIA_TYPE_VIDEO		0x01
#endif
#ifndef MEDIA_TYPE_AUDIO
#define MEDIA_TYPE_AUDIO		0x02
#endif


#define	VIDEO_CODEC_H264		0x1C
#define AUDIO_CODEC_AAC			0x40

typedef void *MP4C_Handler;


//��ʼ�����
int	LIB_MP4CREATOR_API	MP4C_Init(MP4C_Handler *handler);
//����¼���ļ�
int	LIB_MP4CREATOR_API	MP4C_CreateMp4File(MP4C_Handler handler, char *filename, unsigned int _file_buf_size/*�ڴ滺��,����������֮��Ż�д���ļ�, ���Ϊ0��ֱ��д���ļ�*/);
//������Ƶ����
int	LIB_MP4CREATOR_API	MP4C_SetMp4VideoInfo(MP4C_Handler handler, unsigned int codec,	unsigned short width, unsigned short height, unsigned int fps);
//������Ƶ����
int	LIB_MP4CREATOR_API	MP4C_SetMp4AudioInfo(MP4C_Handler handler, unsigned int codec,	unsigned int sampleFrequency, unsigned int channel);
//����H264�е�SPS
int	LIB_MP4CREATOR_API	MP4C_SetH264Sps(MP4C_Handler handler, unsigned short sps_len, unsigned char *sps);
//����H264�е�PPS
int	LIB_MP4CREATOR_API	MP4C_SetH264Pps(MP4C_Handler handler, unsigned short pps_len, unsigned char *pps);

//��֡��������ȡSPS��PPS,��ȡ��������MP4C_SetH264Sps��MP4C_SetH264Pps
//֡����������� start code
int LIB_MP4CREATOR_API  MP4C_GetSPSPPS(char *pbuf, int bufsize, char *_sps, int *_spslen, char *_pps, int *_ppslen);


/*
д��ý������
//������������Ƶ����Ƶ, ֱ�ӵ���MP4C_AddFrameд�뼴��, ���ڲ�������ж�,��д��һ��GOP��д���Ӧʱ��ε���Ƶ
//����Ƶ��֧��AAC   8KHz ��  44.1KHz
pbuf������start code 00 00 00 01Ҳ����û��,�����������ж�
���û��start code, �������MP4C_SetH264Sps��MP4C_SetH264Pps������Ӧ��sps��pps

֡����Ϊ���¼������ʱ�ɲ�����MP4C_SetH264Sps��MP4C_SetH264Pps:
1.  start code + sps + start code + pps + start code + idr
2.  start code + sps            start code + pps   ��start code+spsΪһ֡, start code+ppsΪһ֡
*/
int	LIB_MP4CREATOR_API	MP4C_AddFrame(MP4C_Handler handler, unsigned int mediatype, unsigned char *pbuf, unsigned int framesize, unsigned char keyframe, unsigned int timestamp_sec, unsigned int timestamp_rtp, unsigned int fps);

//�ر�MP4�ļ�, �����ļ���С
unsigned int LIB_MP4CREATOR_API	MP4C_CloseMp4File(MP4C_Handler handler);

//�ͷž��
int	LIB_MP4CREATOR_API	MP4C_Deinit(MP4C_Handler *handler);



/*
MP4C_Handler	mp4Handle = NULL;

MP4C_Init(&mp4Handle);
MP4C_SetMp4VideoInfo(mp4Handle, VIDEO_CODEC_H264, frameinfo.width, frameinfo.height, frameinfo.fps);
MP4C_SetMp4AudioInfo(mp4Handle, AUDIO_CODEC_AAC, 8000, 1);
MP4C_CreateMp4File(mp4Handle, szMp4Filename, 1024*1024*2);


MP4C_AddFrame(mp4Handle, MEDIA_TYPE_VIDEO, (unsigned char*)pbuf, frameinfo.length, frameinfo.type, frameinfo.timestamp_sec, frameinfo.rtptimestamp, pts);
MP4C_AddFrame(mp4Handle, MEDIA_TYPE_AUDIO, (unsigned char*)pAudBuf, lSize, 0x01, frameinfo.timestamp_sec, frameinfo.rtptimestamp, fps);


MP4C_CloseMp4File(mp4Handle);
MP4C_Deinit(&mp4Handle);
	
*/


#endif
