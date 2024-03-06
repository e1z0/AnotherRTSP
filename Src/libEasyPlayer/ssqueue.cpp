/*
	Copyright (c) 2013-2014 EasyDarwin.ORG.  All rights reserved.
	Github: https://github.com/EasyDarwin
	WEChat: EasyDarwin
	Website: http://www.easydarwin.org
	Author: Sword@easydarwin.org
*/
#include "ssqueue.h"
#include <time.h>
#include <stdarg.h>
#include "trace.h"

int		SSQ_Init(SS_QUEUE_OBJ_T *pObj, unsigned int sharememory, unsigned int channelid, wchar_t *sharename, unsigned int bufsize, unsigned int prerecordsecs, unsigned int createsharememory)
{
	wchar_t wszHeaderName[36] = {0,};
	wchar_t wszFramelistName[36] = {0,};
	wchar_t wszDataName[36] = {0,};
	if (NULL==pObj)											return -1;
	if (createsharememory==0x01 && bufsize<1)				return -1;
	if ( (sharememory==0x01) && (NULL==sharename || (0==wcscmp(sharename, TEXT("\0")))) )	return -1;

	memset(pObj, 0x00, sizeof(SS_QUEUE_OBJ_T));
	pObj->channelid = channelid;
	pObj->shareinfo.id = channelid;
	wcscpy(pObj->shareinfo.name, sharename);

	wchar_t wszMutexName[36] = {0,};
	wsprintf(wszMutexName, TEXT("%s%d_mutex"), sharename, channelid);
	pObj->hMutex = OpenMutex(NULL, FALSE, wszMutexName);
	if (NULL == pObj->hMutex)
	{
		pObj->hMutex = CreateMutex(NULL, FALSE, wszMutexName);
		if (NULL == pObj->hMutex)							return -1;
	}

	//Create Header map
	
	
#ifdef _WIN32
	if (sharememory == 0x01)
	{
		wsprintf(wszHeaderName, TEXT("%s%d_h"), sharename, channelid);
		pObj->hSSHeader = OpenFileMapping(FILE_MAP_ALL_ACCESS, FALSE, wszHeaderName);
		if (NULL==pObj->hSSHeader && createsharememory==0x01)
		{
			pObj->hSSHeader = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE|SEC_COMMIT, 0, sizeof(SS_HEADER_T), wszHeaderName);
			if (NULL==pObj->hSSHeader || pObj->hSSHeader==INVALID_HANDLE_VALUE)
			{
				return -1;
			}
		}
		pObj->pQueHeader = (SS_HEADER_T*)MapViewOfFile(pObj->hSSHeader, FILE_MAP_READ|FILE_MAP_WRITE, 0, 0, 0);
		if (createsharememory==0x01)
		{
			if (pObj->pQueHeader->bufsize < 1)
			{
				memset(pObj->pQueHeader, 0x00, sizeof(SS_HEADER_T));
				pObj->pQueHeader->bufsize = bufsize;
			}
		}
		else if (NULL==pObj->pQueHeader)
		{
			return -1;
		}
		else
		{
			bufsize = pObj->pQueHeader->bufsize;
 		}
	}
	else
	{
		pObj->pQueHeader = new SS_HEADER_T;
		memset(pObj->pQueHeader, 0x00, sizeof(SS_HEADER_T));
	}

	//==========================================
	//Create frame list map
	if (prerecordsecs > 0)
	{
		wsprintf(wszFramelistName, TEXT("%s%d_f"), sharename, channelid);
		unsigned int nFramelistNum = prerecordsecs * 30;	//ÿ��30֡
		unsigned int nFrameQueSize = nFramelistNum*sizeof(FRAMEINFO_LIST_T);

		if (sharememory == 0x01)
		{
			pObj->hSSFrameList = OpenFileMapping(FILE_MAP_ALL_ACCESS, FALSE, wszFramelistName);
			if (NULL==pObj->hSSFrameList && createsharememory==0x01)
			{
				pObj->hSSFrameList = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE|SEC_COMMIT, 0, nFrameQueSize, wszFramelistName);
				if (NULL==pObj->hSSFrameList || pObj->hSSFrameList==INVALID_HANDLE_VALUE)
				{
					return -1;
				}
			}
			pObj->pFrameinfoList = (FRAMEINFO_LIST_T*)MapViewOfFile(pObj->hSSFrameList, FILE_MAP_READ|FILE_MAP_WRITE, 0, 0, 0);
			if (createsharememory==0x01)
			{
				memset(pObj->pFrameinfoList, 0x00, nFrameQueSize);
				pObj->pQueHeader->framelistNum = nFramelistNum;
			}
			else if (NULL==pObj->hSSFrameList)
			{
				return -1;
			}
		}
		else
		{
			pObj->pFrameinfoList = new FRAMEINFO_LIST_T[nFramelistNum];
			memset(&pObj->pFrameinfoList[0], 0x00, sizeof(FRAMEINFO_LIST_T)*nFramelistNum);
			pObj->pQueHeader->framelistNum = nFramelistNum;
		}
	}

	//Create data map	
	if (sharememory == 0x01)
	{
		wsprintf(wszDataName, TEXT("%s%d_b"), sharename, channelid);
		pObj->hSSData	= OpenFileMapping(FILE_MAP_ALL_ACCESS, FALSE, wszDataName);
		if (NULL==pObj->hSSData && createsharememory==0x01)
		{
			pObj->hSSData = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE|SEC_COMMIT, 0, bufsize, wszDataName);
		}
		if (NULL == pObj->hSSData || pObj->hSSData==INVALID_HANDLE_VALUE)
		{
			return -1;
		}
		pObj->pQueData = (char*)MapViewOfFile(pObj->hSSData, FILE_MAP_READ|FILE_MAP_WRITE, 0, 0, 0);
	}
	else
	{
		pObj->pQueData = new char [bufsize];
		pObj->pQueHeader->bufsize = bufsize;
	}
	if (createsharememory==0x01)
	{
		//memset(pQueHeader, 0x00, sizeof(SS_HEADER_T));
		memset(pObj->pQueData, 0x00, bufsize);
	}
