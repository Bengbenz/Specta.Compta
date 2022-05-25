using Beng.Specta.Compta.Core.Entities;

namespace Beng.Specta.Compta.Core.Dtos;

public static class DTOsExtension
{
    public static ProjectDto ToDto(this Project item)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new ProjectDto
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            StartDate = item.StartDate,
            Duration = item.Duration
        };
    }
}