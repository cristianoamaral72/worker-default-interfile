using Dapper.FluentMap.Dommel.Mapping;
using Worker.Domain.Entities;

namespace Worker.Data.Mappers;
public class CategoriaMap : DommelEntityMap<Categoria>
{
    public CategoriaMap()
    {
        // Mapeia a chave primária
        ToTable("ClickBankCategories");
        Map(p => p.CategoryID).IsKey().IsIdentity();

        // Mapeia outras propriedades
        Map(p => p.CategoryName).ToColumn("CategoryName");
        Map(p => p.DataCadastro).ToColumn("DataCadastro");
        Map(p => p.Status).ToColumn("Status");
        Map(p => p.WordPressCategoryId).ToColumn("WordPressCategoryId");
        Map(p => p.WordPressCategoryName).ToColumn("WordPressCategoryName");
        Map(p => p.WordPressSlug).ToColumn("WordPressSlug");
    }
}
