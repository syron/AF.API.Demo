using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace af.apidemo.webapi.Models
{
    public class CarEntity : TableEntity
    {
        public CarEntity() { }

        public CarEntity(string make, int carId)
        {
            this.PartitionKey = make;
            this.RowKey = carId.ToString();
        }

        public CarEntity(Car car)
        {
            this.PartitionKey = car.Make;
            this.RowKey = car.CarId.ToString();
            this.Year = car.Year;
            this.Color = car.Color;
        }
        
        public int Year { get; set; }
        public string Color { get; set; }
    }
}