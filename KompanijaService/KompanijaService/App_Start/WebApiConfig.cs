using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using KompanijaService.Interface;
using KompanijaService.Repository;
using Unity;
using Unity.Lifetime;
using KompanijaService.Resolver;
using AutoMapper;
using KompanijaService.Models;

namespace KompanijaService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Unity
            var container = new UnityContainer();
            container.RegisterType<IKompanijaRepository, KompanijaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IZaposleniRepository, ZaposleniRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            // AutoMapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Zaposleni, ZaposleniDTO>();
            });
        }
    }
}
