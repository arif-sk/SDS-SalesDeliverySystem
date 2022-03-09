using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Caretaskr.Data;
using Caretaskr.Domain.Persistance;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SDS.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericUnitOfWork<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationContext dataContext)
        {
            DataContext = dataContext;
            _dbSet = DataContext.Set<TEntity>();
        }

        public ApplicationContext DataContext { get; }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression == null)
                return _dbSet;

            return _dbSet.Where(expression);
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("entity");
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("entity");
            if (DataContext.Entry(entity).State == EntityState.Detached) _dbSet.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }



        public void Delete(TEntity entity)
        {
            if (DataContext.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression == null)
                return await _dbSet.FirstOrDefaultAsync();
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression == null)
                return await _dbSet.SingleOrDefaultAsync();
            return await _dbSet.SingleOrDefaultAsync(expression);
        }

        public IQueryable<TEntity> GetAllIgnoreGlobalQueries(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression == null)
                return _dbSet;

            return _dbSet.IgnoreQueryFilters().Where(expression);
        }



        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<TEntity>> ListAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<List<TEntity>> ListAllFromSql(string sql, dynamic[] parems)
        {
            return await _dbSet.FromSqlRaw<TEntity>(sql, parems).IgnoreQueryFilters().ToListAsync();
        }

        public async Task<List<TEntity>> ListAsync(ISpecification<TEntity> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.ToListAsync();
        }
        public async Task<int> ToCountAsync(ISpecification<TEntity> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.CountAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.CountAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }




        public async Task<TEntity> FirstAsync(ISpecification<TEntity> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(ISpecification<TEntity> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            var evaluator = new SpecificationEvaluator();
            return evaluator.GetQuery(_dbSet.AsQueryable(), spec);
        }


    }
}