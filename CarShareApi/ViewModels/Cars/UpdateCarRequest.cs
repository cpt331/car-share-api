using CarShareApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.ViewModels
{
    public class UpdateCarRequest
    {
        public int? Id { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string CarCategory { get; set; }
        public string Transmission { get; set; }
        public string Status { get; set; }
        public decimal LatPos { get; set; }
        public decimal LongPos { get; set; }
    }
}