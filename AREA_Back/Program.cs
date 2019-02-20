using AREA_Back.Action;
using Nancy.Hosting.Self;
using System;
using System.Threading;

namespace AREA_Back
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(new ThreadStart(RunActions));
            thread.Start();
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            LaunchServer(autoEvent);
            autoEvent.WaitOne();
        }

        private static void LaunchServer(AutoResetEvent autoEvent)
        {
            HostConfiguration config = new HostConfiguration()
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };
            NancyHost host = new NancyHost(config, new Uri("http://localhost:8081/"));
            host.Start();
            Console.WriteLine("Host Started... Do ^C to exit.");
            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("^C received, exitting...");
                host.Dispose();
                autoEvent.Set();
            };
        }

        private static void RunActions()
        {
            IAction[] actions = new IAction[]
            {
                new Konachan("Xwilarg"),
                new GitHub("Xwilarg", "AREA_Back"),
                new NHentai("84616/zirk")
            };
            Reactions.Discord reaction = new Reactions.Discord();
            while (Thread.CurrentThread.IsAlive)
            {
                foreach (IAction a in actions)
                {
                    try
                    {
                        a.Update(reaction.Callback);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An error occured: " + e.Message);
                    }
                }
            }
        }
    }
}
