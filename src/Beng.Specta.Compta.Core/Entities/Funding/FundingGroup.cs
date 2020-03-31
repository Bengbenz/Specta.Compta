using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Beng.Specta.Compta.Core.ValueTypes.Funding;
using Beng.Specta.Compta.SharedKernel;

using Dawn;

using NodaMoney;

namespace Beng.Specta.Compta.Core.Entities.Funding {
    public class FundingGroup : BaseEntity
    {
        public FundingGroup(FundingOrganization name)
        {
            Guard.Argument(name, nameof(name)).NotNull();

            this.Name = name;
        }

        public FundingGroup(string name)
        {
            Guard.Argument(name, nameof(name)).NotNull().NotEmpty();

            this.Name = new FundingOrganization(name);
        }

        [Required]
        public FundingOrganization Name { get; set; }

        private Money GetHTTotalAmount()
        {
            return Dotations.Aggregate(Money.Euro(0m), (m, item) => m + item.AmountHT);
        }

        // References (OneToMany)
        public IList<FundingItem> Dotations { get; set; } = new List<FundingItem>();
    }
}