using System.Collections.Generic;

namespace Rat.Data.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
    }
}
