using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabkaDB2.Models;

public partial class Car : Entity
{
    public int Id { get; set; }
    [Display(Name = "Модель")]
    public int ModelId { get; set; }
    [Display(Name = "Зона каршерінгу")]
    public int Cszid { get; set; }

    [Display(Name = "Орендується")]
    public bool IsRented { get; set; }

    [Display(Name = "Рік виготовлення")]
    public DateOnly ProduceYear { get; set; }
    [Display(Name = "Дата тех.огляду")]
    public DateOnly? TechInspirationDate { get; set; }
    [Display(Name = "Колір")]
    public string Color { get; set; } = null!;

    [Display(Name = "Зона")]
    public virtual CarSharingZone Csz { get; set; } = null!;

    [Display(Name = "Модель")]
    public virtual Model Model { get; set; } = null!;

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();
}
