using MediatR;

namespace Rat.Core.Queries.ProjectTypes
{
	internal record GetProjectTypeByIdRequest : IRequest<GetProjectTypeByIdResponse>
	{
		public int Id { get; init; }
	}
}
