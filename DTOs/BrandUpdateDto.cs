using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;

public class BrandUpdateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres.")]
    public string Name { get; set; }
}
