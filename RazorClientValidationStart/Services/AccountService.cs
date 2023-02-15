using System.Collections.Immutable;
using SkysFormsDemo.Data;

namespace SkysFormsDemo.Services;

public class AccountService : IAccountService
{
    private readonly ApplicationDbContext _context;

    public AccountService(ApplicationDbContext context)
    {
        _context = context;
    }
    public List<Account> GetAll()
    {
        return _context.Accounts.ToList();
    }

    public void Update(Account person)
    {
        _context.SaveChanges();
    }

    public Account GetAccount(int id)
    {
        return _context.Accounts.First(e => e.Id == id);
    }
}