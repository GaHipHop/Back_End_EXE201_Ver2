﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaHipHop_Model.DTO.Request
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }
    }
}
