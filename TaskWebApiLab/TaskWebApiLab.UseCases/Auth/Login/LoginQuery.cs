using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace TaskWebApiLab.UseCases.Auth.Login
{
    public record LoginQuery(string Username, string Password) : IQuery<Result<TokenDTO>>
    {
    }
}
