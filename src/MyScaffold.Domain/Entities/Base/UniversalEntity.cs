using System.ComponentModel.DataAnnotations;

namespace MyScaffold.Domain.Entities.Base
{
    public class UniversalEntity : IAggregateRoot
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}