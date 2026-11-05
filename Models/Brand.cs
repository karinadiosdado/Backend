using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models;

public class Brand
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BrandId { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Beer> Beers { get; set; } = new List<Beer>();
}
