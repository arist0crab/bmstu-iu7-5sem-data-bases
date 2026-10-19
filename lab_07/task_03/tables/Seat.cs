using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Lab07.Task03.DeskModule;


namespace Lab07.Task03.SeatModule
{
    [Table("seats")]
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("desk_id")]
        public int DeskId { get; set; }

        [ForeignKey("DeskId")]
        public Desk? SeatDesk { get; set; }

        [Required]
        [Column("seat_index")]
        public int SeatIndex { get; set; }
    }
}