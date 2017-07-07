using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NVCodingTestTask.Models
{
    public class UserRepository : IRepository<User>
    {
        private UserContext db;
        private bool disposed;

        public UserRepository(UserContext context)
        {
            db = context;
        }

        public virtual void Dispose(bool disposed)
        {
            if (!this.disposed)
            {
                if (disposed)
                {
                    db.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<User> ListUsers()
        {
            return db.Users;
        }

        public User GetUser(int id)
        {
            return db.Users.Find(id);
        }

        public void Create(User item)
        {
            db.Users.Add(item);
        }

        public void Update(User item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            User user = db.Users.Find(id);

            if (user != null)
            {
                db.Users.Remove(user);
            }
        }

        public User GetLast()
        {
            return db.Users.OrderByDescending(m => m.Id).FirstOrDefault();
        }
    }
}