using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using api6test.BLL;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
            buildersw =>
            {
                buildersw.WithOrigins("http://localhost:8080","http://172.16.3.119:8080/")
                .SetIsOriginAllowed(l => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
                //builder.WithOrigins("http://127.0.0.1:5500","http://172.16.3.73:5500");
                //跨域请求时，授权http://127.。。。。。。等可以访问
            });
    });

builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApiContext")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddTransient<StudentService>();//注册业务逻辑层
builder.Services.AddScoped<OrderbillService>();//注册业务逻辑层,

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);//增加跨域支持设置
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
