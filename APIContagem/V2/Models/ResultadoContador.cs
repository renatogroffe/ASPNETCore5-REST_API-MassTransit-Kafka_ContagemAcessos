namespace APIContagem.V2.Models
{
    public class ResultadoContador : IResultadoContador
    {
        public int ValorAtual { get; set; } 
        public string Versao { get; set; }
        public string Local { get; set; } 
        public string Kernel { get; set; } 
        public string TargetFramework { get; set; } 
        public string Mensagem { get; set; }
    }
}