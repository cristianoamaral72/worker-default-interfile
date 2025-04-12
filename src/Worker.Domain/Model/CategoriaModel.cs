using System;

namespace Worker.Domain.Model;

public class CategoriaModel
{
    public int Id { get; set; }
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public string Title { get; set; }
    public string HopLink { get; set; }
    public string SubCategoryName { get; set; }
    public string LinkPresell { get; set; }
    public int SubCategoryID { get; set; }
    public int WordPressCategoryId { get; set; }
    public string Status { get; set; }
    public string WordPressCategoryName { get; set; }
    public string WordPressSlug { get; set; }
    public DateTime? DataCadastro { get; set; }
}