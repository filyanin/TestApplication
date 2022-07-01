using TestApplication.data_models;
using TestApplication.main;

namespace TestApplication.logic
{
    public static class TaskLogic
    {
        private static void rollback(TestApplicationDBContext dBContext)
        {
            foreach (var entry in dBContext.ChangeTracker.Entries().Where(a => a.State != Microsoft.EntityFrameworkCore.EntityState.Unchanged).ToList())
            {
                switch (entry.State)
                {
                    case Microsoft.EntityFrameworkCore.EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                        break;
                    case Microsoft.EntityFrameworkCore.EntityState.Added:
                        entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                        break;
                    case Microsoft.EntityFrameworkCore.EntityState.Deleted:
                        entry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                        break;
                }
            }
            
        }

        public static bool AddNewTaskWithSubtask(TestApplicationDBContext dBContext, string title, string desc, string? subtask)
        {
            WorkingTask tmpTask = new WorkingTask();
            tmpTask.TaskId = dBContext.WorkingTasks.Count() + 1;
            tmpTask.Title = title;
            tmpTask.Description = desc;

            //
            if (subtask == null)
            {

                tmpTask.Status = dBContext.Statuses.First(a => a.StatusId == 2);
                dBContext.WorkingTasks.Add(tmpTask);
                dBContext.SaveChanges();
                return true;

            }
            else
            {
                tmpTask.Status = dBContext.Statuses.First(a => a.StatusId == 1);

                bool isAddedYet = false;

                foreach (var tmpSubtaskId in subtask.Split(' '))
                {
                    var tmpSubtask = dBContext.WorkingTasks.FirstOrDefault(a => a.TaskId.ToString() == tmpSubtaskId);
                    if (tmpSubtask != null)
                    {
                        if (!isAddedYet)
                        {
                            isAddedYet = true;
                            dBContext.WorkingTasks.Add(tmpTask);
                        }
                        tmpSubtask.ParentId = tmpTask.TaskId;
                        
                    }
                    else
                    {
                        //Rollback
                        rollback(dBContext);
                        return false;
                    }
                }
                dBContext.SaveChanges();
                return true;
            }
        }

        public static bool AddNewTaskWithParent(TestApplicationDBContext dBContext, string title, string desc, string? parentTask)
        {
            WorkingTask tmpTask = new WorkingTask();
            tmpTask.TaskId = dBContext.WorkingTasks.Count() + 1;
            tmpTask.Title = title;
            tmpTask.Description = desc;

            
            if (parentTask == null)
            {

                tmpTask.Status = dBContext.Statuses.First(a => a.StatusId == 2);
                dBContext.WorkingTasks.Add(tmpTask);
                dBContext.SaveChanges();
                return true;

            }
            else
            {
                tmpTask.Status = dBContext.Statuses.First(a => a.StatusId == 2);

                var tmpParentTask = dBContext.WorkingTasks.FirstOrDefault(a => a.TaskId.ToString() == parentTask);

                if (tmpParentTask != null)
                {
                    tmpTask.ParentId = tmpParentTask.TaskId;
                    dBContext.WorkingTasks.Add(tmpTask);
                    dBContext.SaveChanges();
                }
                else
                {
                    rollback(dBContext);
                    return false;
                }
                
                return true;
            }
        }

        public static bool ChangeTaskStatus(TestApplicationDBContext dBContext, string taskId)
        {
            var tmpTask = dBContext.WorkingTasks.FirstOrDefault(a => a.TaskId.ToString() == taskId);

            if (tmpTask != null)
            {
                if (tmpTask.StatusId.ToString() != "2")
                {
                    return false;
                }
                else
                {
                    foreach (var tmp in dBContext.WorkingTasks.Where(a => a.ParentId == tmpTask.TaskId))
                    {
                        if (tmp.StatusId.ToString() != "3")
                        {
                            return false;
                        }
                    }

                    tmpTask.Status = dBContext.Statuses.FirstOrDefault(a => a.StatusId.ToString() == "3");
                    tmpTask.StatusId = tmpTask.Status.StatusId;
                    dBContext.SaveChanges();
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

    }
}
