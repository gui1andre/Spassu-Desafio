using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public DateTime? AtualizadoEm { get; protected set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CriadoEm = DateTime.UtcNow;
        }
    }
}
