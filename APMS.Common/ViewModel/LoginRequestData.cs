using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APMS.Common.ViewModel
{
    public class LoginRequestData
    {
        public RegisterVM register { get; set; }
        public string auth_token { get; set; }

    }
}
