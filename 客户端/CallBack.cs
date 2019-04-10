using System.Collections.Generic;
using System.ServiceModel;
using 客户端.host;

namespace 客户端
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CallBack : host.IServicesCallback
    {
        static MainWindow main = null;
        public Person[] people = null;
        public CallBack() { }
        public MainWindow M1 { get => main; set => main = value; }
        public CallBack(MainWindow m)
        {
            main = m;
        }
        public void Onlineuser(Person[] p1)
        {
            people = p1;
            M1.online_users.Items.Clear();
            foreach (var p in people)
            {
                main.online_users.Items.Add(p.Name);
            }
        }
        public void Recevie(Message m1)
        {
            if (m1.Receiver == null)
            {
                main.chatting_records.Items.Add(m1.Sender.Name + "对大家说:" + m1.Word);
            }
            else
            {
                main.chatting_records.Items.Add(m1.Sender.Name + "对我私聊说:" + m1.Word);
            }
        }
        public void Recevie1(string s1)
        {
            main.chatting_records.Items.Add("群主说:" + s1);
        }
        public void Myname(Person p1)
        {
            main.personown = p1;
        }

        public void DisableSendMsg()
        {
            main.pubic_chat.Content = "您已经被禁言";
            main.pubic_chat.IsEnabled = false;
        }

        public void SendMsg()
        {
            main.pubic_chat.Content = "私聊";
            main.pubic_chat.IsEnabled = true ;
        }
    }
}
