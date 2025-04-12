using Worker.Domain.Interfaces.Base;

namespace Worker.Domain.Entities.Autenticacao;

public interface IEntity<TKey> : IBaseEntity
{
    #region IEntity Members

    TKey Id { get; set; }

    #endregion
}