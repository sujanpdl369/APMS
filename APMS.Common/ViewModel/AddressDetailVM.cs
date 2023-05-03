using APMS.Common.Models;
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
        }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Status { get; set; }

    }
}
