using System;
using Beng.Specta.Compta.Core.Entities;

namespace Beng.Specta.Compta.Core.DTOs
{
    public static class DTOsExtension
    {
        public static ProjectDTO ToDTO(this Project item)
        {
            return new ProjectDTO
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
}
