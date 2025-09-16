using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repositories;
using api.Services;
using api.Token;
using api.Token;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddScoped<AdminSeeder>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure() 
    )
);


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User", policy => policy.RequireRole("User"));
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});


//cors 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


builder.Services.AddAutoMapper(typeof(UserMapper));
builder.Services.AddAutoMapper(typeof(PetMapper));
builder.Services.AddAutoMapper(typeof(ReviewMapper));
builder.Services.AddAutoMapper(typeof(SpecialPackageMapper));
builder.Services.AddAutoMapper(typeof(RentMapper));

builder.Services.AddScoped<IPet, PetRepository>();
builder.Services.AddScoped<IFavoritePets, FavoritePetsRepository>();
builder.Services.AddScoped<IReview, ReviewRepository>();
builder.Services.AddScoped<ISpecialPackage, SpecialPackageRepository>();
builder.Services.AddScoped<IRentInfo, RentInfoRepository>();

builder.Services.AddScoped<IToken, TokenService>();
builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddHttpContextAccessor();



builder.Services.AddScoped<Query>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<api.GraphQL.Query>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization();



builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    dbContext.Database.EnsureCreated(); // kreira tabele u Railway MySQL
//}



using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        var created = context.Database.EnsureCreated();
        if (created)
        {
            Console.WriteLine("Database 'PetBuddyApi' created successfully!");
        }
        else
        {
            Console.WriteLine("Database 'PetBuddyApi' already exists.");
        }

        // Alternative: If you're using migrations, use this instead:
        // context.Database.Migrate();

        Console.WriteLine("Database is ready!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database creation failed: {ex.Message}");
        throw;
    }
}


using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        Console.WriteLine("Seeding roles...");
        await RoleSeeder.SeedRolesAsync(serviceProvider);
        Console.WriteLine("Roles seeded successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Role seeding failed: {ex.Message}");
    }
}

// Seed admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var adminSeeder = services.GetRequiredService<AdminSeeder>();
    try
    {
        Console.WriteLine("Seeding admin user...");
        await adminSeeder.SeedAdminAsync();
        Console.WriteLine("Admin user seeded successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Admin seeding failed: {ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.MapGraphQL("/graphql")
.WithOptions(new GraphQLServerOptions
{
    Tool = { Enable = true }
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.MapGet("/", () => "API radi!");

//app.Run();
app.Run("http://0.0.0.0:8085");