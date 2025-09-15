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
        /// <param name="token">GitHub token for authenticated access.
        /// Optional for public repos.</param>
        /// <param name="branch">The branch name. If not specified, defaults to "master".</param>
        /// <returns>A collection of <see cref="CommitInfo"/> containing commit metadata.</returns>
        public static async Task<IEnumerable<CommitInfo>> GetCommitsAsync(
            string owner,
            string repo,
            DateTime since,
            DateTime? until = null,
            string? token = null,
            string? branch = null)
        {
            token ??= string.Empty;
            until ??= DateTime.UtcNow;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("CommitFetcher");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);
            }

            var result = new List<CommitInfo>();
            int page = 1;
            const int perPage = 100;

            if (string.IsNullOrEmpty(branch))
                branch = "master";

            while (true)
            {
                string url = $"https://api.github.com/repos/{owner}/{repo}/commits" +
                             $"?sha={branch}&since={since:O}&until={until:O}&per_page={perPage}&page={page}";
                try
                {
                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        AddError($"{response.StatusCode}: {errorContent}");
                        return result;
                    }

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
                catch (HttpRequestException ex)
                {
                    AddError($"Request failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    AddError($"Unexpected error: {ex.Message}");
                }
            }

            return result;

            void AddError(string message)
            {
                result.Add(new CommitInfo { ErrorMessage = "[Error] " + message });
            }
        }

        /// <summary>
        /// Logs the commit history for the AlternetUI repository from the last 7 days.
        /// </summary>
        /// <remarks>This method retrieves and logs the SHA and message of each commit made
        /// to the AlternetUI repository within the past week.</remarks>
        public static void LogCommitsForAlternetUI()
        {
            DialogFactory.AskTextAsync(
                "Specify Alternet.UI branch name (empty for master)",
                async (branch) =>
                {
                    var commits = await GetCommitsAsync(
                        "alternetsoft",
                        "AlternetUI",
                        DateTime.UtcNow.AddDays(-7),
                        token: null,
                        branch: branch);

                    var s = CommitsToString(commits);

                    WindowWithMemoAndButton.ShowDialog("AlternetUI Commits in the Last 7 Days", s);
                });
        }

        /// <summary>
        /// Converts a collection of <see cref="CommitInfo"/> to a formatted string.
        /// </summary>
        /// <param name="commits">The collection of commits.</param>
        /// <returns>A string representation of the commits.</returns>
        public static string CommitsToString(IEnumerable<CommitInfo> commits)
        {
            var sb = new StringBuilder();
            foreach (var commit in commits)
            {
                sb.AppendLine($"{commit.Message}");
                sb.AppendLine(LogUtils.SectionSeparator);
            }

            return sb.ToString();
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
            /// Initializes a new instance of the <see cref="CommitInfo"/> class.
            /// </summary>
            public CommitInfo()
            {
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
            /// Gets or sets the error message associated with the current operation or state.
            /// </summary>
            public string? ErrorMessage { get; set; }

            /// <summary>
            /// Gets or sets the JSON element associated with this instance.
            /// </summary>
            public JsonElement? Element { get; set; }

            /// <summary>
            /// Gets a value indicating whether the current state represents an error.
            /// </summary>
            public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
        }
    }
}
