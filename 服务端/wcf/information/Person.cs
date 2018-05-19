using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace 服务端
{
   [DataContract]
    public class Person
    {
        string name;
        string sessionID;
        ICallBackServices callBackmessage;
       [DataMember]
        public string Name { get => name; set => name = value; }
        [DataMember]
        public string SessionID { get => sessionID; set => sessionID = value; }        
        public ICallBackServices CallBackmessage { get => callBackmessage; set => callBackmessage = value; }
        public Person(string s1,string s2,ICallBackServices c1)
        {
            Name = s1;
            SessionID = s2;
            CallBackmessage = c1;
        }
    }
}
