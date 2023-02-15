using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkysFormsDemo.Data;

namespace RazorAjax3UseCasesEnd.Pages.Person
{
    public class ViewModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public ViewModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string Name { get; set; }
        public int Id { get; set; }

        public class Car
        {
            public int Id { get; set; }
            public string Vin { get; set; }
            public string Manufacturer { get; set; }
            public string Model { get; set; }
            public string Type { get; set; }
            public string Fuel { get; set; }
            public DateTime BoughtDate { get; set; }
        }

        public IActionResult OnGetShowMore(int personId, long lastTicks)
        {
            DateTime dateOfLastShown = new DateTime(lastTicks)
                .AddMilliseconds(100);

            var listOfCars = _dbContext.Person
                .Where(p => p.Id == personId)
                .SelectMany(p => p.OwnedCars)
                .Where(ca => lastTicks == 0 || ca.BoughtDate > dateOfLastShown)
                .OrderBy(ca => ca.BoughtDate)
                .Take(5)
                .Select(c => new Car
                {
                    BoughtDate = c.BoughtDate,
                    Id = c.Id,
                    Model = c.Model,
                    Fuel = c.Fuel,
                    Manufacturer = c.Manufacturer,
                    Type = c.Type,
                    Vin = c.Vin
                }).ToList();

            if (listOfCars.Any())
                lastTicks = listOfCars.Last().BoughtDate.Ticks;
            return new JsonResult(new { cars = listOfCars, lastTicks });
        }

        public IActionResult OnGetFetchValue(int id)
        {
            // Detta är så klart löjligt!
            // Jag vill bara demonstrera att här gör man sin "dyr" beräkning
            return new JsonResult(new
            {
                value = id * 1000
            });
        }
        public void OnGet(int personId)
        {
            var person = _dbContext.Person
                .First(person => person.Id == personId);
            Id = personId;
            Name = person.Name;
        }
    }
}
