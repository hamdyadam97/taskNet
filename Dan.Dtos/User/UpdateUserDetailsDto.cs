using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Dtos.User
{
    public class UpdateUserDetailsDto
    {
        public string DisplayName { get; set; }
        public int? IdNational { get; set; }
        public string PhoneNumber { get; set; }
        public int? AccountMoto { get; set; }
        public string File { get; set; } // Adjust as needed for file storage path
        public bool? RulesAgreed { get; set; }
        public bool? Agreement { get; set; }
        public string PhoneCountryCode { get; set; }
    }
}
