using System.ComponentModel.DataAnnotations;
using Beng.Specta.Compta.SharedKernel;

namespace Beng.Specta.Compta.Core.Entities;

/// <summary>
/// Projects
/// </summary>
public sealed class Project : BaseEntity
{
    private Project() { } //needed by EF Core
        
    public Project(string code, string name)
    {
        Code = code;
        Name = name;
    }

    [Required(AllowEmptyStrings = false)]
    public string Code { get; set; } = null!;

    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = null!;

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