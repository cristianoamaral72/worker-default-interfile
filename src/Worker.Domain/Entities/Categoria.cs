using System;

namespace Worker.Domain.Entities;

public class Categoria
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;
    public string Status { get; set; }
    public int WordPressCategoryId { get; set; }
    public string WordPressCategoryName { get; set; }
    public string WordPressSlug { get; set; }
}