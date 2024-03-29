﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConsultorioApi.Core;
using ConsultorioApi.DataAccess;
using ConsultorioApi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.Hosting;
using ConsultorioApi.Web;
using AutoMapper;
using ConsultorioApi.Core.Interfaces;
using ConsultorioApi.Core.Bussiness;

namespace ConsultorioApi
{
    /// <summary>
    /// Clase Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy.WithOrigins(new[] { "http://localhost:52510" })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("conteo", "totalPaginas")
                    .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization, "x-custom-header")
                    .AllowCredentials());
            });

            // Creamos la conexion a base de datos
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            // Agregamos el servicio para conectarnos por medio de la base de datos a los usuarios y roles
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();

            services.AddMvc(option => option.EnableEndpointRouting=false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Agregamos Automapper
            services.AddAutoMapper(typeof(Startup));

            // CONFIGURACIÓN DEL SERVICIO DE AUTENTICACIÓN JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["JWT:key"])
                        ),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            SetTransient(services);
            services.AddHttpContextAccessor();

            // Register the Swagger generator, defining one or more Swagger documents
            AddSwagger(services);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseCors();
            // Se utiliza para mostrar los archivos que estan en la wwwroot
            app.UseStaticFiles();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerConfiguration.EndpointUrl, SwaggerConfiguration.EndpointDescription);
                c.DocExpansion(DocExpansion.None);
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }

        private void SetTransient(IServiceCollection services)
        {
            services.AddSingleton<IConnectionFactory, ConnectionFactory>(_ => new ConnectionFactory(Configuration.GetConnectionString("defaultConnection")));
            services.AddSingleton<IBaseRepository, BaseRepository>();
            services.AddScoped<ICompaniaReporitorio, CompaniaReporitorio>();
            services.AddScoped<ICompania, Compania>();
            services.AddScoped<ICuentasRepositorio, CuentasRepositorio>();
            services.AddScoped<ICuentas, Cuentas>();
            services.AddScoped<ICatalogoRepositorio, CatalogoRepositorio>();
            services.AddScoped<ICatalogo, Catalogo>();
            services.AddScoped<IAlmacenadorDeArchivos, AlmacenadorArchivosLocal>();
        }

        private void AddSwagger(IServiceCollection services)
        {
            var contact = new OpenApiContact()
            {
                Name = SwaggerConfiguration.ContactName,
                Url = new System.Uri(SwaggerConfiguration.ContactUrl),
                Email = SwaggerConfiguration.Email
            };

            services.AddSwaggerGen(swagger =>
            {
                swagger.EnableAnnotations();
                // Agrega el control de seguridad y la descripcion de esta
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with the Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                // Configura el como estara llegando las credenciales para validar en el sistema
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
                //Información de la documentación
                swagger.SwaggerDoc(
                    SwaggerConfiguration.DocNameV1,
                    new OpenApiInfo
                    {
                        Title = SwaggerConfiguration.DocInfoTitle,
                        Version = SwaggerConfiguration.DocInfoVersion,
                        Description = SwaggerConfiguration.DocInfoDescription,
                        Contact = contact
                    }
                );
                var filePath = Path.Combine(AppContext.BaseDirectory, "ConsultorioApi.Web.xml");
                swagger.IncludeXmlComments(filePath);
            });
        }

    }
}
