using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SkysFormsDemo.Data;
using SkysFormsDemo.Services;

namespace SkysFormsDemo.Pages.Person
{
    //[Authorize(Roles="Admin")]
    [BindProperties]
    public class NewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        
        [StringLength(100)]
        public string StreetAddress { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }


        [Range(0, 100000, ErrorMessage = "Skriv ett tal mellan 0 och 100000")]
        public decimal Salary { get; set; }


        [Range(0, 100)]
        public int CarCount { get; set; } //Krysta fram ett int-usecase

        [StringLength(50)]
        [Required]
        public string City { get; set; }

        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [Range(1,10000, ErrorMessage = "Välj en country tack")]
        public int CountryId { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public PlayerPosition Position { get; set; }
        public List<SelectListItem> PlayerPositions { get; set; }

        public NewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            FillCountryList();
            FillPositionsList();

        }

        private void FillPositionsList()
        {
            PlayerPositions = Enum.GetValues<PlayerPosition>()
                .Select(r => new SelectListItem
                {
                    Value = r.ToString(),
                    Text = r.ToString()
                }).ToList();
        }

        private void FillCountryList()
        {
            Countries = _context.Countries.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString()
            }).ToList();
            Countries.Insert(0,new SelectListItem
            {
                Text = "Var god välj...",
                Value = "0"
            });
        }


        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var person = new Data.Person
                {
                    CarCount = CarCount,
                    StreetAddress = StreetAddress,
                    Email = Email,
                    City = City,
                    Country = _context.Countries.First(e=>e.Id == CountryId),
                    Salary = Salary,
                    Name = Name,
                    PostalCode = PostalCode,
                    LastModified = DateTime.UtcNow,
                    Registered = DateTime.UtcNow
                };
                _context.Person.Add(person);
                _context.SaveChanges();
                return RedirectToPage("Index");
            }

            FillCountryList();
            return Page();
        }

    }
}
