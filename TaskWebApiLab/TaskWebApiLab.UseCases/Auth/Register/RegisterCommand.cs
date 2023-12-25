using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWebApiLab.UseCases.Auth.Register
{
    public record RegisterCommand(string Username, string Password, string Email) : Ardalis.SharedKernel.ICommand<Result<RegisterMessageDTO>>;
}
