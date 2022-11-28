using AutoMapper;
using ProjectWorkAPI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ProjectWorkAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MapperConfiguration mc = new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>());
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
