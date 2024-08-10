namespace Antares.VTravel.Shared.Core.ResultFluent;

public class ResultSuccess : Result<ResultSuccess>
{
    public ResultSuccess()
    {
        IsSuccess = true;
    }
}
