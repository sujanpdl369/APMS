
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
            Title = register.Title;
            FirstName = register.FirstName;
            LastName = register.LastName;
            DOB = register.DOB;
            UserName = register.UserName;
            MobileNumber = register.MobileNumber;
            Email = register.Email;
            Password = register.Password;
            Location = register.Location;
            CreatedDate = register.CreatedDate;
        }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string UserName { get; set; } = null!;

        public int MobileNumber { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Location { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
    }
}
