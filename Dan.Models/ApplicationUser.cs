using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? DisplayName { get; set; }
        public int? IdNational { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsEmailVerified { get; set; }
        public string? EmailVerificationCode { get; set; }
        public int? AccountMoto { get; set; }
        public string? File { get; set; } // Adjust as needed for file storage path
        public bool? RulesAgreed { get; set; }
        public bool? Agreement { get; set; }
        public bool? IsDeactivated { get; set; }
        public DateTime?  DateJoined { get; set; }
        public DateTime? LastOnline { get; set; }
        public bool? IsSetPassword { get; set; }
        public bool? IsNewUser { get; set; }
        public DateTime? LastActive { get; set; }
        public bool? IsNotificationSeen { get; set; }
        public bool? IsPhoneVerified { get; set; }
        public string?   PhoneCountryCode { get; set; }
        public string? PhoneCode { get; set; }
    }
}
