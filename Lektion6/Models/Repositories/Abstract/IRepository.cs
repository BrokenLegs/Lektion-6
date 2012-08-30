using System;
using System.Collections.Generic;
using Lektion6.Models.Entities.Abstract;

namespace Lektion6.Models.Repositories.Abstract
{
    /// <summary>
    /// Interface för Repository
    /// </summary>
    public interface IRepository
    {
        List<T> All<T>() where T : IEntity;
        T Get<T>(Guid id) where T : IEntity;
        void Delete<T>(T t) where T : IEntity;
        void Save<T>(T t) where T : IEntity;
    }
}
