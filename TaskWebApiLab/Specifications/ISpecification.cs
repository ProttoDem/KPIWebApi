using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications
{
    public interface ISpecification<in T>
    {
        bool IsSatisfied(T obj);
    }
}
