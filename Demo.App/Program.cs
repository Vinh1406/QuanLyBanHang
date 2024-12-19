using Demo.Application;
using Demo.Domain.ApplicationServices.Users;
using Demo.Infrastructure;
using Demo.Persistence;
using Microsoft.EntityFrameworkCore;
using Demo.App;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//service off application
builder.Services.AddServiceApplication();
//service off infratruture
builder.Services.AddServiceInfrastucture();
builder.Services.AddOptionInfrastucture(builder.Configuration);
//
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSqlServerPersistence(builder.Configuration);
builder.Services.AddRepositotyUnitOfWork();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
InitDatabase(app);
app.Run();


void InitDatabase(IApplicationBuilder app)
{
    using var serviceScope=app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
    var dbcontext=serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbcontext.Database.Migrate();
    var userService = serviceScope.ServiceProvider.GetRequiredService<IUserService>();
    userService.InitializeUserAdminAsync().Wait();
}
