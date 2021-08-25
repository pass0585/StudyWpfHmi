using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace NewScadaApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private string serverIpNum = "192.168.200.105"; // 윈도우(MQTT Broker, SQLServer) 아이피
        private string clientId = "SCADA_system";
        private string factoryId = "Kasan01";  //   Kasan01/4001/  Kasan01/4002/ 
        private string publishAddr = "4002";
        private string subcribAddr = "";

        private string connectionString = string.Empty; // SQLServer 연결문자열
        private MqttClient client; // MQTT 접속을 객체 null

        public MainWindow()
        {
            InitializeComponent();
            App.LOGGER.Info("SCADA Start!!");  // 시작 로그
            InitAllCustomComponent();
        }

        // SCADA 시스템용 커스텀 초기화메서드
        private void InitAllCustomComponent()
        {
            LblStatus.Content = string.Empty;
            //IPAddress serverAddress = IPAddress.Parse(serverIpNum);
            client = new MqttClient(serverIpNum);
            client.MqttMsgPublished += Client_MqttMsgPublished;
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.ConnectionClosed += Client_ConnectionClosed;

            connectionString = "";
        }
        
        // MQTT 서버와 접속이 끊어졌을때 이벤트처리
        private void Client_ConnectionClosed(object sender, EventArgs e)
        {
            
        }

        // MQTT에서 메시지를 구독하면 이벤트처리(★★★★★)
        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            
        }

        // MQTT로 메시지를 발행(한뒤) 이벤트처리
        private void Client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            
        }

        // 모니터링 시작 버튼 클릭처리
        private void BtnMonitoring_Click(object sender, RoutedEventArgs e)
        {
            App.LOGGER.Info("모니터링 시작 : BtnMonitoring_Click");
            MessageBox.Show("모니터링 시작!");
        }

        // 위급시 모터 동작처리
        private void BtnMotor_CustomClick(object sender, RoutedEventArgs e)
        {
            App.LOGGER.Warn("위급처리 : FuelTank정지할 수 있습니다!");
            MessageBox.Show("위급처리!");
        }

        // 메인윈도우 닫히기 전 처리
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 리소스 해제
            App.LOGGER.Info("SCADA 프로그램 종료!");
        }
    }
}
