namespace Antares.VTravel.UI.Handlers;
using Antares.VTravel.Shared.Core;
using Antares.VTravel.Shared.Dto;
using Antares.VTravel.Shared.Request;
using Antares.VTravel.UI.Core;
using Antares.VTravel.UI.Data;
using Antares.VTravel.UI.Mapper;
using MediatR;

public class CreateTourRequestHandler(
    VTravelDbContext dbContext,
    CurrentUser currentUser,
    MapperService mapper
    ) : IRequestHandler<CreateTourRequest, Result<TourDto>>
{
    public async Task<Result<TourDto>> Handle(CreateTourRequest request, CancellationToken cancellationToken)
    {
        return await ErrorBuilder.Create()
            .Add(!currentUser.IsAuthenticated, Error.Invalid("Invalid User"))
            .Add(string.IsNullOrWhiteSpace(request.Name), Error.Invalid("Invalid name"))
            .Add(string.IsNullOrWhiteSpace(request.Description), Error.Invalid("Invalid Description"))
            .ToResultAsync(async () =>
            {
                var tour = new Tour
                {
                    Name = request.Name,
                    UserId = currentUser.Name,
                    Description = request.Description,
                };

                dbContext.Add(tour);
                await dbContext.SaveChangesAsync(cancellationToken);

                return mapper.ToDto(tour);
            });
    }
}
