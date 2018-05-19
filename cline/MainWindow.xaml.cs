using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using 客户端.host;
using System.Threading;
using System;
using System.Configuration;
using System.IdentityModel.Selectors;
using System.ServiceModel.Configuration;
using System.Text.RegularExpressions;

namespace 客户端
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly object InstObj = new object();
        public MainWindow()
        {
            InitializeComponent();
            List<Person> People = new List<Person>();
        }
        host.ServicesClient client = null;
        CallBack back = null;
        public Person personown = null;
        private void Login_Click(object sender, RoutedEventArgs e)
        {

            if (user_name.Text.ToString() != "")
            {
                try
                {
                    client.Register(user_name.Text.ToString());
                    login.IsEnabled = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("连接错误，请检查IP");
                }
            }
            else
            {
                MessageBox.Show("请先输入名字");
            }

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client == null)
                {
                    back = new CallBack(this);
                    InstanceContext context = new InstanceContext(back);
                    client = new ServicesClient(context);
                    string a = client.State.ToString();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            IP.Text = ConfigurationManager.AppSettings["localhost"].ToString();
        }
        private void pubic_chat_Click(object sender, RoutedEventArgs e)
        {
            host.Message message = new Message();
            message.Sender = personown;
            message.Word = input_text.Text;
            client.Sendmessage(message);
        }
        private void private_chat_Click(object sender, RoutedEventArgs e)
        {
            if (online_users.SelectedItem != null)
            {
                Message m1 = new Message();
                m1.Sender = personown;
                m1.Receiver = back.people[online_users.SelectedIndex];
                m1.Word = input_text.Text.ToString();
                client.Sendmessage(m1);
                chatting_records.Items.Add("我对"+back.people[online_users.SelectedIndex].Name+input_text .Text .ToString());
            }
            else
            {
                MessageBox.Show("请先点击私聊对象");
                return;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                back = new CallBack(this);
                InstanceContext context = new InstanceContext(back);
                client = new ServicesClient(context);
                client.Endpoint.Address = new EndpointAddress("net.tcp://" +IP.Text .ToString () + "/5000/host");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Open();
                MessageBox.Show("IP正确，可以连接。");
            }
            catch (Exception)
            {
                MessageBox.Show("IP错误，无法连接到服务器。");
            }
        }
    }
}
