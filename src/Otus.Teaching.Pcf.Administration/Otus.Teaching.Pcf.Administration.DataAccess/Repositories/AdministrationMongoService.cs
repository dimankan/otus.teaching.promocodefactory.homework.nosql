using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.Core.Abstractions.Repositories;
using Otus.Teaching.Pcf.Administration.Core.Abstractions.Settings;
using Otus.Teaching.Pcf.Administration.Core.Domain;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.Pcf.Administration.DataAccess.Repositories
{
    public class AdministrationMongoService<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _entities;

        public AdministrationMongoService(IAdministrationMongoDatabaseSettings settings)
        {
            string collectionName = null;

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            if (typeof(T) == typeof(Employee))
            {
                collectionName = settings.EmployeesCollectionName;
            }

            if (typeof(T) == typeof(Role))
            {
                collectionName = settings.RolesCollectionName;
            }

            _entities = database.GetCollection<T>(collectionName);

        }

        public async Task AddAsync(T entity)
        {
            await _entities.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            await _entities.DeleteOneAsync(e => e.Id == entity.Id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var s = typeof(T).Name;
            return (await _entities.FindAsync(e => true)).ToEnumerable();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return (await _entities.FindAsync(e => e.Id == id)).FirstOrDefault();
        }

        public async Task<T> GetFirstWhere(Expression<Func<T, bool>> predicate)
        {
            return (await _entities.FindAsync(predicate)).FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            return (await _entities.FindAsync(e => ids.Contains(e.Id))).ToEnumerable();
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return (await _entities.FindAsync(predicate)).ToEnumerable();
        }

        public async Task UpdateAsync(T entity)
        {
            await _entities.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }
    }
}
