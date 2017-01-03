using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Termoservis.Models
{
    public class WorkItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [DisplayName("Datum")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; } = DateTime.Today;

        [DisplayName("Cijena")]
        public int Price { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Opis")]
        public string Description { get; set; } = string.Empty;
        
        [DisplayName("Vrsta")]
        public WorkItemType Type { get; set; } = WorkItemType.Service;

        [DisplayName("Serviser")]
        public long? WorkerId { get; set; }

        [DisplayName("Serviser")]
        [ForeignKey(nameof(WorkerId))]
        public Worker Worker { get; set; }
    }
}