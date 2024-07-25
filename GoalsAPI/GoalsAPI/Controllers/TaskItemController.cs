using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GoalsAPI.Services;
using GoalsAPI.Models.TaskItemDtos;
using GoalsAPI.Data.Entities;

namespace TaskItemsAPI.Controllers
{
    [Route("api/taskItems")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        readonly IGoalService _goalService;
        readonly IMapper _mapper;
        public TaskItemController(IGoalService goalService,
            IMapper mapper)
        {
            _goalService = goalService ??
                throw new ArgumentNullException(nameof(goalService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTaskItems()
        {
            var taskItemEntity = await _goalService.GetTaskItemsAsync();
            return Ok(_mapper.Map<IEnumerable<TaskItemDto>>(taskItemEntity));
        }

        [HttpGet("{taskItemId}", Name = "GetTaskItem")]
        public async Task<ActionResult> GetTaskItem(int taskItemId)
        {
            var taskItem = await _goalService
                .GetTaskItemAsync(taskItemId);
            if (taskItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TaskItemDto>(taskItem));
        }

        [HttpPost]
        public async Task<ActionResult<TaskItemDto>> CreateTaskItem(
            TaskItemForCreationDto taskItem)
        {
            if (await _goalService.TaskItemNameExistsAsync(taskItem.Name))
            {
                return BadRequest("Ya existe una tarea con ese nombre!");
            }

            var finalTaskItem = _mapper.Map<TaskItem>(taskItem);
            _goalService.AddTaskItem(finalTaskItem);
            await _goalService.SaveChangesAsync();

            var createTaskItemToReturn =
                _mapper.Map<TaskItemDto>(finalTaskItem);

            return CreatedAtRoute("GetTaskItem",
                new
                {
                    taskItemId = createTaskItemToReturn.TaskItemId
                },
                createTaskItemToReturn);
        }


        [HttpPut("{taskItemID}")]
        public async Task<ActionResult> UpdateTaskItem(int taskItemId,
            TaskItemForUpdateDto taskItem)
        {

            var taskItemEntity = await _goalService.GetTaskItemAsync(taskItemId);
            if (taskItemEntity == null)
            {
                return NotFound();
            }
            if (taskItemEntity.Name != taskItem.Name && await _goalService.TaskItemNameExistsAsync(taskItem.Name))
            {
                return BadRequest("Ya existe una tarea con ese nombre!");
            }

            _mapper.Map(taskItem, taskItemEntity);
            await _goalService.SaveChangesAsync();

            return Ok();

        }


        [HttpDelete("{taskItemId}")]
        public async Task<ActionResult> DeleteTaskItem(int taskItemId)
        {
            var taskItemEntity = await _goalService.GetTaskItemAsync(taskItemId);
            if (taskItemEntity == null)
            {
                return NotFound();
            }

            _goalService.DeleteTaskItem(taskItemEntity);
            await _goalService.SaveChangesAsync();

            return Ok();
        }
    }
}
