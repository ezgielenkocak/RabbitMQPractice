using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
namespace RabbitMQ.Publisher
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            //1-)bağlantı oluşturma;
            ConnectionFactory factory = new(); //hangi sunucuya bağlanacağını factory instance'ında bildirmen gerekir.
            factory.Uri = new("amqps://wshfrkdl:mQeHhSM2SYagOsk4ttKUSwjiz3j8AnoV@moose.rmq.cloudamqp.com/wshfrkdl");

            //2-)bağlantıyı aktifleştirme;
           using IConnection connection=factory.CreateConnection(); //connection nesnesi oluşturdum.

            //3-)kanal açma;
           using IModel channel= connection.CreateModel(); //chanellar; connection üstünde işlem yapmamızı sağlar

            //4-)Queue(kuyruk) oluşturma; publisherın mesajo yollayabileceği kuyruğu oluşturmuş oldum
            channel.QueueDeclare(queue:"example-queue", exclusive:false); //exclusive:true ise o kuyruk o bağlantıya özel oluşturulur sonra silinir. 

            //5-)Queue'ya (kuyruğa) mesaj gönderme; (Rabbitmq; kuyruğa atacağı mesajı byte türünden kabul eder. O yüzden mesajları byte dizi'ne convert etmeliyiz.)
            //byte[] message= Encoding.UTF8.GetBytes("Merhaba");
            //channel.BasicPublish(exchange:"", routingKey: "example-queue", body: message);  //=> bu mesajı rabbitmqda oluşturduğumuz kuyruğa 'channel üzerinden göndericez' göndericez.

            for (int i = 0; i < 50; i++)
            {
                await Task.Delay(200);
                byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
                channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);
            }
            //Default Exhange=>Direct Exchangedir.(YANİ; Mesajın gideceği kuyruğun adı routing key ile aynı olunca mesaj gider)
            //Routing Key=> Exchange bind edilmiş kuyruklardan hangisine mesaj göndereceğimi belirler.
            //Excahnge default 'direct' ise o zaman routing key=mesaj kuyruğunun ismine karşılık gelir.
            //O zaman hangi mesaj kuyruğuna mesaj göndereceksem o kuyruğun ismi routing key ile aynı olacak.

            Console.Read();

            //Bağlantının oluştuğunu görmek için; cloudamqp de rabbit manager'a giderek sunucuya bağlanırım. Connection kısmındaki aktif bağlantıyı görebiliriz.
        }
    }
}
