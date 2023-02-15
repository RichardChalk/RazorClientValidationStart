using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SkysFormsDemo.Data;

public class DataInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public DataInitializer(ApplicationDbContext dbContext,
        UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public void SeedData()
    {
        _dbContext.Database.Migrate();
        SeedAccounts();
        SeedCountries();
        SeedRoles();
        SeedUsers();
        SeedPersons();
        SeedProducts();
    }

    private void SeedProducts()
    {
        while (_dbContext.Products.Count() < 100)
        {
            _dbContext.Products.Add(GenerateProduct());
            _dbContext.SaveChanges();
        }
    }

    private Product GenerateProduct()
    {
        var testUser = new Faker<Product>()
            .StrictMode(true)
            .RuleFor(e => e.Id, f => 0)
            .RuleFor(e => e.Name, (f, u) => f.Commerce.Product())
            .RuleFor(e => e.Color, (f, u) => f.Commerce.Color())
            .RuleFor(e => e.Created, (f, u) => f.Date.Past())
            .RuleFor(e => e.LastBought, (f, u) => f.Date.Recent())
            .RuleFor(e => e.Ean13, (f, u) => f.Commerce.Ean13())
            .RuleFor(e => e.PopularityPercent, (f, u) => f.Random.Number(0, 100))
            .RuleFor(e => e.Price, (f, u) => Convert.ToDecimal(f.Commerce.Price()));

        var person = testUser.Generate(1).First();
        return person;
    }

    private void SeedPersons()
    {
        while (_dbContext.Person.Count() < 10)
        {
            _dbContext.Person.Add(GeneratePerson());
            _dbContext.SaveChanges();
        }

        foreach (var person in _dbContext.Person.Include(e => e.OwnedCars).ToList())
        {
            if (!person.OwnedCars.Any())
            {
                GenerateCars(person);
                _dbContext.SaveChanges();
            }
        }
    }

    private void GenerateCars(Person person)
    {
        person.OwnedCars = GetCars();
    }

    private Person GeneratePerson()
    {
        var testUser = new Faker<Person>()
            .StrictMode(true)
            .RuleFor(e => e.Id, f => 0)
            .RuleFor(e => e.Name, (f, u) => f.Name.FullName())
            .RuleFor(e => e.StreetAddress, (f, u) => f.Address.StreetAddress())
            .RuleFor(e => e.City, (f, u) => f.Address.City())
            .RuleFor(e => e.CarCount, (f, u) => f.Random.Number(3))
            .RuleFor(e => e.Country, (f, u) => _dbContext.Countries.First())
            .RuleFor(e => e.Email, (f, u) => f.Internet.Email())
            .RuleFor(e => e.PostalCode, (f, u) => f.Address.ZipCode())
            .RuleFor(e => e.Registered, (f, u) => f.Date.Past())
            .RuleFor(e => e.LastModified, (f, u) => f.Date.Recent())
            .RuleFor(e => e.Salary, (f, u) => f.Finance.Amount() * 20)
            .RuleFor(e => e.Position, (f, u) => PlayerPosition.Center)
            .RuleFor(e => e.OwnedCars, (f, u) => GetCars());


        var person = testUser.Generate(1).First();
        return person;
    }

    private List<Car> GetCars()
    {
        var testUser = new Faker<Car>()
            .StrictMode(true)
            .RuleFor(e => e.Id, f => 0)
            .RuleFor(e => e.Vin, (f, u) => f.Vehicle.Vin())
            .RuleFor(e => e.Model, (f, u) => f.Vehicle.Model())
            .RuleFor(e => e.Fuel, (f, u) => f.Vehicle.Fuel())
            .RuleFor(e => e.Manufacturer, (f, u) => f.Vehicle.Manufacturer())
            .RuleFor(e => e.Type, (f, u) => f.Vehicle.Type())
            .RuleFor(e => e.BoughtDate, (f, u) => f.Date.Past(19, DateTime.Now));

        return testUser.Generate(new Random().Next(5, 30));

    }


    private void SeedUsers()
    {
        AddUserIfNotExists("richard.chalk@systementor.se", "Hejsan123#", new string[] { "Admin" });
        AddUserIfNotExists("richard.chalk@customer.systementor.se", "Hejsan123#", new string[] { "Customer" });
    }


    private void SeedRoles()
    {
        AddRoleIfNotExisting("Admin");
        AddRoleIfNotExisting("Customer");
    }


    private void AddRoleIfNotExisting(string roleName)
    {
        var role = _dbContext.Roles.FirstOrDefault(r => r.Name == roleName);
        if (role == null)
        {
            _dbContext.Roles.Add(new IdentityRole { Name = roleName, NormalizedName = roleName });
            _dbContext.SaveChanges();
        }
    }


    private void AddUserIfNotExists(string userName, string password, string[] roles)
    {
        if (_userManager.FindByEmailAsync(userName).Result != null) return;

        var user = new IdentityUser
        {
            UserName = userName,
            Email = userName,
            EmailConfirmed = true
        };
        _userManager.CreateAsync(user, password).Wait();
        _userManager.AddToRolesAsync(user, roles).Wait();
    }



    private void SeedCountries()
    {
        AddCountryIfNotExists("FI", "Finland");
        AddCountryIfNotExists("SE", "Sverige");
        AddCountryIfNotExists("DK", "Danmark");
        AddCountryIfNotExists("NO", "Norge");
    }

    private void AddCountryIfNotExists(string code, string namn)
    {
        if (_dbContext.Countries.Any(r => r.CountryCode == code)) return;
        _dbContext.Countries.Add(new Country { CountryCode = code, Name = namn });
        _dbContext.SaveChanges();
    }

    private void SeedAccounts()
    {
        AddAccountIfNotExists("12345");
        AddAccountIfNotExists("55555");
    }

    private void AddAccountIfNotExists(string accountNo)
    {
        if (_dbContext.Accounts.Any(e => e.AccountNo == accountNo)) return;
        _dbContext.Accounts.Add(new Account
        {
            AccountNo = accountNo,
            Balance = 1000
        });
        _dbContext.SaveChanges();
    }
}