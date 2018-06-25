using System;
using System.Collections.Generic;
using System.Text;

namespace newsscraper.DAL
{
    using Microsoft.EntityFrameworkCore;
    using System.Data;

    internal static class RawSqlExtensions
    {
        public static IEnumerable<T> RawQuery<T>(this ScraperContext ctx, string sql, Func<System.Data.IDataReader,T> map, params Tuple<string, string>[] parameters)
        {
            using (var command = ctx.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                foreach (var param in parameters)
                {
                    var p = command.CreateParameter();
                    p.DbType = DbType.String;
                    p.ParameterName = param.Item1;
                    p.Value = param.Item2;
                    command.Parameters.Add(p);
                }

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
