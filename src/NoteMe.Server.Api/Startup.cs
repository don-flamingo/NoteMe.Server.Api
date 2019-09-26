using System;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NoteMe.Common.Services.Json;
using NoteMe.Server.Api.Extensions;
using NoteMe.Server.Api.Middlewares;
using NoteMe.Server.Infrastructure.IoC;
using NoteMe.Server.Infrastructure.Settings;
using NoteMe.Server.Infrastructure.Sql;
using Microsoft.Extensions.DependencyInjection;

namespace NoteMe.Server.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }
        
        public Startup(IHostingEnvironment env)
        {
#if FRONTEND
            env.EnvironmentName = "Frontend";
#endif
            
#if RELEASE
            env.EnvironmentName = "Production";
#endif

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ExceptionMiddleware>();
            
            services.AddMemoryCache()
                .AddMvc()
                .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented)
                .AddJsonOptions(x => x.SerializerSettings.ContractResolver = JsonSerializeService.CamelCaseContractResolver)
                .AddJsonOptions(x => x.SerializerSettings.TypeNameHandling = TypeNameHandling.None)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<NoteMeContext>()
                .AddJwt(Container)
                .AddOData();

            return CreateAutofacContainer(services);
        }
        

        private IServiceProvider CreateAutofacContainer(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new MainModule(Configuration));
            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection()
                .UseMiddleware<ExceptionMiddleware>()
                .UseMvc(routerBuilder =>
                {
                    routerBuilder.EnableDependencyInjection();
                    routerBuilder.Expand().Select().Count().OrderBy().Filter();
                });
        }
    }
}