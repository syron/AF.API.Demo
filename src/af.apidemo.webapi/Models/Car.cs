using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace af.apidemo.webapi.Models
{
    public class Car
    {
        public Car() { }
        public Car(CarEntity car)
        {
            this.CarId = int.Parse(car.RowKey);
            this.Make = car.PartitionKey;
            this.Year = car.Year;
            this.Color = car.Color;
        }
        public int CarId { get; set; }
        public string Make { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
    }
}