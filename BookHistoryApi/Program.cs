using BaseMongoDb;
using BaseRabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.UseRabbitMqLib();
builder.UseMongoDatabase("MongoDbConfig");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigins",
    builder =>
    {
        builder.WithOrigins(
                            "http://localhost",
                            "http://localhost:4200",
                            "http://20.226.66.139",
                            "http://rafaelsenfflabs.brazilsouth.cloudapp.azure.com"
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowAngularOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
