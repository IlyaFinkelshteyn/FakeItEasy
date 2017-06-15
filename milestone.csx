#r "./packages/Octokit.0.24.0/lib/net45/Octokit.dll"

using Octokit;

var client = new GitHubClient(new ProductHeaderValue("my-cool-app"));
var userTask = client.User.Get("alfhenrik");
var user = userTask.Result;
Console.WriteLine(user.Name);

var repoTask = client.Repository.GetAllBranches("alfhenrik", "octokit.net");
var branches = repoTask.Result;
Console.WriteLine(branches.Count);
foreach(var branch in branches)
{
	Console.WriteLine(branch.Name);
}