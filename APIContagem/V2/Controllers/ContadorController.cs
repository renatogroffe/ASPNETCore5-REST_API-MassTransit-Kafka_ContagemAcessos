using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using APIContagem.V2.Models;
using MassTransit.KafkaIntegration;

namespace APIContagem.V2.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class ContadorController : ControllerBase
    {
        private static readonly Contador _CONTADOR = new Contador();
        private readonly ILogger<ContadorController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITopicProducer<IResultadoContador> _producer;

        public ContadorController(ILogger<ContadorController> logger,
            IConfiguration configuration,
            ITopicProducer<IResultadoContador> producer)
        {
            _logger = logger;
            _configuration = configuration;
            _producer = producer;
        }

        [HttpGet]
        public ResultadoContador GetV2_0()
        {
            int valorAtualContador;
            lock (_CONTADOR)
            {
                _CONTADOR.Incrementar();
                valorAtualContador = _CONTADOR.ValorAtual;
            }

            _logger.LogInformation($"Contador - Valor atual: {valorAtualContador}");

            var resultado = new ResultadoContador()
            {
                ValorAtual = valorAtualContador,
                Versao = "2.0",
                Local = _CONTADOR.Local,
                Kernel = _CONTADOR.Kernel,
                TargetFramework = _CONTADOR.TargetFramework,
                Mensagem = _configuration["MensagemVariavel"]
            };
            _producer.Produce(resultado);

            return resultado;
        }
    }
}