using MediatR;
using Rat.Core.Properties;
using Rat.Data.Entities;

namespace Rat.Core.Commands.Projects.PatchProject
{
    internal record PatchProjectRequest : IRequest<PatchProjectResponse>
    {
        private const string Class_Name = nameof(PatchProjectRequest) + ".";
        internal const string Id_Signature = Class_Name + nameof(Id);
        internal const string Name_Signature = Class_Name + nameof(Name);
        internal const string ProjectTypeId_Signature = Class_Name + nameof(ProjectTypeId);
        internal const int Name_Max_Length = 248;

        public int Id { get; init; }

        public string Name { get; init; }

        public int ProjectTypeId { get; init; }

        public RatContext Context { get; init; } = new();
    }

    internal static class PatchProjectRequestExtensions
    {
        public static void Validate(this PatchProjectRequest request, ProjectType projectType)
        {
            request.Context.Status = ProcessingStatus.Ok;

            if (request.Id <= 0)
            {
                request.Context.ValidationErrors.Add(
                    PatchProjectRequest.Id_Signature,
                    Resources.IdMustBeLargerThenZero);
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                request.Context.ValidationErrors.Add(
                    PatchProjectRequest.Name_Signature,
                    Resources.MustNotBeNullOrEmpty);
            }

            var length = request.Name?.Length ?? -1;
            if (length > PatchProjectRequest.Name_Max_Length)
            {
                request.Context.ValidationErrors.Add(
                    PatchProjectRequest.Name_Signature,
                    string.Format(Resources.MaximumLengthLimitExceeded, length, PatchProjectRequest.Name_Max_Length));
            }

            if (projectType == null)
            {
                request.Context.ValidationErrors.Add(
                    PatchProjectRequest.ProjectTypeId_Signature,
                    Resources.NotFound);
            }

            if(request.Context.ValidationErrors.Count > 0)
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
