using System;
using System.Collections.Generic;

namespace Rat.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ICollection<Project> Projects { get; set; } = Array.Empty<Project>();
    }
}
