using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Entities;

[Table("sale")]
public partial class Sale
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [Column("qty")]
    public int? Qty { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Sales")]
    public virtual Product? Product { get; set; }
}
