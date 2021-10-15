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
        public async Task<IActionResult> Upsert(Trail obj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] picture = null;
                    using (var fileStream = files[0].OpenReadStream())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            fileStream.CopyTo(memoryStream);
                            picture = memoryStream.ToArray();
                        }
                    }
                    obj.Picture = picture;
                }
                else
                {
                    var objFromDb = await _npRepo.GetAsync(SD.TrailAPIPath, obj.Id);
                    obj.Picture = objFromDb.Picture;
                }
                if (obj.Id == 0)
                {
                    await _npRepo.CreateAsync(SD.TrailAPIPath, obj);
                }
                else
                {
                    await _npRepo.UpdateAsync(SD.TrailAPIPath+obj.Id, obj);
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
            return Json(new { data = await _npRepo.GetAllAsync(SD.TrailAPIPath) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _npRepo.DeleteAsync(SD.TrailAPIPath, id);

            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
        return Json(new { success = false, message = "Delete Failed" });
        }
    }
}
