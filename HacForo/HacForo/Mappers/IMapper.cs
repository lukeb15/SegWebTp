using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HacForo.Mappers
{
    public interface IMapper<T,U>
    {
        U MapTo(T dbModel);
        T MapTo(U dbModel);
    }
}