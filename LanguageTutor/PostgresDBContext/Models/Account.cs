﻿using System;
using System.Collections.Generic;

namespace DBContext.Models
{
    public partial class Account
    {
        public int IdAccount { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime? DateRegistration { get; set; }
    }
}