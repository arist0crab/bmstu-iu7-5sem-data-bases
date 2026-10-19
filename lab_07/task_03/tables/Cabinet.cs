using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Lab07.Task03.CabinetModule
{
    [Table("cabinets")]
    public class Cabinet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("number")]
        public int Number { get; set; }
        
        [Required]
        [Column("rows_count")]
        public int RowsCount { get; set; }
        
        [Required]
        [Column("cols_count")]
        public int ColsCount { get; set; }
    }
}