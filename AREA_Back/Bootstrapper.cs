﻿using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace AREA_Back
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            pipelines.AfterRequest.AddItemToEndOfPipeline((context) =>
            {
                context.Response.WithHeader("Access-Control-Allow-Origin", "*");
            });
        }
    }
}
