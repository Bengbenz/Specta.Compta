using System.ComponentModel.DataAnnotations;
using Beng.Specta.Compta.Core.ValueTypes.Funding;
using Beng.Specta.Compta.SharedKernel;

using NodaMoney;

namespace Beng.Specta.Compta.Core.Entities.Funding;

/// <summary>
/// Funding Contract
/// </summary>
public sealed class FundingContract : BaseEntity
{
    public FundingContract(FundingOrganization name)
    {
        FundingPartner = name ?? throw new ArgumentNullException(nameof(name));
    }

    public FundingContract(string name) : this(new FundingOrganization(name))
    {
    }

    [Required]
    public FundingOrganization FundingPartner { get; set; }

    [Required]
    public Money AmountHT { get; set; } = default!;

    public Money GetHTTotalFromContracts()
    {
        var subContractSum = SubContracts.Aggregate(Money.Euro(0m), (m, item) => m + item.GetHTTotalFromContracts());
        return PaymentSteps.Aggregate(subContractSum, (m, item) => m + item.AmountHT);
    }

    /// <summary>
    /// List of payment steps of the contracts
    /// </summary>
    /// <remarks>References (OneToMany) to Steps</remarks>
    public ICollection<ContractStep> PaymentSteps { get; private set; } = new List<ContractStep>();
    
    /// <summary>
    /// List of the sub-contracts
    /// </summary>
    /// <remarks>References (OneToMany) to Contracts</remarks>
    public ICollection<FundingContract> SubContracts { get; private set; } = new List<FundingContract>();
}