using System;
using System.Collections.Generic;
using System.Text;

namespace newsscraper.DAL
{
    using Microsoft.EntityFrameworkCore;

    public interface IScraperContext
    {
        DbSet<Entities.Article> Articles { get; }
    }
}
