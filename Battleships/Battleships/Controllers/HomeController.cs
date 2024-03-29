﻿using Battleships.Models;
using Battleships.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Battleships.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static Grid grid;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new GridConfigurationViewModel()
            {
                Rows = 10,
                Columns = 10
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Post(GridConfigurationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index", model);
                }
                grid = new Grid(model.Rows, model.Columns);
                for (int i = 0; i < model.Battleships; i++)
                {
                    grid.BuildShip(5);
                }
                for (int i = 0; i < model.Destroyers; i++)
                {
                    grid.BuildShip(4);
                }

                var viewModel = new HomeViewModel(grid);
                return View("Grid", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting new game");
                return View("Error");
            }
        }

        [Route("shoot")]
        public IActionResult Shoot(char column, int row)
        {
            return Ok(grid.ShootAtCell(column, row));
        }
    }
}
