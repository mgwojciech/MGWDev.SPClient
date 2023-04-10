# MGWDev.SPClient
This library simplifies consuming SharePoint REST API.

The goal of it is to expose lightweight SDK based on HttpClient.

It supports PnP.Auth IAuthenticationProvider to get token. To use it 

```
InteractiveAuthenticationProvider provider = new InteractiveAuthenticationProvider("app-id", "tenant-id", new Uri("reply-url"));
PnPAuthenticator authenticator = new PnPAuthenticator(provider);

HttpClient client = HttpClientFactory.GetHttpClient("http://<tenant>.sharepoint.com", authenticator);

```

The biggest advantage of this library, is it can generate select, expand and filter statement based on objects you define amd Linq expression.

```
    public class InformationMessage : SPListItem
    {
        [JsonPropertyName("MessageDescription")]
        public string? Body { get; set; }
        [JsonPropertyName("MessageStartDate")]
        public DateTime? StartDate { get; set; }
        [JsonPropertyName("MessageEndDate")]
        public DateTime? EndDate { get; set; }
    }
    
    BaseCollectionEntityService<InformationMessage> service = new BaseCollectionEntityService<InformationMessage>(client, "/sites/tea-point/_api/web/lists/getByTitle('InformationMessages')/items");
    List<InformationMessage> messages = await service.Get(message => message.Author.Id == 7);
    List<InformationMessage> otherMessages = await service.Get(message => message.EndDate <= DateTime.UtcNow || message.Title == "Test 6-11-2020-1");
    
```
