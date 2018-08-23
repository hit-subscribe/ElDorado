using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Domain
{
    public interface IHaveIdentity
    {
        int Id { get; set; }
    }
}
