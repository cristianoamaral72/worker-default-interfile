using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Worker.Domain.Entities;
using Worker.Domain.Interfaces.Base;

namespace Worker.Domain.Interfaces.Repositories;

public interface IVivoCargaRepository : IBaseRepository<VivoCarga>
{
    Task<IEnumerable<VivoCarga>> BuscarPorDataAsync(DateTime data);
}