namespace FootballLeague
{
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using FootballLeague.Data;
    using FootballLeague.Data.Contracts;
    using FootballLeague.Data.Implementations;
    using FootballLeague.Services;
    using FootballLeague.Services.Contracts;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using FootballLeague.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Text;

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
            services.AddDbContext<FootballLeagueContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("FootballLeagueConnection")));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FootballLeague API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(s => s.Value.ValidationState == ModelValidationState.Invalid)
                        .Select(e => e.Value.Errors.Select(er => er.ErrorMessage).FirstOrDefault());

                    var errorMessages = new StringBuilder();

                    foreach (var error in errors)
                    {
                        errorMessages.AppendFormat("{0}{1}", error, Environment.NewLine);
                    }

                    return new BadRequestObjectResult(errorMessages.ToString());
                };
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Add Domain Services

            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IMatchService, MatchService>();

            // Auto Mapper
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseErrorHandling();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

        }
    }
}
