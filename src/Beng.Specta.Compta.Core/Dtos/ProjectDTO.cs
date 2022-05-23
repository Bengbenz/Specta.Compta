using System;
using System.ComponentModel.DataAnnotations;
using Beng.Specta.Compta.Core.Entities;

namespace Beng.Specta.Compta.Core.Dtos
{
    // Note: doesn't expose events or behavior
    public class ProjectDTO : BaseDto
    {
        public ProjectDTO ()
        {
        }
        
        public ProjectDTO (ProjectDTO value)
        {
            Id = value.Id;
            Code = value.Code;
            Name = value.Name;
            Description = value.Description;
            StartDate = value.StartDate;
            Duration = value.Duration;
        }

        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public int Duration { get; set; }

        /// <summary>
        /// List of contracts
        /// </summary>
        /// <remarks>References (OneToMany) to Contract</remarks>
        //public IList<FundingGroup> Contracts { get; set; } = new List<FundingGroup>();
    }
}
