using Ardalis.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskWebApiLab.Core.GoalAggregate;

namespace TaskWebApiLab.Core.CategoryAggregate
{
    public class Category(string title, string? description) : EntityBase, IAggregateRoot
    {
        public string Title { get; set; } = title;
        public string? Description { get; set; } = description;
    }
}
