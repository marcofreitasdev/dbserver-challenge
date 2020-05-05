using AspNet.Security.ApiKey.Providers;
using AspNet.Security.ApiKey.Providers.Events;
using AspNet.Security.ApiKey.Providers.Extensions;
using AccountManager.API.Application.Behaviors;
using AccountManager.API.Extensions;
using AccountManager.Domain.Aggregates.ContaCorrenteAggregate;
using AccountManager.Domain.SeedWork;
using AccountManager.InfraStructure;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccountManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var secret_key_api = Configuration.GetValue<string>("Secret-Key-Api");
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddSingleton<IUnitOfWork, UnitOfWorkFake>();
            services.AddSingleton<IContaCorrenteRepository, ContaCorrenteRepositoryInMemory>();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = ApiKeyDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = ApiKeyDefaults.AuthenticationScheme;
            }).AddApiKey(options =>
                {
                    options.Header = "X-API-KEY";
                    options.HeaderKey = string.Empty;
                    options.Events = new ApiKeyEvents
                    {
                        OnApiKeyValidated = context => 
                        {
                            if (context.ApiKey == secret_key_api)
                            {
                                context.Principal = new ClaimsPrincipal();
                                context.Success();
                            }

                            return Task.CompletedTask;  
                        }
                    };
                });
            services.AddMvc()
                .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMediatR(typeof(Startup));
            services.AddSwaggerGen(c =>
            {
            c.SwaggerDoc("v1", new Info {
                Title = "AccountManager API",
                Version = "v1" });

                var s = new ApiKeyScheme
                {
                    Description = "Simples Header X-API-KEY",
                    In = "header",
                    Name = "X-API-KEY",
                    Type = "apiKey" 
                };

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "ApiKeyAuth", new string[] { } }
                });
                c.AddSecurityDefinition("ApiKeyAuth", s);                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.LoadRepository();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccountManager API v1");                
                c.RoutePrefix = string.Empty;
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }        
    }
}
