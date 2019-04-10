using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Threading;
using Easy.MessageHub;
using 服务端.wcf.information;

namespace 服务端
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Person p1 = new Person("群主", null, null);
        private static readonly object InstObj = new object();

        public MainWindow()
        {
            InitializeComponent();

        }
        ServiceHost host;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            host = new ServiceHost(typeof(Services));
            localhost.Content = GetIp.GetLocalIP();
            try
            {
                host.Open();
            }
            catch (Exception)
            {

                throw;
            }
            #region Remote Spy
            Action<Message> action = message =>
            {
                string s1 = null;
                string s2 = null;
                if (message.Sender == p1)
                {
                    s1 = "我自己";
                }
                else
                {
                    s1 = message.Sender.Name.ToString();
                }
                if (message.Receiver == null)
                {
                    s2 = "对大家群聊说：";
                }
                else
                {
                    s2 = string.Format("对{0}私聊说：", message.Receiver.Name);
                }
                this.listening_box.Items.Add(s1 + s2 + message.Word);
            };
            var anotherToken = Services.hub.Subscribe(action);
            #endregion
            #region Add
            Action<string> add = operate =>
            {
                //异步委托处理
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    online_users.Items.Clear();
                    foreach (var l in Services.People)
                    {
                        this.online_users.Items.Add(l.Name);
                        l.CallBackmessage.Onlineuser(Services.People);
                    }
                }));
            };            
            var Token1 = Services.hubadd.Subscribe(add);
        }
        private void private_chat_Click(object sender, RoutedEventArgs e)
        {
            if (Services.People == null || Services.People.Count > 0)
            {
                if (online_users.SelectedItem != null)
                {
                    Message m1 = new Message(p1, Services.People[online_users.SelectedIndex], input_box.Text);
                    Services.People[online_users.SelectedIndex].CallBackmessage.Recevie(m1);
                    listening_box.Items.Add(string.Format("我说{0}私聊说：{1}", online_users.SelectedValue.ToString(), input_box.Text.ToString()));

                }
                else
                {
                    MessageBox.Show("请先点击私聊对象");
                    return;
                }
            }
        }
        private void public_chat_Click(object sender, RoutedEventArgs e)
        {
            foreach (var p in Services.People)
            {
                p.CallBackmessage.Recevie1(input_box.Text);
            }
            listening_box.Items.Add(string.Format(" 我说： {0} ", input_box.Text.ToString()));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (online_users.SelectedItem != null)
            {
                Services.People[online_users.SelectedIndex].CallBackmessage.DisableSendMsg();
            }
            else
            {
                MessageBox.Show("请先点击私聊对象");
                return;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (online_users.SelectedItem != null)
            {
                Services.People[online_users.SelectedIndex].CallBackmessage.SendMsg();
            }
            else
            {
                MessageBox.Show("请先点击私聊对象");
                return;
            }
        }
    }
}
#endregion