#else
	int ret = shm_create((SYNC_VID_SHM_KEY<<8)|channelid, &pObj->shmHdrid, sizeof(SS_HEADER_T), (char**)&pObj->pQueHeader);
	if (ret < 0)
	{
		return -1;
	}
	SSQ_TRACE("[%d]pQueHeader: %d\n", (SYNC_VID_SHM_KEY<<8)|channelid, pObj->shmHdrid);
	ret = shm_create((SYNC_VID_SHM_KEY<<16)|channelid, &pObj->shmDatid, bufsize, (char**)&pObj->pQueData);
	if (ret < 0)
	{
		shm_delete(&pObj->shmHdrid, (char*)pObj->pQueHeader);
		return -1;
	}
	pObj->pQueHeader->bufsize = bufsize;
	SSQ_TRACE("[%d]pQueData: %d\n", (SYNC_VID_SHM_KEY<<16)|channelid, pObj->shmDatid);
#endif

	return 0;
}
int		SSQ_Deinit(SS_QUEUE_OBJ_T *pObj)
{
	if (NULL == pObj)		return -1;


#ifdef _WIN32

	if (NULL != pObj->hMutex)
	{
		CloseHandle(pObj->hMutex);
		pObj->hMutex = NULL;
	}

	if (NULL != pObj->hSSHeader)
	{
		if (! UnmapViewOfFile(pObj->pQueHeader))
		{
		}
		pObj->pQueHeader = NULL;
		CloseHandle(pObj->hSSHeader);
		pObj->hSSHeader = NULL;
	}
	if (NULL != pObj->pQueHeader)
	{
		delete []pObj->pQueHeader;
		pObj->pQueHeader = NULL;
	}

	if (NULL != pObj->hSSData)
	{
		if (! UnmapViewOfFile(pObj->pQueData))
		{
		}
		pObj->pQueData = NULL;
		CloseHandle(pObj->hSSData);
		pObj->hSSData = NULL;
	}
	if (NULL != pObj->pQueData)
	{
		delete []pObj->pQueData;
		pObj->pQueData = NULL;
	}
	if (NULL != pObj->hSSFrameList)
	{
		if (! UnmapViewOfFile(pObj->pFrameinfoList))
		{
		}
		pObj->pFrameinfoList = NULL;
		CloseHandle(pObj->hSSFrameList);
		pObj->hSSFrameList = NULL;
	}
	if (NULL != pObj->pFrameinfoList)
	{
		delete []pObj->pFrameinfoList;
		pObj->pFrameinfoList = NULL;
	}

#else
	if (pObj->shmHdrid>0 && pObj->pQueHeader!=NULL)
	{
		shm_delete(&pObj->shmHdrid, (char*)pObj->pQueHeader);
	}
	if (pObj->shmDatid>0 && pObj->pQueData!=NULL)
	{
		shm_delete(&pObj->shmDatid, (char*)pObj->pQueData);
		pObj->pQueData = NULL;
	}
#endif
	return 0;
}

int		SSQ_SetClearFlag(SS_QUEUE_OBJ_T *pObj, unsigned int _flag)
{
	if (NULL == pObj)		return -1;
	if (NULL==pObj->pQueData)		return -1;

	pObj->pQueHeader->clear_flag = _flag;
	return 0;
}
int		SSQ_Clear(SS_QUEUE_OBJ_T *pObj)
{
	if (NULL == pObj)		return -1;
	if (NULL==pObj->pQueData)		return -1;


	//WaitForSingleObject(pObj->hMutex, INFINITE);
	//Lock();
	pObj->pQueHeader->writepos = 0;
	pObj->pQueHeader->readpos  = 0;
	pObj->pQueHeader->totalsize= 0;

	pObj->pQueHeader->videoframes = 0;	//��Ƶ֡��

	pObj->pQueHeader->maxframeno = 0;
	pObj->pQueHeader->frameno = 0;

	memset(pObj->pQueData, 0x00, pObj->pQueHeader->bufsize);

	//ReleaseMutex(pObj->hMutex);

	//Unlock();
	return 0;
}

int	SSQ_AddFrameInfo(SS_QUEUE_OBJ_T *pObj, unsigned int _pos, MEDIA_FRAME_INFO *frameinfo)
{
	if (NULL == pObj)					return -1;
	if (NULL == pObj->pQueHeader)		return -1;
	if (NULL == pObj->pFrameinfoList)	return -1;

	if ( pObj->pQueHeader->frameno+1 > pObj->pQueHeader->framelistNum)
	{
		memmove((char*)pObj->pFrameinfoList, (char*)pObj->pFrameinfoList+sizeof(FRAMEINFO_LIST_T), sizeof(FRAMEINFO_LIST_T)*pObj->pQueHeader->framelistNum-1);
		pObj->pQueHeader->frameno --;
		pObj->pQueHeader->maxframeno = pObj->pQueHeader->frameno+1;
	}

	pObj->pFrameinfoList[pObj->pQueHeader->frameno].pos = pObj->pQueHeader->writepos;
#ifdef _DEBUG1
	static int nTimestamp = 0;
	pObj->pQueHeader->pFrameinfoList[pObj->pQueHeader->frameno].timestamp = ++nTimestamp;
#else
	pObj->pFrameinfoList[pObj->pQueHeader->frameno].timestamp_sec = frameinfo->timestamp_sec;
	pObj->pFrameinfoList[pObj->pQueHeader->frameno].rtp_timestamp = frameinfo->timestamp_sec*1000+frameinfo->timestamp_usec/1000;
#endif
	//SSQ_TRACE("֡��: %d\n", pObj->pQueHeader->frameno);
	pObj->pQueHeader->frameno ++;
	pObj->pQueHeader->maxframeno = pObj->pQueHeader->frameno;

/*
	if ( pObj->pQueHeader->frameno >= pObj->pQueHeader->framelistNum)
	{
		SSQ_TRACE("���֡: %d\n", pObj->pQueHeader->maxframeno);
		pObj->pQueHeader->frameno = 0;
	}
	if (pObj->pQueHeader->frameno > pObj->pQueHeader->maxframeno)
	{
		pObj->pQueHeader->maxframeno = pObj->pQueHeader->frameno;
	}
*/
	return 0;
}

