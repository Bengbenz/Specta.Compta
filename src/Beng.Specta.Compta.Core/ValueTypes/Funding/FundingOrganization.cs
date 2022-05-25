using System.ComponentModel.DataAnnotations;

using Beng.Specta.Compta.SharedKernel;

namespace Beng.Specta.Compta.Core.ValueTypes.Funding;

public sealed class FundingOrganization : ValueObject
{
    public FundingOrganization (string name, string description = "")
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
    }

    [Required]
    public string Name { get; set; }

    [IgnoreMember]
    public string Description { get; set; }

    public override string ToString () => Name;
}