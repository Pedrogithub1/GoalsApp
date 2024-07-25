﻿using GoalsAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace GoalsAPI.Models.TaskItemDtos
{
    public class TaskItemForCreationDto
    {
        [Required]
        [MaxLength(80)]
        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;

        public int GoalId { get; set; }
        public Goal? Goal { get; set; }
    }
}
