
using System.ComponentModel.DataAnnotations;

using Beng.Specta.Compta.SharedKernel;

using Dawn;

namespace Beng.Specta.Compta.Core.ValueTypes.Funding
{
    public class FundingOrganization : ValueObject
    {
        public FundingOrganization (string name, string description = "")
        {
            Guard.Argument(name, nameof(name)).NotNull();

            this.Name = name;
            this.Description = description;
        }

        [Required]
        public string Name { get; set; }

        [IgnoreMemberAttribute]
        public string Description { get; set; }

        public override string ToString () => Name;
    }
}