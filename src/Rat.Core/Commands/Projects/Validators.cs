using Rat.Core.Commands.Projects.CreateProject;
using Rat.Core.Commands.Projects.DeleteProject;
using Rat.Core.Commands.Projects.PatchProject;
using Rat.Core.Properties;
using Rat.Core.Queries.Projects.GetProjectsForUser;
using Rat.Data.Entities;

using static Rat.Data.DatabaseSchema;

namespace Rat.Core.Commands.Projects
{
    internal static class Validators
    {
        public static void ValidateId(int id, RatContext context)
        {
            if (id <= 0)
            {
                context.ValidationErrors.Add(
                    DeleteProjectRequest.ID_SIGNATURE,
                    Resources.IdMustBeLargerThenZero);
            }
        }

        public static void ValidateName(string name, RatContext context)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                context.ValidationErrors.Add(
                    CreateProjectRequest.Name_Signature,
                    Resources.MustNotBeNullOrEmpty);

                return;
            }

            if (name.Length > ProjectSchema.Max_Name_Length)
            {
                context.ValidationErrors.Add(
                    PatchProjectRequest.Name_Signature,
                    string.Format(Resources.MaximumLengthLimitExceeded, name.Length, ProjectSchema.Max_Name_Length));
            }
        }

		public static void ValidateUser(UserEntity user, RatContext context)
		{
			if (user == null)
			{
				context.ValidationErrors.Add(
					CreateProjectRequest.UserId_Signature,
					Resources.NotFound);
			}
		}

		public static void ValidateProjectType(ProjectTypeEntity projectType, RatContext context)
        {
            if (projectType == null)
            {
                context.ValidationErrors.Add(
                    PatchProjectRequest.ProjectTypeId_Signature,
                    Resources.NotFound);
            }
        }

		public static void ValidateUserId(string userId, RatContext context)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                context.ValidationErrors.Add(
                    GetProjectsForUserRequest.UserId_Signature,
                    Resources.MustNotBeNullOrEmpty);

            }
        }

        public static void MakeGoodOrBad(RatContext context)
        {
            context.Status = ProcessingStatus.GoodRequest;

            if (context.ValidationErrors.Count > 0)
            {
                context.Status = ProcessingStatus.BadRequest;
                context.FailureReason = Resources.BadRequest;
            }
        }
    }
}
