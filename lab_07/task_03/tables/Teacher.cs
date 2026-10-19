using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Lab07.Task03.TeacherModule
{
    [Table("students")]
    public class Teacher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("first_name")]
        public string? Firstname { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("last_name")]
        public string? LastName { get; set; }

        [Required]
        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        [Column("mentor_techer_id")]
        public int? MentorTeacherId { get; set; }

        [ForeignKey("MentorTeacherId")]
        public Teacher? MentorTeacher { get; set; }
    }
}