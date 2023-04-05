using Library.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dtos
{
    public record BookDetailsDto
    (
        [Required]
        int Id,
        [Required]
        string Title,

        [MaxLength(500)]
        string Description,

        [MaxLength(1000)]
        string Image,

        [Range(0, 5)]
        double Rating,

        [Required]
        DateTime PublicationDate,

        [Required]
        bool IsTaken,

        [Required]
        IEnumerable<AuthorDetailsDto> Authors
    );
}