int		SSQ_AddData(SS_QUEUE_OBJ_T *pObj, unsigned int channelid, unsigned int mediatype, MEDIA_FRAME_INFO *frameinfo, char *pbuf)
{
	int ret = 0;
	SS_BUF_T	bufNode;


	if (NULL==pObj || NULL==frameinfo || NULL==pbuf)		return -1;
	if (NULL == pObj->pQueData)					return -1;
	if (NULL == pObj->pQueHeader)				return -1;
	if (frameinfo->length < 1)
	{
#ifdef _DEBUG
		SSQ_TRACE("֡����С��1: %d\n", frameinfo->length);
#endif
		return -1;
	}
	 
#ifdef _DEBUG1
	SSQ_TRACE("frame: %02x%02x%02x%02x%02x\n", (unsigned char)pbuf[0],
				(unsigned char)pbuf[1],
				(unsigned char)pbuf[2],
				(unsigned char)pbuf[3],
				(unsigned char)pbuf[4]);
#endif

	if (frameinfo->length > pObj->pQueHeader->bufsize)
	{
		SSQ_TRACE("Buffer too low.. Current Frame Size: %d\tBuffer Size: %d\n", frameinfo->length, pObj->pQueHeader->bufsize);
		return -1;
	}



	WaitForSingleObject(pObj->hMutex, INFINITE);		//Lock

	if (pObj->pQueHeader->clear_flag == 0x01)
	{
		//SSQ_TRACE("Received clear signal... Clear Buffer..\n");
		SSQ_TRACE("���յ�����ź�.  ��ջ���..  WritePos: %d\n", pObj->pQueHeader->writepos);
		SSQ_Clear(pObj);
		pObj->pQueHeader->clear_flag = 0x00;
	}

	if (pObj->pQueHeader->writepos == pObj->pQueHeader->readpos)
	{
		if (pObj->pQueHeader->totalsize < 0 || pObj->pQueHeader->videoframes < 0)
		{
			SSQ_TRACE("�����ٶ�==д���ٶ�...  ���ݴ�С�쳣: %d    ��Ƶ֡��:%d\n", pObj->pQueHeader->totalsize, pObj->pQueHeader->videoframes);
		}
		//Clear();
	}

	if (sizeof(SS_BUF_T) + frameinfo->length + pObj->pQueHeader->totalsize > pObj->pQueHeader->bufsize)
	{
		SSQ_TRACE("������������С.. ֡��:%d\ttotalsize:%d\tbufsize:%d  ����֡��:%d\n", frameinfo->length, pObj->pQueHeader->totalsize, pObj->pQueHeader->bufsize, pObj->pQueHeader->videoframes);
		ReleaseMutex(pObj->hMutex);
		pObj->pQueHeader->isfull = 0x01;
		return -1;
	}

	pObj->pQueHeader->isfull = 0x00;

	memset(&bufNode, 0x00, sizeof(SS_BUF_T));
	memcpy(&bufNode.frameinfo, frameinfo, sizeof(MEDIA_FRAME_INFO));
	bufNode.channelid = channelid;//++m_FrameTally;
	bufNode.mediatype = mediatype;
	bufNode.flag	=	BUF_QUE_FLAG;

	//_TRACE("WritePos: %d    totalSize: %d\n", pQueHeader->writepos, pQueHeader->totalsize);

	//Lock();
	//��ͷ��β д
	if ((pObj->pQueHeader->writepos + sizeof(SS_BUF_T) + frameinfo->length) <= pObj->pQueHeader->bufsize)
	{
		//copy to queue

		//��¼֡λ��
		if (mediatype==MEDIA_TYPE_VIDEO)	SSQ_AddFrameInfo(pObj, pObj->pQueHeader->writepos, frameinfo);

		unsigned int nAdd = pObj->pQueHeader->writepos;
		memcpy(pObj->pQueData+nAdd, &bufNode, sizeof(SS_BUF_T));
		nAdd += sizeof(SS_BUF_T);

		memcpy(pObj->pQueData+nAdd, pbuf, frameinfo->length);
		nAdd += frameinfo->length;
		pObj->pQueHeader->writepos = nAdd;
		pObj->pQueHeader->totalsize+= sizeof(SS_BUF_T);
		pObj->pQueHeader->totalsize+= frameinfo->length;

		if (mediatype==MEDIA_TYPE_VIDEO)	pObj->pQueHeader->videoframes ++;

		//_TRACE("˳������..  writepos: %d   /   %d\n", pQueHeader->writepos, pQueHeader->size);
	}
	else if (pObj->pQueHeader->writepos == pObj->pQueHeader->bufsize)	//��ͷ��ʼ
	{
		//��¼֡λ��
		if (mediatype==MEDIA_TYPE_VIDEO)	SSQ_AddFrameInfo(pObj, 0, frameinfo);


		memcpy(pObj->pQueData, &bufNode, sizeof(SS_BUF_T));
		pObj->pQueHeader->writepos = sizeof(SS_BUF_T);

		memcpy(pObj->pQueData+pObj->pQueHeader->writepos, pbuf, frameinfo->length);
		pObj->pQueHeader->writepos += frameinfo->length;

		pObj->pQueHeader->totalsize= sizeof(SS_BUF_T);
		pObj->pQueHeader->totalsize+= frameinfo->length;

		if (mediatype==MEDIA_TYPE_VIDEO)	pObj->pQueHeader->videoframes ++;

		SSQ_TRACE("��ͷ��ʼд..  writepos: %d\n", pObj->pQueHeader->writepos);
	}
	//else if (pQueHeader->size - pQueHeader->writepos+pQueHeader->readpos >= (int)(frameinfo->length+sizeof(SS_BUF_T)))	//��βд��ͷ
	//else if (pObj->pQueHeader->bufsize - pObj->pQueHeader->writepos+pObj->pQueHeader->readpos >= (int)(frameinfo->length+sizeof(SS_BUF_T)))	//��βд��ͷ
	else// if (pObj->pQueHeader->bufsize - pObj->pQueHeader->writepos >= (int)(frameinfo->length+sizeof(SS_BUF_T)))	//��βд��ͷ
	{
		unsigned int remain = pObj->pQueHeader->bufsize - pObj->pQueHeader->writepos;	//ʣ��ռ�
		if (remain>=sizeof(SS_BUF_T))
		{
			unsigned int nAdd = pObj->pQueHeader->writepos;

			//��¼֡λ��
			if (mediatype==MEDIA_TYPE_VIDEO)	SSQ_AddFrameInfo(pObj, nAdd, frameinfo);

			//_TRACE("WritePos111: %d\n", pQueHeader->writepos);
			memcpy(pObj->pQueData+nAdd, &bufNode, sizeof(SS_BUF_T));
			nAdd += sizeof(SS_BUF_T);
			//pQueHeader->totalsize+= sizeof(SS_BUF_T);
			//_TRACE("WritePos222: %d\n", pQueHeader->writepos+sizeof(SS_BUF_T));
			remain = pObj->pQueHeader->bufsize - nAdd;//pQueHeader->writepos;
			//_TRACE("WritePos: %d\n", pQueHeader->writepos+sizeof(SS_BUF_T)+remain);
			if (remain>0)
			{
				memcpy(pObj->pQueData+nAdd, pbuf, remain);
				memcpy(pObj->pQueData, pbuf+remain, frameinfo->length-remain);
				nAdd = frameinfo->length-remain;

				pObj->pQueHeader->writepos = nAdd;
				pObj->pQueHeader->totalsize+= sizeof(SS_BUF_T);
				pObj->pQueHeader->totalsize+= frameinfo->length;

				if (pObj->pQueHeader->totalsize>pObj->pQueHeader->bufsize)
				{
					//ԭ��:  rtsp server��ֹͣ���ö���(��ǰû�пͻ��˷���)
					SSQ_TRACE("[RTSP Server��ֹͣ��ȡ�ö���]���� %d > %d    frameinfo->length:%d...\n", pObj->pQueHeader->totalsize, pObj->pQueHeader->bufsize, frameinfo->length);
					//SSQ_Clear(pObj);
				}
				else
				{
					if (mediatype==MEDIA_TYPE_VIDEO)	pObj->pQueHeader->videoframes ++;

					//SSQ_TRACE("111Header������֡����λ�ڻ���β, ����֡����λ�ڻ�����..  remain: %d   writepos: %d  totalsize: %d\n", remain, pObj->pQueHeader->writepos, pObj->pQueHeader->totalsize);
				}
			}
			else if (remain==0)
			{
				memcpy(pObj->pQueData, pbuf, frameinfo->length);
				nAdd = frameinfo->length;
				pObj->pQueHeader->writepos = nAdd;
				pObj->pQueHeader->totalsize+= sizeof(SS_BUF_T);
				pObj->pQueHeader->totalsize+= frameinfo->length;

				if (mediatype==MEDIA_TYPE_VIDEO)	pObj->pQueHeader->videoframes ++;

				if (pObj->pQueHeader->totalsize>pObj->pQueHeader->bufsize)
				{
					SSQ_TRACE("����222 %d > %d    frameinfo->length:%d...\n", pObj->pQueHeader->totalsize, pObj->pQueHeader->bufsize, frameinfo->length);
					//SSQ_Clear(pObj);
				}
				else
				{
					SSQ_TRACE("22222Headerλ�ڻ���β,֡����λ�ڻ�����..  writepos: %d\n", pObj->pQueHeader->writepos);
				}
			}
			else
			{
				SSQ_TRACE("����...  ʣ��ռ�С��0: %d\n", remain);
				ret = -1;
			}
		}
		else if (remain>0)
		{
			char *tmpbuf = (char *)&bufNode;

			
			unsigned int nAdd = pObj->pQueHeader->writepos;

			//��¼֡λ��
			if (mediatype==MEDIA_TYPE_VIDEO)	SSQ_AddFrameInfo(pObj, nAdd, frameinfo);


			//SSQ_TRACE("ADD DATA...%d\n", nAdd);
			memcpy(pObj->pQueData+nAdd, tmpbuf, remain);
			//SSQ_TRACE("ADD DATA222...%d\n", sizeof(SS_BUF_T)-remain);
			memcpy(pObj->pQueData, tmpbuf+remain, sizeof(SS_BUF_T)-remain);
			nAdd = sizeof(SS_BUF_T)-remain;
			memcpy(pObj->pQueData+nAdd, pbuf, frameinfo->length);

			nAdd += frameinfo->length;
			pObj->pQueHeader->writepos  = nAdd;
			pObj->pQueHeader->totalsize+= sizeof(SS_BUF_T);
			pObj->pQueHeader->totalsize+= frameinfo->length;

			if (pObj->pQueHeader->totalsize>pObj->pQueHeader->bufsize)
			{
				SSQ_TRACE("����333 %d > %d    frameinfo->length:%d...\n", pObj->pQueHeader->totalsize, pObj->pQueHeader->bufsize, frameinfo->length);
				//SSQ_Clear(pObj);
			}

			if (mediatype==MEDIA_TYPE_VIDEO)	pObj->pQueHeader->videoframes ++;
		}
		else
		{
			ret = -1;
			SSQ_TRACE("ERROR...\n");
		}
	}
	//else
	{
		//else if (pObj->pQueHeader->bufsize - pObj->pQueHeader->writepos+pObj->pQueHeader->readpos >= (int)(frameinfo->length+sizeof(SS_BUF_T)))	//��βд��ͷ
		//SSQ_TRACE("дβ����..  δ����.  дλ��:%d / pObj->pQueHeader->bufsize  ��λ��:%d\n", pObj->pQueHeader->writepos, pObj->pQueHeader->bufsize, pObj->pQueHeader->readpos);
	}
	//Unlock();

	ReleaseMutex(pObj->hMutex);

	//SSQ_TRACE("writepos: %d\ttotalsize: %d\n", pObj->pQueHeader->writepos, pObj->pQueHeader->totalsize);

#ifdef _DEBUG1
	if (mediatype==MEDIA_TYPE_VIDEO)
	{
		SSQ_TRACE("==========================\n");
		for (int i=0; i<pObj->pQueHeader->maxframeno; i++)
		{
			SSQ_TRACE("[%d] times: %d   pos: %d   %02X%02X%02X%02X%02X\n", i, pObj->pQueHeader->pFrameinfoList[i].timestamp, pObj->pQueHeader->pFrameinfoList[i].pos,
				(unsigned char)pObj->pQueData[pObj->pQueHeader->pFrameinfoList[i].pos+sizeof(SS_BUF_T)+0], (unsigned char)pObj->pQueData[pObj->pQueHeader->pFrameinfoList[i].pos+sizeof(SS_BUF_T)+1], (unsigned char)pObj->pQueData[pObj->pQueHeader->pFrameinfoList[i].pos+sizeof(SS_BUF_T)+2], 
				(unsigned char)pObj->pQueData[pObj->pQueHeader->pFrameinfoList[i].pos+sizeof(SS_BUF_T)+3], (unsigned char)pObj->pQueData[pObj->pQueHeader->pFrameinfoList[i].pos+sizeof(SS_BUF_T)+4]);
		}
	}
#endif


	return ret;
}
int		SSQ_GetData(SS_QUEUE_OBJ_T *pObj, unsigned int *channelid, unsigned int *mediatype, MEDIA_FRAME_INFO *frameinfo, char *pbuf)
{
	int ret = 0;
	unsigned int remain = 0;

	if (NULL == pObj)			return -1;
	
	if (NULL == pObj->pQueHeader)	return -1;
	if (NULL == pObj->pFrameinfoList)	return -1;


	WaitForSingleObject(pObj->hMutex, INFINITE);		//Lock

	if (pObj->pQueHeader->totalsize < 0)
	{
		SSQ_TRACE("pObj->pQueHeader->totalsize<0: %d\n", pObj->pQueHeader->totalsize);
		ReleaseMutex(pObj->hMutex);
		return -1;
	}

	if (pObj->pQueHeader->totalsize <= sizeof(SS_BUF_T))
	{
		ReleaseMutex(pObj->hMutex);
		return -1;
	}
	//_TRACE("��λ��: %d\n", pQueHeader->readpos);

	

	//if (NULL != chid)		*chid = m_chid;

	
	//Lock();


#if 0
	ret = -1;
	for (unsigned int i=0; i<pObj->pQueHeader->maxframeno; i++)
	{
		if (pObj->pFrameinfoList[i].rtp_timestamp > frameinfo->rtptimestamp)
		{
			pObj->pQueHeader->readpos = pObj->pFrameinfoList[i].pos;
			ret = SSQ_GetDataByPosition(pObj, pObj->pFrameinfoList[i].pos, channelid, mediatype, frameinfo, pbuf);
			break;
		}
	}
#else

	if (pObj->pQueHeader->readpos == pObj->pQueHeader->bufsize)
	{
		SSQ_TRACE("���ö�λ��[%d / %d]..\n", pObj->pQueHeader->readpos, pObj->pQueHeader->bufsize);
		pObj->pQueHeader->readpos = 0;
	}

	if ((pObj->pQueHeader->readpos + sizeof(SS_BUF_T)) <= pObj->pQueHeader->bufsize)
	{
		SS_BUF_T *pNode = (SS_BUF_T *)(pObj->pQueData + pObj->pQueHeader->readpos);
		//if (pNode->id<1)
		if (pNode->flag	!= BUF_QUE_FLAG)
		{
			SSQ_TRACE("��־λ����... ������Ƶ֡:%d  �ֽ���:%d  ��ն���\n", pObj->pQueHeader->videoframes, pObj->pQueHeader->totalsize);
			if (pObj->hSSHeader == NULL)	//ͬһ��������ʹ��
			{
				SSQ_Clear(pObj);
			}
			else
			{
				pObj->pQueHeader->clear_flag = 0x01;
#ifdef _WIN32
				while (pObj->pQueHeader->clear_flag!=0x00)	{Sleep(1);}
#else
				while (pObj->pQueHeader->clear_flag!=0x00)	{usleep(100);}
#endif
			}
			//Unlock();
			SSQ_TRACE("111��־λ����... ������Ƶ֡:%d  �ֽ���:%d\n", pObj->pQueHeader->videoframes, pObj->pQueHeader->totalsize);
			SSQ_TRACE("��־λ����.. ��ն���..  readpos: %d\n", pObj->pQueHeader->readpos);
			//pObj->pQueHeader->clear_flag = 0x01;

			//_TRACE("��־λ����.. ��ն������..\n");

			ReleaseMutex(pObj->hMutex);
			return -1;
		}


		if (NULL!=mediatype)		*mediatype = pNode->mediatype;
		if (NULL != channelid)		*channelid = pNode->channelid;


		memcpy(frameinfo, &pNode->frameinfo, sizeof(MEDIA_FRAME_INFO));
		if ( (pObj->pQueHeader->readpos + pNode->frameinfo.length+sizeof(SS_BUF_T)) <= pObj->pQueHeader->bufsize)
		{
			//��ͷ��β��

			if (pObj->pQueHeader->totalsize < frameinfo->length+sizeof(SS_BUF_T))
			{
				//����������
				SSQ_TRACE("����������... ���ֽ���[%d]<֡��[%d]. ��λ��:%d\n", pObj->pQueHeader->totalsize, frameinfo->length+sizeof(SS_BUF_T), pObj->pQueHeader->readpos);
				ReleaseMutex(pObj->hMutex);
				return -1;
			}

			if (frameinfo->length+sizeof(SS_BUF_T) > pObj->pQueHeader->totalsize)
			{
				SSQ_TRACE("���ֽ���[%d]<֡��[%d]. ��λ��:%d\n", pObj->pQueHeader->totalsize, frameinfo->length+sizeof(SS_BUF_T), pObj->pQueHeader->readpos);
			}

			//_TRACE("��λ��00000000...: %d  /  %d\n", pQueHeader->readpos, pQueHeader->size);
			pObj->pQueHeader->readpos += sizeof(SS_BUF_T);

			unsigned int total1 = pObj->pQueHeader->totalsize;
			pObj->pQueHeader->totalsize -= sizeof(SS_BUF_T);

			if (NULL!=pbuf)	memcpy(pbuf, pObj->pQueData+pObj->pQueHeader->readpos, frameinfo->length);

			//memset(pObj->pQueData+pObj->pQueHeader->readpos, 0x00, frameinfo->length);	//clear

			pObj->pQueHeader->readpos += frameinfo->length;
			unsigned int total2 = pObj->pQueHeader->totalsize;
			pObj->pQueHeader->totalsize -= (frameinfo->length);

			if (pObj->pQueHeader->readpos == pObj->pQueHeader->bufsize)
			{
				pObj->pQueHeader->readpos = 0;
			}

			if (pObj->pQueHeader->readpos > pObj->pQueHeader->bufsize)
			{
				SSQ_TRACE("��λ�ô���11111...: %d  /  %d\n", pObj->pQueHeader->readpos, pObj->pQueHeader->bufsize);
			}
			if (pObj->pQueHeader->totalsize < 0)
			{
				SSQ_TRACE("��λ��: %d\tдλ��: %d  ��ǰ֡��С:%d   total1:%d\ttotal2:%d\ttotal3:%d\n", pObj->pQueHeader->readpos, pObj->pQueHeader->writepos, frameinfo->length, total1, total2, pObj->pQueHeader->totalsize);
				SSQ_TRACE("���ֽ�������: %d\n", pObj->pQueHeader->totalsize);
			}
		}
		else
		{
			if (pObj->pQueHeader->totalsize < (pNode->frameinfo.length+sizeof(SS_BUF_T)))
			{
				SSQ_TRACE("���ֽ���<֡��+sizeof(SS_BUF_T)..\n");
				//Unlock();
				ReleaseMutex(pObj->hMutex);
				return -1;
			}

			remain = pObj->pQueHeader->bufsize-pObj->pQueHeader->readpos;
			if (remain>=sizeof(SS_BUF_T))
			{
				pObj->pQueHeader->readpos += sizeof(SS_BUF_T);
				remain = pObj->pQueHeader->bufsize - pObj->pQueHeader->readpos;
				if (remain>0)
				{
					//SSQ_TRACE("111β�в�������... ���в�������... remain>0: %d\n", remain);
					if (NULL!=pbuf)	memcpy(pbuf, pObj->pQueData+pObj->pQueHeader->readpos, remain);
					//memset(pObj->pQueData+pObj->pQueHeader->readpos, 0x00, remain);	//clear
					if (NULL!=pbuf)	memcpy(pbuf+remain, pObj->pQueData, frameinfo->length-remain);
					//memset(pObj->pQueData, 0x00, frameinfo->length-remain);	//clear
				
					pObj->pQueHeader->readpos = frameinfo->length-remain;
					pObj->pQueHeader->totalsize -= frameinfo->length;
					pObj->pQueHeader->totalsize -= sizeof(SS_BUF_T);

						if (pObj->pQueHeader->totalsize < 0)
						{
							SSQ_TRACE("3333pObj->pQueHeader->totalsize<0: %d\n", pObj->pQueHeader->totalsize);
						}
				}
				else
				{
					if (remain < 0)
					{
						SSQ_TRACE("λ�ƴ���: ʣ���ֽ���<0:%d\n", remain);

					}
					else
					{
						SSQ_TRACE("111β�в�������... ���в�������... remain<=0: %d\n", remain);
						if (NULL!=pbuf)	memcpy(pbuf, pObj->pQueData, frameinfo->length);
						//memset(pObj->pQueData, 0x00, frameinfo->length-remain);	//clear
						pObj->pQueHeader->readpos = frameinfo->length;
						pObj->pQueHeader->totalsize -= frameinfo->length;
						pObj->pQueHeader->totalsize -= sizeof(SS_BUF_T);

						if (pObj->pQueHeader->totalsize < 0)
						{
							SSQ_TRACE("4444pObj->pQueHeader->totalsize<0: %d\n", pObj->pQueHeader->totalsize);
						}
					}
				}
				
				//SSQ_TRACE("remain > sizeof(SS_BUF_T): %d\t\treadpos: %d\n", remain, pObj->pQueHeader->readpos);
			}
			else
			{
				//SSQ_TRACE("remain < sizeof(SS_BUF_T): %d\n", remain);

				//ִ�е��˴�,˵���쳣

				//remain = pObj->pQueHeader->bufsize - pObj->pQueHeader->readpos;
				if (remain>0)
				{
					SSQ_TRACE("222β�в�������... ���в�������... remain>0: %d\n", remain);
					if (NULL!=pbuf)	memcpy(pbuf, pObj->pQueData+pObj->pQueHeader->readpos, remain);
					//memset(pObj->pQueData+pObj->pQueHeader->readpos, 0x00, remain);	//clear
					if (NULL!=pbuf)	memcpy(pbuf+remain, pObj->pQueData, frameinfo->length-remain);
					//memset(pObj->pQueData, 0x00, frameinfo->length-remain);	//clear
				

					pObj->pQueHeader->readpos = frameinfo->length-remain;
					pObj->pQueHeader->totalsize -= frameinfo->length;

					if (pObj->pQueHeader->totalsize < 0)
					{
						SSQ_TRACE("555pObj->pQueHeader->totalsize<0: %d\n", pObj->pQueHeader->totalsize);
					}

				}
				else
				{
					SSQ_TRACE("222β�в�������... ���в�������... remain<=0: %d\n", remain);
					if (NULL!=pbuf)	memcpy(pbuf, pObj->pQueData, frameinfo->length);
					//memset(pObj->pQueData, 0x00, frameinfo->length-remain);	//clear
					pObj->pQueHeader->readpos = frameinfo->length;
					pObj->pQueHeader->totalsize -= frameinfo->length;
					if (pObj->pQueHeader->totalsize < 0)
					{
						SSQ_TRACE("666pObj->pQueHeader->totalsize<0: %d\n", pObj->pQueHeader->totalsize);
					}
				}
				pObj->pQueHeader->totalsize -= sizeof(SS_BUF_T);

					if (pObj->pQueHeader->totalsize < 0)
					{
						SSQ_TRACE("777pObj->pQueHeader->totalsize<0: %d\n", pObj->pQueHeader->totalsize);
					}
			}


			//pObj->pQueHeader->readpos += sizeof(SS_BUF_T);
			//pObj->pQueHeader->totalsize -= sizeof(SS_BUF_T);



			if (pObj->pQueHeader->readpos>pObj->pQueHeader->bufsize)
			{
				SSQ_TRACE("��λ�ô���...: %d  /  %d\n", pObj->pQueHeader->readpos, pObj->pQueHeader->bufsize);
			}
			

/*
			if (NULL!=pbuf)	memcpy(pbuf, pShareMemoryBuff+pQueHeader->readpos, remain);
			memset(pShareMemoryBuff+pQueHeader->readpos, 0x00, remain);	//clear
			if (NULL!=pbuf)	memcpy(pbuf+remain, pShareMemoryBuff, frameinfo->length-remain);
			memset(pShareMemoryBuff, 0x00, frameinfo->length-remain);	//clear

			pQueHeader->readpos = frameinfo->length-remain;
			pQueHeader->totalsize -= (frameinfo->length-remain);
*/
		}

		if (MEDIA_TYPE_VIDEO==pNode->mediatype)	pObj->pQueHeader->videoframes --;

		//memset(pNode, 0x00, sizeof(SS_BUF_T));
	}
	else
	{
		unsigned int remain = pObj->pQueHeader->bufsize-pObj->pQueHeader->readpos;
		SS_BUF_T bufnode;
		char *pp = (char *)&bufnode;
		memset(&bufnode, 0x00, sizeof(SS_BUF_T));

		//SSQ_TRACE("GET DATA...\n");

		//SSQ_TRACE("1 REMAIN: %d\n", remain);
		if (remain>0)
		{
			memcpy(pp, pObj->pQueData+pObj->pQueHeader->readpos, remain);
			//memset(pObj->pQueData+pObj->pQueHeader->readpos, 0x00, remain);	//clear
			//SSQ_TRACE("2 read: %d\n", sizeof(SS_BUF_T)-remain);
			memcpy(pp+remain, pObj->pQueData, sizeof(SS_BUF_T)-remain);
			//memset(pObj->pQueData, 0x00, sizeof(SS_BUF_T)-remain);	//clear

			memcpy(frameinfo, &bufnode.frameinfo, sizeof(MEDIA_FRAME_INFO));
			if (NULL != channelid)		*channelid = bufnode.channelid;

			//if (bufnode.id<1)
			if (bufnode.flag	!= BUF_QUE_FLAG)
			{
				//Unlock();
				SSQ_Clear(pObj);
				ReleaseMutex(pObj->hMutex);
				SSQ_TRACE("SSQ_��־������...\n");
				return -1;
			}

			pObj->pQueHeader->readpos = sizeof(SS_BUF_T)-remain;

			if (NULL!=mediatype)	*mediatype = bufnode.mediatype;
			//SSQ_TRACE("3 frame length: %d\n", bufnode.frameinfo.length);
			if (NULL!=pbuf)	memcpy(pbuf, pObj->pQueData+pObj->pQueHeader->readpos, bufnode.frameinfo.length);
			//memset(pObj->pQueData+pObj->pQueHeader->readpos, 0x00, bufnode.frameinfo.length);	//clear
			pObj->pQueHeader->readpos += bufnode.frameinfo.length;

			pObj->pQueHeader->totalsize -= sizeof(SS_BUF_T);
			//pObj->pQueHeader->totalsize -= (frameinfo->length-remain);
			pObj->pQueHeader->totalsize -= (frameinfo->length);//20140521

			if (pObj->pQueHeader->totalsize < 0)
			{
				SSQ_TRACE("888pObj->pQueHeader->totalsize<0: %d\n", pObj->pQueHeader->totalsize);
			}

			//SSQ_TRACE("GET DATA OK..\n");
			if (MEDIA_TYPE_VIDEO==bufnode.mediatype)	pObj->pQueHeader->videoframes --;
		}
		else
		{
			SSQ_TRACE("�쳣...   REMAIN <= 0....\n");
		}


		ret = 1000;
	}
#endif
	
	//Unlock();
	ReleaseMutex(pObj->hMutex);

	return ret;
}


