using AREA_Back.Action;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AREA_Back
{
    class Program
    {
        private static List<Response.PService> services;

        public static Response.PService[] GetServices()
            => services.ToArray();

        public static void AddService(string name)
        {
            if (!services.Any(x => x.Name == name))
                services.Add(new Response.PService()
                {
                    Name = name
                });
        }

        private static Db db;
        public static Db GetDb() { return (db); }

        static async Task Main(string[] args)
        {
            db = new Db();
            await db.InitAsync();
            services = new List<Response.PService>();
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
                new KonachanFavorite("Xwilarg"),
                new GitHub("Xwilarg", "AREA_Back"),
                new NHentai("84616/zirk"),
                new Atfbooru(),
                new DanbooruDonmai(),
                new E621(),
                new E926(),
                new FurryBooru(),
                new Gelbooru(),
                new Konachan(),
                new Lolibooru(),
                new Realbooru(),
                new Rule34(),
                new Safebooru(),
                new Sakugabooru(),
                new Xbooru(),
                new Yandere()
            };
            Reactions.Discord reaction = new Reactions.Discord();
            while (Thread.CurrentThread.IsAlive)
            {
                foreach (IAction a in actions)
                {
                    try
                    {
                        Task.Run(() => { a.Update(reaction.Callback); });
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
