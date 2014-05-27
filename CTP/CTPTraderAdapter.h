/////////////////////////////////////////////////////////////////////////
/// 上期技术 CTP C++ ==> .Net Framework Adapter
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
	/// 托管类,TraderAPI Adapter
	/// </summary>
	public ref class CTPTraderAdapter
	{
	public:
		/// <summary>
		///创建CTPTraderAdapter
		///存贮订阅信息文件的目录，默认为当前目录
		/// </summary>
		CTPTraderAdapter(void);
		/// <summary>
		///创建CTPTraderAdapter
		/// </summary>
		/// <param name="pszFlowPath">存贮订阅信息文件的目录，默认为当前目录</param>
		CTPTraderAdapter(String^ pszFlowPath);
	private:
		~CTPTraderAdapter(void);
		CThostFtdcTraderApi* m_pApi;
		CTraderSpi* m_pSpi;
	public:
		/// <summary>
		///删除接口对象本身
		///@remark 不再使用本接口对象时,调用该函数删除接口对象
		/// </summary>
		void Release();

		/// <summary>
		///初始化
		///@remark 初始化运行环境,只有调用后,接口才开始工作
		/// </summary>
		void Init();

		/// <summary>
		///等待接口线程结束运行
		///@return 线程退出代码
		/// </summary>
		int Join();

		/// <summary>
		///获取当前交易日
		///@remark 只有登录成功后,才能得到正确的交易日
		/// </summary>
		/// <returns>获取到的交易日</returns>
		String^ GetTradingDay();

		/// <summary>
		///注册前置机网络地址
		///@param pszFrontAddress：前置机网络地址。
		///@remark 网络地址的格式为：“protocol://ipaddress:port”，如：”tcp://127.0.0.1:17001”。 
		///@remark “tcp”代表传输协议，“127.0.0.1”代表服务器地址。”17001”代表服务器端口号。
		/// </summary>
		void RegisterFront(String^ pszFrontAddress);

		/// <summary>
		///注册名字服务器网络地址
		///@param pszNsAddress：名字服务器网络地址。
		///@remark 网络地址的格式为：“protocol://ipaddress:port”，如：”tcp://127.0.0.1:12001”。 
		///@remark “tcp”代表传输协议，“127.0.0.1”代表服务器地址。”12001”代表服务器端口号。
		///@remark RegisterNameServer优先于RegisterFront
		/// </summary>
		void RegisterNameServer(String^ pszNsAddress);

		
		///注册名字服务器用户信息
		///@param pFensUserInfo：用户信息。
		void RegisterFensUserInfo(ThostFtdcFensUserInfoField^ pFensUserInfo);

		///注册回调接口
		///@param pSpi 派生自回调接口类的实例
		/// void RegisterSpi(ThostFtdcTraderSpi^ pSpi);

		/// <summary>
		///订阅私有流。
		///@param nResumeType 私有流重传方式  
		///        THOST_TERT_RESTART:从本交易日开始重传
		///        THOST_TERT_RESUME:从上次收到的续传
		///        THOST_TERT_QUICK:只传送登录后私有流的内容
		///@remark 该方法要在Init方法前调用。若不调用则不会收到私有流的数据。
		/// </summary>
		void SubscribePrivateTopic(EnumTeResumeType nResumeType);

		/// <summary>
		///订阅公共流。
		///@param nResumeType 公共流重传方式  
		///        THOST_TERT_RESTART:从本交易日开始重传
		///        THOST_TERT_RESUME:从上次收到的续传
		///        THOST_TERT_QUICK:只传送登录后公共流的内容
		///@remark 该方法要在Init方法前调用。若不调用则不会收到公共流的数据。
		/// </summary>
		void SubscribePublicTopic(EnumTeResumeType nResumeType);

		/// <summary>
		/// 客户端认证请求
		/// </summary>
		int ReqAuthenticate(ThostFtdcReqAuthenticateField^ pReqAuthenticateField, int nRequestID);

		/// <summary>
		///用户登录请求
		/// </summary>
		int ReqUserLogin(ThostFtdcReqUserLoginField^ pReqUserLoginField, int nRequestID);

		/// <summary>
		///登出请求
		/// </summary>
		int ReqUserLogout(ThostFtdcUserLogoutField^ pUserLogout, int nRequestID);

		/// <summary>
		///用户口令更新请求
		/// </summary>
		int ReqUserPasswordUpdate(ThostFtdcUserPasswordUpdateField^ pUserPasswordUpdate, int nRequestID);

		/// <summary>
		///资金账户口令更新请求
		/// </summary>
		int ReqTradingAccountPasswordUpdate(ThostFtdcTradingAccountPasswordUpdateField^ pTradingAccountPasswordUpdate, int nRequestID);

		/// <summary>
		///报单录入请求
		/// </summary>
		int ReqOrderInsert(ThostFtdcInputOrderField^ pInputOrder, int nRequestID);

		/// <summary>
		///预埋单录入请求
		/// </summary>
		int ReqParkedOrderInsert(ThostFtdcParkedOrderField^ pParkedOrder, int nRequestID);

		/// <summary>
		///预埋撤单录入请求
		/// </summary>
		int ReqParkedOrderAction(ThostFtdcParkedOrderActionField^ pParkedOrderAction, int nRequestID);

		/// <summary>
		/// 报单操作请求
		/// </summary>
		int ReqOrderAction(ThostFtdcInputOrderActionField^ pInputOrderAction, int nRequestID);

		/// <summary>
		///查询最大报单数量请求
		/// </summary>
		int ReqQueryMaxOrderVolume(ThostFtdcQueryMaxOrderVolumeField^ pQueryMaxOrderVolume, int nRequestID);

		/// <summary>
		/// 投资者结算结果确认
		/// </summary>
		int ReqSettlementInfoConfirm(ThostFtdcSettlementInfoConfirmField^ pSettlementInfoConfirm, int nRequestID);

		/// <summary>
		/// 请求删除预埋单
		/// </summary>
		int ReqRemoveParkedOrder(ThostFtdcRemoveParkedOrderField^ pRemoveParkedOrder, int nRequestID);

		/// <summary>
		/// 请求删除预埋撤单
		/// </summary>
		int ReqRemoveParkedOrderAction(ThostFtdcRemoveParkedOrderActionField^ pRemoveParkedOrderAction, int nRequestID);


		/// <summary>
		/// 执行宣告录入请求
		/// </summary>
		int ReqExecOrderInsert(ThostFtdcInputExecOrderField^ pInputExecOrder, int nRequestID);

		/// <summary>
		/// 执行宣告操作请求
		/// </summary>
		int ReqExecOrderAction(ThostFtdcInputExecOrderActionField^ pInputExecOrderAction, int nRequestID);


		/// <summary>
		/// 询价录入请求
		/// </summary>
		int ReqForQuoteInsert(ThostFtdcInputForQuoteField^ pInputForQuote, int nRequestID);

		/// <summary>
		/// 报价录入请求
		/// </summary>
		int ReqQuoteInsert(ThostFtdcInputQuoteField^ pInputQuote, int nRequestID);

		/// <summary>
		/// 报价操作请求
		/// </summary>
		int ReqQuoteAction(ThostFtdcInputQuoteActionField^ pInputQuoteAction, int nRequestID);


		/// <summary>
		///请求查询报单
		/// </summary>
		int ReqQryOrder(ThostFtdcQryOrderField^ pQryOrder, int nRequestID);

		/// <summary>
		///请求查询成交
		/// </summary>
		int ReqQryTrade(ThostFtdcQryTradeField^ pQryTrade, int nRequestID);

		/// <summary>
		///请求查询投资者持仓
		/// </summary>
		int ReqQryInvestorPosition(ThostFtdcQryInvestorPositionField^ pQryInvestorPosition, int nRequestID);

		/// <summary>
		///请求查询资金账户
		/// </summary>
		int ReqQryTradingAccount(ThostFtdcQryTradingAccountField^ pQryTradingAccount, int nRequestID);

		/// <summary>
		///请求查询投资者
		/// </summary>
		int ReqQryInvestor(ThostFtdcQryInvestorField^ pQryInvestor, int nRequestID);

		/// <summary>
		///请求查询交易编码
		/// </summary>
		int ReqQryTradingCode(ThostFtdcQryTradingCodeField^ pQryTradingCode, int nRequestID);

		/// <summary>
		///请求查询合约保证金率
		/// </summary>
		int ReqQryInstrumentMarginRate(ThostFtdcQryInstrumentMarginRateField^ pQryInstrumentMarginRate, int nRequestID);

		/// <summary>
		///请求查询合约手续费率
		/// </summary>
		int ReqQryInstrumentCommissionRate(ThostFtdcQryInstrumentCommissionRateField^ pQryInstrumentCommissionRate, int nRequestID);

		/// <summary>
		///请求查询交易所
		/// </summary>
		int ReqQryExchange(ThostFtdcQryExchangeField^ pQryExchange, int nRequestID);

		/// <summary>
		///请求查询产品
		/// </summary>
		int ReqQryProduct(ThostFtdcQryProductField^ pQryProduct, int nRequestID);

		/// <summary>
		///请求查询合约
		/// </summary>
		int ReqQryInstrument(ThostFtdcQryInstrumentField^ pQryInstrument, int nRequestID);

		/// <summary>
		///请求查询行情
		/// </summary>
		int ReqQryDepthMarketData(ThostFtdcQryDepthMarketDataField^ pQryDepthMarketData, int nRequestID);

		/// <summary>
		///请求查询投资者结算结果
		/// </summary>
		int ReqQrySettlementInfo(ThostFtdcQrySettlementInfoField^ pQrySettlementInfo, int nRequestID);

		/// <summary>
		///请求查询转帐银行
		/// </summary>
		int ReqQryTransferBank(ThostFtdcQryTransferBankField^ pQryTransferBank, int nRequestID);

		/// <summary>
		///请求查询投资者持仓明细
		/// </summary>
		int ReqQryInvestorPositionDetail(ThostFtdcQryInvestorPositionDetailField^ pQryInvestorPositionDetail, int nRequestID);

		/// <summary>
		///请求查询客户通知
		/// </summary>
		int ReqQryNotice(ThostFtdcQryNoticeField^ pQryNotice, int nRequestID);

		/// <summary>
		///请求查询结算信息确认
		/// </summary>
		int ReqQrySettlementInfoConfirm(ThostFtdcQrySettlementInfoConfirmField^ pQrySettlementInfoConfirm, int nRequestID);

		/// <summary>
		///请求查询投资者持仓明细
		/// </summary>
		int ReqQryInvestorPositionCombineDetail(ThostFtdcQryInvestorPositionCombineDetailField^ pQryInvestorPositionCombineDetail, int nRequestID);

		/// <summary>
		///请求查询保证金监管系统经纪公司资金账户密钥
		/// </summary>
		int ReqQryCFMMCTradingAccountKey(ThostFtdcQryCFMMCTradingAccountKeyField^ pQryCFMMCTradingAccountKey, int nRequestID);

		/// <summary>
		///请求查询仓单折抵信息
		/// </summary>
		int ReqQryEWarrantOffset(ThostFtdcQryEWarrantOffsetField^ pQryEWarrantOffset, int nRequestID);


		/// <summary>
		///请求查询投资者品种/跨品种保证金
		/// </summary>
		int ReqQryInvestorProductGroupMargin(ThostFtdcQryInvestorProductGroupMarginField^ pQryInvestorProductGroupMargin, int nRequestID);

		/// <summary>
		///请求查询交易所保证金率
		/// </summary>
		int ReqQryExchangeMarginRate(ThostFtdcQryExchangeMarginRateField^ pQryExchangeMarginRate, int nRequestID);

		/// <summary>
		///请求查询交易所调整保证金率
		/// </summary>
		int ReqQryExchangeMarginRateAdjust(ThostFtdcQryExchangeMarginRateAdjustField^ pQryExchangeMarginRateAdjust, int nRequestID);

		/// <summary>
		///请求查询汇率
		/// </summary>
		int ReqQryExchangeRate(ThostFtdcQryExchangeRateField^ pQryExchangeRate, int nRequestID);

		/// <summary>
		///请求查询二级代理操作员银期权限
		/// </summary>
		int ReqQrySecAgentACIDMap(ThostFtdcQrySecAgentACIDMapField^ pQrySecAgentACIDMap, int nRequestID);


		/// <summary>
		///请求查询期权交易成本
		/// </summary>
		int ReqQryOptionInstrTradeCost(ThostFtdcQryOptionInstrTradeCostField^ pQryOptionInstrTradeCost, int nRequestID);

		/// <summary>
		///请求查询期权合约手续费
		/// </summary>
		int ReqQryOptionInstrCommRate(ThostFtdcQryOptionInstrCommRateField^ pQryOptionInstrCommRate, int nRequestID);

		/// <summary>
		///请求查询执行宣告
		/// </summary>
		int ReqQryExecOrder(ThostFtdcQryExecOrderField^ pQryExecOrder, int nRequestID);

		/// <summary>
		///请求查询询价
		/// </summary>
		int ReqQryForQuote(ThostFtdcQryForQuoteField^ pQryForQuote, int nRequestID);

		/// <summary>
		///请求查询报价
		/// </summary>
		int ReqQryQuote(ThostFtdcQryQuoteField^ pQryQuote, int nRequestID);


		/// <summary>
		///请求查询转帐流水
		/// </summary>
		int ReqQryTransferSerial(ThostFtdcQryTransferSerialField^ pQryTransferSerial, int nRequestID);

		/// <summary>
		///请求查询银期签约关系
		/// </summary>
		int ReqQryAccountregister(ThostFtdcQryAccountregisterField^ pQryAccountregister, int nRequestID);

		/// <summary>
		///请求查询签约银行
		/// </summary>
		int ReqQryContractBank(ThostFtdcQryContractBankField^ pQryContractBank, int nRequestID);

		/// <summary>
		///请求查询预埋单
		/// </summary>
		int ReqQryParkedOrder(ThostFtdcQryParkedOrderField^ pQryParkedOrder, int nRequestID);

		/// <summary>
		///请求查询预埋撤单
		/// </summary>
		int ReqQryParkedOrderAction(ThostFtdcQryParkedOrderActionField^ pQryParkedOrderAction, int nRequestID);

		/// <summary>
		///请求查询交易通知
		/// </summary>
		int ReqQryTradingNotice(ThostFtdcQryTradingNoticeField^ pQryTradingNotice, int nRequestID);

		/// <summary>
		///请求查询经纪公司交易参数
		/// </summary>
		int ReqQryBrokerTradingParams(ThostFtdcQryBrokerTradingParamsField^ pQryBrokerTradingParams, int nRequestID);

		/// <summary>
		///请求查询经纪公司交易算法
		/// </summary>
		int ReqQryBrokerTradingAlgos(ThostFtdcQryBrokerTradingAlgosField^ pQryBrokerTradingAlgos, int nRequestID);

		/// <summary>
		///期货发起银行资金转期货请求
		/// </summary>
		int ReqFromBankToFutureByFuture(ThostFtdcReqTransferField^ pReqTransfer, int nRequestID);

		/// <summary>
		///期货发起期货资金转银行请求
		/// </summary>
		int ReqFromFutureToBankByFuture(ThostFtdcReqTransferField^ pReqTransfer, int nRequestID);

		/// <summary>
		///期货发起查询银行余额请求
		/// </summary>
		int ReqQueryBankAccountMoneyByFuture(ThostFtdcReqQueryAccountField^ pReqQueryAccount, int nRequestID);

		// events
	public:
		/// <summary>
		/// 当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
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
		/// 当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
		/// 错误原因
		/// 0x1001 网络读失败
		/// 0x1002 网络写失败
		/// 0x2001 接收心跳超时
		/// 0x2002 发送心跳失败
		/// 0x2003 收到错误报文
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
		///心跳超时警告。当长时间未收到报文时，该方法被调用。
		///@param nTimeLapse 距离上次接收报文的时间
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
		/// 客户端认证响应
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
		/// 登录请求响应
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
		/// 登出请求响应
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
		/// 用户口令更新请求响应
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
		/// 资金账户口令更新请求响应
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
		/// 报单录入请求响应
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
		/// 预埋单录入请求响应
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
		/// 预埋撤单录入请求响应
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
		/// 报单操作请求响应
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
		/// 查询最大报单数量响应
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
		/// 投资者结算结果确认响应
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
		/// 删除预埋单响应
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
		/// 删除预埋撤单响应
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
		/// 执行宣告录入请求响应
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
		/// 执行宣告操作请求响应
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
		/// 询价录入请求响应
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
		/// 报价录入请求响应
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
		/// 报价操作请求响应
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
		/// 请求查询报单响应
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
		/// 请求查询成交响应
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
		/// 请求查询投资者持仓响应
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
		/// 请求查询资金账户响应
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
		/// 请求查询投资者响应
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
		/// 请求查询交易编码响应
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
		/// 请求查询合约保证金率响应
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
		/// 请求查询合约手续费率响应
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
		/// 请求查询交易所响应
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
		/// 请求查询产品响应
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
		/// 请求查询合约响应
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
		/// 请求查询行情响应
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
		/// 请求查询投资者结算结果响应
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
		/// 请求查询转帐银行响应
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
		/// 请求查询投资者持仓明细响应
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
		/// 请求查询客户通知响应
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
		/// 请求查询结算信息确认响应
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
		/// 请求查询投资者持仓明细响应
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
		/// 查询保证金监管系统经纪公司资金账户密钥响应
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
		/// 请求查询仓单折抵信息响应
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
		/// 请求查询投资者品种/跨品种保证金响应
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
		/// 请求查询交易所保证金率响应
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
		/// 请求查询交易所调整保证金率响应
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
		/// 请求查询汇率响应
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
		/// 请求查询二级代理操作员银期权限响应
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
		/// 请求查询期权交易成本响应
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
		/// 请求查询期权合约手续费响应
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
		/// 请求查询执行宣告响应
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
		/// 请求查询询价响应
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
		/// 请求查询报价响应
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
		/// 请求查询转帐流水响应
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
		/// 请求查询银期签约关系响应
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
		/// 错误应答
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
		/// 报单通知
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
		/// 成交通知
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
		/// 报单录入错误回报
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
		/// 报单操作错误回报
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
		/// 合约交易状态通知
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
		/// 交易通知
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
		/// 提示条件单校验错误
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
		/// 执行宣告通知
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
		/// 执行宣告录入错误回报
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
		/// 执行宣告操作错误回报
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
		/// 询价录入错误回报
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
		/// 报价通知
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
		/// 报价录入错误回报
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
		/// 报价操作错误回报
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
		/// 询价通知
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
		/// 请求查询签约银行响应
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
		/// 请求查询预埋单响应
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
		/// 请求查询预埋撤单响应
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
		/// 请求查询交易通知响应
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
		/// 请求查询经纪公司交易参数响应
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
		/// 请求查询经纪公司交易算法响应
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
		/// 银行发起银行资金转期货通知
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
		/// 银行发起期货资金转银行通知
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
		/// 银行发起冲正银行转期货通知
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
		/// 银行发起冲正期货转银行通知
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
		/// 期货发起银行资金转期货通知
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
		/// 期货发起期货资金转银行通知
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
		/// 系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
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
		/// 系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
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
		/// 期货发起查询银行余额通知
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
		/// 期货发起银行资金转期货错误回报
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
		/// 期货发起期货资金转银行错误回报
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
		/// 系统运行时期货端手工发起冲正银行转期货错误回报
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
		/// 系统运行时期货端手工发起冲正期货转银行错误回报
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
		/// 期货发起查询银行余额错误回报
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
		/// 期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
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
		/// 期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
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
		/// 期货发起银行资金转期货应答
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
		/// 期货发起期货资金转银行应答
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
		/// 期货发起查询银行余额应答
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
		/// 银行发起银期开户通知
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
		/// 银行发起银期销户通知
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
		/// 银行发起变更银行账号通知
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
		// 将所有回调函数地址传递给SPI
		void RegisterCallbacks();
#endif

	};
};