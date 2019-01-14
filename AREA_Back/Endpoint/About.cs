using Nancy;
using System;

namespace AREA_Back.Endpoint
{
    public class About : NancyModule
    {
        public About()
        {
            Get("/", x =>
            {
                return (Response.AsRedirect("/about.json"));
            });

            Get("/about.json", x =>
            {
                return (Response.AsJson(new Response.About()
                {
                    Client = new Response.PClient()
                    {
                        Host = Request.UserHostAddress
                    },
                    Server = new Response.PServer()
                    {
                        Current_time = ((int)((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds)),
                        Services = new Response.PService[]
                        { }
                    }
                }));
            });
        }
    }
}
