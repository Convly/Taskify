using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify
{
    public class Task
    {
        private int id;
        private String title;
        private String description;
        private int status;
        private String priority;
        private List<String> tags;
        private DateTime startDate;
        private DateTime dueDate;

        public Task(int id_, String title_, String description_, int status_, String priority_, String tags_, DateTime startdate_, DateTime duedate_)
        {
            Id = id_;
            Title = title_;
            Description = description_;
            Status = status_;
            Priority = priority_;
            Tags = tags_.Split(';').ToList();
            StartDate = startdate_;
            DueDate = duedate_;
        }

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public int Status { get => status; set => status = value; }
        public List<string> Tags { get => tags; set => tags = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public string Priority { get => priority; set => priority = value; }
    }
}
