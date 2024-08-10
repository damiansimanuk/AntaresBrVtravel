namespace Antares.VTravel.Shared.Core.Remote;

using Antares.VTravel.Shared.Request;
using Antares.VTravel.Shared.Dto;
using Antares.VTravel.Shared.Core.ResultFluent;

public class RandomTest
{
    HttpMediator mediator = null!;

    public async Task Test()
    {
        var tour = await mediator.Send(new CreateTourRequest(false, false, "lala", "nada"));

        tour.Match(t => t.CreatedAt, e => throw new Exception());

        Result<int> test;

        test = ErrorBuilder.Create()
            .Add("Code", "Message", "Detail")
            .Add(new Error("Code", "Message", "Detail"))
            .GetErrors();

        var errors = ErrorBuilder.Create()
            .Add("Code", "Message", "Detail")
            .Add(new Error("Code", "Message", "Detail"))
            .Add(new Exception());

        test = errors.HasError ? errors.GetErrors() : 2;
        var test2 = errors.ToResult(33);

        Result<int> resultFromError = errors.GetErrors();
        resultFromError.Map(t => 33);

        var errors2 = test2.Errors;
        var value = test2.Value;

        var test3 = errors.ToResult(() => "nada");

        var result = Validation1(null!)
            .Bind(Validation2)
            .Bind(Validation2)
            .Bind(SendEmail)
            .Map(e => e ? 3f : 1d);


        var realResult = ValidateInput(null)
            .Bind(Validation2);
    }


    public Result<TourDto> ValidateInput(TourDto tourDto)
    {
        var errors = ErrorBuilder.Create();
        errors.Validate(() => string.IsNullOrEmpty(tourDto.Name), () => new Error("Invalid Name", "null"));
        errors.Add(string.IsNullOrEmpty(tourDto.Name), () => new Error("Invalid Name", "null"));
        errors.Add(string.IsNullOrEmpty(tourDto.Name), new Error("Invalid Name", "null"));
        return errors.ToResult(tourDto);
    }

    public Result<TourDto> Validation1(TourDto tourDto)
    {
        if (Equals(tourDto, null))
            return new Error("Serialize", "");

        return tourDto;
    }

    public Result<TourDto> Validation2(TourDto tourDto)
    {
        if (Equals(tourDto, null))
            return new Error("Serialize", "");

        return tourDto;
    }

    public Result<bool> SendEmail(TourDto tourDto)
    {
        if (Equals(tourDto, null))
            return new Error("Serialize", "");

        return true;
    }
}