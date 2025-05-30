﻿using Microsoft.AspNetCore.Identity;
namespace CharityHub.Core.Domain.Entities.Identity;


public sealed class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole(string roleName) : base(roleName)
    {
    }

    public ApplicationRole() : base()
    {
    }
}