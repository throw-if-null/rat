namespace Rat.Data.Entities
{
    public class ProjectUserEntity
    {
        public int ProjectId { get; set; }

        public ProjectEntity Project { get; set; }

        public int UserId { get; set; }

        public UserEntity User { get; set; }
    }
}
