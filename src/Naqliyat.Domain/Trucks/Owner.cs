using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Trucks
{
    public class Owner : AuditedEntity<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Identification { get; set; }
        public string EntityNumber { get; set; }
        public int NationalityId { get; set; }
        public Guid UserId { get; set; }

        public virtual Country Nationality { get; set; }

        public Owner()
        {
        }

        public Owner(
            Guid id,
            string name,
            string phone,
            string identification,
            string entityNumber,
            int nationalityId,
            Guid userId)
            : base(id)
        {
            Name = name;
            Phone = phone;
            Identification = identification;
            EntityNumber = entityNumber;
            NationalityId = nationalityId;
            UserId = userId;
        }
    }
}
