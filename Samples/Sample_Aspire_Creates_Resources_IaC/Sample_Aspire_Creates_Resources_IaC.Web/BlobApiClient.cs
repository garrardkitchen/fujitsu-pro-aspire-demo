namespace Sample_Aspire_Creates_Resources_IaC.Web;

public class BlobApiClient(HttpClient httpClient)
{
    public async Task<string[]> GetBlobs(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<string>? blobs = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<string>("/blobs", cancellationToken))
        {
            if (blobs?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                blobs ??= [];
                blobs.Add(forecast);
            }
        }

        return blobs?.ToArray() ?? [];
    }
}
