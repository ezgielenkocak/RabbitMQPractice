using System;
using System.Text;
using RabbitMQ.Client; //bağlantı oluşturma
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            //1-)Bağlantı oluşturma;
            ConnectionFactory factory = new();
            factory.Uri = new("amqps://wshfrkdl:mQeHhSM2SYagOsk4ttKUSwjiz3j8AnoV@moose.rmq.cloudamqp.com/wshfrkdl");

            //2-)Bağlantı Aktifleştirme
            using IConnection connection = factory.CreateConnection();

            //3-)Kanal Açma
            using IModel channel = connection.CreateModel();

            //4-) Queue Oluşturma
            channel.QueueDeclare(queue: "example-queue", exclusive: false); //=>Consumerdaki kuyruk publisherdaki ile birebir aynı yapıda tanımlanmalıdr.

            //5-)Queue'den Mesaj Okuma (Okuma yapabilmek için bu channel üzerinde bir event operation yapmalıyız)
            EventingBasicConsumer consumer = new(channel); 
            channel.BasicConsume(queue:"example-queue", false, consumer); //consumer instance'ın üzerinde bu channelda bildireceğim kuyrukta mesaj varsa onu işleyeceğiz-tüketeceğiz.

            consumer.Received += (sender, e) => //mesaj geldiğinde consumerın bu mesajı yakalaması için receive etmesi lazım. Receive bir delegatetir.
             {
                 //Kuyruğa gelen mesajın işlendiği yer
                 //e.Body() => Kuyruktaki mesajın verisini bütünsel olrk getirir.
                // e.Body.Span() veya e.Body.ToArray(): Kuyruktaki mesajın byte verisini getirir.

                 Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span)); //byte olarak gelen mesajı stringe çevirdim
             };

            Console.Read();
        }
    }
}
