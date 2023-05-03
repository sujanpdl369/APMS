using APMS.Common.Models;

namespace APMS.Common.ViewModel
{
    public class ProfileImageVM
    {
        public ProfileImageVM(Register register)
        {
            Image = register.ProfilePicture;
        }
        public byte[]? Image { get; set; }
    }
}
