namespace APIContagem
{
    public interface IResultadoContador
    {
        public int ValorAtual { get; } 
        public string Local { get; } 
        public string Kernel { get; } 
        public string TargetFramework { get; } 
        public string Mensagem { get; }
    }
}