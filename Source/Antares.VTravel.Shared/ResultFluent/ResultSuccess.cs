namespace Antares.VTravel.Shared.ResultFluent;

public class ResultSuccess : Result<ResultSuccess>
{
    public ResultSuccess()
    {
        IsSuccess = true;
    }
}
