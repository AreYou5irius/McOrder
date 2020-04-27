using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McOrder
{
    class Product
    {
        public double Price { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }

        public Product(double price, string name, string imagePath)
        {
            Price = price;
            Name = name;
            ImagePath = imagePath;
        }

        //to be deleted later
        public override string ToString()
        {
            return $"Price: {Price}, Name: {Name}, ImagePath: {ImagePath}";
        }
    }
}
