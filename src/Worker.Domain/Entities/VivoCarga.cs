using System;

namespace Worker.Domain.Entities;

public class VivoCarga
{
    public int Id { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public string Protocolo { get; set; } = null!;
    public int IdRobo { get; set; }
    public string Status { get; set; } = null!;
    public DateTime? DataTratativa { get; set; }
    public int DocumentoId { get; set; }
    public string CaminhoPasta { get; set; } = null!;
    public int UsuarioId { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public bool Excluido { get; set; }
    public DateTime? DataRecebimento { get; set; }
    public bool ConcluirManualmente { get; set; }
    public DateTime? DataTriagem { get; set; }
    public int Priorizacao { get; set; }
}