using Worker.Domain.Interfaces.Configs;

namespace Worker.Data.Contexts;

public class DapperContext : DatabaseContext
{
    public DapperContext(IConnStringConfig<DapperContext> config) : base(config.ConnString)
    {
    }
}