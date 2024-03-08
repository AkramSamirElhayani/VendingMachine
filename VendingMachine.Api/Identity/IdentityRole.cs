using Microsoft.AspNetCore.Identity;
using System;

namespace VendingMachine.Api.Identity;

public class ApplicationRole:IdentityRole<Guid>
{
    /// <summary>
    /// Initializes a new instance of <see cref="ApplicationRole`1"/>.
    /// </summary>
    public ApplicationRole()
    {

    }

    /// <summary>
    /// Initializes a new instance of <see cref="ApplicationRole`1"/>.
    /// </summary>
    /// <param name="roleName">The role name.</param>
    public ApplicationRole(string roleName) : base(roleName)
    {

    }
}
