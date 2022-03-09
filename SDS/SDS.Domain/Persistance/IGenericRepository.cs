using Ardalis.Specification;
using System.Linq.Expressions;

namespace Caretaskr.Domain.Persistance
{
    public interface IGenericUnitOfWork<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression = null);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);    
        TEntity GetById(int id);
        //Task<TEntity> FirstOrDefault();
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression=null);
        Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetAllIgnoreGlobalQueries(Expression<Func<TEntity, bool>> expression = null);

        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> ListAllAsync();
        Task<List<TEntity>> ListAllFromSql(string sql, dynamic[] parems);
        Task<List<TEntity>> ListAsync(ISpecification<TEntity> spec);
        Task<int> ToCountAsync(ISpecification<TEntity> spec);
        //Task<TEntity> AddAsync(TEntity entity);
        //Task UpdateAsync(TEntity entity);
        //Task DeleteAsync(TEntity entity);
        Task<int> CountAsync(ISpecification<TEntity> spec);
        Task<TEntity> FirstAsync(ISpecification<TEntity> spec);
        Task<TEntity> FirstOrDefaultAsync(ISpecification<TEntity> spec);
        IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec);
    }
}