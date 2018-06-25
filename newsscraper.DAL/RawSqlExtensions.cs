using System;
using System.Collections.Generic;
using System.Text;

namespace newsscraper.DAL
{
    using Microsoft.EntityFrameworkCore;
    using System.Data;

    internal static class RawSqlExtensions
    {
        public static IEnumerable<T> RawQuery<T>(this ScraperContext ctx, string sql, Func<System.Data.IDataReader,T> map)
        {
            using (var command = ctx.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                ctx.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();
                    while (result.Read())
                    {
                        var entity = map(result);
                        entities.Add(entity);
                    }

                    return entities;
                }
            }
        }
    }
}
