using Rat.Commands.Projects.CreateProject;
using Rat.Commands.Projects.DeleteProject;
using Rat.Commands.Projects.PatchProject;
using Rat.Commands.Properties;
using Rat.Data.Entities;
using static Rat.Data.DatabaseSchema;

namespace Rat.Commands.Projects
{
	internal static class Validators
	{
		private static readonly KeyValuePair<string, string>[] Empty = Array.Empty<KeyValuePair<string, string>>();

		public static KeyValuePair<string, string>[] ValidateId(int id)
		{
			if (id <= 0)
			{
				return new KeyValuePair<string, string>[1] { new(DeleteProjectRequest.ID_SIGNATURE, Resources.IdMustBeLargerThenZero) };
			}

			return Empty;
		}

		public static KeyValuePair<string, string>[] ValidateName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return new KeyValuePair<string, string>[1] { new(CreateProjectRequest.Name_Signature, Resources.MustNotBeNullOrEmpty) };
			}

			if (name.Length > ProjectSchema.Max_Name_Length)
			{
				return new KeyValuePair<string, string>[1]
				{
					new (PatchProjectRequest.Name_Signature, string.Format(Resources.MaximumLengthLimitExceeded, name.Length, ProjectSchema.Max_Name_Length))
				};
			}

			return Empty;
		}

		public static KeyValuePair<string, string>[] ValidateUser(UserEntity user)
		{
			if (user == null)
			{
				return new KeyValuePair<string, string>[1] { new(CreateProjectRequest.UserId_Signature, Resources.NotFound) };
			}

			return Empty;
		}

		public static KeyValuePair<string, string>[] ValidateProjectType(ProjectTypeEntity projectType)
		{
			if (projectType == null)
			{
				return new KeyValuePair<string, string>[1] { new(PatchProjectRequest.ProjectTypeId_Signature, Resources.NotFound) };
			}

			return Empty;
		}
	}
}
