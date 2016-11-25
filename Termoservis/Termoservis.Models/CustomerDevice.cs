using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Termoservis.Models
{
    public class CustomerDevice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [DisplayName("Model")]
        public string Name { get; set; }

        [DisplayName("Proizvodac")]
        public string Manufacturer { get; set; }

        [DisplayName("Pusten u pogon")]
        public DateTime? CommissionDate { get; set; }
    }
}