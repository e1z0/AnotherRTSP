
// Intel Media Hardware Codec SDK Interface [8/17/2016 SwordTwelve]

#ifndef INTELHARDCODEC_INTERFACE_H
#define INTELHARDCODEC_INTERFACE_H

#ifdef __cplusplus


class IntelHardCodec_Interface
{
	//�����ӿ�
public:
public:	//DLL �ӿ�
	virtual int  WINAPI Init(HWND hWnd,int mode = 1) = 0;
	virtual int  WINAPI Decode(unsigned char * pData, int len, OUT unsigned char * pYUVData) = 0;
	virtual void WINAPI	Close() = 0;

};

//��Ƶ��ȡ����ӿ�ָ������
typedef	IntelHardCodec_Interface*	LPIntelHardDecoder;	

LPIntelHardDecoder	APIENTRY Create_IntelHardDecoder();//�������ƽӿ�ָ��
void APIENTRY Release_IntelHardDecoder(LPIntelHardDecoder lpHardDecoder);//���ٿ��ƽӿ�ָ��

#endif//__cplusplus
#endif//INTELHARDCODEC_INTERFACE_H