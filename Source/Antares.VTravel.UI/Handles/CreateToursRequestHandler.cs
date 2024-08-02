namespace Antares.VTravel.UI.Handles;

using Antares.VTravel.Shared.Dto;
using Antares.VTravel.Shared.Request;
using MediatR;
 
public class CreateToursRequestHandler : IRequestHandler<CreateToursRequest, TourDto>
{
    public Task<TourDto> Handle(CreateToursRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TourDto
        {
            Name = "Lala",
            CreatedAt = DateTimeOffset.Now
        });
    }
}