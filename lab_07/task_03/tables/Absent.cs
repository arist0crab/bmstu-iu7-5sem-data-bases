using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab07.Task03.StudentModule;


namespace Lab07.Task03.AbsentModule
{
    [Table("absents")]
    public class Absent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("student_id")]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? AbsentingStudent { get; set; }

        [Required]
        [Column("absent_date")]
        public DateTime AbsentDate { get; set; }

        [Required]
        [Column("reason")]
        [MaxLength(100)]
        public string? Reason { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(100)]
        public string? Status { get; set; }
    }
}