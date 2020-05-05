// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;

namespace Beng.Specta.Compta.Core.Entities.Security.Permissions
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LinkedToModuleAttribute : Attribute
    {
        public PaidForModule PaidForModule { get; private set; }

        public LinkedToModuleAttribute(PaidForModule paidForModule)
        {
            PaidForModule = paidForModule;
        }
    }
}