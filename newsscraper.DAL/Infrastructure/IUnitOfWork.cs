using System;
using System.Collections.Generic;
using System.Text;

namespace newsscraper.DAL.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
