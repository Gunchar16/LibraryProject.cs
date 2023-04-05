using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dtos
{
    public record AuthorAddDto([Required][MaxLength(50)]string FirstName, [Required][MaxLength(50)]string LastName,[Required] int YearOfBirth);
}
