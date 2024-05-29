using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabkaDB2.Models;

public partial class Rent : Entity
{
    public int Id { get; set; }

    [Display(Name = "Користувач")]
    public int CustId { get; set; }
    [Display(Name = "Автомобіль")]
    public int CarId { get; set; }
    [Display(Name = "Зона каршерингу")]
    public int Cszid { get; set; }
    [Display(Name = "Дата початку")]
    public DateOnly StartDate { get; set; }
    [Display(Name = "Дата кінця")]
    public DateOnly FinishDate { get; set; }
    [Display(Name = "Добова вартість")]
    public double CostPerDay { get; set; }
    [Display(Name = "Авто")]
    public virtual Car Car { get; set; } = null!;
    [Display(Name = "Зона")]
    public virtual CarSharingZone Csz { get; set; } = null!;
    [Display(Name = "Замовник")]
    public virtual Customer Cust { get; set; } = null!;
}
