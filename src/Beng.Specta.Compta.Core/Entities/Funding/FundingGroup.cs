using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Beng.Specta.Compta.Core.ValueTypes.Funding;
using Beng.Specta.Compta.Core.ValueTypes.Ids;
using Beng.Specta.Compta.SharedKernel;

using Dawn;

using NodaMoney;

namespace Beng.Specta.Compta.Core.Entities.Funding
{
    /// <summary>
    /// Funding Contract
    /// </summary>
    public class FundingGroup : BaseEntity
    {
        public FundingGroup(FundingOrganization name)
        {
            Guard.Argument(name, nameof(name)).NotNull();

            this.FundingSource = name;
        }

        public FundingGroup(string name)
        {
            Guard.Argument(name, nameof(name)).NotNull().NotEmpty();

            this.FundingSource = new FundingOrganization(name);
        }

        [Required]
        public FundingOrganization FundingSource { get; set; }

        private Money GetHTTotalAmount()
        {
            var subContractSum = SubContracts.Aggregate(Money.Euro(0m), (m, item) => m + item.GetHTTotalAmount());
            return PaymentSteps.Aggregate(subContractSum, (m, item) => m + item.AmountHT);
        }

        /// <summary>
        /// List of payment steps of the contracts
        /// </summary>
        /// <remarks>References (OneToMany) to Steps</remarks>
        public IList<FundingItem> PaymentSteps { get; set; } = new List<FundingItem>();


        /// <summary>
        /// List of the sub-contracts
        /// </summary>
        /// <remarks>References (OneToMany) to Contracts</remarks>
        public IList<FundingGroup> SubContracts { get; set; } = new List<FundingGroup>();
    }
}