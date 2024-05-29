using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabkaDB2.Models;

public partial class Model : Entity
{
    public int Id { get; set; }

    [Display(Name = "Назва моделі")]
    public string ModelName { get; set; } = null!;
    [Display(Name = "Кількість сидінь")]

    public int NumOfSeats { get; set; }
    [Display(Name = "Коробка передач")]
    public bool IsAutomatic { get; set; }
    [Display(Name = "Об'єм двигуна")]
    public double EngineVolume { get; set; }
    [Display(Name = "Кондиціонер")]
    public bool HasCooling { get; set; }
    [Display(Name = "Бренд")]
    public string Brand { get; set; } = null!;
    [Display(Name = "Клас")]
    public int Type { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
