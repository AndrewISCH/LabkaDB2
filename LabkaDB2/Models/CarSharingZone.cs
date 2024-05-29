using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabkaDB2.Models;

public partial class CarSharingZone : Entity
{
    public int Id { get; set; }
    [Display(Name = "Місто")]
    public string Town { get; set; } = null!;
    [Display(Name = "Вмістимість авто")]
    public int CarCapacity { get; set; }
    [Display(Name = "Широта")]
    public decimal? Latitude { get; set; }
    [Display(Name = "Довгота")]
    public decimal? Longtitude { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();
}
