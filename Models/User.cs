namespace DotnetAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Index(nameof(User.Email), IsUnique = true)]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column(name:"Created_at")]
    public DateTime CreatedAt { get; set; }

    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}
