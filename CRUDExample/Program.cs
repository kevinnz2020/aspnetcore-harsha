using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;
using System.Reflection.Emit;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// add services into IoC container
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

builder.Services.AddDbContext<PersonsDbContext>
    (options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
///Data Source=(localdb)\ProjectModels;Initial Catalog=PersonsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False
///
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<PersonsDbContext>();
        //DbInitializer.InitializeCountries(context);
        DbInitializer.InitializePersons(context);

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}


if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public static class DbInitializer
{
    public static void InitializeCountries(PersonsDbContext context)
    {

        string countriesJson = System.IO.File.ReadAllText("countries.json");
        List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

        foreach (Country country in countries)
        {
            context.Countries.Add(country);
        }

                  
        
        context.SaveChanges();
    }

    public static void InitializePersons(PersonsDbContext context)
    {

        //Seed to Countries
        string personsJson = System.IO.File.ReadAllText("persons.json");
        List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

        foreach (Person person in persons)
        {
            context.Persons.Add(person);    
        }

        



        context.SaveChanges();
    }
}