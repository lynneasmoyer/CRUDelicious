using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;
using Microsoft.EntityFrameworkCore;


namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext dbContext;

        public HomeController(HomeContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<Dish> AllDishes = dbContext.Dishes.OrderByDescending(d => d.CreatedAt).ToList();

            return View(AllDishes);
        }

        [HttpGet("new")]

        public IActionResult New()
        {
            return View("New");
        }

        [HttpPost("process")]
        public IActionResult Process(Dish newDish)
        {
            if(ModelState.IsValid)
            {
                dbContext.Dishes.Add(newDish);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("new");
            }
        }

        [HttpGet]
        [Route("display/{DishId}")]

        public IActionResult Display(Dish aDish)
        {
            Dish oneDish = dbContext.Dishes.FirstOrDefault(d => d.DishId == aDish.DishId);
            return View("display", oneDish);
        }


        [HttpGet("edit/{DishId}")]
        public IActionResult Edit(Dish editDish)
        {
            Dish toeditDish = dbContext.Dishes.FirstOrDefault(d => d.DishId == editDish.DishId);
            return View("edit", toeditDish);
        }


        [HttpPost("update/{DishId}")]

        public IActionResult Update(Dish toUpdatedish, int DishId)
        {
            Dish UpdateDish = dbContext.Dishes.FirstOrDefault(d => d.DishId == DishId);
            UpdateDish.Name = toUpdatedish.Name;
            UpdateDish.Chef = toUpdatedish.Chef;
            UpdateDish.Calories = toUpdatedish.Calories;
            UpdateDish.Tastiness = toUpdatedish.Tastiness;
            UpdateDish.UpdatedAt = DateTime.Now;

            dbContext.SaveChanges();
            return Redirect($"/display/{UpdateDish.DishId}");
        }

        [HttpGet("delete/{DishId}")]

        public IActionResult DeleteDish(int DishId)
        {
            Dish DishToDelete = dbContext.Dishes.SingleOrDefault(d => d.DishId == DishId);
            dbContext.Dishes.Remove(DishToDelete);
            dbContext.SaveChanges();

            return Redirect("/");
        }

        ////////////////////////////////////////////////////////////////

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
