using ClassificationService.Exceptions;
using ClassificationService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClassificationService.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly IMongoCollection<T> _collection;

        /// <summary>
        /// Initialize a new instance of <see cref="Repository{T}"/> class
        /// </summary>
        /// <param name="database"></param>
        public Repository(IMongoDatabase database)
            : this(database, typeof(T).Name)
        {
        }

        /// <summary>
        /// Initialize a new instance of <see cref="Repository{T}"/> class
        /// </summary>
        /// <param name="database"></param>
        /// <param name="collectionName"></param>
        public Repository(IMongoDatabase database, string collectionName)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database), "value can not be null");
            }

            _collection = database.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public async Task<bool> Add(T entity)
        {
            try
            {
              await  _collection.InsertOneAsync(entity);
            }
            catch (MongoWriteException e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(i => i.Id, id);
            var result = _collection.Find(filter).FirstOrDefaultAsync();
            return await result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> ListAsync()
        {
            var result = _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
            return await result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public async Task<bool> Delete(string id)
        {
            var result = await _collection.DeleteOneAsync(d => d.Id == id);
            return result.IsAcknowledged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public async Task<bool> Update(T entity)
        {
            var result = await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq(d => d.Id, entity.Id), entity);
            return result.IsAcknowledged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate)
        {
            var result = _collection.Find(predicate).ToListAsync();
            return await result;
        }
    }
    }
