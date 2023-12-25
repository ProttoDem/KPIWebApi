using Ardalis.Result;
using Ardalis.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskWebApiLab.UseCases.Auth;

namespace TaskWebApiLab.UseCases.Goals.Get
{
    public record GetGoalQuery(int id) : IQuery<Result<GoalDTO>>;
}
