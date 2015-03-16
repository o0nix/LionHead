using LionHead.Core;
using LionHead.Core.Interfaces;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace LionHead.WebAPI
{
    public static class UnityConfig
    {
        public static UnityContainer RegisteredTypes;

        public static void RegisterComponents(HttpConfiguration config)
        {
            RegisteredTypes = new UnityContainer();

            RegisteredTypes.RegisterInstance<IGameAPI>(new DummyGameAPI());

            config.DependencyResolver = new UnityDependencyResolver(RegisteredTypes);
        }
    }
}