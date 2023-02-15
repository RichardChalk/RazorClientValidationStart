using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkysFormsDemo.Services;

namespace SkysFormsDemo.Pages.Account
{
    [BindProperties]
    public class WithdrawModel : PageModel
    {
    private readonly IAccountService _accountService;
    private readonly IRealAccountService _realAccountService;

    [Range(10, 3000)]
    public int Amount { get; set; }


    public WithdrawModel(IAccountService accountService, IRealAccountService realAccountService)
    {
        _accountService = accountService;
        _realAccountService = realAccountService;
    }

    public void OnGet(int accountId)
    {
        Amount = 100;
    }


    public IActionResult OnPost(int accountId)
    {


        if (ModelState.IsValid)
        {
            var status = _realAccountService.Withdraw(accountId, Amount);
            if(status == IRealAccountService.ErrorCode.Ok)
            {
                return RedirectToPage("Index");
            }
            ModelState.AddModelError("Amount", "Beloppet är fel");
        }

        return Page();


        //var account = _accountService.GetAccount(accountId);
        //if (account.Balance < Amount)
        //{
        //    ModelState.AddModelError("Amount", "För stort belopp");
        //}



        //    if (ModelState.IsValid)
        //{
        //    account.Balance -= Amount;
        //    _accountService.Update(account);
        //    return RedirectToPage("Index");
        //}

        return Page();
    }

}}
