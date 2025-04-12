namespace Worker.Domain.Interfaces.Configs
{
    public interface IConnStringConfig<T> where T : class
    {
        string ConnString { get; set; }
    }
}