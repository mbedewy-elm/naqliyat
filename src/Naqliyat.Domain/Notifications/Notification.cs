using System;
using Naqliyat.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace Naqliyat.Notifications
{
    public class Notification : AuditedEntity<Guid>
    {
        public string Receiver { get; set; }
        public string Content { get; set; }
        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }
        public DateTime? ReadTime { get; set; }

        public Notification()
        {
        }

        public Notification(
            Guid id,
            string receiver,
            string content,
            NotificationType type)
            : base(id)
        {
            Receiver = receiver;
            Content = content;
            Type = type;
            IsRead = false;
        }

        public void MarkAsRead()
        {
            if (!IsRead)
            {
                IsRead = true;
                ReadTime = DateTime.UtcNow;
            }
        }
    }
}
