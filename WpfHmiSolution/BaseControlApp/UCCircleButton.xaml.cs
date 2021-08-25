using System.Windows;
using System.Windows.Controls;

namespace BaseControlApp
{
    /// <summary>
    /// UCCircleButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UCCircleButton : UserControl
    {
        // Step1. RoutedEvent 정의
        public static readonly RoutedEvent CustomClickEvent =
            EventManager.RegisterRoutedEvent("CustomClick", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),typeof(UCCircleButton));

        // Step2. RoutedEventHandler 선언
        public event RoutedEventHandler CustomClick
        {
            add { AddHandler(CustomClickEvent, value); }        // +=
            remove { RemoveHandler(CustomClickEvent, value); }  // -= eventHandler
        }
        public UCCircleButton()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Step3. 버튼클릭 이벤트를 상위로 올려주는 실행
            RaiseEvent(new RoutedEventArgs(CustomClickEvent, sender));
        }
    }
}
