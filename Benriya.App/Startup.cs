using Benriya.Core.Services;
using Benriya.Share.Abstractions;
using Benriya.Utils;
using Benriya.Utils.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
//using AutoMapper;
using System.Collections.Generic;
using System.Linq;
//using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Benriya.Data.Context;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Benriya.Share.ViewModels;
using ExtCore.Infrastructure;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.HttpOverrides;
using Benriya.Core.Middlewares;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Benriya.Core.Services.LoggingDB;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.FileProviders;
using Mapster;
using System.Reflection;
using Benriya.Core.Extensions;


//  dotnet watch --project Benriya.App run
//  https://marketplace.visualstudio.com/items?itemName=VisualStudioProductTeam.ProjectSystemTools
namespace Benriya.App
{
    public class Startup
    {
        private string _extensions_path;        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSetting;
        private readonly JwtOptions _JwtOptions;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            _extensions_path = env.ContentRootPath + this.Configuration["Extensions:Path"];
            var appSetting = new AppSettings();
            Configuration.GetSection(AppSettings.Settings).Bind(appSetting);
            _appSetting = appSetting;
            var JwtOptions = new JwtOptions();
            Configuration.GetSection(JwtOptions.Name).Bind(JwtOptions);
            _JwtOptions = JwtOptions;

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IRequestServices, RequestServices>();
            services.Configure<AppSettings>(Configuration.GetSection(AppSettings.Settings));
            services.Configure<JwtOptions>(Configuration.GetSection(JwtOptions.Name));
            services.AddExtCore(_extensions_path, Configuration["Extensions:IncludingSubpaths"] == true.ToString());
            
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (_env.IsDevelopment()) { options.EnableSensitiveDataLogging(); }
                options.UseNpgsql(Configuration.GetConnectionString("Default"));
            });
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            /*
            services.Configure<MongoSettings>(Configuration.GetSection(nameof(MongoSettings)));
            services.AddSingleton<IMongoSettings>(x => x.GetRequiredService<IOptions<MongoSettings>>().Value);
            services.AddSingleton<LoggingDBServices>();
            */

            services.AddScoped(typeof(IStorageContext), typeof(ApplicationDbContext));
            //string[] assembliesIngored = new string[] { "Microsoft.AspNetCore", "Microsoft.Extensions" };
            //services.AddServicesOfType<IScopedService>(assembliesIngored);
            //services.AddServicesWithAttributeOfType<ScopedServiceAttribute>(assembliesIngored);
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@_env.ContentRootPath + "/keys/")).UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA512
            });

            //services.Configure<FileStorageOptions>(options =>
            //{
            //    options.RootPath = $@"{_env.ContentRootPath}\FileStorage\Uploads";
            //    //options.Secret = "[Dropbox access token]";
            //    //options.RootPath = @"/";
            //});            
            //services.AddScoped<IKeyCachingServices, KeyCachingServices>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = _JwtOptions.RequireHttps;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_JwtOptions.SecurityKey)),
                    ValidateIssuer = _JwtOptions.ValidateIssuer,
                    ValidateAudience = _JwtOptions.ValidateAudience,
                    ValidateLifetime = true, // Check if the token is not expired and the signing key of the issuer is valid 
                    ValidIssuer = _JwtOptions.Issuer,
                    ValidAudience = _JwtOptions.Audience
                };
            });
            services.AddScoped<IUserServices,UserServices>();
            //services.AddSingleton<IUserServices,UserServices>();
            services.AddSingleton<IRequestServices,RequestServices>();
            services.AddControllers();
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            // List of public directory
            //services.AddDirectoryBrowser();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "RTAPP Web API",
                    Description = "RTAPP base API",
                    TermsOfService = null,
                    Contact = new OpenApiContact
                    {
                        Name = "Sornarin (Tom)",
                        Email = "tom.sornarin@gmail.com",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "<- Go back",
                        Url = new Uri(_appSetting.Hostname)
                    }
                });
                var dir = AppContext.BaseDirectory + _appSetting.DocumentsPath;
                string[] files = Directory.GetFiles(dir, "*.xml", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    c.IncludeXmlComments(Path.Combine(dir, file));
                }
                //var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } },
                // };
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme() {
                    Description = "API Key Authorization header using the custom scheme. Example: \"ApiKey:{key}\"",                    
                    Name = "ApiKey",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    },
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "ApiKey"
                       }
                      },
                      new string[] { }
                    }

                  });
            });           

            services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", opt => opt
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("X-Pagination"));
            });
            

            services.AddAuthorization(options =>
           {
               //https://stackoverflow.com/questions/42562660/recommended-best-practice-for-role-claims-as-permissions
               var perms = new ClaimPermission().GetPermissions();
               var extensions = new List<IExtension>();
               if (_appSetting.AssemblyCandidate)
               {
                   string[] starts = new string[] { "ExtCore" };
                   extensions = ExtensionManager.GetInstances<IExtension>().Where(x => !starts.Any(sl => x.Name.StartsWith(sl))).ToList();
               }
               else
                   extensions = ExtensionManager.GetInstances<IExtension>().ToList();

               foreach (var ext in extensions)
               {
                   foreach (var perm in perms)
                   {
                       var policy_name = $"{ext.Code}.{perm}";
                       options.AddPolicy(policy_name, policy => policy.RequireClaim(ClaimTypes.Role, policy_name));
                       //options.AddPolicy(policy_name, p => p.Requirements.Add(new PermissionRequirement(policy_name)));
                   }
               }
           });
            //https://devblogs.microsoft.com/aspnet/jwt-validation-and-authorization-in-asp-net-core/
            //services.AddSingleton<IAuthorizationHandler, MaximumOfficeNumberAuthorizationHandler>();

            // WASM
            services.AddRazorPages();
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {
            app.UseGCMiddleware();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.useswaggerui(c =>
                //{
                //   c.swaggerendpoint(url: "/swagger/v1/swagger.json", name: "benriya api documents");
                //   c.routeprefix = "api";
                //});
            }
            else
            {
                app.UseExceptionHandler("/errors/500");
                app.UseHsts();
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto //| ForwardedHeaders.All
                });
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseExceptionMiddleware();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            //app.UseSerilogRequestLogging();

            // WASM
            app.UseHttpsRedirection();
            //app.UseBlazorFrameworkFiles();
            app.UseBlazorFrameworkFiles();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            // Use auth to request static files
            //app.UseAuthentication();
            //app.UseRouting();
            //app.UseAuthorization();
            //app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, _appSetting.PublicDirectory)),
            //    RequestPath = "/FileStore",
            //});

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            
            app.UseEndpoints(endpoints =>
            {
                // WASM
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                // WASM
                endpoints.MapFallbackToFile("index.html");
                //endpoints.MapFallbackToPage("/index");
            });
            app.UseExtCore();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Benriya API documents");
                c.RoutePrefix = "api";
            });
            app.UseKeyCaching();
        }
    }
}
