using SkysFormsDemo.Data;

namespace SkysFormsDemo.Services;

public class PersonService : IPersonService
{
    private readonly ApplicationDbContext _context;

    public PersonService(ApplicationDbContext context)
    {
        _context = context;
    }
    public IEnumerable<Person> GetPersons()
    {
        return _context.Person;
    }

    public int SaveNew(Person person)
    {
        person.Registered = DateTime.UtcNow;
        person.LastModified = DateTime.UtcNow;
        _context.Person.Add(person);
        _context.SaveChanges();
        return person.Id;
    }

    public void Update(Person person)
    {
        person.LastModified = DateTime.UtcNow;
        _context.SaveChanges();
    }

    public Person GetPerson(int personId)
    {
        return _context.Person.First(e => e.Id == personId);
    }
}