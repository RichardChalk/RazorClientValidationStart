using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace SkysFormsDemo.Data;

public class Account
{
    public int Id { get; set; }

    [StringLength(20)]
    public string AccountNo { get; set; }

    [Required]
    public decimal Balance { get; set; }
}