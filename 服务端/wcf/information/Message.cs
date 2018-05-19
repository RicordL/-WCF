using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace 服务端
{
    [DataContract]
    public  class Message
    {
        Person sender;
        Person receiver;
        String word;
        [DataMember]
        public Person Sender { get => sender; set => sender = value; }
        [DataMember]
        public Person Receiver { get => receiver; set => receiver = value; }
        [DataMember]
        public string Word { get => word; set => word = value; }
        public Message(Person p1,Person p2,string s1)
        {
            Sender = p1;
            Receiver = p2;
            Word = s1;
        }
    }
}
