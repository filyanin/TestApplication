using System;
using System.Collections.Generic;

namespace TestApplication.data_models
{
    public partial class Status
    {
        public Status()
        {
            WorkingTasks = new HashSet<WorkingTask>();
        }

        public int StatusId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<WorkingTask> WorkingTasks { get; set; }
    }
}
