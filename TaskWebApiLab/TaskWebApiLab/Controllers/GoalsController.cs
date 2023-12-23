using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using TaskWebApiLab.ApiModels;
using TaskWebApiLab.Auth;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Specifications;
using TaskWebApiLab.UnitOfWork;
using System.Net;

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

        public GoalsController(UserManager<IdentityUser> userManager, IGoalRepository goalRepository)
        {
            //_context = context;
            this._goalRepository = goalRepository;
            _userManager = userManager;
        }

        // GET: api/Goals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Goal>>> GetGoals()
        {
            return await _goalRepository.GetGoals();
        }
        
        // GET: api/Goals
        [HttpGet("goals-by-status")]
        public async Task<ActionResult<IEnumerable<Goal>>> GetGoalsSortByStatus([FromQuery]Status? minimumStatus, [FromQuery] Status? maximumStatus)
        {
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

        // GET: api/Goals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Goal>> GetGoal(int id)
        {
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
            if (id != goal.Id)
            {
                return BadRequest();
            }
            _goalRepository.UpdateGoal(goal);

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
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userId = user.Id;
            //var user = await _context.Users.Where(x => x.UserName == User.Identity.Name).SingleAsync();

            var goalData = new Goal
            {
                CategoryId = goal.CategoryId,
                CreatedAt = DateTime.Now,
                Description = goal.Description,
                ParentTaskId = goal.ParentTaskId ?? 0,
                Status = goal.Status,
                Title = goal.Title,
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
            var goal = _goalRepository.GetGoalByID(id);
            if (goal == null)
            {
                return NotFound();
            }

            _goalRepository.DeleteGoal(id);
            _goalRepository.Save();

            return NoContent();
        }

        private bool GoalExists(int id)
        {
            if (_goalRepository.GetGoalByID(id) == null)
            {
                return false;
            } 
            return true;
        }
    }
}
