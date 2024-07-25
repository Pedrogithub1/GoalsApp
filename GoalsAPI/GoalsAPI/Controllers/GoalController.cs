using AutoMapper;
using GoalsAPI.Data.Entities;
using GoalsAPI.Models.GoalDtos;
using GoalsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoalsAPI.Controllers
{
    [Route("api/goals")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        readonly IGoalService _goalService;
        readonly IMapper _mapper;
        public GoalController(IGoalService goalService,
            IMapper mapper)
        {
            _goalService = goalService ??
                throw new ArgumentNullException(nameof(goalService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GoalDto>>> GetGoals(bool includeTaskItems = false)
        {
            var goalEntity = await _goalService.GetGoalsAsync(includeTaskItems);
            return Ok(_mapper.Map<IEnumerable<GoalDto>>(goalEntity));
        }

        [HttpGet("{goalId}", Name = "GetGoal")]
        public async Task<ActionResult> GetGoal(int goalId, bool includeTaskItems = false)
        {
            var goal = await _goalService
                .GetGoalAsync(goalId, includeTaskItems);
            if (goal == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GoalDto>(goal));
        }

        [HttpPost]
        public async Task<ActionResult<GoalDto>> CreateGoal(
            GoalForCreationDto goal)
        {

            if (await _goalService.GoalNameExistsAsync(goal.Name))
            {
                return BadRequest("Ya existe una meta con ese nombre!");
            }

            var finalGoal = _mapper.Map<Goal>(goal);
            _goalService.AddGoal(finalGoal);
            await _goalService.SaveChangesAsync();

            var createGoalToReturn =
                _mapper.Map<GoalDto>(finalGoal);

            return CreatedAtRoute("GetGoal",
                new
                {
                    goalId = createGoalToReturn.GoalId
                },
                createGoalToReturn);
        }


        [HttpPut("{goalID}")]
        public async Task<ActionResult> UpdateGoal(int goalId,
            GoalForUpdateDto goal)
        {
            var goalEntity = await _goalService.GetGoalAsync(goalId);
            if (goalEntity == null)
            {
                return NotFound();
            }

            if (goalEntity.Name != goal.Name && await _goalService.GoalNameExistsAsync(goal.Name))
            {
                return BadRequest("Ya existe una meta con ese nombre!");
            }

            _mapper.Map(goal, goalEntity);
            await _goalService.SaveChangesAsync();

            return Ok();

        }


        [HttpDelete("{goalId}")]
        public async Task<ActionResult> DeleteGoal(int goalId)
        {
            var goalEntity = await _goalService.GetGoalAsync(goalId);
            if (goalEntity == null)
            {
                return NotFound();
            }

            _goalService.DeleteGoal(goalEntity);
            await _goalService.SaveChangesAsync();

            return Ok();
        }
    }
}
