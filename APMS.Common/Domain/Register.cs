using System.ComponentModel.DataAnnotations;

namespace APMS.Common.Models;

public partial class Register
{
    [Key]
    public int Id { get; set; } 
    public string UserName { get; set; } = null!;

    public int MobileNumber { get; set; } 

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime CreatedDate { get; set; } 
}
