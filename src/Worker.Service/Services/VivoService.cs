using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.Domain.Entities;
using Worker.Domain.Interfaces.Base;
using Worker.Domain.Interfaces.Services;
using Worker.Service.Services.Autenticacao;

namespace Worker.Service.Services
{
    public class VivoService : ServiceBase<VivoCarga>, IVivoCargaService
    {
        public VivoService(IBaseRepository<VivoCarga> repositoryBase) : base(repositoryBase)
        {
        }
    }
}
