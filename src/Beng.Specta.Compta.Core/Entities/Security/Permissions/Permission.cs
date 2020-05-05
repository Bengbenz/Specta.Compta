// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;

namespace Beng.Specta.Compta.Core.Entities.Security.Permissions
{
    public enum Permission : short //I set this to short because the PermissionsPacker stores them as Unicode chars 
    {
        NotSet = 0, // error condition

        //See the Permissions.cs in the PermissionAccessControl2 repo for an example of how to define permissions 
        //see https://www.thereformedprogrammer.net/a-better-way-to-handle-authorization-in-asp-net-core/

        [Display(GroupName = "Project", Name = "Read", Description = "Can read project")]
        ProjectRead,

        [Display(GroupName = "Project", Name = "Add new", Description = "Can add a new project item and update also")]
        ProjectAddNew,

        [Display(GroupName = "Project", Name = "Update existing project", Description = "Can update a existing project")]
        ProjectUpdate,

        [Display(GroupName = "Project", Name = "Remove", Description = "Can remove project data")]
        ProjectRemove,

        [Display(GroupName = "Project", Name = "All access to Project", Description = "Can do anything to the Project: Can create, update or delete.")]
        ProjectAllAccess,

        [Display(GroupName = "Employees", Name = "Read", Description = "Can read company employees")]
        EmployeeRead,

        [Display(GroupName = "UserAdmin", Name = "Read users", Description = "Can list User")]
        UserRead,

        //This is an example of grouping multiple actions under one permission
        [Display(GroupName = "UserAdmin", Name = "All access to User", Description = "Can do anything to the User: Can create, update or delete.")]
        UserAllAccess,

        [Display(GroupName = "UserAdmin", Name = "Read Roles", Description = "Can list Role")]
        RoleRead,

        [Display(GroupName = "UserAdmin", Name = "All access to Role", Description = "Can do anything to the Role: Can create, update or delete a Role")]
        RoleAllAccess,

        //This is a special Permission used by the SuperAdmin user. 
        //A user has this permission can access any other permission.
        [Display(GroupName = "SuperAdmin", Name = "Access All", Description = "This allows the user to access every feature")]
        AccessAll = Int16.MaxValue, 
    }
}