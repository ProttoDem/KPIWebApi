using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using TaskWebApiLab.ApiModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Specifications;
using TaskWebApiLab.UnitOfWork;
using System.Net;
using System.Security.Claims;

namespace TaskWebApiLab.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IGoalRepository _goalRepository;
        private readonly string _userName;

        public GoalsController(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, IGoalRepository goalRepository)
        {
            _userName = httpContextAccessor.HttpContext.User.Identity.Name;
            this._goalRepository = goalRepository;
            _userManager = userManager;
        }

        // GET: api/Goals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GoalModel>>> GetGoals()
        {
            await SetUser();
            List<Goal> goals = await _goalRepository.GetGoals();
            
            if (!goals.Any())
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return StatusCode((int)HttpStatusCode.OK, new { totalCount = goals.Count, goals });
        }

        // GET: api/Goals/goals-by-status
        [HttpGet("goals-by-status")]
        public async Task<ActionResult<IEnumerable<Goal>>> GetGoalsSortByStatus([FromQuery]Status? minimumStatus, [FromQuery] Status? maximumStatus)
        {
            await SetUser();
            ExpressionSpecification<Goal> specification = null;

            if (maximumStatus.HasValue)
            {

                specification = new LessThanSpecification(maximumStatus.Value);
            }

            if (minimumStatus.HasValue)
            {
                var biggerThanSpecification = new BiggerThanSpecification(minimumStatus.Value);
                specification = specification?.And(biggerThanSpecification) ?? biggerThanSpecification;
            }

            List<Goal> goals = _goalRepository.GetGoals(specification).ToList();

            if (!goals.Any())
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return StatusCode((int)HttpStatusCode.OK, new { totalCount = goals.Count, goals });
        }
        
        // GET: api/Goals/goals-by-status
        [HttpGet("goals-by-status-category")]
        public async Task<ActionResult<IEnumerable<Goal>>> GetGoalsSortByStatusCategory([FromQuery]Status? minimumStatus, [FromQuery] Status? maximumStatus, [FromQuery]int? categoryId)
        {
            await SetUser();
            ExpressionSpecification<Goal> specification = null;

            if (maximumStatus.HasValue)
            {

                specification = new LessThanSpecification(maximumStatus.Value);
            }

            if (minimumStatus.HasValue)
            {
                var biggerThanSpecification = new BiggerThanSpecification(minimumStatus.Value - 1);
                specification = specification?.And(biggerThanSpecification) ?? biggerThanSpecification;
            }
            
            if (categoryId.HasValue)
            {
                var inCategorySpecification = new InCategorySpecification(categoryId.Value + 1);
                specification = specification?.And(inCategorySpecification) ?? inCategorySpecification;
            }

            List<Goal> goals = null;
            if (specification != null)
            {
                goals = _goalRepository.GetGoals(specification).ToList();
            }
            else
            {
                goals = await _goalRepository.GetGoals();
            }

            if (!goals.Any())
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return StatusCode((int)HttpStatusCode.OK, new { totalCount = goals.Count, goals });
        }
        
        // GET: api/Goals/goals-by-status
        [HttpGet("goals-by-category")]
        public async Task<ActionResult<IEnumerable<GoalModel>>> GetGoalsSortByCategory([FromQuery]int? categoryId)
        {
            await SetUser();
            ExpressionSpecification<Goal> specification = null;

            if (categoryId.HasValue)
            {

                specification = new InCategorySpecification(categoryId.Value);
            }

            List<Goal> goals = _goalRepository.GetGoals(specification).ToList();

            if (!goals.Any())
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return StatusCode((int)HttpStatusCode.OK, new { totalCount = goals.Count, goals });
        }

        // GET: api/Goals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Goal>> GetGoal(int id)
        {
            await SetUser();
            var goal = _goalRepository.GetGoalByID(id);

            if (goal == null)
            {
                return NotFound();
            }

            return goal;
        }
        
        // GET: api/Goals/5
        [HttpGet("child-goals/{id}")]
        public async Task<ActionResult<IEnumerable<Goal>>> GetChildGoals(int id)
        {
            await SetUser();
            var childGoals = await _goalRepository.GetChildGoals(id);

            if (childGoals.IsNullOrEmpty())
            {
                return NotFound();
            }

            return childGoals;
        }

        // PUT: api/Goals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGoal(int id, Goal goal)
        {
            await SetUser();
            if (id != goal.Id)
            {
                return BadRequest();
            }
            _goalRepository.UpdateGoal(id, goal);

            try
            {
                _goalRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Goals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GoalModel>> PostGoal([FromBody]GoalModel goal)
        {
            await SetUser();
            var user = await _userManager.FindByNameAsync(_userName);
            var userId = user.Id;

            var goalData = new Goal
            {
                CategoryId = goal.CategoryId,
                CreatedAt = DateTime.Now,
                Description = goal.Description,
                ParentTaskId = goal.ParentTaskId ?? 0,
                Status = goal.Status,
                Title = goal.Title,
                DueTime = goal.DueTime,
                UserId = userId
            };

            _goalRepository.InsertGoal(goalData);
            _goalRepository.Save();

            return CreatedAtAction("GetGoal", new { id = goalData.Id }, goalData);
        }

        // DELETE: api/Goals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoal(int id)
        {
            await SetUser();
            var goal = _goalRepository.GetGoalByID(id);
            if (goal == null)
            {
                return NotFound();
            }

            _goalRepository.DeleteGoal(id);
            _goalRepository.Save();

            return NoContent();
        }

        [NonAction]
        public bool GoalExists(int id)
        {
            if (_goalRepository.GetGoalByID(id) == null)
            {
                return false;
            } 
            return true;
        }

        private async Task SetUser()
        {
            var user = await _userManager.FindByNameAsync(_userName);
            var userId = user.Id;
            _goalRepository.SetCurrentUser(userId);
        }
    }
}
