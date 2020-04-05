using System;
using System.ComponentModel.DataAnnotations;

namespace Beng.Specta.Compta.Core.DTOs
{
    public class BaseDTO
    {
        [Key]
        public long Id { get; set; }
    }
}
