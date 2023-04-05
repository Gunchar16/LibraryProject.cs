using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dtos
{
    public record BookUpdateDto(
            [Required]
    [MaxLength(100)]
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
    IEnumerable<int> AuthorIds
        );
}
