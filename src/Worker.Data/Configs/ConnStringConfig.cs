using Worker.Domain.Interfaces.Configs;

namespace Worker.Data.Configs
{
    public class ConnStringConfig<T> : IConnStringConfig<T> where T : class
    {
        public string ConnString { get; set; }
    }
}
