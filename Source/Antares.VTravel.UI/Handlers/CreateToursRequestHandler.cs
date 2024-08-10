namespace Antares.VTravel.UI.Handlers; 
using Antares.VTravel.Client.Dto;
using Antares.VTravel.Client.Event;
using Antares.VTravel.Client.Request;
using Antares.VTravel.Shared.Event;
using Antares.VTravel.Shared.ResultFluent;
using Antares.VTravel.UI.Core;
using Antares.VTravel.UI.Data;
using Antares.VTravel.UI.Mapper;
using MediatR;

public class CreateTourRequestHandler(
    VTravelDbContext dbContext,
    CurrentUser currentUser,
    DomainEventBus eventBus,
    MapperService mapper
    ) : IRequestHandler<CreateTourRequest, Result<TourDto>>
{

    public async Task<Result<TourDto>> Handle(CreateTourRequest request, CancellationToken cancellationToken)
    {
        if (request.Exception)
        {
            throw new InvalidOperationException("Excepcion genearad por pedido del cliente!");
        }

        var eb = ErrorBuilder.Create()
            .Add(request.Failure, new Error("Alto", $"Error, Failure {request.Failure}"))
            //.Add(!currentUser.IsAuthenticated, Error.Invalid("Authentication fail"))
            .Add(string.IsNullOrWhiteSpace(request.Name), Error.Invalid("Name is empty"))
            .Add(string.IsNullOrWhiteSpace(request.Description), Error.Invalid("Description is empty"));

        if (eb.HasError)
        {
            //eventBus.SendMessage(new TourCreated(1, "tour.Name", "tour.Description"));
            return eb.GetErrors();
        }

        var tour = new Tour
        {
            Name = request.Name,
            //UserId = currentUser.Name,
            Description = request.Description,
        };

        tour.AddDomainEvent(() => new TourCreated(tour.Id, tour.Name, tour.Description));

        dbContext.Add(tour);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.ToDto(tour);
    }

    //public async Task<Result<TourDto>> Handle(CreateTourRequest request, CancellationToken cancellationToken)
    //{
    //    return await ErrorBuilder.Create()
    //        //.Add(!currentUser.IsAuthenticated, Error.Invalid("Authentication fail"))
    //        .Add(string.IsNullOrWhiteSpace(request.Name), Error.Invalid("Name is empty"))
    //        .Add(string.IsNullOrWhiteSpace(request.Description), Error.Invalid("Description is empty"))
    //        .ToResult()
    //        .BindAsync(async () =>
    //        {
    //            var tour = new Tour
    //            {
    //                Name = request.Name,
    //                //UserId = currentUser.Name,
    //                Description = request.Description,
    //            };

    //            tour.AddDomainEvent(() => new TourCreated(tour.Id, tour.Name, tour.Description));

    //            dbContext.Add(tour);
    //            await dbContext.SaveChangesAsync(cancellationToken);

    //            return mapper.ToDto(tour);
    //        });
    //}
}
