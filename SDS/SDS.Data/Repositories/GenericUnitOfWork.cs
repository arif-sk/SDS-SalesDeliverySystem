using System;
using System.Linq;
using System.Threading.Tasks;
using Caretaskr.Common.Exceptions;
using Caretaskr.Data;
using Caretaskr.Domain.Persistance;
using Microsoft.EntityFrameworkCore;
using SDS.Data;

namespace SDS.Data.Repositories
{
    public class GenericUnitOfWork : IGenericUnitOfWork
    {
        protected ApplicationContext _context;

        public GenericUnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public IGenericUnitOfWork<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new GenericRepository<TEntity>(_context);
        }

        public void UndoChanges()
        {
            var context = _context;
            var changedEntries = context.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }

            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                throw new DBException(ex);
            }
        }

        /// <summary>
        ///     Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
        }

        public async Task ExecuteNonQuery(string commandText, params object[] parameters)
        {
            await _context.ExecuteNonQuery(commandText, parameters);
        }
    }
    public class OfflineGenericUnitOfWork : GenericUnitOfWork
    {
        public OfflineGenericUnitOfWork(ApplicationContext applicationContext) : base(applicationContext)
        {

        }
        //public void SetLoggedInUser(int userId)
        //{
        //    this._context._loggedInUserId = userId;
        //}

    }
}