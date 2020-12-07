using System;
using System.Collections.Generic;
using ApplicationCore.Interfaces.Logger;
using ApplicationCore.Interfaces.Repositories;
using Infrastructure.Data.Repositories;
using Infrastructure.Logger;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApplicationCore;
using FluentValidation.AspNetCore;
using Presentation.Validators;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Diagnostics;
using Presentation.Hubs;
using Presentation.Admin;
using Presentation.Admin.Automapping;
using Presentation.Admin.Models;
using ApplicationCore.Interfaces.Services;
using Infrastructure.Services;
using MelaMandiUI.Interfaces;
using MelaMandiUI.Services;
using Presentation.Interfaces;
using Presentation.Services;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Presentation.Admin.Interfaces;
using Presentation.Admin.Services;
using ApplicationCore.Interfaces.Services.Home;
using Infrastructure.Services.Home;

namespace Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            // use real database
            // Requires LocalDB which can be installed with SQL Server Express 2016
            // https://www.microsoft.com/en-us/download/details.aspx?id=54284
            services.AddDbContext<AppDbContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("AppDbConnection")));

            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            // use real database
            // Requires LocalDB which can be installed with SQL Server Express 2016
            // https://www.microsoft.com/en-us/download/details.aspx?id=54284
            services.AddDbContext<AppDbContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("AppDbConnection")));

            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureCookieSettings(services);

            services.AddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(ISqlQueryRepository<>), typeof(SqlQueryRepository<>));
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAnnouncementService, AnnouncementService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IPollService, PollService>();
            services.AddTransient<IDeSerializeJwtToken, DeSerializeJwtToken>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<ITokenBuilder, TokenBuilder>();
            services.AddTransient<ISelectOptionService, SelectOptionService>();
            services.AddTransient<IEventValueFieldsService, EventValueFieldsService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IParticipantService, ParticipantService>();
            services.AddTransient<ISponsorService, SponsorService>();
            services.AddTransient<ISpeakerService, SpeakerService>();
            services.AddTransient<IImageHelper, ImageHelper>();
            services.AddTransient<IPendingSpeakerService, PendingSpeakerService>();
            services.AddTransient<IGuestService, GuestService>();
            services.AddTransient<IZoomApiService, ZoomApiService>();

            services.AddRouting(options =>
            {
                // Replace the type and the name used to refer to it with your own
                // IOutboundParameterTransformer implementation
                options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
            });

            services
               .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie()
               .AddJwtBearer(cfg =>
               {
                   cfg.RequireHttpsMetadata = true;
                   cfg.SaveToken = true;
                   cfg.TokenValidationParameters = new TokenValidationParameters()
                   {
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SYMMETRIC_SECURITY_KEY)),
                       ValidateAudience = false,
                       ValidateIssuer = false,
                       ValidateLifetime = false,
                       RequireExpirationTime = false,
                       ClockSkew = TimeSpan.Zero,
                       ValidateIssuerSigningKey = true
                   };
               });

            services.AddAutoMapper(c => c.AddProfile<AutoMappingProfile>(), typeof(Startup));

            services.AddCors(opt => opt.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder
                .WithOrigins(Configuration.GetValue<string>("AllowedHosts"))
                .AllowAnyMethod()
                .AllowAnyHeader();
                //.SetIsOriginAllowed((x) => true)
                //.AllowCredentials();
            }));

            services.AddSignalR();

            services.AddMvc(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));

            }).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginBindingModelValidator>());

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddHttpContextAccessor();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EDC Admin API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                });
            });

            // To avoid MultiPartBodyLength error
            services.Configure<FormOptions>(opt =>
            {
                opt.ValueLengthLimit = int.MaxValue;
                opt.MultipartBodyLengthLimit = int.MaxValue;
                opt.MemoryBufferThreshold = int.MaxValue;
            });
        }

        private static void ConfigureCookieSettings(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.Cookie = new CookieBuilder
                {
                    IsEssential = true // required for auth to work without explicit user consent; adjust to suit your privacy policy
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();

                        var responseText = JsonConvert.SerializeObject(new InternalServerErrorResponseModel(exceptionHandlerPathFeature.Error?.Message, exceptionHandlerPathFeature.Error?.StackTrace, exceptionHandlerPathFeature.Error?.Data));

                        await context.Response.WriteAsync(responseText);

                    });
                });
            //}

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("ApiCorsPolicy");

            // It's important that you place the Authentication and Authorization middleware between UseRouting and UseEndPoints .
            app.UseAuthentication();
            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            var title = Configuration.GetValue<string>("currentConfig:title");
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{title} API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapControllerRoute("default", "{controller:slugify=Account}/{action:slugify=Login}/{id?}/{slugifiedTitle?}");
            });
        }
    }
}
