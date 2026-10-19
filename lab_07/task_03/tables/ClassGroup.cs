using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Lab07.Task03.ClassGroupModule
{
    [Table("class_groups")]
    public class ClassGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("grade")]
        public int Grade { get; set; }

        [Required]
        [Column("internal_id")]
        public int InternalId { get; set; }

        [Required]
        [Column("letter_id")]
        public char LetterId { get; set; }

        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [Column("cabinet_id")]
        public int CabinetId { get; set; }
    }
}