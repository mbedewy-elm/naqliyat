using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Attachments
{
    public class Attachment : AuditedEntity<Guid>
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
        public byte[] Content { get; set; }

        public string Description { get; set; }

        public Attachment()
        {
        }

        public Attachment(
            Guid id,
            string fileName,
            string contentType,
            long length,
            byte[] content,
            string description = null)
            : base(id)
        {
            FileName = fileName;
            ContentType = contentType;
            Length = length;
            Content = content;
            Description = description;
        }
    }
}
