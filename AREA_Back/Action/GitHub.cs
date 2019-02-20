using Octokit;
using System;
using System.IO;

namespace AREA_Back.Action
{
    public class GitHub : IAction
    {
        public GitHub(string username, string project) : base(60f)
        {
            this.username = username;
            this.project = project;
            client = new GitHubClient(new ProductHeaderValue("AREA"));
            client.Credentials = new Credentials(File.ReadAllText("Keys/github.txt"));
            lastMessage = client.Repository.Commit.GetAll(username, project).GetAwaiter().GetResult()[0].Commit.Message;
        }

        public override void InternalUpdate(Action<string, string> action)
        {
            var commit = client.Repository.Commit.GetAll(username, project).GetAwaiter().GetResult()[0];
            string msg = commit.Commit.Message;
            if (msg != lastMessage)
            {
                lastMessage = msg;
                msg = "New commit by " + commit.Committer.Login + ":" + Environment.NewLine + Environment.NewLine + commit.Commit.Message;
                action(msg, commit.Author.AvatarUrl);
            }
        }

        private string lastMessage;
        private string username;
        private string project;
        private GitHubClient client;
    }
}
