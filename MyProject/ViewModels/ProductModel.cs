using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    [Serializable]
    public class ProductModel
    {
        //public string ProductName { get; set; }
        public int CakeId { get; set; }
        public string CakeName { get; set; }
        public double CakePrice { get; set; }
        public string ImageUrl { get; set; }
        public int CakeQuantity { get; set; }
    }
}
