﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaHipHop_Model.DTO.Request
{
    public class CreateContactRequest
    {
        [RegularExpression("^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "Số điện thoại này fake!")]
        [Required(ErrorMessage = "Số điện thoại là trường bắt buộc phải nhập.")]
        public string Phone { get; set; }
        public required string Email { get; set; }
        public required string Facebook { get; set; }
        public required string Instagram { get; set; }
        public required string Tiktok { get; set; }
        public required string Shoppee { get; set; }
    }
}
