using RabbitMqExample.Common;
using RabbitMqExample.Common.Models;

namespace RabbitMqExample.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var conf = builder.Configuration;

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Services
            RabbitMqSettings? rabbitMqSettings = conf.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();
            builder.Services.AddSingleton<IRabbitMqSettings>(rabbitMqSettings);
            builder.Services.RabbitMqRegistiration(rabbitMqSettings);


            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}