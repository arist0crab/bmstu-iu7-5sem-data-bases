using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab07.Task03.SeatModule;
using Lab07.Task03.StudentModule;


namespace Lab07.Task03.SeatingsAssignmentsModule
{
    [Table("seatings_assignments")]
    public class SeatingsAssignments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("student_id")]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? SeatingsAssignmentStudent { get; set; }
        
        [Required]
        [Column("seat_id")]
        public int SeatId { get; set; }

        [ForeignKey("SeatId")]
        public Seat? SeatingsAssignmentSeat { get; set; }
        
        [Required]
        [Column("start_date")]
        public DateTime Startdate { get; set; }
        
        [Column("end_date")]
        public DateTime EndDate { get; set; }
    }
}