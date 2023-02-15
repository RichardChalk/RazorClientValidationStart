using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkysFormsDemo.Services;

namespace SkysFormsDemo.Pages.Account
{
    [BindProperties]
    public class DepositModel : PageModel
    {
        private readonly IAccountService _accountService;

        [Range(10,3000)]
        public int Amount { get; set; }

        public DateTime DateWhen { get; set; }

        [Required(ErrorMessage = "Skriv en en kommentar dummer")]
        [MinLength(5)]
        [MaxLength(100)]
        public string Comment { get; set; }

        public DepositModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public void OnGet(int accountId )
        {
            DateWhen = DateTime.Now.AddDays(1).Date;
            Amount = 100;
        }


        public IActionResult OnPost(int accountId)
        {
            if (DateWhen < DateTime.Now.AddDays(1).Date)  //2022-01-11 00:00
            {
                ModelState.AddModelError("DateWhen","Datum måste vara minst en dag fram ");
            }
            if (ModelState.IsValid)
            {
                var account = _accountService.GetAccount(accountId);
                account.Balance += Amount;
                _accountService.Update(account);
                return RedirectToPage("Index");
            }

            return Page();
        }

    }
}
