/*
	Copyright (c) 2013-2014 EasyDarwin.ORG.  All rights reserved.
	Github: https://github.com/EasyDarwin
	WEChat: EasyDarwin
	Website: http://www.easydarwin.org
	Author: Sword@easydarwin.org
*/
#ifndef __SHARE_MEMORY_QUEUE_H__
#define __SHARE_MEMORY_QUEUE_H__

#ifdef _WIN32
#include <winsock2.h>
#else
#include "sync_shm.h"

#define	SYNC_VID_SHM_KEY		2000
#endif
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
//#include "common.h"

#define		LOCK_WAIT_TIMES		1000

#ifndef MEDIA_TYPE_VIDEO
#define	MEDIA_TYPE_VIDEO	0x01
#endif
#ifndef MEDIA_TYPE_AUDIO
#define	MEDIA_TYPE_AUDIO	0x02
#endif
#ifndef MEDIA_TYPE_EVENT
#define	MEDIA_TYPE_EVENT	0x04
#endif

typedef struct __MEDIA_FRAME_INFO
{
	unsigned int	codec;			/* ����Ƶ��ʽ */
	unsigned int	type;			/* ��Ƶ֡���� */
	unsigned char	fps;			/* ��Ƶ֡�� */
	unsigned short	width;			/* ��Ƶ�� */
	unsigned short  height;			/* ��Ƶ�� */

	unsigned int	reserved1;		/* ��������1 */
	unsigned int	reserved2;		/* ��������2 */

	unsigned int	sample_rate;	/* ��Ƶ������ */
	unsigned int	channels;		/* ��Ƶ������ */
	// ������Ƶ�������� [5/9/2016 SwordTwelve]
	unsigned int	bits_per_sample;	/* ��Ƶ�������� */

	unsigned int	length;			/* ����Ƶ֡��С */
	unsigned int    timestamp_usec;	/* ʱ���,΢�� */
	unsigned int	timestamp_sec;	/* ʱ��� �� */
	
	float			bitrate;		/* ������ */
	float			losspacket;		/* ������ */
}MEDIA_FRAME_INFO;

typedef struct __SS_BUF_T
{
	unsigned int		channelid;
	unsigned int		mediatype;
#define BUF_QUE_FLAG	0x0FFFFFFF
	unsigned int flag;
	MEDIA_FRAME_INFO	frameinfo;
	unsigned int timestamp;
}SS_BUF_T;


typedef struct __FRAMEINFO_LIST_T
{
	unsigned int	pos;
	unsigned int	timestamp_sec;
	unsigned int	rtp_timestamp;
}FRAMEINFO_LIST_T;
typedef struct __SS_HEADER_T
{
	//int		headersize;
	unsigned int		bufsize;
	unsigned int		writepos;
	unsigned int		readpos;
	unsigned int		totalsize;
	unsigned int		videoframes;	//��Ƶ֡��
	unsigned int		isfull;

	unsigned int		clear_flag;	//��ձ�ʶ

	unsigned int		framelistNum;
	unsigned int		maxframeno;
	unsigned int		frameno;

	//char	*pbuf;
}SS_HEADER_T;

typedef struct __SHARE_INFO_T
{
	unsigned int		id;
	wchar_t	name[36];
}SHARE_INFO_T;


typedef struct __SS_QUEUE_OBJ_T
{
	unsigned int	channelid;
	SHARE_INFO_T	shareinfo;
	
#ifdef _WIN32
	HANDLE			hSSHeader;
	HANDLE			hSSFrameList;
	HANDLE			hSSData;
#else
	key_t			shmkey;
	int				shmHdrid;
	int				shmDatid;
#endif
	SS_HEADER_T		*pQueHeader;
	char			*pQueData;
	FRAMEINFO_LIST_T	*pFrameinfoList;

	HANDLE			hMutex;
}SS_QUEUE_OBJ_T;


#if defined (__cplusplus)
extern "C"
{
#endif
	int		SSQ_Init(SS_QUEUE_OBJ_T *pObj, unsigned int sharememory, unsigned int channelid, wchar_t *sharename, unsigned int bufsize, unsigned int prerecordsecs, unsigned int createsharememory);
	int		SSQ_Deinit(SS_QUEUE_OBJ_T *pObj);

	int		SSQ_SetClearFlag(SS_QUEUE_OBJ_T *pObj, int _flag);
	int		SSQ_Clear(SS_QUEUE_OBJ_T *pObj);
	int		SSQ_AddData(SS_QUEUE_OBJ_T *pObj, unsigned int channelid, unsigned int mediatype, MEDIA_FRAME_INFO *frameinfo, char *pbuf);
	int		SSQ_GetData(SS_QUEUE_OBJ_T *pObj, unsigned int *channelid, unsigned int *mediatype, MEDIA_FRAME_INFO *frameinfo, char *pbuf);
	int		SSQ_GetDataByPosition(SS_QUEUE_OBJ_T *pObj, unsigned int position, unsigned int clearflag, unsigned int *channelid, unsigned int *mediatype, MEDIA_FRAME_INFO *frameinfo, char *pbuf);
	int		SSQ_AddFrameInfo(SS_QUEUE_OBJ_T *pObj, unsigned int _pos, MEDIA_FRAME_INFO *frameinfo);



	int		SSQ_TRACE(char* szFormat, ...);
#if defined (__cplusplus)
}
#endif


#endif
