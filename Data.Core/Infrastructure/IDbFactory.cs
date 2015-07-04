using Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Core.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        DIContext Init();
    }
}
