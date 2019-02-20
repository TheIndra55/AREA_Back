using Nancy;
using System;

namespace AREA_Back.Endpoint
{
    public class Authentification : NancyModule
    {
        public Authentification()
        {
            Post("/authentification.json", x =>
            {
                if (string.IsNullOrWhiteSpace(Request.Query["username"]) || string.IsNullOrWhiteSpace(Request.Query["mail"])
                || string.IsNullOrWhiteSpace(Request.Query["password"]))
                    return (Response.AsJson(new Response.Error()
                    {
                        Code = 400,
                        Message = "Missing arguments"
                    }, HttpStatusCode.BadRequest));
                if (Program.GetDb().AddUserAsync(Request.Query["username"], Request.Query["mail"], Request.Query["password"]).GetAwaiter().GetResult())
                    return (Response.AsJson(new Response.Error()
                    {
                        Code = 200,
                        Message = "User added"
                    }));
                return (Response.AsJson(new Response.Error()
                {
                    Code = 403,
                    Message = "User already exist"
                }, HttpStatusCode.Forbidden));
            });
        }
    }
}
