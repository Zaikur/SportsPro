﻿/*
 * Jason Nelson
 * 04/14/2024
 * Added Repository interface for data encapsulation
 */

namespace SportsPro.Data.DataLayer.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> List(QueryOptions<T> options);

        int Count { get; }  // read-only property

        // overloaded Get() method
        T? Get(QueryOptions<T> options);
        T? Get(int id);
        T? Get(string id);

        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);

        void Save();
    }
}
