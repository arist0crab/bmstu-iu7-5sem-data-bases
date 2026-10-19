using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab07.Task03.ClassGroupModule;


namespace Lab07.Task03.StudentModule
{
    [Table("students")]
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("first_name")]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("last_name")]
        public string? LastName { get; set; }

        [Required]
        [Column("sex")]
        public char Sex { get; set; }

        [Required]
        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Column("class_group_id")]
        public int ClassGroupId { get; set; }

        [ForeignKey("ClassGroupId")]
        public ClassGroup? StudentClassGroup { get; set; }
        
        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}