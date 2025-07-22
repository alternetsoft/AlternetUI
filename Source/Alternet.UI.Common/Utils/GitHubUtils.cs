using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides utility methods for interacting with GitHub repositories,
    /// such as retrieving commit information.
    /// </summary>
    /// <remarks>This class includes methods that utilize the GitHub REST API to perform
    /// operations like fetching commit details. It is designed to handle common tasks
    /// related to GitHub repositories, facilitating
    /// easier integration with GitHub data.</remarks>
    public static class GitHubUtils
    {
        /// <summary>
        /// Retrieves commits from a GitHub repository within a specified time range.
        /// Handles pagination for repositories with many commits.
        /// </summary>
        /// <param name="owner">Repository owner.</param>
        /// <param name="repo">Repository name.</param>
        /// <param name="since">Start date (UTC).</param>
        /// <param name="until">End date (UTC). Defaults to current UTC time if null.</param>
        /// <param name="token">GitHub token for authenticated access. Optional for public repos.</param>
        /// <returns>A collection of <see cref="CommitInfo"/> containing commit metadata.</returns>
        public static async Task<IEnumerable<CommitInfo>> GetCommitsAsync(
            string owner,
            string repo,
            DateTime since,
            DateTime? until = null,
            string? token = null)
        {
            token ??= string.Empty;
            until ??= DateTime.UtcNow;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("CommitFetcher");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var result = new List<CommitInfo>();
            int page = 1;
            const int perPage = 100;

            while (true)
            {
                string url = $"https://api.github.com/repos/{owner}/{repo}/commits" +
                             $"?since={since:O}&until={until:O}&per_page={perPage}&page={page}";

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var commits = JsonDocument.Parse(json).RootElement;

                if (commits.GetArrayLength() == 0)
                    break;

                foreach (var commit in commits.EnumerateArray())
                {
                    result.Add(new CommitInfo(commit));
                }

                page++;
            }

            return result;
        }

        /// <summary>
        /// Logs the commit history for the AlternetUI repository from the last 7 days.
        /// </summary>
        /// <remarks>This method retrieves and logs the SHA and message of each commit made
        /// to the AlternetUI repository within the past week.</remarks>
        public static async void LogCommitsForAlternetUI()
        {
            var commits = await GetCommitsAsync(
                "alternetsoft",
                "AlternetUI",
                DateTime.UtcNow.AddDays(-7));

            LogListBox.ShowMessageIdentifier = false;

            App.LogBeginSection("AlternetUI Commits in the Last 7 Days");

            foreach (var item in commits)
            {
                App.Log($"{item.Message}\n \n");
            }

            App.LogEndSection();

            LogListBox.ShowMessageIdentifier = true;
        }

        /// <summary>
        /// Represents information about a commit in a version control system.
        /// </summary>
        /// <remarks>This class provides basic details about a commit, including its SHA identifier and
        /// commit message.</remarks>
        public class CommitInfo
        {
            /// <summary>
            /// Represents information about a commit, encapsulated in a JSON element.
            /// </summary>
            /// <param name="element">The JSON element containing commit information.
            /// Can be <see langword="null"/> if no data is available.</param>
            public CommitInfo(JsonElement? element)
            {
                Element = element;
            }

            /// <summary>
            /// Gets the SHA-1 hash value.
            /// </summary>
            public string? Sha
            {
                get
                {
                    var sha = Element?.GetProperty("sha").GetString();
                    return sha;
                }
            }

            /// <summary>
            /// Gets the message associated with the commit.
            /// </summary>
            public string? Message
            {
                get
                {
                    var message = Element?.GetProperty("commit").GetProperty("message").GetString();
                    return message;
                }
            }

            /// <summary>
            /// Gets or sets the JSON element associated with this instance.
            /// </summary>
            public JsonElement? Element { get; set; }
        }
    }
}
