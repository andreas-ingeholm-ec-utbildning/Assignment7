using System.ComponentModel.DataAnnotations;

namespace Crito.Models;

public class EmailListForm
{

    [Required, EmailAddress]
    public string Email { get; set; } = null!;

}
