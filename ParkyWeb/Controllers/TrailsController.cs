using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepo;
        private readonly INationalParkRepository _npRepo;

        public TrailsController(ITrailRepository trailRepo, INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
        }
        public IActionResult Index()
        {
            return View(new Trail() { });
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> nationalParks = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);

            TrailsViewModel objViewModel = new TrailsViewModel()
            {
                NationalParkList = nationalParks.Select(i => new SelectListItem 
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            
            if (id == null)
            {
                return View(objViewModel);
            }

            objViewModel.Trail = await _trailRepo.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());

            if (objViewModel == null)
            {
                return NotFound();
            }
            return View(objViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsViewModel obj)
        {
            if (ModelState.IsValid)
            {
                
                if (obj.Trail.Id == 0)
                {
                    await _trailRepo.CreateAsync(SD.TrailAPIPath, obj.Trail);
                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TrailAPIPath+obj.Trail.Id, obj.Trail);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailAPIPath) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(SD.TrailAPIPath, id);

            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
        return Json(new { success = false, message = "Delete Failed" });
        }
    }
}
