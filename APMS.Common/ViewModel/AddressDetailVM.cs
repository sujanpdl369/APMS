using APMS.Common.Enum;
using APMS.Common.Models;
using Nager.Date.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APMS.Common.ViewModel
{
    public class AddressDetailVM
    {
        public AddressDetailVM()
        {
            
        }
        public AddressDetailVM(Register register)
        {
            Country = register.Country;
            City = register.City;
            State = register.State;
            Status = register.Status;
            gender = register.Gender.ToString();
        }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public string gender { get; set; }
    }
}
