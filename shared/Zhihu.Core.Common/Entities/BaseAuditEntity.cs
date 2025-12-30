namespace Zhihu.Core.Common.Entities;

public abstract class BaseAuditEntity : BaseEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
}