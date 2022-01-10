using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace WPFUserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // EXCEPT for an event you can return void and not Task
        private void executeSync_Click(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            RunDownloadSync();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time: { elapsedMs }";
        }

        private async void executeAsync_Click(object sender, RoutedEventArgs e)
        {
            // is better for timing than DateTime.now
            var watch = System.Diagnostics.Stopwatch.StartNew();

            await RunDownloadParallelAsync();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time: { elapsedMs }";
        }

        private List<string> PrepData()
        {
            List<string> output = new List<string>();

            resultsWindow.Text = "";

            output.Add("https://www.yahoo.com");
            output.Add("https://www.google.com");
            output.Add("https://www.microsoft.com");
            output.Add("https://www.cnn.com");
            output.Add("https://www.codeproject.com");
            output.Add("https://www.stackoverflow.com");

            return output;
        }

        // Don't return void if you don't have anything to return, return a Task.
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

        // Naming convention is to append async on the end
        // This one will run all website results at the same time
        private async Task RunDownloadParallelAsync()
        {
            List<string> websites = PrepData();
            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();

            foreach (string site in websites)
            {
                // Adding each task into a list. 
                tasks.Add(DownloadWebsiteAsync(site));
            }

            // Whenall, we pass in some amount of tasks until these are all done. then pass the results back into this var
            WebsiteDataModel[] results = await Task.WhenAll(tasks);

            foreach (var item in results)
            {
                // write to the window
                ReportWebsiteInfo(item);
            }
        }

        private void RunDownloadSync()
        {
            List<string> websites = PrepData();

            foreach (string site in websites)
            {
                WebsiteDataModel results = DownloadWebsite(site);
                ReportWebsiteInfo(results);
            }
        }

        private WebsiteDataModel DownloadWebsite(string websiteURL)
        {
            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();

            output.WebsiteUrl = websiteURL;
            output.WebsiteData = client.DownloadString(websiteURL);

            return output;
        }

        // Here we are returning a Type  of <WebsiteDataModel>
        private async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteURL)
        {
            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();

            output.WebsiteUrl = websiteURL;
            output.WebsiteData = await client.DownloadStringTaskAsync(websiteURL);

            return output;
        }

        private void ReportWebsiteInfo(WebsiteDataModel data)
        {
            resultsWindow.Text += $"{ data.WebsiteUrl } downloaded: { data.WebsiteData.Length } characters long.{ Environment.NewLine }";
        }
    }
}
