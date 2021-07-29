using MediatR;
using Rat.Core.Properties;
using Rat.Data.Entities;

namespace Rat.Core.Commands.Projects.CreateProject
{
    internal record CreateProjectRequest : IRequest<CreateProjectResponse>
    {
        private const string Class_Name = nameof(CreateProject);
        internal const string Name_Signature = Class_Name + "." + nameof(Name);
        internal const string ProjectTypeId_Signature = Class_Name + "." + nameof(ProjectTypeId);
        internal const int Nam_Max_Length = 248;

        public int UserId { get; init; }

        public string Name { get; set; }

        public int ProjectTypeId { get; set; }

        public RatContext Context { get; init; } = new();
    }

    internal static class CreateProjectRequestExtensions
    {
        public static void Validate(this CreateProjectRequest request, ProjectType projectType)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                request.Context.ValidationErrors.Add(
                    CreateProjectRequest.Name_Signature,
                    Resources.MustNotBeNullOrEmpty);
            }

            var length = request.Name?.Length ?? -1;

            if (length > CreateProjectRequest.Nam_Max_Length)
            {
                request.Context.ValidationErrors.Add(
                    CreateProjectRequest.Name_Signature,
                    string.Format(Resources.MaximumLengthLimitExceeded, length, CreateProjectRequest.Nam_Max_Length));
            }

            if (projectType == null)
            {
                request.Context.Status = ProcessingStatus.BadRequest;
                request.Context.ValidationErrors.Add(
                    CreateProjectRequest.ProjectTypeId_Signature,
                    string.Format(Resources.NotFound, request.ProjectTypeId));
            }

            if( request.Context.ValidationErrors.Count > 0)
            {
                request.Context.Status = ProcessingStatus.BadRequest;
                request.Context.FailureReason = Resources.BadRequest;
            }
            else
            {
                request.Context.Status = ProcessingStatus.GoodRequest;
            }
        }
    }
}
