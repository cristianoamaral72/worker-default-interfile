using System.Runtime.Serialization;

namespace Worker.Domain.Entities.Autenticacao;

public abstract class Entity<TKey> : IEntity<TKey>
{
    #region Public Properties

    [DataMember]
    public TKey Id { get; set; }

    #endregion

    #region Public Methods

    public virtual object Clone() => MemberwiseClone();

    #endregion
}