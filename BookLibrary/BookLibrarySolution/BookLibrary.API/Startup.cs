using BookLibrary.API.Entities;
using BookLibrary.API.Interfaces;
using BookLibrary.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;

namespace BookLibrary.API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Adding a refrence to MVC service Middleware.
            services.AddMvc()

                // Optional - Default output formatter is JSON. 
                // With this we can add XML to our list of output formatters
                .AddMvcOptions(o =>
                    o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()))

                // Optional - Will prevent lowercase as default for JSON properties.
                .AddJsonOptions(o =>
                {
                    if (o.SerializerSettings.ContractResolver != null)
                    {
                        var castedResolver = o.SerializerSettings.ContractResolver
                            as DefaultContractResolver;
                        castedResolver.NamingStrategy = null;
                    }
                });

#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
            // Setting the connectionstring
            var connectionString = Startup.Configuration["connectionStrings:BookLibraryDBConnectionString"];
            services.AddDbContext<ApplicationContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<IBookLibraryRepository, BookLibraryRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            ApplicationContext applicationContext)
        {
            //loggerFactory.AddConsole();
            //loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            // The Exception Middleware will try to catch (Exceptions) before handing over the request to the MVC Middleware.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Add seed data to database.
            applicationContext.EnsureSeedDataForContext();

            // Adding the Status code pages Middleware to the request pipeline.
            // This will show status codes on the client side.
            app.UseStatusCodePages();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Author, Models.AuthorWithoutBookDto>();
                cfg.CreateMap<Entities.Author, Models.AuthorDto>();
                cfg.CreateMap<Entities.Book, Models.BookDto>();

                cfg.CreateMap<Models.BookForCreationDto, Entities.Book>();
                cfg.CreateMap<Models.BookForUpdateDto, Entities.Book>();

                cfg.CreateMap<Entities.Book, Models.BookForUpdateDto>();
                cfg.CreateMap<Entities.Book, Models.BookWithAuthorDto>();
            });

            // Adding the MVC Middleware to the request pipeline.
            // MVC Middleware will handle (HTTP) requests.
            app.UseMvc();
        }
    }
}
