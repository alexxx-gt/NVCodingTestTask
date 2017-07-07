using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NVCodingTestTask.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private UserContext db;
        private IRepository<User> userRepository;
        private bool disposed = false;

        public UnitOfWork()
        {
            db = new UserContext();
        }

        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(db);
                }

                return userRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }

                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}