using System.ComponentModel.DataAnnotations;

namespace Beng.Specta.Compta.Core.Dtos;

// Note: doesn't expose events or behavior
public class ProjectDto : BaseDto
{
    public ProjectDto ()
    {
    }
        
    public ProjectDto (ProjectDto value)
    {
        Id = value.Id;
        Code = value.Code;
        Name = value.Name;
        Description = value.Description;
        StartDate = value.StartDate;
        Duration = value.Duration;
    }

    [Required]
    public string Code { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

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