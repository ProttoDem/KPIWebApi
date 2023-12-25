using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace TaskWebApiLab.UseCases.Auth.Register
{
    public class RegisterHandler(UserManager<IdentityUser> userManager) : ICommandHandler<RegisterCommand, Result<RegisterMessageDTO>>
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        public async Task<Result<RegisterMessageDTO>> Handle(RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByNameAsync(request.Username);
            if (userExists != null)
                return new RegisterMessageDTO("Error", "User already exists!");

            IdentityUser user = new()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Username
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return new RegisterMessageDTO("Error", "User creation failed! Please check user details and try again.");

            return new RegisterMessageDTO("Success", "User created successfully!");
        }
    }
}
