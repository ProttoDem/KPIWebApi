using Ardalis.Result;
using Ardalis.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWebApiLab.UseCases.Goals.GetAll
{
    public record GetAllGoalsQuery() : IQuery<Result<IEnumerable<GoalDTO>>>;
}
