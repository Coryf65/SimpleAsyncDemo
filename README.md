# SimpleAsyncDemo

- Demo from Tim Corey on YouTube [here](https://youtu.be/2moh18sh5p4)
- Microsoft Docs about Async Programming [here](https://docs.microsoft.com/en-us/dotnet/csharp/async)

## Async Info

>If you have any I/O-bound needs (such as requesting data from a network, accessing a database, or reading and writing to a file system), you'll want to utilize >asynchronous programming. You could also have CPU-bound code, such as performing an expensive calculation, which is also a good scenario for writing async code.
>C# has a language-level asynchronous programming model, which allows for easily writing asynchronous code without having to juggle callbacks or conform to a library >that supports asynchrony. It follows what is known as the Task-based Asynchronous Pattern (TAP).
-From Microsoft Documentation

1. Learning about the new way to handle Threading. Using Tasks and Async / Await.

## Notes

- If you have nothing to return on a function return a Task

> Note: Expceptions like Events do happen. like an onClick that is async is okay to return void

```C#
private async Task RunDownloadAsync()
{
    List<string> websites = PrepData();

    foreach (string site in websites)
    {
        // This allows us to run async for a method we may not be able to modify
        // The Task ususally returns a task<T> so then we run a lambda to make this work
        WebsiteDataModel results = await Task.Run(() => DownloadWebsite(site));
        ReportWebsiteInfo(results);
    }
}
```

- We can make a method we cannot change use aysnc by wrapping it like this.

```C#
// This allows us to run async for a method we may not be able to modify
// The Task ususally returns a task<T> so then we run a lambda to make this work
WebsiteDataModel results = await Task.Run(() => DownloadWebsite(site));
ReportWebsiteInfo(results);
```

- If you need to return a value, use like so

```C#
// Here we are returning a Type  of <WebsiteDataModel>
private async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteURL)
{
    WebsiteDataModel output = new WebsiteDataModel();
    WebClient client = new WebClient();

    output.WebsiteUrl = websiteURL;
    output.WebsiteData = await client.DownloadStringTaskAsync(websiteURL);

    return output;
}
```

- 
