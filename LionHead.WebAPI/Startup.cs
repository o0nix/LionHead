using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Unity.WebApi;
using LionHead.Core.Interfaces;
using LionHead.Core;

[assembly: OwinStartup(typeof(LionHead.WebAPI.Startup))]

namespace LionHead.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);

            UnityConfig.RegisterComponents(config);

            app.UseWebApi(config); 

            ConfigureAuth(app);
        }
    }
}
