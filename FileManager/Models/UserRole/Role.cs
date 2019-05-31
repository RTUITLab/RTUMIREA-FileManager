﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FileManager.Models
{
    public class Role:IdentityRole<Guid>
    {
        public List<RoleStatus> RoleStatuses { get; set; }

        public Role(string name):base(name)
        {
        }
    }
}
