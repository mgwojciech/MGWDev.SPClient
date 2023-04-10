using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Utilities
{
    public interface IExpressionMapper
    {
        string BuildFilterQuery<T>(Expression<Func<T, bool>> predicate);
    }
}