//===========================================
//����λ�û�ȡ��Ӧ��֡����
int		SSQ_GetDataByPosition(SS_QUEUE_OBJ_T *pObj, unsigned int position, unsigned int clearflag, unsigned int *channelid, unsigned int *mediatype, MEDIA_FRAME_INFO *frameinfo, char *pbuf)
{
	int ret = 0;
	unsigned int remain = 0;

	if (NULL == pObj)					return -1;
	if (NULL == pObj->pQueHeader)		return -1;
	if (NULL == pObj->pFrameinfoList)	return -1;

	unsigned int *pOffset = (unsigned int *)&position;
	unsigned int totalsize = pObj->pQueHeader->totalsize;
	unsigned int *pTotalSize = (unsigned int*)&totalsize;

	if (clearflag == 0x01)
	{
		pOffset    = (unsigned int*)&pObj->pQueHeader->readpos;
		pTotalSize = (unsigned int*)&pObj->pQueHeader->totalsize;
	}

	WaitForSingleObject(pObj->hMutex, INFINITE);		//Lock

	if (*pOffset == pObj->pQueHeader->bufsize)
	{
		SSQ_TRACE("���ö�λ��[%d / %d]..\n", *pOffset, pObj->pQueHeader->bufsize);
		*pOffset = 0;
	}

	if (clearflag==0x01)
	{
		if (pObj->pQueHeader->totalsize <= sizeof(SS_BUF_T))
		{
			ReleaseMutex(pObj->hMutex);
			return -1;
		}


		if (pObj->pQueHeader->readpos == pObj->pQueHeader->bufsize)
		{
			pObj->pQueHeader->readpos = 0;
		}
	}

	if ((*pOffset + sizeof(SS_BUF_T)) <= pObj->pQueHeader->bufsize)
	{
		SS_BUF_T *pNode = (SS_BUF_T *)(pObj->pQueData + *pOffset);
		if (pNode->flag	!= BUF_QUE_FLAG)
		{
			SSQ_TRACE("[SSQ_GetDataByPosition]��־λ����...\n");
			if (clearflag == 0x01)
			{
				if (pObj->hSSHeader==NULL)	//ͬһ����
				{
					SSQ_Clear(pObj);
				}
				else
				{
					pObj->pQueHeader->clear_flag	=	0x01;
					while (pObj->pQueHeader->clear_flag!=0x00)	{Sleep(1);}
				}
			}
			ReleaseMutex(pObj->hMutex);
			return -1;
		}

		if (NULL!=mediatype)		*mediatype = pNode->mediatype;
		if (NULL != channelid)		*channelid = pNode->channelid;


		memcpy(frameinfo, &pNode->frameinfo, sizeof(MEDIA_FRAME_INFO));
		if ( (*pOffset + pNode->frameinfo.length+sizeof(SS_BUF_T)) <= pObj->pQueHeader->bufsize)
		{
			//��ͷ��β��
			*pOffset += sizeof(SS_BUF_T);

			if (NULL!=pbuf)	memcpy(pbuf, pObj->pQueData+*pOffset, frameinfo->length);
			*pOffset += frameinfo->length;
			*pTotalSize -= (frameinfo->length+sizeof(SS_BUF_T));

			if (*pOffset == pObj->pQueHeader->bufsize)
			{
				*pOffset = 0;
			}

			if (*pOffset > pObj->pQueHeader->bufsize)
			{
				SSQ_TRACE("[SSQ_GetDataByPosition]��λ�ô���11111...: %d  /  %d\n", *pOffset, pObj->pQueHeader->bufsize);
			}
		}
		else
		{
			remain = pObj->pQueHeader->bufsize - *pOffset;
			if (remain>=sizeof(SS_BUF_T))
			{
				*pOffset += sizeof(SS_BUF_T);
				remain = pObj->pQueHeader->bufsize - *pOffset;
				if (remain>0)
				{
					if (NULL!=pbuf)	memcpy(pbuf, pObj->pQueData + *pOffset, remain);
					if (NULL!=pbuf)	memcpy(pbuf+remain, pObj->pQueData, frameinfo->length-remain);

					*pOffset = frameinfo->length-remain;
					*pTotalSize -= (frameinfo->length+sizeof(SS_BUF_T));
				}
				else
				{
					if (remain < 0)
					{
						SSQ_TRACE("[SSQ_GetDataByPosition]λ�ƴ���: ʣ���ֽ���<0:%d\n", remain);
					}
					else
					{
						SSQ_TRACE("[SSQ_GetDataByPosition]β�в�������... ���в�������... remain<=0: %d\n", remain);
						if (NULL!=pbuf)	memcpy(pbuf, pObj->pQueData, frameinfo->length);
						//memset(pObj->pQueData, 0x00, frameinfo->length-remain);	//clear
						*pOffset = frameinfo->length;
						*pTotalSize -= (frameinfo->length+sizeof(SS_BUF_T));
					}
				}
			}
			else
			{
				//SSQ_TRACE("remain < sizeof(SS_BUF_T): %d\n", remain);

				//ִ�е��˴�,˵���쳣
				SSQ_TRACE("[SSQ_GetDataByPosition]  �쳣   #########... remain>0: %d\n", remain);
				if (remain>0)
				{
					SSQ_TRACE("[SSQ_GetDataByPosition]222β�в�������... ���в�������... remain>0: %d\n", remain);
					if (NULL!=pbuf)	memcpy(pbuf, pObj->pQueData + *pOffset, remain);
					if (NULL!=pbuf)	memcpy(pbuf+remain, pObj->pQueData, frameinfo->length-remain);
				
					*pOffset = frameinfo->length-remain;
					*pTotalSize -= (frameinfo->length);
				}
				else
				{
					SSQ_TRACE("[SSQ_GetDataByPosition]222β�в�������... ���в�������... remain<=0: %d\n", remain);
					if (NULL!=pbuf)	memcpy(pbuf, pObj->pQueData, frameinfo->length);
					//memset(pObj->pQueData, 0x00, frameinfo->length-remain);	//clear
					*pOffset = frameinfo->length;
				}
			}

			if (*pOffset > pObj->pQueHeader->bufsize)
			{
				SSQ_TRACE("[SSQ_GetDataByPosition]��λ�ô���...: %d  /  %d\n", *pOffset, pObj->pQueHeader->bufsize);
			}
		}

		if (MEDIA_TYPE_VIDEO==pNode->mediatype && clearflag==0x01)	pObj->pQueHeader->videoframes --;
	}
	else
	{
		unsigned int remain = pObj->pQueHeader->bufsize - *pOffset;
		SS_BUF_T bufnode;
		char *pp = (char *)&bufnode;
		memset(&bufnode, 0x00, sizeof(SS_BUF_T));

		//SSQ_TRACE("GET DATA...\n");

		SSQ_TRACE("[SSQ_GetDataByPosition]1 REMAIN: %d\n", remain);
		if (remain>0)
		{
			memcpy(pp, pObj->pQueData + *pOffset, remain);
			SSQ_TRACE("[SSQ_GetDataByPosition]2 read: %d\n", sizeof(SS_BUF_T)-remain);
			memcpy(pp+remain, pObj->pQueData, sizeof(SS_BUF_T)-remain);

			memcpy(frameinfo, &bufnode.frameinfo, sizeof(MEDIA_FRAME_INFO));
			if (NULL != channelid)		*channelid = bufnode.channelid;

			if (bufnode.flag	!= BUF_QUE_FLAG)
			{
				//Unlock();
				SSQ_Clear(pObj);
				ReleaseMutex(pObj->hMutex);
				SSQ_TRACE("[SSQ_GetDataByPosition]SSQ_��־������...\n");
				return -1;
			}

			*pOffset = sizeof(SS_BUF_T)-remain;

			if (NULL!=mediatype)	*mediatype = bufnode.mediatype;
			SSQ_TRACE("[SSQ_GetDataByPosition]3 frame length: %d\n", bufnode.frameinfo.length);
			if (NULL!=pbuf)	memcpy(pbuf, pObj->pQueData + *pOffset, bufnode.frameinfo.length);
			*pOffset += bufnode.frameinfo.length;
			*pTotalSize -= (frameinfo->length+sizeof(SS_BUF_T));

			SSQ_TRACE("[SSQ_GetDataByPosition]GET DATA OK..\n");
			if (MEDIA_TYPE_VIDEO==bufnode.mediatype && clearflag==0x01)	pObj->pQueHeader->videoframes --;
		}
		else
		{
			SSQ_TRACE("[SSQ_GetDataByPosition]�쳣...   REMAIN <= 0....\n");
		}

		ret = 1000;
	}
	
	//Unlock();
	ReleaseMutex(pObj->hMutex);

	return ret;
}


int		SSQ_TRACE(char* szFormat, ...)
{
#ifdef _DEBUG
  char buff[1024] = {0,};
  wchar_t wszbuff[1024] = {0,};
  va_list args;
  va_start(args,szFormat);
  _vsnprintf(buff, 1023, szFormat,args);
  va_end(args);

  MByteToWChar(buff, wszbuff, sizeof(wszbuff)/sizeof(wszbuff[0]));
#ifdef _WIN32
	OutputDebugString(wszbuff);
#endif
	printf("TRACE: %s", buff);
#endif

	return 0;
}
