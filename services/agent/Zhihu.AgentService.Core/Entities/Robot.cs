using Zhihu.Core.Common;
using Zhihu.Core.Common.Entities;
using Zhihu.SharedKernel.Domain;

namespace Zhihu.AgentService.Core.Entities;

public class Robot : BaseAuditEntity, IAggregateRoot
{
    public required string Name { get; set; }

    public required string Knowledge { get; set; }

    public string? Character { get; set; }

    public string? ExtraPrompt { get; set; }

    public double Temperature { get; set; }

    public string? Token { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset? LastActionTime { get; set; }
}