using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private CheeseDbContext context;

        public MenuController (CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            IList<Menu> menus = context.Menus.ToList();
            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };
                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addMenuViewModel);
        }

        public IActionResult ViewMenu(int ID)
        {
            Menu menu = context.Menus.Single(m => m.ID == ID);
            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == ID)
                .ToList();

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
            {
                Menu = menu,
                Items = items
            };

            return View(viewMenuViewModel);
        }

        public IActionResult AddItem(int ID)
        {
            Menu menu = context.Menus.Single(m => m.ID == ID);
            List<Cheese> cheeses = context.Cheeses.ToList();
            AddItemViewModel addItemViewModel = new AddItemViewModel(menu, cheeses);

            return View(addItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem (AddItemViewModel addItemViewModel)
        {
            if (ModelState.IsValid)
            {
                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == addItemViewModel.cheeseID)
                    .Where(cm => cm.MenuID == addItemViewModel.menuID).ToList();

                if (existingItems.Count == 0)
                {
                    CheeseMenu cheeseMenu = new CheeseMenu
                    {
                        MenuID = addItemViewModel.menuID,
                        CheeseID = addItemViewModel.cheeseID
                    };

                    context.CheeseMenus.Add(cheeseMenu);
                    context.SaveChanges();

                    return Redirect("/Menu/ViewMenu/" + addItemViewModel.menuID);
                }
            }

            return Redirect("/Menu/Index");
        }
    }
}
