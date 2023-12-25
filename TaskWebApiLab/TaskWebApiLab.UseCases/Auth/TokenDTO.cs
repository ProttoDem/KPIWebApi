using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWebApiLab.UseCases.Auth
{
    public record TokenDTO(string Token, DateTime Expiration);
}
