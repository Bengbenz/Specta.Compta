﻿using Beng.Specta.Compta.Core.Entities;

namespace Beng.Specta.Compta.Infrastructure.Data.Config
{
    public class ProjectConfiguration : BaseEntityConfiguration<Project>
    {
        protected override void Configure()
        {
            Builder.HasIndex(p => p.Code)
                   .IsUnique();
        }
    }
}
