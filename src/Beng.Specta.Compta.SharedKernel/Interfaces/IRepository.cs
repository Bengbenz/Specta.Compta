﻿using System.Collections.Generic;

namespace Beng.Specta.Compta.SharedKernel.Interfaces
{
    public interface IRepository
    {
        T GetById<T>(long id) where T : BaseEntity;
        List<T> List<T>() where T : BaseEntity;
        T Add<T>(T entity) where T : BaseEntity;
        void Update<T>(T entity) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
    }
}