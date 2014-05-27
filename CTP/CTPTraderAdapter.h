/////////////////////////////////////////////////////////////////////////
/// ���ڼ��� CTP C++ ==> .Net Framework Adapter
/// Author:	shawn666.liu@gmail.com
/// Date: 20120422
/////////////////////////////////////////////////////////////////////////

#pragma once


#include "Util.h"
#include "TraderSpi.h"

using namespace Native;

namespace  Native{
	class CTraderSpi;
};

namespace CTP {

	/// <summary>
	/// �й���,TraderAPI Adapter
	/// </summary>
	public ref class CTPTraderAdapter
	{
	public:
		/// <summary>
		///����CTPTraderAdapter
		///����������Ϣ�ļ���Ŀ¼��Ĭ��Ϊ��ǰĿ¼
		/// </summary>
		CTPTraderAdapter(void);
		/// <summary>
		///����CTPTraderAdapter
		/// </summary>
		/// <param name="pszFlowPath">����������Ϣ�ļ���Ŀ¼��Ĭ��Ϊ��ǰĿ¼</param>
		CTPTraderAdapter(String^ pszFlowPath);
	private:
		~CTPTraderAdapter(void);
		CThostFtdcTraderApi* m_pApi;
		CTraderSpi* m_pSpi;
	public:
		/// <summary>
		///ɾ���ӿڶ�����
		///@remark ����ʹ�ñ��ӿڶ���ʱ,���øú���ɾ���ӿڶ���
		/// </summary>
		void Release();

		/// <summary>
		///��ʼ��
		///@remark ��ʼ�����л���,ֻ�е��ú�,�ӿڲſ�ʼ����
		/// </summary>
		void Init();

		/// <summary>
		///�ȴ��ӿ��߳̽�������
		///@return �߳��˳�����
		/// </summary>
		int Join();

		/// <summary>
		///��ȡ��ǰ������
		///@remark ֻ�е�¼�ɹ���,���ܵõ���ȷ�Ľ�����
		/// </summary>
		/// <returns>��ȡ���Ľ�����</returns>
		String^ GetTradingDay();

		/// <summary>
		///ע��ǰ�û������ַ
		///@param pszFrontAddress��ǰ�û������ַ��
		///@remark �����ַ�ĸ�ʽΪ����protocol://ipaddress:port�����磺��tcp://127.0.0.1:17001���� 
		///@remark ��tcp��������Э�飬��127.0.0.1�������������ַ����17001������������˿ںš�
		/// </summary>
		void RegisterFront(String^ pszFrontAddress);

		/// <summary>
		///ע�����ַ����������ַ
		///@param pszNsAddress�����ַ����������ַ��
		///@remark �����ַ�ĸ�ʽΪ����protocol://ipaddress:port�����磺��tcp://127.0.0.1:12001���� 
		///@remark ��tcp��������Э�飬��127.0.0.1�������������ַ����12001������������˿ںš�
		///@remark RegisterNameServer������RegisterFront
		/// </summary>
		void RegisterNameServer(String^ pszNsAddress);

		
		///ע�����ַ������û���Ϣ
		///@param pFensUserInfo���û���Ϣ��
		void RegisterFensUserInfo(ThostFtdcFensUserInfoField^ pFensUserInfo);

		///ע��ص��ӿ�
		///@param pSpi �����Իص��ӿ����ʵ��
		/// void RegisterSpi(ThostFtdcTraderSpi^ pSpi);

		/// <summary>
		///����˽������
		///@param nResumeType ˽�����ش���ʽ  
		///        THOST_TERT_RESTART:�ӱ������տ�ʼ�ش�
		///        THOST_TERT_RESUME:���ϴ��յ�������
		///        THOST_TERT_QUICK:ֻ���͵�¼��˽����������
		///@remark �÷���Ҫ��Init����ǰ���á����������򲻻��յ�˽���������ݡ�
		/// </summary>
		void SubscribePrivateTopic(EnumTeResumeType nResumeType);

		/// <summary>
		///���Ĺ�������
		///@param nResumeType �������ش���ʽ  
		///        THOST_TERT_RESTART:�ӱ������տ�ʼ�ش�
		///        THOST_TERT_RESUME:���ϴ��յ�������
		///        THOST_TERT_QUICK:ֻ���͵�¼�󹫹���������
		///@remark �÷���Ҫ��Init����ǰ���á����������򲻻��յ������������ݡ�
		/// </summary>
		void SubscribePublicTopic(EnumTeResumeType nResumeType);

		/// <summary>
		/// �ͻ�����֤����
		/// </summary>
		int ReqAuthenticate(ThostFtdcReqAuthenticateField^ pReqAuthenticateField, int nRequestID);

		/// <summary>
		///�û���¼����
		/// </summary>
		int ReqUserLogin(ThostFtdcReqUserLoginField^ pReqUserLoginField, int nRequestID);

		/// <summary>
		///�ǳ�����
		/// </summary>
		int ReqUserLogout(ThostFtdcUserLogoutField^ pUserLogout, int nRequestID);

		/// <summary>
		///�û������������
		/// </summary>
		int ReqUserPasswordUpdate(ThostFtdcUserPasswordUpdateField^ pUserPasswordUpdate, int nRequestID);

		/// <summary>
		///�ʽ��˻������������
		/// </summary>
		int ReqTradingAccountPasswordUpdate(ThostFtdcTradingAccountPasswordUpdateField^ pTradingAccountPasswordUpdate, int nRequestID);

		/// <summary>
		///����¼������
		/// </summary>
		int ReqOrderInsert(ThostFtdcInputOrderField^ pInputOrder, int nRequestID);

		/// <summary>
		///Ԥ��¼������
		/// </summary>
		int ReqParkedOrderInsert(ThostFtdcParkedOrderField^ pParkedOrder, int nRequestID);

		/// <summary>
		///Ԥ�񳷵�¼������
		/// </summary>
		int ReqParkedOrderAction(ThostFtdcParkedOrderActionField^ pParkedOrderAction, int nRequestID);

		/// <summary>
		/// ������������
		/// </summary>
		int ReqOrderAction(ThostFtdcInputOrderActionField^ pInputOrderAction, int nRequestID);

		/// <summary>
		///��ѯ��󱨵���������
		/// </summary>
		int ReqQueryMaxOrderVolume(ThostFtdcQueryMaxOrderVolumeField^ pQueryMaxOrderVolume, int nRequestID);

		/// <summary>
		/// Ͷ���߽�����ȷ��
		/// </summary>
		int ReqSettlementInfoConfirm(ThostFtdcSettlementInfoConfirmField^ pSettlementInfoConfirm, int nRequestID);

		/// <summary>
		/// ����ɾ��Ԥ��
		/// </summary>
		int ReqRemoveParkedOrder(ThostFtdcRemoveParkedOrderField^ pRemoveParkedOrder, int nRequestID);

		/// <summary>
		/// ����ɾ��Ԥ�񳷵�
		/// </summary>
		int ReqRemoveParkedOrderAction(ThostFtdcRemoveParkedOrderActionField^ pRemoveParkedOrderAction, int nRequestID);


		/// <summary>
		/// ִ������¼������
		/// </summary>
		int ReqExecOrderInsert(ThostFtdcInputExecOrderField^ pInputExecOrder, int nRequestID);

		/// <summary>
		/// ִ�������������
		/// </summary>
		int ReqExecOrderAction(ThostFtdcInputExecOrderActionField^ pInputExecOrderAction, int nRequestID);


		/// <summary>
		/// ѯ��¼������
		/// </summary>
		int ReqForQuoteInsert(ThostFtdcInputForQuoteField^ pInputForQuote, int nRequestID);

		/// <summary>
		/// ����¼������
		/// </summary>
		int ReqQuoteInsert(ThostFtdcInputQuoteField^ pInputQuote, int nRequestID);

		/// <summary>
		/// ���۲�������
		/// </summary>
		int ReqQuoteAction(ThostFtdcInputQuoteActionField^ pInputQuoteAction, int nRequestID);


		/// <summary>
		///�����ѯ����
		/// </summary>
		int ReqQryOrder(ThostFtdcQryOrderField^ pQryOrder, int nRequestID);

		/// <summary>
		///�����ѯ�ɽ�
		/// </summary>
		int ReqQryTrade(ThostFtdcQryTradeField^ pQryTrade, int nRequestID);

		/// <summary>
		///�����ѯͶ���ֲ߳�
		/// </summary>
		int ReqQryInvestorPosition(ThostFtdcQryInvestorPositionField^ pQryInvestorPosition, int nRequestID);

		/// <summary>
		///�����ѯ�ʽ��˻�
		/// </summary>
		int ReqQryTradingAccount(ThostFtdcQryTradingAccountField^ pQryTradingAccount, int nRequestID);

		/// <summary>
		///�����ѯͶ����
		/// </summary>
		int ReqQryInvestor(ThostFtdcQryInvestorField^ pQryInvestor, int nRequestID);

		/// <summary>
		///�����ѯ���ױ���
		/// </summary>
		int ReqQryTradingCode(ThostFtdcQryTradingCodeField^ pQryTradingCode, int nRequestID);

		/// <summary>
		///�����ѯ��Լ��֤����
		/// </summary>
		int ReqQryInstrumentMarginRate(ThostFtdcQryInstrumentMarginRateField^ pQryInstrumentMarginRate, int nRequestID);

		/// <summary>
		///�����ѯ��Լ��������
		/// </summary>
		int ReqQryInstrumentCommissionRate(ThostFtdcQryInstrumentCommissionRateField^ pQryInstrumentCommissionRate, int nRequestID);

		/// <summary>
		///�����ѯ������
		/// </summary>
		int ReqQryExchange(ThostFtdcQryExchangeField^ pQryExchange, int nRequestID);

		/// <summary>
		///�����ѯ��Ʒ
		/// </summary>
		int ReqQryProduct(ThostFtdcQryProductField^ pQryProduct, int nRequestID);

		/// <summary>
		///�����ѯ��Լ
		/// </summary>
		int ReqQryInstrument(ThostFtdcQryInstrumentField^ pQryInstrument, int nRequestID);

		/// <summary>
		///�����ѯ����
		/// </summary>
		int ReqQryDepthMarketData(ThostFtdcQryDepthMarketDataField^ pQryDepthMarketData, int nRequestID);

		/// <summary>
		///�����ѯͶ���߽�����
		/// </summary>
		int ReqQrySettlementInfo(ThostFtdcQrySettlementInfoField^ pQrySettlementInfo, int nRequestID);

		/// <summary>
		///�����ѯת������
		/// </summary>
		int ReqQryTransferBank(ThostFtdcQryTransferBankField^ pQryTransferBank, int nRequestID);

		/// <summary>
		///�����ѯͶ���ֲ߳���ϸ
		/// </summary>
		int ReqQryInvestorPositionDetail(ThostFtdcQryInvestorPositionDetailField^ pQryInvestorPositionDetail, int nRequestID);

		/// <summary>
		///�����ѯ�ͻ�֪ͨ
		/// </summary>
		int ReqQryNotice(ThostFtdcQryNoticeField^ pQryNotice, int nRequestID);

		/// <summary>
		///�����ѯ������Ϣȷ��
		/// </summary>
		int ReqQrySettlementInfoConfirm(ThostFtdcQrySettlementInfoConfirmField^ pQrySettlementInfoConfirm, int nRequestID);

		/// <summary>
		///�����ѯͶ���ֲ߳���ϸ
		/// </summary>
		int ReqQryInvestorPositionCombineDetail(ThostFtdcQryInvestorPositionCombineDetailField^ pQryInvestorPositionCombineDetail, int nRequestID);

		/// <summary>
		///�����ѯ��֤����ϵͳ���͹�˾�ʽ��˻���Կ
		/// </summary>
		int ReqQryCFMMCTradingAccountKey(ThostFtdcQryCFMMCTradingAccountKeyField^ pQryCFMMCTradingAccountKey, int nRequestID);

		/// <summary>
		///�����ѯ�ֵ��۵���Ϣ
		/// </summary>
		int ReqQryEWarrantOffset(ThostFtdcQryEWarrantOffsetField^ pQryEWarrantOffset, int nRequestID);


		/// <summary>
		///�����ѯͶ����Ʒ��/��Ʒ�ֱ�֤��
		/// </summary>
		int ReqQryInvestorProductGroupMargin(ThostFtdcQryInvestorProductGroupMarginField^ pQryInvestorProductGroupMargin, int nRequestID);

		/// <summary>
		///�����ѯ��������֤����
		/// </summary>
		int ReqQryExchangeMarginRate(ThostFtdcQryExchangeMarginRateField^ pQryExchangeMarginRate, int nRequestID);

		/// <summary>
		///�����ѯ������������֤����
		/// </summary>
		int ReqQryExchangeMarginRateAdjust(ThostFtdcQryExchangeMarginRateAdjustField^ pQryExchangeMarginRateAdjust, int nRequestID);

		/// <summary>
		///�����ѯ����
		/// </summary>
		int ReqQryExchangeRate(ThostFtdcQryExchangeRateField^ pQryExchangeRate, int nRequestID);

		/// <summary>
		///�����ѯ�����������Ա����Ȩ��
		/// </summary>
		int ReqQrySecAgentACIDMap(ThostFtdcQrySecAgentACIDMapField^ pQrySecAgentACIDMap, int nRequestID);


		/// <summary>
		///�����ѯ��Ȩ���׳ɱ�
		/// </summary>
		int ReqQryOptionInstrTradeCost(ThostFtdcQryOptionInstrTradeCostField^ pQryOptionInstrTradeCost, int nRequestID);

		/// <summary>
		///�����ѯ��Ȩ��Լ������
		/// </summary>
		int ReqQryOptionInstrCommRate(ThostFtdcQryOptionInstrCommRateField^ pQryOptionInstrCommRate, int nRequestID);

		/// <summary>
		///�����ѯִ������
		/// </summary>
		int ReqQryExecOrder(ThostFtdcQryExecOrderField^ pQryExecOrder, int nRequestID);

		/// <summary>
		///�����ѯѯ��
		/// </summary>
		int ReqQryForQuote(ThostFtdcQryForQuoteField^ pQryForQuote, int nRequestID);

		/// <summary>
		///�����ѯ����
		/// </summary>
		int ReqQryQuote(ThostFtdcQryQuoteField^ pQryQuote, int nRequestID);


		/// <summary>
		///�����ѯת����ˮ
		/// </summary>
		int ReqQryTransferSerial(ThostFtdcQryTransferSerialField^ pQryTransferSerial, int nRequestID);

		/// <summary>
		///�����ѯ����ǩԼ��ϵ
		/// </summary>
		int ReqQryAccountregister(ThostFtdcQryAccountregisterField^ pQryAccountregister, int nRequestID);

		/// <summary>
		///�����ѯǩԼ����
		/// </summary>
		int ReqQryContractBank(ThostFtdcQryContractBankField^ pQryContractBank, int nRequestID);

		/// <summary>
		///�����ѯԤ��
		/// </summary>
		int ReqQryParkedOrder(ThostFtdcQryParkedOrderField^ pQryParkedOrder, int nRequestID);

		/// <summary>
		///�����ѯԤ�񳷵�
		/// </summary>
		int ReqQryParkedOrderAction(ThostFtdcQryParkedOrderActionField^ pQryParkedOrderAction, int nRequestID);

		/// <summary>
		///�����ѯ����֪ͨ
		/// </summary>
		int ReqQryTradingNotice(ThostFtdcQryTradingNoticeField^ pQryTradingNotice, int nRequestID);

		/// <summary>
		///�����ѯ���͹�˾���ײ���
		/// </summary>
		int ReqQryBrokerTradingParams(ThostFtdcQryBrokerTradingParamsField^ pQryBrokerTradingParams, int nRequestID);

		/// <summary>
		///�����ѯ���͹�˾�����㷨
		/// </summary>
		int ReqQryBrokerTradingAlgos(ThostFtdcQryBrokerTradingAlgosField^ pQryBrokerTradingAlgos, int nRequestID);

		/// <summary>
		///�ڻ����������ʽ�ת�ڻ�����
		/// </summary>
		int ReqFromBankToFutureByFuture(ThostFtdcReqTransferField^ pReqTransfer, int nRequestID);

		/// <summary>
		///�ڻ������ڻ��ʽ�ת��������
		/// </summary>
		int ReqFromFutureToBankByFuture(ThostFtdcReqTransferField^ pReqTransfer, int nRequestID);

		/// <summary>
		///�ڻ������ѯ�����������
		/// </summary>
		int ReqQueryBankAccountMoneyByFuture(ThostFtdcReqQueryAccountField^ pReqQueryAccount, int nRequestID);

		// events
	public:
		/// <summary>
		/// ���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
		/// </summary>
		event FrontConnected^ OnFrontConnected {
			void add(FrontConnected^ handler ) {
				OnFrontConnected_delegate += handler;
			}
			void remove(FrontConnected^ handler ) {
				OnFrontConnected_delegate -= handler;
			}
			void raise() {
				if(OnFrontConnected_delegate)
					OnFrontConnected_delegate();
			}
		}
		/// <summary>
		/// ���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
		/// ����ԭ��
		/// 0x1001 �����ʧ��
		/// 0x1002 ����дʧ��
		/// 0x2001 ����������ʱ
		/// 0x2002 ��������ʧ��
		/// 0x2003 �յ�������
		/// </summary>
		event FrontDisconnected^ OnFrontDisconnected {
			void add(FrontDisconnected^ handler ) {
				OnFrontDisconnected_delegate += handler;
			}
			void remove(FrontDisconnected^ handler ) {
				OnFrontDisconnected_delegate -= handler;
			}
			void raise(int nReason) {
				if(OnFrontDisconnected_delegate)
					OnFrontDisconnected_delegate(nReason);
			}
		}
		/// <summary>
		///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
		///@param nTimeLapse �����ϴν��ձ��ĵ�ʱ��
		/// </summary>
		event HeartBeatWarning^ OnHeartBeatWarning {
			void add(HeartBeatWarning^ handler ) {
				OnHeartBeatWarning_delegate += handler;
			}
			void remove(HeartBeatWarning^ handler ) {
				OnHeartBeatWarning_delegate -= handler;
			}
			void raise(int nTimeLapse) {
				if(OnHeartBeatWarning_delegate)
					OnHeartBeatWarning_delegate(nTimeLapse);
			}
		}
		/// <summary>
		/// �ͻ�����֤��Ӧ
		/// </summary>
		event RspAuthenticate^ OnRspAuthenticate {
			void add(RspAuthenticate^ handler ) {
				OnRspAuthenticate_delegate += handler;
			}
			void remove(RspAuthenticate^ handler ) {
				OnRspAuthenticate_delegate -= handler;
			}
			void raise(ThostFtdcRspAuthenticateField^ pRspAuthenticateField, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspAuthenticate_delegate)
					OnRspAuthenticate_delegate(pRspAuthenticateField, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// ��¼������Ӧ
		/// </summary>
		event RspUserLogin^ OnRspUserLogin {
			void add(RspUserLogin^ handler ) {
				OnRspUserLogin_delegate += handler;
			}
			void remove(RspUserLogin^ handler ) {
				OnRspUserLogin_delegate -= handler;
			}
			void raise(ThostFtdcRspUserLoginField^ pRspUserLogin, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspUserLogin_delegate)
					OnRspUserLogin_delegate(pRspUserLogin, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �ǳ�������Ӧ
		/// </summary>
		event RspUserLogout^ OnRspUserLogout {
			void add(RspUserLogout^ handler ) {
				OnRspUserLogout_delegate += handler;
			}
			void remove(RspUserLogout^ handler ) {
				OnRspUserLogout_delegate -= handler;
			}
			void raise(ThostFtdcUserLogoutField^ pUserLogout, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspUserLogout_delegate)
					OnRspUserLogout_delegate(pUserLogout, pRspInfo, nRequestID, bIsLast);
			}
		}
		
		/// <summary>
		/// �û��������������Ӧ
		/// </summary>
		event RspUserPasswordUpdate^ OnRspUserPasswordUpdate {
			void add(RspUserPasswordUpdate^ handler ) {
				OnRspUserPasswordUpdate_delegate += handler;
			}
			void remove(RspUserPasswordUpdate^ handler ) {
				OnRspUserPasswordUpdate_delegate -= handler;
			}
			void raise(ThostFtdcUserPasswordUpdateField^ pUserPasswordUpdate, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspUserPasswordUpdate_delegate)
					OnRspUserPasswordUpdate_delegate(pUserPasswordUpdate, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �ʽ��˻��������������Ӧ
		/// </summary>
		event RspTradingAccountPasswordUpdate^ OnRspTradingAccountPasswordUpdate {
			void add(RspTradingAccountPasswordUpdate^ handler ) {
				OnRspTradingAccountPasswordUpdate_delegate += handler;
			}
			void remove(RspTradingAccountPasswordUpdate^ handler ) {
				OnRspTradingAccountPasswordUpdate_delegate -= handler;
			}
			void raise(ThostFtdcTradingAccountPasswordUpdateField^ pTradingAccountPasswordUpdate, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspTradingAccountPasswordUpdate_delegate)
					OnRspTradingAccountPasswordUpdate_delegate(pTradingAccountPasswordUpdate, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// ����¼��������Ӧ
		/// </summary>
		event RspOrderInsert^ OnRspOrderInsert {
			void add(RspOrderInsert^ handler ) {
				OnRspOrderInsert_delegate += handler;
			}
			void remove(RspOrderInsert^ handler ) {
				OnRspOrderInsert_delegate -= handler;
			}
			void raise(ThostFtdcInputOrderField^ pInputOrder, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspOrderInsert_delegate)
					OnRspOrderInsert_delegate(pInputOrder, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// Ԥ��¼��������Ӧ
		/// </summary>
		event RspParkedOrderInsert^ OnRspParkedOrderInsert {
			void add(RspParkedOrderInsert^ handler ) {
				OnRspParkedOrderInsert_delegate += handler;
			}
			void remove(RspParkedOrderInsert^ handler ) {
				OnRspParkedOrderInsert_delegate -= handler;
			}
			void raise(ThostFtdcParkedOrderField^ pParkedOrder, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspParkedOrderInsert_delegate)
					OnRspParkedOrderInsert_delegate(pParkedOrder, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// Ԥ�񳷵�¼��������Ӧ
		/// </summary>
		event RspParkedOrderAction^ OnRspParkedOrderAction {
			void add(RspParkedOrderAction^ handler ) {
				OnRspParkedOrderAction_delegate += handler;
			}
			void remove(RspParkedOrderAction^ handler ) {
				OnRspParkedOrderAction_delegate -= handler;
			}
			void raise(ThostFtdcParkedOrderActionField^ pParkedOrderAction, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspParkedOrderAction_delegate)
					OnRspParkedOrderAction_delegate(pParkedOrderAction, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// ��������������Ӧ
		/// </summary>
		event RspOrderAction^ OnRspOrderAction {
			void add(RspOrderAction^ handler ) {
				OnRspOrderAction_delegate += handler;
			}
			void remove(RspOrderAction^ handler ) {
				OnRspOrderAction_delegate -= handler;
			}
			void raise(ThostFtdcInputOrderActionField^ pInputOrderAction, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspOrderAction_delegate)
					OnRspOrderAction_delegate(pInputOrderAction, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// ��ѯ��󱨵�������Ӧ
		/// </summary>
		event RspQueryMaxOrderVolume^ OnRspQueryMaxOrderVolume {
			void add(RspQueryMaxOrderVolume^ handler ) {
				OnRspQueryMaxOrderVolume_delegate += handler;
			}
			void remove(RspQueryMaxOrderVolume^ handler ) {
				OnRspQueryMaxOrderVolume_delegate -= handler;
			}
			void raise(ThostFtdcQueryMaxOrderVolumeField^ pQueryMaxOrderVolume, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQueryMaxOrderVolume_delegate)
					OnRspQueryMaxOrderVolume_delegate(pQueryMaxOrderVolume, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// Ͷ���߽�����ȷ����Ӧ
		/// </summary>
		event RspSettlementInfoConfirm^ OnRspSettlementInfoConfirm {
			void add(RspSettlementInfoConfirm^ handler ) {
				OnRspSettlementInfoConfirm_delegate += handler;
			}
			void remove(RspSettlementInfoConfirm^ handler ) {
				OnRspSettlementInfoConfirm_delegate -= handler;
			}
			void raise(ThostFtdcSettlementInfoConfirmField^ pSettlementInfoConfirm, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspSettlementInfoConfirm_delegate)
					OnRspSettlementInfoConfirm_delegate(pSettlementInfoConfirm, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// ɾ��Ԥ����Ӧ
		/// </summary>
		event RspRemoveParkedOrder^ OnRspRemoveParkedOrder {
			void add(RspRemoveParkedOrder^ handler ) {
				OnRspRemoveParkedOrder_delegate += handler;
			}
			void remove(RspRemoveParkedOrder^ handler ) {
				OnRspRemoveParkedOrder_delegate -= handler;
			}
			void raise(ThostFtdcRemoveParkedOrderField^ pRemoveParkedOrder, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspRemoveParkedOrder_delegate)
					OnRspRemoveParkedOrder_delegate(pRemoveParkedOrder, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// ɾ��Ԥ�񳷵���Ӧ
		/// </summary>
		event RspRemoveParkedOrderAction^ OnRspRemoveParkedOrderAction {
			void add(RspRemoveParkedOrderAction^ handler ) {
				OnRspRemoveParkedOrderAction_delegate += handler;
			}
			void remove(RspRemoveParkedOrderAction^ handler ) {
				OnRspRemoveParkedOrderAction_delegate -= handler;
			}
			void raise(ThostFtdcRemoveParkedOrderActionField^ pRemoveParkedOrderAction, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspRemoveParkedOrderAction_delegate)
					OnRspRemoveParkedOrderAction_delegate(pRemoveParkedOrderAction, pRspInfo, nRequestID, bIsLast);
			}
		}

		/// <summary>
		/// ִ������¼��������Ӧ
		/// </summary>
		event RspExecOrderInsert^ OnRspExecOrderInsert {
			void add(RspExecOrderInsert^ handler ) {
				OnRspExecOrderInsert_delegate += handler;
			}
			void remove(RspExecOrderInsert^ handler ) {
				OnRspExecOrderInsert_delegate -= handler;
			}
			void raise(ThostFtdcInputExecOrderField^ pInputExecOrder, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspExecOrderInsert_delegate)
					OnRspExecOrderInsert_delegate(pInputExecOrder, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// ִ���������������Ӧ
		/// </summary>
		event RspExecOrderAction^ OnRspExecOrderAction {
			void add(RspExecOrderAction^ handler ) {
				OnRspExecOrderAction_delegate += handler;
			}
			void remove(RspExecOrderAction^ handler ) {
				OnRspExecOrderAction_delegate -= handler;
			}
			void raise(ThostFtdcInputExecOrderActionField^ pInputExecOrderAction, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspExecOrderAction_delegate)
					OnRspExecOrderAction_delegate(pInputExecOrderAction, pRspInfo, nRequestID, bIsLast);
			}
		}

		/// <summary>
		/// ѯ��¼��������Ӧ
		/// </summary>
		event RspForQuoteInsert^ OnRspForQuoteInsert {
			void add(RspForQuoteInsert^ handler ) {
				OnRspForQuoteInsert_delegate += handler;
			}
			void remove(RspForQuoteInsert^ handler ) {
				OnRspForQuoteInsert_delegate -= handler;
			}
			void raise(ThostFtdcInputForQuoteField^ pInputForQuote, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspForQuoteInsert_delegate)
					OnRspForQuoteInsert_delegate(pInputForQuote, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// ����¼��������Ӧ
		/// </summary>
		event RspQuoteInsert^ OnRspQuoteInsert {
			void add(RspQuoteInsert^ handler ) {
				OnRspQuoteInsert_delegate += handler;
			}
			void remove(RspQuoteInsert^ handler ) {
				OnRspQuoteInsert_delegate -= handler;
			}
			void raise(ThostFtdcInputQuoteField^ pInputQuote, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQuoteInsert_delegate)
					OnRspQuoteInsert_delegate(pInputQuote, pRspInfo, nRequestID, bIsLast);
			}
		}

		/// <summary>
		/// ���۲���������Ӧ
		/// </summary>
		event RspQuoteAction^ OnRspQuoteAction {
			void add(RspQuoteAction^ handler ) {
				OnRspQuoteAction_delegate += handler;
			}
			void remove(RspQuoteAction^ handler ) {
				OnRspQuoteAction_delegate -= handler;
			}
			void raise(ThostFtdcInputQuoteActionField^ pInputQuoteAction, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQuoteAction_delegate)
					OnRspQuoteAction_delegate(pInputQuoteAction, pRspInfo, nRequestID, bIsLast);
			}
		}

		/// <summary>
		/// �����ѯ������Ӧ
		/// </summary>
		event RspQryOrder^ OnRspQryOrder {
			void add(RspQryOrder^ handler ) {
				OnRspQryOrder_delegate += handler;
			}
			void remove(RspQryOrder^ handler ) {
				OnRspQryOrder_delegate -= handler;
			}
			void raise(ThostFtdcOrderField^ pOrder, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryOrder_delegate)
					OnRspQryOrder_delegate(pOrder, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ�ɽ���Ӧ
		/// </summary>
		event RspQryTrade^ OnRspQryTrade {
			void add(RspQryTrade^ handler ) {
				OnRspQryTrade_delegate += handler;
			}
			void remove(RspQryTrade^ handler ) {
				OnRspQryTrade_delegate -= handler;
			}
			void raise(ThostFtdcTradeField^ pTrade, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryTrade_delegate)
					OnRspQryTrade_delegate(pTrade, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯͶ���ֲ߳���Ӧ
		/// </summary>
		event RspQryInvestorPosition^ OnRspQryInvestorPosition {
			void add(RspQryInvestorPosition^ handler ) {
				OnRspQryInvestorPosition_delegate += handler;
			}
			void remove(RspQryInvestorPosition^ handler ) {
				OnRspQryInvestorPosition_delegate -= handler;
			}
			void raise(ThostFtdcInvestorPositionField^ pInvestorPosition, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryInvestorPosition_delegate)
					OnRspQryInvestorPosition_delegate(pInvestorPosition, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ�ʽ��˻���Ӧ
		/// </summary>
		event RspQryTradingAccount^ OnRspQryTradingAccount {
			void add(RspQryTradingAccount^ handler ) {
				OnRspQryTradingAccount_delegate += handler;
			}
			void remove(RspQryTradingAccount^ handler ) {
				OnRspQryTradingAccount_delegate -= handler;
			}
			void raise(ThostFtdcTradingAccountField^ pTradingAccount, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryTradingAccount_delegate)
					OnRspQryTradingAccount_delegate(pTradingAccount, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯͶ������Ӧ
		/// </summary>
		event RspQryInvestor^ OnRspQryInvestor {
			void add(RspQryInvestor^ handler ) {
				OnRspQryInvestor_delegate += handler;
			}
			void remove(RspQryInvestor^ handler ) {
				OnRspQryInvestor_delegate -= handler;
			}
			void raise(ThostFtdcInvestorField^ pInvestor, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryInvestor_delegate)
					OnRspQryInvestor_delegate(pInvestor, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ���ױ�����Ӧ
		/// </summary>
		event RspQryTradingCode^ OnRspQryTradingCode {
			void add(RspQryTradingCode^ handler ) {
				OnRspQryTradingCode_delegate += handler;
			}
			void remove(RspQryTradingCode^ handler ) {
				OnRspQryTradingCode_delegate -= handler;
			}
			void raise(ThostFtdcTradingCodeField^ pTradingCode, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryTradingCode_delegate)
					OnRspQryTradingCode_delegate(pTradingCode, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ��Լ��֤������Ӧ
		/// </summary>
		event RspQryInstrumentMarginRate^ OnRspQryInstrumentMarginRate {
			void add(RspQryInstrumentMarginRate^ handler ) {
				OnRspQryInstrumentMarginRate_delegate += handler;
			}
			void remove(RspQryInstrumentMarginRate^ handler ) {
				OnRspQryInstrumentMarginRate_delegate -= handler;
			}
			void raise(ThostFtdcInstrumentMarginRateField^ pInstrumentMarginRate, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryInstrumentMarginRate_delegate)
					OnRspQryInstrumentMarginRate_delegate(pInstrumentMarginRate, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ��Լ����������Ӧ
		/// </summary>
		event RspQryInstrumentCommissionRate^ OnRspQryInstrumentCommissionRate {
			void add(RspQryInstrumentCommissionRate^ handler ) {
				OnRspQryInstrumentCommissionRate_delegate += handler;
			}
			void remove(RspQryInstrumentCommissionRate^ handler ) {
				OnRspQryInstrumentCommissionRate_delegate -= handler;
			}
			void raise(ThostFtdcInstrumentCommissionRateField^ pInstrumentCommissionRate, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryInstrumentCommissionRate_delegate)
					OnRspQryInstrumentCommissionRate_delegate(pInstrumentCommissionRate, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ��������Ӧ
		/// </summary>
		event RspQryExchange^ OnRspQryExchange {
			void add(RspQryExchange^ handler ) {
				OnRspQryExchange_delegate += handler;
			}
			void remove(RspQryExchange^ handler ) {
				OnRspQryExchange_delegate -= handler;
			}
			void raise(ThostFtdcExchangeField^ pExchange, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryExchange_delegate)
					OnRspQryExchange_delegate(pExchange, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ��Ʒ��Ӧ
		/// </summary>
		event RspQryProduct^ OnRspQryProduct {
			void add(RspQryProduct^ handler ) {
				OnRspQryProduct_delegate += handler;
			}
			void remove(RspQryProduct^ handler ) {
				OnRspQryProduct_delegate -= handler;
			}
			void raise(ThostFtdcProductField^ pProduct, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryProduct_delegate)
					OnRspQryProduct_delegate(pProduct, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ��Լ��Ӧ
		/// </summary>
		event RspQryInstrument^ OnRspQryInstrument {
			void add(RspQryInstrument^ handler ) {
				OnRspQryInstrument_delegate += handler;
			}
			void remove(RspQryInstrument^ handler ) {
				OnRspQryInstrument_delegate -= handler;
			}
			void raise(ThostFtdcInstrumentField^ pInstrument, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryInstrument_delegate)
					OnRspQryInstrument_delegate(pInstrument, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ������Ӧ
		/// </summary>
		event RspQryDepthMarketData^ OnRspQryDepthMarketData {
			void add(RspQryDepthMarketData^ handler ) {
				OnRspQryDepthMarketData_delegate += handler;
			}
			void remove(RspQryDepthMarketData^ handler ) {
				OnRspQryDepthMarketData_delegate -= handler;
			}
			void raise(ThostFtdcDepthMarketDataField^ pDepthMarketData, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryDepthMarketData_delegate)
					OnRspQryDepthMarketData_delegate(pDepthMarketData, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯͶ���߽�������Ӧ
		/// </summary>
		event RspQrySettlementInfo^ OnRspQrySettlementInfo {
			void add(RspQrySettlementInfo^ handler ) {
				OnRspQrySettlementInfo_delegate += handler;
			}
			void remove(RspQrySettlementInfo^ handler ) {
				OnRspQrySettlementInfo_delegate -= handler;
			}
			void raise(ThostFtdcSettlementInfoField^ pSettlementInfo, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQrySettlementInfo_delegate)
					OnRspQrySettlementInfo_delegate(pSettlementInfo, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯת��������Ӧ
		/// </summary>
		event RspQryTransferBank^ OnRspQryTransferBank {
			void add(RspQryTransferBank^ handler ) {
				OnRspQryTransferBank_delegate += handler;
			}
			void remove(RspQryTransferBank^ handler ) {
				OnRspQryTransferBank_delegate -= handler;
			}
			void raise(ThostFtdcTransferBankField^ pTransferBank, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryTransferBank_delegate)
					OnRspQryTransferBank_delegate(pTransferBank, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯͶ���ֲ߳���ϸ��Ӧ
		/// </summary>
		event RspQryInvestorPositionDetail^ OnRspQryInvestorPositionDetail {
			void add(RspQryInvestorPositionDetail^ handler ) {
				OnRspQryInvestorPositionDetail_delegate += handler;
			}
			void remove(RspQryInvestorPositionDetail^ handler ) {
				OnRspQryInvestorPositionDetail_delegate -= handler;
			}
			void raise(ThostFtdcInvestorPositionDetailField^ pInvestorPositionDetail, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryInvestorPositionDetail_delegate)
					OnRspQryInvestorPositionDetail_delegate(pInvestorPositionDetail, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ�ͻ�֪ͨ��Ӧ
		/// </summary>
		event RspQryNotice^ OnRspQryNotice {
			void add(RspQryNotice^ handler ) {
				OnRspQryNotice_delegate += handler;
			}
			void remove(RspQryNotice^ handler ) {
				OnRspQryNotice_delegate -= handler;
			}
			void raise(ThostFtdcNoticeField^ pNotice, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryNotice_delegate)
					OnRspQryNotice_delegate(pNotice, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ������Ϣȷ����Ӧ
		/// </summary>
		event RspQrySettlementInfoConfirm^ OnRspQrySettlementInfoConfirm {
			void add(RspQrySettlementInfoConfirm^ handler ) {
				OnRspQrySettlementInfoConfirm_delegate += handler;
			}
			void remove(RspQrySettlementInfoConfirm^ handler ) {
				OnRspQrySettlementInfoConfirm_delegate -= handler;
			}
			void raise(ThostFtdcSettlementInfoConfirmField^ pSettlementInfoConfirm, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQrySettlementInfoConfirm_delegate)
					OnRspQrySettlementInfoConfirm_delegate(pSettlementInfoConfirm, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯͶ���ֲ߳���ϸ��Ӧ
		/// </summary>
		event RspQryInvestorPositionCombineDetail^ OnRspQryInvestorPositionCombineDetail {
			void add(RspQryInvestorPositionCombineDetail^ handler ) {
				OnRspQryInvestorPositionCombineDetail_delegate += handler;
			}
			void remove(RspQryInvestorPositionCombineDetail^ handler ) {
				OnRspQryInvestorPositionCombineDetail_delegate -= handler;
			}
			void raise(ThostFtdcInvestorPositionCombineDetailField^ pInvestorPositionCombineDetail, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryInvestorPositionCombineDetail_delegate)
					OnRspQryInvestorPositionCombineDetail_delegate(pInvestorPositionCombineDetail, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// ��ѯ��֤����ϵͳ���͹�˾�ʽ��˻���Կ��Ӧ
		/// </summary>
		event RspQryCFMMCTradingAccountKey^ OnRspQryCFMMCTradingAccountKey {
			void add(RspQryCFMMCTradingAccountKey^ handler ) {
				OnRspQryCFMMCTradingAccountKey_delegate += handler;
			}
			void remove(RspQryCFMMCTradingAccountKey^ handler ) {
				OnRspQryCFMMCTradingAccountKey_delegate -= handler;
			}
			void raise(ThostFtdcCFMMCTradingAccountKeyField^ pCFMMCTradingAccountKey, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryCFMMCTradingAccountKey_delegate)
					OnRspQryCFMMCTradingAccountKey_delegate(pCFMMCTradingAccountKey, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ�ֵ��۵���Ϣ��Ӧ
		/// </summary>
		event RspQryEWarrantOffset^ OnRspQryEWarrantOffset {
			void add(RspQryEWarrantOffset^ handler ) {
				OnRspQryEWarrantOffset_delegate += handler;
			}
			void remove(RspQryEWarrantOffset^ handler ) {
				OnRspQryEWarrantOffset_delegate -= handler;
			}
			void raise(ThostFtdcEWarrantOffsetField^ pEWarrantOffset, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryEWarrantOffset_delegate)
					OnRspQryEWarrantOffset_delegate(pEWarrantOffset, pRspInfo, nRequestID, bIsLast);
			}
		}

		/// <summary>
		/// �����ѯͶ����Ʒ��/��Ʒ�ֱ�֤����Ӧ
		/// </summary>
		event RspQryInvestorProductGroupMargin^ OnRspQryInvestorProductGroupMargin {
			void add(RspQryInvestorProductGroupMargin^ handler ) {
				OnRspQryInvestorProductGroupMargin_delegate += handler;
			}
			void remove(RspQryInvestorProductGroupMargin^ handler ) {
				OnRspQryInvestorProductGroupMargin_delegate -= handler;
			}
			void raise(ThostFtdcInvestorProductGroupMarginField^ pInvestorProductGroupMargin, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryInvestorProductGroupMargin_delegate)
					OnRspQryInvestorProductGroupMargin_delegate(pInvestorProductGroupMargin, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ��������֤������Ӧ
		/// </summary>
		event RspQryExchangeMarginRate^ OnRspQryExchangeMarginRate {
			void add(RspQryExchangeMarginRate^ handler ) {
				OnRspQryExchangeMarginRate_delegate += handler;
			}
			void remove(RspQryExchangeMarginRate^ handler ) {
				OnRspQryExchangeMarginRate_delegate -= handler;
			}
			void raise(ThostFtdcExchangeMarginRateField^ pExchangeMarginRate, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryExchangeMarginRate_delegate)
					OnRspQryExchangeMarginRate_delegate(pExchangeMarginRate, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ������������֤������Ӧ
		/// </summary>
		event RspQryExchangeMarginRateAdjust^ OnRspQryExchangeMarginRateAdjust {
			void add(RspQryExchangeMarginRateAdjust^ handler ) {
				OnRspQryExchangeMarginRateAdjust_delegate += handler;
			}
			void remove(RspQryExchangeMarginRateAdjust^ handler ) {
				OnRspQryExchangeMarginRateAdjust_delegate -= handler;
			}
			void raise(ThostFtdcExchangeMarginRateAdjustField^ pExchangeMarginRateAdjust, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryExchangeMarginRateAdjust_delegate)
					OnRspQryExchangeMarginRateAdjust_delegate(pExchangeMarginRateAdjust, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ������Ӧ
		/// </summary>
		event RspQryExchangeRate^ OnRspQryExchangeRate {
			void add(RspQryExchangeRate^ handler ) {
				OnRspQryExchangeRate_delegate += handler;
			}
			void remove(RspQryExchangeRate^ handler ) {
				OnRspQryExchangeRate_delegate -= handler;
			}
			void raise(ThostFtdcExchangeRateField^ pExchangeRate, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryExchangeRate_delegate)
					OnRspQryExchangeRate_delegate(pExchangeRate, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ�����������Ա����Ȩ����Ӧ
		/// </summary>
		event RspQrySecAgentACIDMap^ OnRspQrySecAgentACIDMap {
			void add(RspQrySecAgentACIDMap^ handler ) {
				OnRspQrySecAgentACIDMap_delegate += handler;
			}
			void remove(RspQrySecAgentACIDMap^ handler ) {
				OnRspQrySecAgentACIDMap_delegate -= handler;
			}
			void raise(ThostFtdcSecAgentACIDMapField^ pSecAgentACIDMap, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQrySecAgentACIDMap_delegate)
					OnRspQrySecAgentACIDMap_delegate(pSecAgentACIDMap, pRspInfo, nRequestID, bIsLast);
			}
		}

		/// <summary>
		/// �����ѯ��Ȩ���׳ɱ���Ӧ
		/// </summary>
		event RspQryOptionInstrTradeCost^ OnRspQryOptionInstrTradeCost {
			void add(RspQryOptionInstrTradeCost^ handler ) {
				OnRspQryOptionInstrTradeCost_delegate += handler;
			}
			void remove(RspQryOptionInstrTradeCost^ handler ) {
				OnRspQryOptionInstrTradeCost_delegate -= handler;
			}
			void raise(ThostFtdcOptionInstrTradeCostField^ pOptionInstrTradeCost, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryOptionInstrTradeCost_delegate)
					OnRspQryOptionInstrTradeCost_delegate(pOptionInstrTradeCost, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ��Ȩ��Լ��������Ӧ
		/// </summary>
		event RspQryOptionInstrCommRate^ OnRspQryOptionInstrCommRate {
			void add(RspQryOptionInstrCommRate^ handler ) {
				OnRspQryOptionInstrCommRate_delegate += handler;
			}
			void remove(RspQryOptionInstrCommRate^ handler ) {
				OnRspQryOptionInstrCommRate_delegate -= handler;
			}
			void raise(ThostFtdcOptionInstrCommRateField^ pOptionInstrCommRate, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryOptionInstrCommRate_delegate)
					OnRspQryOptionInstrCommRate_delegate(pOptionInstrCommRate, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯִ��������Ӧ
		/// </summary>
		event RspQryExecOrder^ OnRspQryExecOrder {
			void add(RspQryExecOrder^ handler ) {
				OnRspQryExecOrder_delegate += handler;
			}
			void remove(RspQryExecOrder^ handler ) {
				OnRspQryExecOrder_delegate -= handler;
			}
			void raise(ThostFtdcExecOrderField^ pExecOrder, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryExecOrder_delegate)
					OnRspQryExecOrder_delegate(pExecOrder, pRspInfo, nRequestID, bIsLast);
			}
		}

		/// <summary>
		/// �����ѯѯ����Ӧ
		/// </summary>
		event RspQryForQuote^ OnRspQryForQuote {
			void add(RspQryForQuote^ handler ) {
				OnRspQryForQuote_delegate += handler;
			}
			void remove(RspQryForQuote^ handler ) {
				OnRspQryForQuote_delegate -= handler;
			}
			void raise(ThostFtdcForQuoteField^ pForQuote, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryForQuote_delegate)
					OnRspQryForQuote_delegate(pForQuote, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ������Ӧ
		/// </summary>
		event RspQryQuote^ OnRspQryQuote {
			void add(RspQryQuote^ handler ) {
				OnRspQryQuote_delegate += handler;
			}
			void remove(RspQryQuote^ handler ) {
				OnRspQryQuote_delegate -= handler;
			}
			void raise(ThostFtdcQuoteField^ pQuote, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryQuote_delegate)
					OnRspQryQuote_delegate(pQuote, pRspInfo, nRequestID, bIsLast);
			}
		}

		/// <summary>
		/// �����ѯת����ˮ��Ӧ
		/// </summary>
		event RspQryTransferSerial^ OnRspQryTransferSerial {
			void add(RspQryTransferSerial^ handler ) {
				OnRspQryTransferSerial_delegate += handler;
			}
			void remove(RspQryTransferSerial^ handler ) {
				OnRspQryTransferSerial_delegate -= handler;
			}
			void raise(ThostFtdcTransferSerialField^ pTransferSerial, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryTransferSerial_delegate)
					OnRspQryTransferSerial_delegate(pTransferSerial, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ����ǩԼ��ϵ��Ӧ
		/// </summary>
		event RspQryAccountregister^ OnRspQryAccountregister {
			void add(RspQryAccountregister^ handler ) {
				OnRspQryAccountregister_delegate += handler;
			}
			void remove(RspQryAccountregister^ handler ) {
				OnRspQryAccountregister_delegate -= handler;
			}
			void raise(ThostFtdcAccountregisterField^ pAccountregister, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryAccountregister_delegate)
					OnRspQryAccountregister_delegate(pAccountregister, pRspInfo, nRequestID, bIsLast);
			}
		}

		/// <summary>
		/// ����Ӧ��
		/// </summary>
		event RspError^ OnRspError {
			void add(RspError^ handler ) {
				OnRspError_delegate += handler;
			}
			void remove(RspError^ handler ) {
				OnRspError_delegate -= handler;
			}
			void raise(ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspError_delegate)
					OnRspError_delegate(pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// ����֪ͨ
		/// </summary>
		event RtnOrder^ OnRtnOrder {
			void add(RtnOrder^ handler ) {
				OnRtnOrder_delegate += handler;
			}
			void remove(RtnOrder^ handler ) {
				OnRtnOrder_delegate -= handler;
			}
			void raise(ThostFtdcOrderField^ pOrder) {
				if(OnRtnOrder_delegate)
					OnRtnOrder_delegate(pOrder);
			}
		}
		/// <summary>
		/// �ɽ�֪ͨ
		/// </summary>
		event RtnTrade^ OnRtnTrade {
			void add(RtnTrade^ handler ) {
				OnRtnTrade_delegate += handler;
			}
			void remove(RtnTrade^ handler ) {
				OnRtnTrade_delegate -= handler;
			}
			void raise(ThostFtdcTradeField^ pTrade) {
				if(OnRtnTrade_delegate)
					OnRtnTrade_delegate(pTrade);
			}
		}
		/// <summary>
		/// ����¼�����ر�
		/// </summary>
		event ErrRtnOrderInsert^ OnErrRtnOrderInsert {
			void add(ErrRtnOrderInsert^ handler ) {
				OnErrRtnOrderInsert_delegate += handler;
			}
			void remove(ErrRtnOrderInsert^ handler ) {
				OnErrRtnOrderInsert_delegate -= handler;
			}
			void raise(ThostFtdcInputOrderField^ pInputOrder, ThostFtdcRspInfoField^ pRspInfo) {
				if(OnErrRtnOrderInsert_delegate)
					OnErrRtnOrderInsert_delegate(pInputOrder, pRspInfo);
			}
		}
		/// <summary>
		/// ������������ر�
		/// </summary>
		event ErrRtnOrderAction^ OnErrRtnOrderAction {
			void add(ErrRtnOrderAction^ handler ) {
				OnErrRtnOrderAction_delegate += handler;
			}
			void remove(ErrRtnOrderAction^ handler ) {
				OnErrRtnOrderAction_delegate -= handler;
			}
			void raise(ThostFtdcOrderActionField^ pOrderAction, ThostFtdcRspInfoField^ pRspInfo) {
				if(OnErrRtnOrderAction_delegate)
					OnErrRtnOrderAction_delegate(pOrderAction, pRspInfo);
			}
		}
		/// <summary>
		/// ��Լ����״̬֪ͨ
		/// </summary>
		event RtnInstrumentStatus^ OnRtnInstrumentStatus {
			void add(RtnInstrumentStatus^ handler ) {
				OnRtnInstrumentStatus_delegate += handler;
			}
			void remove(RtnInstrumentStatus^ handler ) {
				OnRtnInstrumentStatus_delegate -= handler;
			}
			void raise(ThostFtdcInstrumentStatusField^ pInstrumentStatus) {
				if(OnRtnInstrumentStatus_delegate)
					OnRtnInstrumentStatus_delegate(pInstrumentStatus);
			}
		}
		/// <summary>
		/// ����֪ͨ
		/// </summary>
		event RtnTradingNotice^ OnRtnTradingNotice {
			void add(RtnTradingNotice^ handler ) {
				OnRtnTradingNotice_delegate += handler;
			}
			void remove(RtnTradingNotice^ handler ) {
				OnRtnTradingNotice_delegate -= handler;
			}
			void raise(ThostFtdcTradingNoticeInfoField^ pTradingNoticeInfo) {
				if(OnRtnTradingNotice_delegate)
					OnRtnTradingNotice_delegate(pTradingNoticeInfo);
			}
		}
		/// <summary>
		/// ��ʾ������У�����
		/// </summary>
		event RtnErrorConditionalOrder^ OnRtnErrorConditionalOrder {
			void add(RtnErrorConditionalOrder^ handler ) {
				OnRtnErrorConditionalOrder_delegate += handler;
			}
			void remove(RtnErrorConditionalOrder^ handler ) {
				OnRtnErrorConditionalOrder_delegate -= handler;
			}
			void raise(ThostFtdcErrorConditionalOrderField^ pErrorConditionalOrder) {
				if(OnRtnErrorConditionalOrder_delegate)
					OnRtnErrorConditionalOrder_delegate(pErrorConditionalOrder);
			}
		}

		/// <summary>
		/// ִ������֪ͨ
		/// </summary>
		event RtnExecOrder^ OnRtnExecOrder {
			void add(RtnExecOrder^ handler ) {
				OnRtnExecOrder_delegate += handler;
			}
			void remove(RtnExecOrder^ handler ) {
				OnRtnExecOrder_delegate -= handler;
			}
			void raise(ThostFtdcExecOrderField^ pExecOrder) {
				if(OnRtnExecOrder_delegate)
					OnRtnExecOrder_delegate(pExecOrder);
			}
		}
		/// <summary>
		/// ִ������¼�����ر�
		/// </summary>
		event ErrRtnExecOrderInsert^ OnErrRtnExecOrderInsert {
			void add(ErrRtnExecOrderInsert^ handler ) {
				OnErrRtnExecOrderInsert_delegate += handler;
			}
			void remove(ErrRtnExecOrderInsert^ handler ) {
				OnErrRtnExecOrderInsert_delegate -= handler;
			}
			void raise(ThostFtdcInputExecOrderField^ pInputExecOrder, ThostFtdcRspInfoField^ pRspInfo) {
				if(OnErrRtnExecOrderInsert_delegate)
					OnErrRtnExecOrderInsert_delegate(pInputExecOrder, pRspInfo);
			}
		}
		/// <summary>
		/// ִ�������������ر�
		/// </summary>
		event ErrRtnExecOrderAction^ OnErrRtnExecOrderAction {
			void add(ErrRtnExecOrderAction^ handler ) {
				OnErrRtnExecOrderAction_delegate += handler;
			}
			void remove(ErrRtnExecOrderAction^ handler ) {
				OnErrRtnExecOrderAction_delegate -= handler;
			}
			void raise(ThostFtdcExecOrderActionField^ pExecOrderAction, ThostFtdcRspInfoField^ pRspInfo) {
				if(OnErrRtnExecOrderAction_delegate)
					OnErrRtnExecOrderAction_delegate(pExecOrderAction, pRspInfo);
			}
		}

		/// <summary>
		/// ѯ��¼�����ر�
		/// </summary>
		event ErrRtnForQuoteInsert^ OnErrRtnForQuoteInsert {
			void add(ErrRtnForQuoteInsert^ handler ) {
				OnErrRtnForQuoteInsert_delegate += handler;
			}
			void remove(ErrRtnForQuoteInsert^ handler ) {
				OnErrRtnForQuoteInsert_delegate -= handler;
			}
			void raise(ThostFtdcInputForQuoteField^ pInputForQuote, ThostFtdcRspInfoField^ pRspInfo) {
				if(OnErrRtnForQuoteInsert_delegate)
					OnErrRtnForQuoteInsert_delegate(pInputForQuote, pRspInfo);
			}
		}
		/// <summary>
		/// ����֪ͨ
		/// </summary>
		event RtnQuote^ OnRtnQuote {
			void add(RtnQuote^ handler ) {
				OnRtnQuote_delegate += handler;
			}
			void remove(RtnQuote^ handler ) {
				OnRtnQuote_delegate -= handler;
			}
			void raise(ThostFtdcQuoteField^ pQuote) {
				if(OnRtnQuote_delegate)
					OnRtnQuote_delegate(pQuote);
			}
		}
		/// <summary>
		/// ����¼�����ر�
		/// </summary>
		event ErrRtnQuoteInsert^ OnErrRtnQuoteInsert {
			void add(ErrRtnQuoteInsert^ handler ) {
				OnErrRtnQuoteInsert_delegate += handler;
			}
			void remove(ErrRtnQuoteInsert^ handler ) {
				OnErrRtnQuoteInsert_delegate -= handler;
			}
			void raise(ThostFtdcInputQuoteField^ pInputQuote, ThostFtdcRspInfoField^ pRspInfo) {
				if(OnErrRtnQuoteInsert_delegate)
					OnErrRtnQuoteInsert_delegate(pInputQuote, pRspInfo);
			}
		}
		/// <summary>
		/// ���۲�������ر�
		/// </summary>
		event ErrRtnQuoteAction^ OnErrRtnQuoteAction {
			void add(ErrRtnQuoteAction^ handler ) {
				OnErrRtnQuoteAction_delegate += handler;
			}
			void remove(ErrRtnQuoteAction^ handler ) {
				OnErrRtnQuoteAction_delegate -= handler;
			}
			void raise(ThostFtdcQuoteActionField^ pQuoteAction, ThostFtdcRspInfoField^ pRspInfo) {
				if(OnErrRtnQuoteAction_delegate)
					OnErrRtnQuoteAction_delegate(pQuoteAction, pRspInfo);
			}
		}
		/// <summary>
		/// ѯ��֪ͨ
		/// </summary>
		event RtnForQuoteRsp^ OnRtnForQuoteRsp {
			void add(RtnForQuoteRsp^ handler ) {
				OnRtnForQuoteRsp_delegate += handler;
			}
			void remove(RtnForQuoteRsp^ handler ) {
				OnRtnForQuoteRsp_delegate -= handler;
			}
			void raise(ThostFtdcForQuoteRspField^ pForQuoteRsp) {
				if(OnRtnForQuoteRsp_delegate)
					OnRtnForQuoteRsp_delegate(pForQuoteRsp);
			}
		}

		/// <summary>
		/// �����ѯǩԼ������Ӧ
		/// </summary>
		event RspQryContractBank^ OnRspQryContractBank {
			void add(RspQryContractBank^ handler ) {
				OnRspQryContractBank_delegate += handler;
			}
			void remove(RspQryContractBank^ handler ) {
				OnRspQryContractBank_delegate -= handler;
			}
			void raise(ThostFtdcContractBankField^ pContractBank, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryContractBank_delegate)
					OnRspQryContractBank_delegate(pContractBank, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯԤ����Ӧ
		/// </summary>
		event RspQryParkedOrder^ OnRspQryParkedOrder {
			void add(RspQryParkedOrder^ handler ) {
				OnRspQryParkedOrder_delegate += handler;
			}
			void remove(RspQryParkedOrder^ handler ) {
				OnRspQryParkedOrder_delegate -= handler;
			}
			void raise(ThostFtdcParkedOrderField^ pParkedOrder, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryParkedOrder_delegate)
					OnRspQryParkedOrder_delegate(pParkedOrder, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯԤ�񳷵���Ӧ
		/// </summary>
		event RspQryParkedOrderAction^ OnRspQryParkedOrderAction {
			void add(RspQryParkedOrderAction^ handler ) {
				OnRspQryParkedOrderAction_delegate += handler;
			}
			void remove(RspQryParkedOrderAction^ handler ) {
				OnRspQryParkedOrderAction_delegate -= handler;
			}
			void raise(ThostFtdcParkedOrderActionField^ pParkedOrderAction, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryParkedOrderAction_delegate)
					OnRspQryParkedOrderAction_delegate(pParkedOrderAction, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ����֪ͨ��Ӧ
		/// </summary>
		event RspQryTradingNotice^ OnRspQryTradingNotice {
			void add(RspQryTradingNotice^ handler ) {
				OnRspQryTradingNotice_delegate += handler;
			}
			void remove(RspQryTradingNotice^ handler ) {
				OnRspQryTradingNotice_delegate -= handler;
			}
			void raise(ThostFtdcTradingNoticeField^ pTradingNotice, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryTradingNotice_delegate)
					OnRspQryTradingNotice_delegate(pTradingNotice, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ���͹�˾���ײ�����Ӧ
		/// </summary>
		event RspQryBrokerTradingParams^ OnRspQryBrokerTradingParams {
			void add(RspQryBrokerTradingParams^ handler ) {
				OnRspQryBrokerTradingParams_delegate += handler;
			}
			void remove(RspQryBrokerTradingParams^ handler ) {
				OnRspQryBrokerTradingParams_delegate -= handler;
			}
			void raise(ThostFtdcBrokerTradingParamsField^ pBrokerTradingParams, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryBrokerTradingParams_delegate)
					OnRspQryBrokerTradingParams_delegate(pBrokerTradingParams, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �����ѯ���͹�˾�����㷨��Ӧ
		/// </summary>
		event RspQryBrokerTradingAlgos^ OnRspQryBrokerTradingAlgos {
			void add(RspQryBrokerTradingAlgos^ handler ) {
				OnRspQryBrokerTradingAlgos_delegate += handler;
			}
			void remove(RspQryBrokerTradingAlgos^ handler ) {
				OnRspQryBrokerTradingAlgos_delegate -= handler;
			}
			void raise(ThostFtdcBrokerTradingAlgosField^ pBrokerTradingAlgos, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQryBrokerTradingAlgos_delegate)
					OnRspQryBrokerTradingAlgos_delegate(pBrokerTradingAlgos, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// ���з��������ʽ�ת�ڻ�֪ͨ
		/// </summary>
		event RtnFromBankToFutureByBank^ OnRtnFromBankToFutureByBank {
			void add(RtnFromBankToFutureByBank^ handler ) {
				OnRtnFromBankToFutureByBank_delegate += handler;
			}
			void remove(RtnFromBankToFutureByBank^ handler ) {
				OnRtnFromBankToFutureByBank_delegate -= handler;
			}
			void raise(ThostFtdcRspTransferField^ pRspTransfer) {
				if(OnRtnFromBankToFutureByBank_delegate)
					OnRtnFromBankToFutureByBank_delegate(pRspTransfer);
			}
		}
		/// <summary>
		/// ���з����ڻ��ʽ�ת����֪ͨ
		/// </summary>
		event RtnFromFutureToBankByBank^ OnRtnFromFutureToBankByBank {
			void add(RtnFromFutureToBankByBank^ handler ) {
				OnRtnFromFutureToBankByBank_delegate += handler;
			}
			void remove(RtnFromFutureToBankByBank^ handler ) {
				OnRtnFromFutureToBankByBank_delegate -= handler;
			}
			void raise(ThostFtdcRspTransferField^ pRspTransfer) {
				if(OnRtnFromFutureToBankByBank_delegate)
					OnRtnFromFutureToBankByBank_delegate(pRspTransfer);
			}
		}
		/// <summary>
		/// ���з����������ת�ڻ�֪ͨ
		/// </summary>
		event RtnRepealFromBankToFutureByBank^ OnRtnRepealFromBankToFutureByBank {
			void add(RtnRepealFromBankToFutureByBank^ handler ) {
				OnRtnRepealFromBankToFutureByBank_delegate += handler;
			}
			void remove(RtnRepealFromBankToFutureByBank^ handler ) {
				OnRtnRepealFromBankToFutureByBank_delegate -= handler;
			}
			void raise(ThostFtdcRspRepealField^ pRspRepeal) {
				if(OnRtnRepealFromBankToFutureByBank_delegate)
					OnRtnRepealFromBankToFutureByBank_delegate(pRspRepeal);
			}
		}
		/// <summary>
		/// ���з�������ڻ�ת����֪ͨ
		/// </summary>
		event RtnRepealFromFutureToBankByBank^ OnRtnRepealFromFutureToBankByBank {
			void add(RtnRepealFromFutureToBankByBank^ handler ) {
				OnRtnRepealFromFutureToBankByBank_delegate += handler;
			}
			void remove(RtnRepealFromFutureToBankByBank^ handler ) {
				OnRtnRepealFromFutureToBankByBank_delegate -= handler;
			}
			void raise(ThostFtdcRspRepealField^ pRspRepeal) {
				if(OnRtnRepealFromFutureToBankByBank_delegate)
					OnRtnRepealFromFutureToBankByBank_delegate(pRspRepeal);
			}
		}
		/// <summary>
		/// �ڻ����������ʽ�ת�ڻ�֪ͨ
		/// </summary>
		event RtnFromBankToFutureByFuture^ OnRtnFromBankToFutureByFuture {
			void add(RtnFromBankToFutureByFuture^ handler ) {
				OnRtnFromBankToFutureByFuture_delegate += handler;
			}
			void remove(RtnFromBankToFutureByFuture^ handler ) {
				OnRtnFromBankToFutureByFuture_delegate -= handler;
			}
			void raise(ThostFtdcRspTransferField^ pRspTransfer) {
				if(OnRtnFromBankToFutureByFuture_delegate)
					OnRtnFromBankToFutureByFuture_delegate(pRspTransfer);
			}
		}
		/// <summary>
		/// �ڻ������ڻ��ʽ�ת����֪ͨ
		/// </summary>
		event RtnFromFutureToBankByFuture^ OnRtnFromFutureToBankByFuture {
			void add(RtnFromFutureToBankByFuture^ handler ) {
				OnRtnFromFutureToBankByFuture_delegate += handler;
			}
			void remove(RtnFromFutureToBankByFuture^ handler ) {
				OnRtnFromFutureToBankByFuture_delegate -= handler;
			}
			void raise(ThostFtdcRspTransferField^ pRspTransfer) {
				if(OnRtnFromFutureToBankByFuture_delegate)
					OnRtnFromFutureToBankByFuture_delegate(pRspTransfer);
			}
		}
		/// <summary>
		/// ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
		/// </summary>
		event RtnRepealFromBankToFutureByFutureManual^ OnRtnRepealFromBankToFutureByFutureManual {
			void add(RtnRepealFromBankToFutureByFutureManual^ handler ) {
				OnRtnRepealFromBankToFutureByFutureManual_delegate += handler;
			}
			void remove(RtnRepealFromBankToFutureByFutureManual^ handler ) {
				OnRtnRepealFromBankToFutureByFutureManual_delegate -= handler;
			}
			void raise(ThostFtdcRspRepealField^ pRspRepeal) {
				if(OnRtnRepealFromBankToFutureByFutureManual_delegate)
					OnRtnRepealFromBankToFutureByFutureManual_delegate(pRspRepeal);
			}
		}
		/// <summary>
		/// ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
		/// </summary>
		event RtnRepealFromFutureToBankByFutureManual^ OnRtnRepealFromFutureToBankByFutureManual {
			void add(RtnRepealFromFutureToBankByFutureManual^ handler ) {
				OnRtnRepealFromFutureToBankByFutureManual_delegate += handler;
			}
			void remove(RtnRepealFromFutureToBankByFutureManual^ handler ) {
				OnRtnRepealFromFutureToBankByFutureManual_delegate -= handler;
			}
			void raise(ThostFtdcRspRepealField^ pRspRepeal) {
				if(OnRtnRepealFromFutureToBankByFutureManual_delegate)
					OnRtnRepealFromFutureToBankByFutureManual_delegate(pRspRepeal);
			}
		}
		/// <summary>
		/// �ڻ������ѯ�������֪ͨ
		/// </summary>
		event RtnQueryBankBalanceByFuture^ OnRtnQueryBankBalanceByFuture {
			void add(RtnQueryBankBalanceByFuture^ handler ) {
				OnRtnQueryBankBalanceByFuture_delegate += handler;
			}
			void remove(RtnQueryBankBalanceByFuture^ handler ) {
				OnRtnQueryBankBalanceByFuture_delegate -= handler;
			}
			void raise(ThostFtdcNotifyQueryAccountField^ pNotifyQueryAccount) {
				if(OnRtnQueryBankBalanceByFuture_delegate)
					OnRtnQueryBankBalanceByFuture_delegate(pNotifyQueryAccount);
			}
		}
		/// <summary>
		/// �ڻ����������ʽ�ת�ڻ�����ر�
		/// </summary>
		event ErrRtnBankToFutureByFuture^ OnErrRtnBankToFutureByFuture {
			void add(ErrRtnBankToFutureByFuture^ handler ) {
				OnErrRtnBankToFutureByFuture_delegate += handler;
			}
			void remove(ErrRtnBankToFutureByFuture^ handler ) {
				OnErrRtnBankToFutureByFuture_delegate -= handler;
			}
			void raise(ThostFtdcReqTransferField^ pReqTransfer, ThostFtdcRspInfoField^ pRspInfo) {
				if(OnErrRtnBankToFutureByFuture_delegate)
					OnErrRtnBankToFutureByFuture_delegate(pReqTransfer, pRspInfo);
			}
		}
		/// <summary>
		/// �ڻ������ڻ��ʽ�ת���д���ر�
		/// </summary>
		event ErrRtnFutureToBankByFuture^ OnErrRtnFutureToBankByFuture {
			void add(ErrRtnFutureToBankByFuture^ handler ) {
				OnErrRtnFutureToBankByFuture_delegate += handler;
			}
			void remove(ErrRtnFutureToBankByFuture^ handler ) {
				OnErrRtnFutureToBankByFuture_delegate -= handler;
			}
			void raise(ThostFtdcReqTransferField^ pReqTransfer, ThostFtdcRspInfoField^ pRspInfo) {
				if(OnErrRtnFutureToBankByFuture_delegate)
					OnErrRtnFutureToBankByFuture_delegate(pReqTransfer, pRspInfo);
			}
		}
		/// <summary>
		/// ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ�����ر�
		/// </summary>
		event ErrRtnRepealBankToFutureByFutureManual^ OnErrRtnRepealBankToFutureByFutureManual {
			void add(ErrRtnRepealBankToFutureByFutureManual^ handler ) {
				OnErrRtnRepealBankToFutureByFutureManual_delegate += handler;
			}
			void remove(ErrRtnRepealBankToFutureByFutureManual^ handler ) {
				OnErrRtnRepealBankToFutureByFutureManual_delegate -= handler;
			}
			void raise(ThostFtdcReqRepealField^ pReqRepeal, ThostFtdcRspInfoField^ pRspInfo) {
				if(OnErrRtnRepealBankToFutureByFutureManual_delegate)
					OnErrRtnRepealBankToFutureByFutureManual_delegate(pReqRepeal, pRspInfo);
			}
		}
		/// <summary>
		/// ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת���д���ر�
		/// </summary>
		event ErrRtnRepealFutureToBankByFutureManual^ OnErrRtnRepealFutureToBankByFutureManual {
			void add(ErrRtnRepealFutureToBankByFutureManual^ handler ) {
				OnErrRtnRepealFutureToBankByFutureManual_delegate += handler;
			}
			void remove(ErrRtnRepealFutureToBankByFutureManual^ handler ) {
				OnErrRtnRepealFutureToBankByFutureManual_delegate -= handler;
			}
			void raise(ThostFtdcReqRepealField^ pReqRepeal, ThostFtdcRspInfoField^ pRspInfo) {
				if(OnErrRtnRepealFutureToBankByFutureManual_delegate)
					OnErrRtnRepealFutureToBankByFutureManual_delegate(pReqRepeal, pRspInfo);
			}
		}
		/// <summary>
		/// �ڻ������ѯ����������ر�
		/// </summary>
		event ErrRtnQueryBankBalanceByFuture^ OnErrRtnQueryBankBalanceByFuture {
			void add(ErrRtnQueryBankBalanceByFuture^ handler ) {
				OnErrRtnQueryBankBalanceByFuture_delegate += handler;
			}
			void remove(ErrRtnQueryBankBalanceByFuture^ handler ) {
				OnErrRtnQueryBankBalanceByFuture_delegate -= handler;
			}
			void raise(ThostFtdcReqQueryAccountField^ pReqQueryAccount, ThostFtdcRspInfoField^ pRspInfo) {
				if(OnErrRtnQueryBankBalanceByFuture_delegate)
					OnErrRtnQueryBankBalanceByFuture_delegate(pReqQueryAccount, pRspInfo);
			}
		}
		/// <summary>
		/// �ڻ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
		/// </summary>
		event RtnRepealFromBankToFutureByFuture^ OnRtnRepealFromBankToFutureByFuture {
			void add(RtnRepealFromBankToFutureByFuture^ handler ) {
				OnRtnRepealFromBankToFutureByFuture_delegate += handler;
			}
			void remove(RtnRepealFromBankToFutureByFuture^ handler ) {
				OnRtnRepealFromBankToFutureByFuture_delegate -= handler;
			}
			void raise(ThostFtdcRspRepealField^ pRspRepeal) {
				if(OnRtnRepealFromBankToFutureByFuture_delegate)
					OnRtnRepealFromBankToFutureByFuture_delegate(pRspRepeal);
			}
		}
		/// <summary>
		/// �ڻ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
		/// </summary>
		event RtnRepealFromFutureToBankByFuture^ OnRtnRepealFromFutureToBankByFuture {
			void add(RtnRepealFromFutureToBankByFuture^ handler ) {
				OnRtnRepealFromFutureToBankByFuture_delegate += handler;
			}
			void remove(RtnRepealFromFutureToBankByFuture^ handler ) {
				OnRtnRepealFromFutureToBankByFuture_delegate -= handler;
			}
			void raise(ThostFtdcRspRepealField^ pRspRepeal) {
				if(OnRtnRepealFromFutureToBankByFuture_delegate)
					OnRtnRepealFromFutureToBankByFuture_delegate(pRspRepeal);
			}
		}
		/// <summary>
		/// �ڻ����������ʽ�ת�ڻ�Ӧ��
		/// </summary>
		event RspFromBankToFutureByFuture^ OnRspFromBankToFutureByFuture {
			void add(RspFromBankToFutureByFuture^ handler ) {
				OnRspFromBankToFutureByFuture_delegate += handler;
			}
			void remove(RspFromBankToFutureByFuture^ handler ) {
				OnRspFromBankToFutureByFuture_delegate -= handler;
			}
			void raise(ThostFtdcReqTransferField^ pReqTransfer, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspFromBankToFutureByFuture_delegate)
					OnRspFromBankToFutureByFuture_delegate(pReqTransfer, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �ڻ������ڻ��ʽ�ת����Ӧ��
		/// </summary>
		event RspFromFutureToBankByFuture^ OnRspFromFutureToBankByFuture {
			void add(RspFromFutureToBankByFuture^ handler ) {
				OnRspFromFutureToBankByFuture_delegate += handler;
			}
			void remove(RspFromFutureToBankByFuture^ handler ) {
				OnRspFromFutureToBankByFuture_delegate -= handler;
			}
			void raise(ThostFtdcReqTransferField^ pReqTransfer, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspFromFutureToBankByFuture_delegate)
					OnRspFromFutureToBankByFuture_delegate(pReqTransfer, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// �ڻ������ѯ�������Ӧ��
		/// </summary>
		event RspQueryBankAccountMoneyByFuture^ OnRspQueryBankAccountMoneyByFuture {
			void add(RspQueryBankAccountMoneyByFuture^ handler ) {
				OnRspQueryBankAccountMoneyByFuture_delegate += handler;
			}
			void remove(RspQueryBankAccountMoneyByFuture^ handler ) {
				OnRspQueryBankAccountMoneyByFuture_delegate -= handler;
			}
			void raise(ThostFtdcReqQueryAccountField^ pReqQueryAccount, ThostFtdcRspInfoField^ pRspInfo, int nRequestID, bool bIsLast) {
				if(OnRspQueryBankAccountMoneyByFuture_delegate)
					OnRspQueryBankAccountMoneyByFuture_delegate(pReqQueryAccount, pRspInfo, nRequestID, bIsLast);
			}
		}
		/// <summary>
		/// ���з������ڿ���֪ͨ
		/// </summary>
		event RtnOpenAccountByBank^ OnRtnOpenAccountByBank {
			void add(RtnOpenAccountByBank^ handler ) {
				OnRtnOpenAccountByBank_delegate += handler;
			}
			void remove(RtnOpenAccountByBank^ handler ) {
				OnRtnOpenAccountByBank_delegate -= handler;
			}
			void raise(ThostFtdcOpenAccountField^ pOpenAccount) {
				if(OnRtnOpenAccountByBank_delegate)
					OnRtnOpenAccountByBank_delegate(pOpenAccount);
			}
		}
		/// <summary>
		/// ���з�����������֪ͨ
		/// </summary>
		event RtnCancelAccountByBank^ OnRtnCancelAccountByBank {
			void add(RtnCancelAccountByBank^ handler ) {
				OnRtnCancelAccountByBank_delegate += handler;
			}
			void remove(RtnCancelAccountByBank^ handler ) {
				OnRtnCancelAccountByBank_delegate -= handler;
			}
			void raise(ThostFtdcCancelAccountField^ pCancelAccount) {
				if(OnRtnCancelAccountByBank_delegate)
					OnRtnCancelAccountByBank_delegate(pCancelAccount);
			}
		}
		/// <summary>
		/// ���з����������˺�֪ͨ
		/// </summary>
		event RtnChangeAccountByBank^ OnRtnChangeAccountByBank {
			void add(RtnChangeAccountByBank^ handler ) {
				OnRtnChangeAccountByBank_delegate += handler;
			}
			void remove(RtnChangeAccountByBank^ handler ) {
				OnRtnChangeAccountByBank_delegate -= handler;
			}
			void raise(ThostFtdcChangeAccountField^ pChangeAccount) {
				if(OnRtnChangeAccountByBank_delegate)
					OnRtnChangeAccountByBank_delegate(pChangeAccount);
			}
		}
		
		// delegates
	private:
		FrontConnected^ OnFrontConnected_delegate;
		FrontDisconnected^ OnFrontDisconnected_delegate;
		HeartBeatWarning^ OnHeartBeatWarning_delegate;
		RspAuthenticate^ OnRspAuthenticate_delegate;
		RspUserLogin^ OnRspUserLogin_delegate;
		RspUserLogout^ OnRspUserLogout_delegate;
		RspUserPasswordUpdate^ OnRspUserPasswordUpdate_delegate;
		RspTradingAccountPasswordUpdate^ OnRspTradingAccountPasswordUpdate_delegate;
		RspOrderInsert^ OnRspOrderInsert_delegate;
		RspParkedOrderInsert^ OnRspParkedOrderInsert_delegate;
		RspParkedOrderAction^ OnRspParkedOrderAction_delegate;
		RspOrderAction^ OnRspOrderAction_delegate;
		RspQueryMaxOrderVolume^ OnRspQueryMaxOrderVolume_delegate;
		RspSettlementInfoConfirm^ OnRspSettlementInfoConfirm_delegate;
		RspRemoveParkedOrder^ OnRspRemoveParkedOrder_delegate;
		RspRemoveParkedOrderAction^ OnRspRemoveParkedOrderAction_delegate;
		RspExecOrderInsert^ OnRspExecOrderInsert_delegate;
		RspExecOrderAction^ OnRspExecOrderAction_delegate;
		RspForQuoteInsert^ OnRspForQuoteInsert_delegate;
		RspQuoteInsert^ OnRspQuoteInsert_delegate;
		RspQuoteAction^ OnRspQuoteAction_delegate;
		RspQryOrder^ OnRspQryOrder_delegate;
		RspQryTrade^ OnRspQryTrade_delegate;
		RspQryInvestorPosition^ OnRspQryInvestorPosition_delegate;
		RspQryTradingAccount^ OnRspQryTradingAccount_delegate;
		RspQryInvestor^ OnRspQryInvestor_delegate;
		RspQryTradingCode^ OnRspQryTradingCode_delegate;
		RspQryInstrumentMarginRate^ OnRspQryInstrumentMarginRate_delegate;
		RspQryInstrumentCommissionRate^ OnRspQryInstrumentCommissionRate_delegate;
		RspQryExchange^ OnRspQryExchange_delegate;
		RspQryProduct^ OnRspQryProduct_delegate;
		RspQryInstrument^ OnRspQryInstrument_delegate;
		RspQryDepthMarketData^ OnRspQryDepthMarketData_delegate;
		RspQrySettlementInfo^ OnRspQrySettlementInfo_delegate;
		RspQryTransferBank^ OnRspQryTransferBank_delegate;
		RspQryInvestorPositionDetail^ OnRspQryInvestorPositionDetail_delegate;
		RspQryNotice^ OnRspQryNotice_delegate;
		RspQrySettlementInfoConfirm^ OnRspQrySettlementInfoConfirm_delegate;
		RspQryInvestorPositionCombineDetail^ OnRspQryInvestorPositionCombineDetail_delegate;
		RspQryCFMMCTradingAccountKey^ OnRspQryCFMMCTradingAccountKey_delegate;
		RspQryEWarrantOffset^ OnRspQryEWarrantOffset_delegate;
		RspQryInvestorProductGroupMargin^ OnRspQryInvestorProductGroupMargin_delegate;
		RspQryExchangeMarginRate^ OnRspQryExchangeMarginRate_delegate;
		RspQryExchangeMarginRateAdjust^ OnRspQryExchangeMarginRateAdjust_delegate;
		RspQryExchangeRate^ OnRspQryExchangeRate_delegate;
		RspQrySecAgentACIDMap^ OnRspQrySecAgentACIDMap_delegate;
		RspQryOptionInstrTradeCost^ OnRspQryOptionInstrTradeCost_delegate;
		RspQryOptionInstrCommRate^ OnRspQryOptionInstrCommRate_delegate;
		RspQryExecOrder^ OnRspQryExecOrder_delegate;
		RspQryForQuote^ OnRspQryForQuote_delegate;
		RspQryQuote^ OnRspQryQuote_delegate;
		RspQryTransferSerial^ OnRspQryTransferSerial_delegate;
		RspQryAccountregister^ OnRspQryAccountregister_delegate;
		RspError^ OnRspError_delegate;
		RtnOrder^ OnRtnOrder_delegate;
		RtnTrade^ OnRtnTrade_delegate;
		ErrRtnOrderInsert^ OnErrRtnOrderInsert_delegate;
		ErrRtnOrderAction^ OnErrRtnOrderAction_delegate;
		RtnInstrumentStatus^ OnRtnInstrumentStatus_delegate;
		RtnTradingNotice^ OnRtnTradingNotice_delegate;
		RtnErrorConditionalOrder^ OnRtnErrorConditionalOrder_delegate;
		RtnExecOrder^ OnRtnExecOrder_delegate;
		ErrRtnExecOrderInsert^ OnErrRtnExecOrderInsert_delegate;
		ErrRtnExecOrderAction^ OnErrRtnExecOrderAction_delegate;
		ErrRtnForQuoteInsert^ OnErrRtnForQuoteInsert_delegate;
		RtnQuote^ OnRtnQuote_delegate;
		ErrRtnQuoteInsert^ OnErrRtnQuoteInsert_delegate;
		ErrRtnQuoteAction^ OnErrRtnQuoteAction_delegate;
		RtnForQuoteRsp^ OnRtnForQuoteRsp_delegate;
		RspQryContractBank^ OnRspQryContractBank_delegate;
		RspQryParkedOrder^ OnRspQryParkedOrder_delegate;
		RspQryParkedOrderAction^ OnRspQryParkedOrderAction_delegate;
		RspQryTradingNotice^ OnRspQryTradingNotice_delegate;
		RspQryBrokerTradingParams^ OnRspQryBrokerTradingParams_delegate;
		RspQryBrokerTradingAlgos^ OnRspQryBrokerTradingAlgos_delegate;
		RtnFromBankToFutureByBank^ OnRtnFromBankToFutureByBank_delegate;
		RtnFromFutureToBankByBank^ OnRtnFromFutureToBankByBank_delegate;
		RtnRepealFromBankToFutureByBank^ OnRtnRepealFromBankToFutureByBank_delegate;
		RtnRepealFromFutureToBankByBank^ OnRtnRepealFromFutureToBankByBank_delegate;
		RtnFromBankToFutureByFuture^ OnRtnFromBankToFutureByFuture_delegate;
		RtnFromFutureToBankByFuture^ OnRtnFromFutureToBankByFuture_delegate;
		RtnRepealFromBankToFutureByFutureManual^ OnRtnRepealFromBankToFutureByFutureManual_delegate;
		RtnRepealFromFutureToBankByFutureManual^ OnRtnRepealFromFutureToBankByFutureManual_delegate;
		RtnQueryBankBalanceByFuture^ OnRtnQueryBankBalanceByFuture_delegate;
		ErrRtnBankToFutureByFuture^ OnErrRtnBankToFutureByFuture_delegate;
		ErrRtnFutureToBankByFuture^ OnErrRtnFutureToBankByFuture_delegate;
		ErrRtnRepealBankToFutureByFutureManual^ OnErrRtnRepealBankToFutureByFutureManual_delegate;
		ErrRtnRepealFutureToBankByFutureManual^ OnErrRtnRepealFutureToBankByFutureManual_delegate;
		ErrRtnQueryBankBalanceByFuture^ OnErrRtnQueryBankBalanceByFuture_delegate;
		RtnRepealFromBankToFutureByFuture^ OnRtnRepealFromBankToFutureByFuture_delegate;
		RtnRepealFromFutureToBankByFuture^ OnRtnRepealFromFutureToBankByFuture_delegate;
		RspFromBankToFutureByFuture^ OnRspFromBankToFutureByFuture_delegate;
		RspFromFutureToBankByFuture^ OnRspFromFutureToBankByFuture_delegate;
		RspQueryBankAccountMoneyByFuture^ OnRspQueryBankAccountMoneyByFuture_delegate;
		RtnOpenAccountByBank^ OnRtnOpenAccountByBank_delegate;
		RtnCancelAccountByBank^ OnRtnCancelAccountByBank_delegate;
		RtnChangeAccountByBank^ OnRtnChangeAccountByBank_delegate;
#ifdef __CTP_MA__
		// callbacks for MA
	private:
		void cbk_OnFrontConnected();
		void cbk_OnFrontDisconnected(int nReason);
		void cbk_OnHeartBeatWarning(int nTimeLapse);
		void cbk_OnRspAuthenticate(CThostFtdcRspAuthenticateField *pRspAuthenticateField, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspUserLogin(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspUserLogout(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspUserPasswordUpdate(CThostFtdcUserPasswordUpdateField *pUserPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspTradingAccountPasswordUpdate(CThostFtdcTradingAccountPasswordUpdateField *pTradingAccountPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspParkedOrderInsert(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspOrderAction(CThostFtdcInputOrderActionField *pInputOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQueryMaxOrderVolume(CThostFtdcQueryMaxOrderVolumeField *pQueryMaxOrderVolume, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspSettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspRemoveParkedOrder(CThostFtdcRemoveParkedOrderField *pRemoveParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspRemoveParkedOrderAction(CThostFtdcRemoveParkedOrderActionField *pRemoveParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspExecOrderAction(CThostFtdcInputExecOrderActionField *pInputExecOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQuoteInsert(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQuoteAction(CThostFtdcInputQuoteActionField *pInputQuoteAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryOrder(CThostFtdcOrderField *pOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryTrade(CThostFtdcTradeField *pTrade, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryInvestorPosition(CThostFtdcInvestorPositionField *pInvestorPosition, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryTradingAccount(CThostFtdcTradingAccountField *pTradingAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryInvestor(CThostFtdcInvestorField *pInvestor, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryTradingCode(CThostFtdcTradingCodeField *pTradingCode, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryInstrumentMarginRate(CThostFtdcInstrumentMarginRateField *pInstrumentMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryInstrumentCommissionRate(CThostFtdcInstrumentCommissionRateField *pInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryExchange(CThostFtdcExchangeField *pExchange, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryProduct(CThostFtdcProductField *pProduct, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryInstrument(CThostFtdcInstrumentField *pInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQrySettlementInfo(CThostFtdcSettlementInfoField *pSettlementInfo, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryTransferBank(CThostFtdcTransferBankField *pTransferBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryInvestorPositionDetail(CThostFtdcInvestorPositionDetailField *pInvestorPositionDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryNotice(CThostFtdcNoticeField *pNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQrySettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryInvestorPositionCombineDetail(CThostFtdcInvestorPositionCombineDetailField *pInvestorPositionCombineDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryCFMMCTradingAccountKey(CThostFtdcCFMMCTradingAccountKeyField *pCFMMCTradingAccountKey, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryEWarrantOffset(CThostFtdcEWarrantOffsetField *pEWarrantOffset, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryInvestorProductGroupMargin(CThostFtdcInvestorProductGroupMarginField *pInvestorProductGroupMargin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryExchangeMarginRate(CThostFtdcExchangeMarginRateField *pExchangeMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryExchangeMarginRateAdjust(CThostFtdcExchangeMarginRateAdjustField *pExchangeMarginRateAdjust, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryExchangeRate(CThostFtdcExchangeRateField *pExchangeRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQrySecAgentACIDMap(CThostFtdcSecAgentACIDMapField *pSecAgentACIDMap, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryOptionInstrTradeCost(CThostFtdcOptionInstrTradeCostField *pOptionInstrTradeCost, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryOptionInstrCommRate(CThostFtdcOptionInstrCommRateField *pOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryExecOrder(CThostFtdcExecOrderField *pExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryForQuote(CThostFtdcForQuoteField *pForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryQuote(CThostFtdcQuoteField *pQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryTransferSerial(CThostFtdcTransferSerialField *pTransferSerial, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryAccountregister(CThostFtdcAccountregisterField *pAccountregister, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspError(CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRtnOrder(CThostFtdcOrderField *pOrder);
		void cbk_OnRtnTrade(CThostFtdcTradeField *pTrade);
		void cbk_OnErrRtnOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo);
		void cbk_OnErrRtnOrderAction(CThostFtdcOrderActionField *pOrderAction, CThostFtdcRspInfoField *pRspInfo);
		void cbk_OnRtnInstrumentStatus(CThostFtdcInstrumentStatusField *pInstrumentStatus);
		void cbk_OnRtnTradingNotice(CThostFtdcTradingNoticeInfoField *pTradingNoticeInfo);
		void cbk_OnRtnErrorConditionalOrder(CThostFtdcErrorConditionalOrderField *pErrorConditionalOrder);
		void cbk_OnRtnExecOrder(CThostFtdcExecOrderField *pExecOrder);
		void cbk_OnErrRtnExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo);
		void cbk_OnErrRtnExecOrderAction(CThostFtdcExecOrderActionField *pExecOrderAction, CThostFtdcRspInfoField *pRspInfo);
		void cbk_OnErrRtnForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo);
		void cbk_OnRtnQuote(CThostFtdcQuoteField *pQuote);
		void cbk_OnErrRtnQuoteInsert(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo);
		void cbk_OnErrRtnQuoteAction(CThostFtdcQuoteActionField *pQuoteAction, CThostFtdcRspInfoField *pRspInfo);
		void cbk_OnRtnForQuoteRsp(CThostFtdcForQuoteRspField *pForQuoteRsp);
		void cbk_OnRspQryContractBank(CThostFtdcContractBankField *pContractBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryParkedOrder(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryTradingNotice(CThostFtdcTradingNoticeField *pTradingNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryBrokerTradingParams(CThostFtdcBrokerTradingParamsField *pBrokerTradingParams, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQryBrokerTradingAlgos(CThostFtdcBrokerTradingAlgosField *pBrokerTradingAlgos, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRtnFromBankToFutureByBank(CThostFtdcRspTransferField *pRspTransfer);
		void cbk_OnRtnFromFutureToBankByBank(CThostFtdcRspTransferField *pRspTransfer);
		void cbk_OnRtnRepealFromBankToFutureByBank(CThostFtdcRspRepealField *pRspRepeal);
		void cbk_OnRtnRepealFromFutureToBankByBank(CThostFtdcRspRepealField *pRspRepeal);
		void cbk_OnRtnFromBankToFutureByFuture(CThostFtdcRspTransferField *pRspTransfer);
		void cbk_OnRtnFromFutureToBankByFuture(CThostFtdcRspTransferField *pRspTransfer);
		void cbk_OnRtnRepealFromBankToFutureByFutureManual(CThostFtdcRspRepealField *pRspRepeal);
		void cbk_OnRtnRepealFromFutureToBankByFutureManual(CThostFtdcRspRepealField *pRspRepeal);
		void cbk_OnRtnQueryBankBalanceByFuture(CThostFtdcNotifyQueryAccountField *pNotifyQueryAccount);
		void cbk_OnErrRtnBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo);
		void cbk_OnErrRtnFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo);
		void cbk_OnErrRtnRepealBankToFutureByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo);
		void cbk_OnErrRtnRepealFutureToBankByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo);
		void cbk_OnErrRtnQueryBankBalanceByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo);
		void cbk_OnRtnRepealFromBankToFutureByFuture(CThostFtdcRspRepealField *pRspRepeal);
		void cbk_OnRtnRepealFromFutureToBankByFuture(CThostFtdcRspRepealField *pRspRepeal);
		void cbk_OnRspFromBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspFromFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRspQueryBankAccountMoneyByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
		void cbk_OnRtnOpenAccountByBank(CThostFtdcOpenAccountField *pOpenAccount);
		void cbk_OnRtnCancelAccountByBank(CThostFtdcCancelAccountField *pCancelAccount);
		void cbk_OnRtnChangeAccountByBank(CThostFtdcChangeAccountField *pChangeAccount);
		// �����лص�������ַ���ݸ�SPI
		void RegisterCallbacks();
#endif

	};
};