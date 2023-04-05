using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Library.Infrastructure.Entities
{
    public class Book : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(1000)]
        public string Image { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        [Required]
        public bool IsTaken { get; set; }

        [AllowNull]
        public int? TakenById { get; set; }

        [ForeignKey(nameof(TakenById))]
        public virtual User TakenBy { get; set; }

        public virtual ICollection<BookAuthor> BookAuthors { get; set; }

    }
}
