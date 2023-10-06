using System.Diagnostics;
using ManticoreSearch.Api;
using ManticoreSearch.Client;
using ManticoreSearch.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Program
{
    public static void Main()
    {
        Configuration config = new Configuration
        {
            BasePath = "http://127.0.0.1:9308" // this is the default in stock searchd config, see manticore.conf or your custom config file
        };

        HttpClient httpClient = new HttpClient();
        HttpClientHandler httpClientHandler = new HttpClientHandler();

        UtilsApi utilsApi = new UtilsApi(httpClient, config, httpClientHandler);
        var result2 = utilsApi.Sql("create table test(id int, title text) morphology='stem_en'");
        
        IndexApi apiInstance = new IndexApi(httpClient, config, httpClientHandler);
        string body = "{\"insert\": {\"index\": \"test\", \"id\": 1, \"title\": \"Title 1\"}},\\n{\"insert\": {\"index\": \"test\", \"id\": 2, \"title\": \"Title 2\"}}";

        try
        {
            // Bulk index operations
            //BulkResponse result = apiInstance.Bulk(body);
            //Debug.WriteLine(result);
        }
        catch (ApiException e)
        {
            Debug.Print("Exception when calling IndexApi.Bulk: " + e.Message );
            Debug.Print("Status Code: "+ e.ErrorCode);
            Debug.Print(e.StackTrace);
        }
            
        SearchApi apiInstance2 = new SearchApi(httpClient, config, httpClientHandler);
        
        SearchRequest searchRequest = new SearchRequest("test")
        {
            Query = JsonConvert.DeserializeObject("""
                    {   
                        "index" : "test",
                        "query":
                        {
                            "match_phrase":
                            {
                                "*" : "Muj nadpis"
                            }
                        }
                    }
                    """)
            //FulltextFilter = new QueryFilter("Title 1"),
        };
        
        try
        {
            SearchResponse result = apiInstance2.Search(searchRequest);
            Debug.WriteLine(result);
        }
        catch (ApiException  e)
        {
            Debug.Print("Exception when calling SearchApi.Search: " + e.Message);
            Debug.Print("Status Code: " + e.ErrorCode);
            Debug.Print(e.StackTrace);
        }

    }
}