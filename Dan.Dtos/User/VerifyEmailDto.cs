﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Dtos.User
{
    public class VerifyEmailDto
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}
