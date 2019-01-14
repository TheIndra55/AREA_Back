using Newtonsoft.Json;
using Octokit;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace AREA_Back.Action
{
    public class GitHub : IAction
    {
        public GitHub(string username, string project)
        {
            this.username = username;
            this.project = project;
            client = new GitHubClient(new ProductHeaderValue("AREA"));
            token = File.ReadAllText("Keys/github.txt");
            lastMessage = client.Repository.Commit.GetAll(username, project).GetAwaiter().GetResult()[0].Commit.Message;
            lastRequest = DateTime.Now;
        }

        public void Update(Action<string, string> action)
        {
            if (DateTime.Now.Subtract(lastRequest).TotalSeconds > Constants.timeRef)
            {
                var commit = client.Repository.Commit.GetAll(username, project).GetAwaiter().GetResult()[0];
                string msg = commit.Commit.Message;
                if (msg != lastMessage)
                {
                    action(msg, commit.Author.AvatarUrl);
                    lastMessage = msg;
                }
                lastRequest = DateTime.Now;
            }
        }

        private string lastMessage;
        private string username;
        private string project;
        private DateTime lastRequest;
        private string token;
        private GitHubClient client;
    }
}
