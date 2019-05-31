﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FileManager.Models
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public Guid DepartamentID { get; set; }
        public Departament Departament { get; set; }
    }
}
