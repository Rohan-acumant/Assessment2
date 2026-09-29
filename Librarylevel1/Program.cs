using Librarylevel1.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new BookRepository(Path.Combine(builder.Environment.ContentRootPath, "books.json")));
builder.Services.AddSingleton(new UserRepository(Path.Combine(builder.Environment.ContentRootPath, "users.json")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();