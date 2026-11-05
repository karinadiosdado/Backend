using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;

public class BeerInsertDto
{
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres.")]
    public string Name { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El BrandId debe ser un valor positivo.")]
    public int BrandId { get; set; }

    [Range(0.0, 100.0, ErrorMessage = "El alcohol debe estar entre 0.0 y 100.0.")]
    public decimal Alcohol { get; set; }
}
