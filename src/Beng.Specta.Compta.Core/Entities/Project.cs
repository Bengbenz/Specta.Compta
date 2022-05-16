using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Beng.Specta.Compta.Core.Entities.Funding;
using Beng.Specta.Compta.SharedKernel;

namespace Beng.Specta.Compta.Core.Entities
{
    /// <summary>
    /// Projects
    /// </summary>
    public class Project : BaseEntity
    {
        public Project()
        {
        }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Duration in month
        /// </summary>
        [Required]
        public int Duration { get; set; }

        /// <summary>
        /// List of contracts
        /// </summary>
        /// <remarks>References (OneToMany) to Contract</remarks>
        /// public ICollection<FundingContract> Contracts { get; private set; } = new List<FundingContract>();
    }
}
