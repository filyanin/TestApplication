using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApplication.data_models;
using TestApplication.logic;
using TestApplication.main;

namespace TestApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TestApplicationDBContext _dbContext;

        public TaskController(TestApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        [HttpGet("CreateTaskWithSubtask/{title}/{description}")]
        public bool CreateTaskWithSubtask(string title, string description, string? subtaskIds = null)
        {
            return TaskLogic.AddNewTaskWithSubtask(_dbContext, title,description,subtaskIds);
        }

        [HttpGet("CreateTaskWithParent/{title}/{description}")]
        public bool CreateTaskWithParent(string title, string description, string? parentId = null)
        {
            return TaskLogic.AddNewTaskWithParent(_dbContext, title, description, parentId);
        }

        [HttpGet("ChangeStatus/{taskId}")]
        public bool ChangeStatus(string taskId)
        {
            return TaskLogic.ChangeTaskStatus(_dbContext, taskId);
        }

        [HttpGet("GetTaskToDo")]
        public async Task<List<WorkingTask>> GetListToDoAsync()
        {
            var resultList = await _dbContext.WorkingTasks.Where(a => a.StatusId.ToString() == "2").ToListAsync();
            return resultList;
        }

        [HttpGet("GetTask")]
        public async Task<List<WorkingTask>> GetListTaskAsync()
        {
            var resultList = await _dbContext.WorkingTasks.ToListAsync();
            return resultList;
        }

    }
}
