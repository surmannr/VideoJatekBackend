using Autofac.Extensions.DependencyInjection;
using Autofac;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VideoJatekBackend.Dal;
using VideoJatekBackend.Extensions;
using VideoJatekBackend.Services;
using VideoJatekBackend.Services.PublisherFeature.Queries;
using Hellang.Middleware.ProblemDetails;
using FluentValidation.AspNetCore;
using NSwag.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Autofac.Core;

namespace VideoJatekBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowAnyHeader());
            });
            #endregion

            #region Hangfire
            // Add Hangfire services.
            builder.Services.AddHangfire(configuration =>
            {
                configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultHangfireConnection"), new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    });
            });

            // Add the processing server as IHostedService
            builder.Services.AddHangfireServer(options =>
            {
                options.Queues = new[] {
                    "refresh_data_from_file",
                    "default" };
            });
            #endregion

            #region DatabaseConfiguration
            // Add services to the container.
            builder.Services.AddDbContext<VideogameDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
            );
            #endregion

            #region Automapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            #endregion

            #region Business logic services (CQRS)
            builder.Services.AddMediatR(typeof(GetAllPublishers));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            builder.Services.AddTransient<TimingService>();
            #endregion

            #region Autofac
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterAssemblyTypes(Assembly.Load("VideoJatekBackend"))
                    .Where(x => x.Name.EndsWith("Validator"))
                    .AsImplementedInterfaces()
                    .InstancePerDependency();

            });
            #endregion

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };

            #region Swagger
            builder.Services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "VideoJatekBackend API";
                    document.Info.Description = "Munka jelentkez�s - Surmann Roland";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Surmann Roland",
                        Email = "surmannroland@gmail.com",
                        Url = "https://www.linkedin.com/in/rolandsurmann/"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };
                };
            });
            #endregion

            #region ProblemDetails
            builder.Services.AddProblemDetails(ConfigureProblemDetails);
            #endregion

            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            #region FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();
            #endregion


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
            var manager = new RecurringJobManager();
            manager.AddOrUpdate<TimingService>("refresh_data", (timingService) => timingService.LoadDataFromFile(), Cron.Daily, queue: "refresh_data_from_file");

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();

            app.UseProblemDetails();

            app.MapControllers();

            app.MigrateDatabase<VideogameDbContext>().Run();
        }

        private static void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            options.IncludeExceptionDetails = (ctx, ex) => false;

            options.Map<NullReferenceException>(
              (ctx, ex) =>
              {
                  var pd = StatusCodeProblemDetails.Create(StatusCodes.Status404NotFound);
                  pd.Title = ex.Message;
                  return pd;
              });

            options.Map<Exception>(
              (ctx, ex) =>
              {
                  var pd = StatusCodeProblemDetails.Create(StatusCodes.Status500InternalServerError);
                  pd.Title = ex.Message;
                  return pd;
              });
        }
    }
}