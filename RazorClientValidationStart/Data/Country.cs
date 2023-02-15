using System.ComponentModel.DataAnnotations;

namespace SkysFormsDemo.Data;

public class Country
{
    public int Id { get; set; }

    [MaxLength(2)]
    public string CountryCode { get; set; }

    [MaxLength(50)]
    public string Name{ get; set; }

}