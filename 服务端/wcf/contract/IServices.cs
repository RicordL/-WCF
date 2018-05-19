using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace 服务端
{
    [ServiceContract(SessionMode = SessionMode.Required,CallbackContract = typeof(ICallBackServices))]
    public  interface IServices
    {
        /// <summary>
        /// 客户端发送消息（群聊/私聊）
        /// </summary>
        /// <param name="m1"></param>
        [OperationContract (IsOneWay =true)]
        void Sendmessage(Message m1);
        /// <summary>
        /// 客户端注册
        /// </summary>
        /// <param name="usename"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay =true)]
        void Register(string username);
    }

    public interface ICallBackServices
    {        
        /// <summary>
        /// 服务端向客户端发送信息(异步)
        /// </summary>
        /// <param name="Message"></param>
        [OperationContract(IsOneWay = true)]
        void Recevie(Message m1);
        [OperationContract(IsOneWay = true)]
        void Recevie1(string s1);
        /// <summary>
        /// 服务端向客户端发送在线客户端情况
        /// </summary>
        /// <param name="p1"></param>
        [OperationContract(IsOneWay = true)]
        void Onlineuser(List<Person> p1);
        [OperationContract(IsOneWay = true)]
        void Myname(Person p1);
        [OperationContract(IsOneWay = true)]
        void DisableSendMsg();
        [OperationContract(IsOneWay = true)]
        void SendMsg();
    }
}
