using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Entities
{
    public interface IPrimaryKey
    {
        int Id { get; set; }
    }

    public abstract class BaseEntity : IPrimaryKey
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
