using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabkaDB2.Models;

public partial class Customer : Entity
{
    public int Id { get; set; }
    [Display(Name = "Ім'я")]
    public string FirstName { get; set; } = null!;
    [Display(Name = "Прізвище")]

    public string LastName { get; set; } = null!;
    [Display(Name = "Дата створення")]
    public DateOnly CreationDate { get; set; }
    [Display(Name = "Телефон")]
    public string Phone { get; set; } = null!;
    [Display(Name = "Пошта")]
    public string Email { get; set; } = null!;
    [Display(Name = "Кількість замовлень")]
    public int NumOfOrders { get; set; }
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;

    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();
}
