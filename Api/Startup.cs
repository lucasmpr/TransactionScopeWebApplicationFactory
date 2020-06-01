using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Startup
    {
        string connString = $"Server=.\\SQLEXPRESS;Database={Guid.NewGuid().ToString()};Integrated Security=True;";

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IPostsRepository, PostsRepository>();
            
            services.AddDbContext<BloggingContext>(options =>           options.UseSqlServer(connString));
            services.AddControllers();
            
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}