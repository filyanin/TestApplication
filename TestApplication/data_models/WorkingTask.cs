using System;
using System.Collections.Generic;

namespace TestApplication.data_models
{
    public partial class WorkingTask
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? StatusId { get; set; }
        public int? ParentId { get; set; }

        public virtual Status? Status { get; set; }
    }
}
