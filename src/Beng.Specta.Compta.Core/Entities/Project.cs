using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Beng.Specta.Compta.Core.Entities.Funding;
using Beng.Specta.Compta.Core.ValueTypes.Ids;
using Beng.Specta.Compta.SharedKernel;

namespace Beng.Specta.Compta.Core.Entities
{
    /// <summary>
    /// Projects
    /// </summary>
    public class Project : BaseEntity
    {
        public Project(
            string code,
            string name,
            DateTime startDate,
            int duration,
            string description="")
        {
            Code = code;
            Name = name;
            Description = description;
            StartDate = startDate;
            Duration = duration;
        }

        public string Code { get; private set; }

        [Required]
        public string Name { get; private set; }

        public string Description { get; private set; }

        [Required]
        public DateTime StartDate { get; private set; }

        [Required]
        public int Duration { get; private set; }

        /// <summary>
        /// List of contracts
        /// </summary>
        /// <remarks>References (OneToMany) to Contract</remarks>
        public IList<FundingGroup> Contracts { get; private set; } = new List<FundingGroup>();
    }
}
