using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheeseMVC.ViewModels
{
    public class EditCheeseViewModel : AddCheeseViewModel
    {
        public Cheese Cheese { get; set; }
        public int cheeseID { get; set; }

        public EditCheeseViewModel(Cheese cheese, IEnumerable<CheeseCategory> categories)
        {
            Name = cheese.Name;
            Description = cheese.Description;
            CategoryID = cheese.CategoryID;

            Cheese = cheese;

            Categories = BuildCategories(categories);
        }

        public EditCheeseViewModel() { }
    }
}
