using System;
using System.Data;
using System.Threading.Tasks;

namespace Caretaskr.Domain.Persistance
{
    public interface IGenericUnitOfWork : IDisposable
    {
        IGenericUnitOfWork<TEntity> GetRepository<TEntity>() where TEntity : class;
        Task ExecuteNonQuery(string commandText, params object[] parameters);
        //void SaveChanges();
        Task SaveChangesAsync();
        void UndoChanges();
    }
}