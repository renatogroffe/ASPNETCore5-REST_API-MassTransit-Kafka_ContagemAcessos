using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkerConsumoKafka
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(
                "Testando o consumo de mensagens com Apache Kafka + Azure Event Hubs");            
            
            if (args.Length != 4)
            {
                Console.WriteLine(
                    "Informe 4 parametros: " +
                    "no primeiro o Host Name do Namespace do Azure Event Hubs (sera assumida a porta 9093), " +
                    "no segundo a string de conexão fornecida pelo Portal do Azure, " +
                    "no terceiro o Topic (Event Hub) a ser utilizado no consumo das mensagens, " +
                    "no quarto o Group Id da aplicacao...");
                return;
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ParametrosExecucao>(
                        new ParametrosExecucao()
                        {
                            BootstrapServers = $"{args[0]}:9093",
                            SaslUsername = "$ConnectionString",
                            SaslPassword = args[1],
                            Topic = args[2],
                            GroupId = args[3]
                        });
                    services.AddHostedService<Worker>();
                });
    }
}