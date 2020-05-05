// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;

namespace Beng.Specta.Compta.Core.Entities.Security.Permissions
{
    /// <summary>
    /// This is an example of how you would manage what optional parts of your system a user can access
    /// NOTE: You can add Display attributes (as done on Permissions) to give more information about a module
    /// </summary>
    [Flags]
    public enum PaidForModule : long
    {
        None = 0,
        Dashboard,
        Project,
        Funding,
        Budget,
        Accounting,
        Treasury,
        Admin
    }
}