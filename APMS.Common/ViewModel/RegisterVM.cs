
using APMS.Common.Models;

namespace APMS.Common.ViewModel
{
    public class RegisterVM
    {
        public RegisterVM()
        {
            
        }
        public RegisterVM(Register register)
        {
            UserName = register.UserName;
            MobileNumber = register.MobileNumber;
            Email = register.Email;
            Password = register.Password;
            Location = register.Location;
            CreatedDate = register.CreatedDate;
        }
        public string UserName { get; set; } = null!;

        public int MobileNumber { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Location { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
    }
}
