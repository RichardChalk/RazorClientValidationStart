using SkysFormsDemo.Data;

namespace SkysFormsDemo.Services;

public interface IPersonService
{
    public IEnumerable<Person> GetPersons();
    int SaveNew(Person person);

    void Update(Person person);
    Person GetPerson(int personId);
}