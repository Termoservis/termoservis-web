using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Termoservis.Models
{
    public class WorkItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime? Date { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public WorkItemType Type { get; set; }

        public long? DeviceId { get; set; }

        [ForeignKey(nameof(DeviceId))]
        public CustomerDevice Device { get; set; }
    }
}