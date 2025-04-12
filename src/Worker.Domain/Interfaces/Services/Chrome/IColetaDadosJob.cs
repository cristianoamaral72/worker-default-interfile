using System.Threading.Tasks;

namespace Worker.Domain.Interfaces.Services.Chrome;

public interface IColetaDadosJob
{
    Task ExecuteChrome();
}