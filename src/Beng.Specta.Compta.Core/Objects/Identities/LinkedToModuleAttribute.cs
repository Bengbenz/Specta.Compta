using System;

namespace Beng.Specta.Compta.Core.Objects.Identities
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