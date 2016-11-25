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
        public DateTime? Date { get; set; }

        [DisplayName("Cijena")]
        public int Price { get; set; }

        [DisplayName("Opis")]
        public string Description { get; set; }

        [DisplayName("Vrsta")]
        public WorkItemType Type { get; set; }

        [DisplayName("Uredaj")]
        public long? DeviceId { get; set; }

        [DisplayName("Uredaj")]
        [ForeignKey(nameof(DeviceId))]
        public CustomerDevice Device { get; set; }
    }
}