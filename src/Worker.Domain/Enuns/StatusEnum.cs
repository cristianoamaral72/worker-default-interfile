using System.ComponentModel;

namespace Worker.Domain.Enuns;

public enum StatusEnum
{
    [Description("Agendado")]
    Agendado = 1,

    [Description("Publicado")]
    Publicado = 2,

    [Description("Rascunho")]
    Rascunho = 3,

    [Description("Revisão Pendente")]
    RevisaoPendente = 4,

    [Description("Privado")]
    Privado = 5
}