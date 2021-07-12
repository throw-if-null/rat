using System;

namespace Rat.Api.Controllers.Projects.Models
{
    public record ProjectModel
    {
        public int Id { get; init; }

        public DateTimeOffset Created { get; init; }

        public int CreatedBy { get; init; }

        public string Name { get; init; }
    }
}
