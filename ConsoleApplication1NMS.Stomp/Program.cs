using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Apache.NMS;
using Apache.NMS.Stomp;

namespace ApacheNMS
{
    class Program
    {
        static void Main(string[] args)
        {
            IConnectionFactory factory = new NMSConnectionFactory(new Uri("stomp:tcp://api.sample.com:61616"));

            IConnection connection = factory.CreateConnection("username", "password");
            ISession session = connection.CreateSession();

            IDestination destination = session.GetDestination("topic://" + "DATA_FEED");
            IMessageConsumer consumer = session.CreateConsumer(destination);

            connection.Start();
            //IMessage message = consumer.Receive();
            consumer.Listener += new MessageListener(OnMessage);
            Console.WriteLine("Consumer started, waiting for messages... (Press ENTER to stop.)");

            Console.ReadLine();
            connection.Close();
        }

        private static void OnMessage(IMessage message)
        {
            
            try
            {
                Console.WriteLine("Median-Server (.NET): Message received");

                ITextMessage msg = (ITextMessage)message;
                
                message.Acknowledge();

                Console.WriteLine(msg.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("---");
                Console.WriteLine(ex.InnerException);
                Console.WriteLine("---");
                Console.WriteLine(ex.InnerException.Message);
            }
        }
    }
}
