using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab07.Task03.CabinetModule;


namespace Lab07.Task03.DeskModule
{
    [Table("desks")]
    public class Desk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("cabinet_id")]
        public int CabinetId { get; set; }

        [ForeignKey("CabinetId")]
        public Cabinet? DeskCabinet { get; set; }

        [Required]
        [Column("row_index")]
        public int RowIndex { get; set; }

        [Required]
        [Column("col_index")]
        public int ColIndex { get; set; }

        [Required]
        [Column("seats_count")]
        public int SeatsCount { get; set; }
    }
}