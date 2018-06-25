using System;
using System.Collections.Generic;
using System.Text;

namespace newsscraper.DAL.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ScraperContext _ctx;

        public UnitOfWork(ScraperContext ctx)
        {
            if (ctx == null)
                throw new ArgumentNullException(nameof(ctx));
            _ctx = ctx;
        }

        public void Dispose()
        {
        }

        public void SaveChanges()
        {
            _ctx.SaveChanges();
        }
    }
}
