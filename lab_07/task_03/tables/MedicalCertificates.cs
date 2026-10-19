using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab07.Task03.StudentModule;

namespace Lab07.Task03.MedicalCertificatesModule
{
    [Table("medical_certificates")]
    public class MedicalCertificates
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("student_id")]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? MedicalCertificateStudent { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("diagnosis")]
        public string? Diagnosis { get; set; }

        [Required]
        [Column("issue_date")]
        public DateTime IssueDate { get; set; }

        [Column("expire_date")]
        public DateTime ExpireDate { get; set; }

        [Column("recommended_zone")]
        public int[]? RecommendedZone { get; set; }
    }
}