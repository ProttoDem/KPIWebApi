﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TaskWebApiLab.Core.Interfaces
{
    public interface IUserAccessor { ClaimsPrincipal User { get; } }
}
