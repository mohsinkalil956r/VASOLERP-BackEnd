﻿using ERP.DAL.DB.Entities;

namespace ERP.API.Models.Projects
{
    public class ProjectPostVM
    {
        public string Name { get; set; }
        public DateTime PlannedCompletedAt { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public double Budget { get; set; }
        public int ClientId { get; set; }
        public string Location { get; set; }

        public int StatusId { get; set; }
  //      public List<int> EmployeeIds { get; set; } = new();

    }
}
