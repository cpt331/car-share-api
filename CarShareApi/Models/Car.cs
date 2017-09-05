using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.Models
{
    public class Car
    {
        public int Id { get; set; }

        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int Year { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }

    }
}