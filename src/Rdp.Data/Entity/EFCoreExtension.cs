using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rdp.Data.Entity
{
    public static class EFCoreExtension
    {
        public static Task<List<TSource>> ToListAsync<TSource>(this IQueryable<TSource> source)
        {
            return EntityFrameworkQueryableExtensions.ToListAsync(source);
        }

        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source)
        {
            return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(source);
        }


        public static Task<int> CountAsync<TSource>(this IQueryable<TSource> source)
        {
            return EntityFrameworkQueryableExtensions.CountAsync(source);
        }
    }
}
