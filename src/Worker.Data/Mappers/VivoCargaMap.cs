using Dapper.FluentMap.Dommel.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.Domain.Entities;

namespace Worker.Data.Mappers
{
    public class VivoCargaMap : DommelEntityMap<VivoCarga>
    {
        public VivoCargaMap()
        {

            // Mapeia a chave primária
            ToTable("VivoCarga");
            Map(p => p.Id).IsKey().IsIdentity();

            // Mapeia outras propriedades
            Map(p => p.Status).ToColumn("Status");
            Map(p => p.Ativo).ToColumn("Ativo");

        }
    }
}
