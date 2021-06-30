namespace WorkerConsumoKafka
{
    public class ParametrosExecucao
    {
        public string BootstrapServers { get; set; }
        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
        public string Topic { get; set; }
        public string GroupId { get; set; }
    }
}