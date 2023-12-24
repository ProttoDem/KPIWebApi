using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host;
using System.Net;
using TaskWebApiLab.ApiModels;
using TaskWebApiLab.UnitOfWork;

namespace TaskWebApiLab.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IGoalRepository _goalRepository;
        private readonly string _userName;

        public NotificationsController( UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, IGoalRepository goalRepository)
        {
            _userName = httpContextAccessor.HttpContext.User.Identity.Name;
            _userManager = userManager;
            _goalRepository = goalRepository;
        }

        // GET: api/notifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationModel>>> GetGoalsTime([FromQuery] int hours)
        {
            await SetUser();
            List<Goal> goals = await _goalRepository.GetGoals();

            // Calculate the threshold time
            DateTime thresholdTime = DateTime.Now.AddHours(hours);

            // Filter goals that are due within the specified time frame
            var notifications = goals.Where(g => g.DueTime.HasValue && g.DueTime.Value <= thresholdTime && g.Status != Status.Done)
                .Select(g => new NotificationModel
                {
                    DueTime = g.DueTime.Value,
                    GoalId = g.Id
                })
                .ToList();

            return notifications;
        }

        [NonAction]
        private async Task SetUser()
        {
            var user = await _userManager.FindByNameAsync(_userName);
            var userId = user.Id;
            _goalRepository.SetCurrentUser(userId);
        }

    }


}
