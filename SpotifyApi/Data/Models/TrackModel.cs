using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    [Table("Track")]
    [Index(nameof(ISRC), IsUnique = true)]
    public class TrackModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string ISRC { get; set; }
        public string Name { get; set; }
        public int duration_ms { get; set; }
        public bool is_explicit { get; set; }
    }
}
