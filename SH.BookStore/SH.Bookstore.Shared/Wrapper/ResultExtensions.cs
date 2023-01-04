using System.Text.Json;
using System.Text.Json.Serialization;

namespace SH.Bookstore.Shared.Wrapper;

public static class ResultExtensions
{
    public static async Task<IResult<T>> ToResult<T>(this HttpResponseMessage response)
    {
        try
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<Result<T>>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return responseObject;
        }
        catch (Exception ex)
        {
            return await Result<T>.FailAsync(ex.Message);
        }

    }
    public static async Task<IResult> ToResult(this HttpResponseMessage response)
    {
        try
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<Result>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return responseObject;
        }
        catch (Exception ex)
        {
            return await Result.FailAsync(ex.Message);
        }

    }

    public static async Task<PaginatedResult<T>> ToPaginatedResult<T>(this HttpResponseMessage response)
    {
        try
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<PaginatedResult<T>>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return responseObject;
        }
        catch (Exception ex)
        {
            return new PaginatedResult<T>(false, messages: new() { ex.Message });
        }

    }
}
