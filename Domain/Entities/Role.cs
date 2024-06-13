﻿using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class Role : IdentityRole
{
    public ICollection<UserRole> Users { get; set; }
}
