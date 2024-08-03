namespace Antares.VTravel.UI.Handles;

using Antares.VTravel.Shared.Core;
using Antares.VTravel.Shared.Dto;
using Antares.VTravel.Shared.Request;
using MediatR;

public class CreateToursRequestHandler : IRequestHandler<CreateToursRequest, Result<TourDto>>
{
    public async Task<Result<TourDto>> Handle(CreateToursRequest request, CancellationToken cancellationToken)
    {
        if (request.failure)
        {
            return new Error("Alta falla", "failure");
        }

        return new TourDto(1, request.Name, request.Description, null!, DateTimeOffset.Now, DateTimeOffset.Now, true);
    }
}