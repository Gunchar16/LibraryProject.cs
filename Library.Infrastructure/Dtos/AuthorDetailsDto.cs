using Library.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dtos
{
    public record AuthorDetailsDto
    (
        [Required]
        int Id,
        [Required]
        [MaxLength(50)]
        string FirstName,
        [Required]
        [MaxLength(50)]
        string LastName,

        [Required]
        int YearOfBirth
    );
}
