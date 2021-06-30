using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;

namespace WorkerConsumoKafka
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ParametrosExecucao _parametrosExecucao;
        private readonly IConsumer<Ignore, string> _consumer;

        public Worker(ILogger<Worker> logger,
            ParametrosExecucao parametrosExecucao)
        {
            _logger = logger;
            _parametrosExecucao = parametrosExecucao;

            _logger.LogInformation($"Bootstrap Servers = {parametrosExecucao.BootstrapServers}");
            _logger.LogInformation($"Topic = {parametrosExecucao.Topic}");
            _logger.LogInformation($"Group Id = {parametrosExecucao.GroupId}");

            _consumer = new ConsumerBuilder<Ignore, string>(
                new ConsumerConfig()
                {
                    BootstrapServers = parametrosExecucao.BootstrapServers,
                    SecurityProtocol = SecurityProtocol.SaslSsl,
                    SaslMechanism = SaslMechanism.Plain,
                    SaslUsername = parametrosExecucao.SaslUsername,
                    SaslPassword = parametrosExecucao.SaslPassword,
                    GroupId = parametrosExecucao.GroupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest
                }).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Aguardando mensagens...");
            _consumer.Subscribe(_parametrosExecucao.Topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() =>
                {
                    var result = _consumer.Consume(stoppingToken);
                    _logger.LogInformation(
                        $"[{_parametrosExecucao.GroupId} | Nova mensagem] " +
                        result.Message.Value);
                });
            }
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _consumer.Close();
            _logger.LogInformation(
                "Conexao com o Apache Kafka fechada!");
            return Task.CompletedTask;
        }
    }
}