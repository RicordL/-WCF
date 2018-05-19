using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Easy.MessageHub;
namespace 服务端
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Services : IServices
    {
        public static MessageHub hub = MessageHub.Instance;//聊天记录的HUB
        public static MessageHub hubadd = MessageHub.Instance;//聊天对象
        private static readonly object InstObj = new object();//单一实例     
        public static List<Person> People = new List<Person>();//记录客户端
        public void Register(string username)
        {
            ICallBackServices client = OperationContext.Current.GetCallbackChannel<ICallBackServices>();
            string sessionid = OperationContext.Current.SessionId;
            OperationContext.Current.Channel.Closing += new EventHandler(Channel_Closing);//注册客户端关闭触发事件
            Person p = new Person(username, sessionid, client);
            People.Add(p);
            hubadd.Publish("刷新");
            p.CallBackmessage.Myname(p);
        }
/// <summary>
/// ///////
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void Channel_Closing(object sender, EventArgs e)
        {
          lock (InstObj)//加锁，处理并发aA   Q
            {
                if (People != null && People.Count > 0)
                {
                    foreach (Person d in People)
                    {
                        if (d.CallBackmessage == (ICallBackServices)sender)//删除此关闭的客户端信息
                        {
                            People.Remove(d);                         
                            break;
                        }
                    }
                }
                hubadd.Publish("刷新");
            }            
        }
        public void Sendmessage(Message m1)
        {
            if (m1.Receiver == null)
            {
                foreach (var p in People)
                {
                    p.CallBackmessage.Recevie(m1);
                }
            }
            else
            {
                foreach (var p in People)
                {
                    if (p.SessionID == m1.Receiver.SessionID)
                    {
                        p.CallBackmessage.Recevie(m1);
                    }
                }
            }
            hub.Publish(m1);
        }
    }
}
