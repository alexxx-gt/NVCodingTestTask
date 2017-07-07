using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVCodingTestTask.Models
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        IEnumerable<T> ListUsers();
        T GetUser(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        T GetLast();
    }
}
