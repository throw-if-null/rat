using System;
using System.Collections.Generic;

namespace Rat.Data.Entities
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ProjectType Type { get; set; }

        public ICollection<User> Users { get; set; } = Array.Empty<User>();
    }
}
