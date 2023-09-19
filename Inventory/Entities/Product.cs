using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Entities;

[Table("product")]
public partial class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column("price", TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
