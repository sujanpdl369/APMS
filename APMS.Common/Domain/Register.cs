using APMS.Common.Domain;
using APMS.Common.Enum;

namespace APMS.Common.Models;

public partial class Register : BaseEntity
{
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DOB { get; set; }
    public string UserName { get; set; } = null!;
    public int MobileNumber { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Gender Gender { get; set; }
    public string Location { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Status {get; set; }
    
}
