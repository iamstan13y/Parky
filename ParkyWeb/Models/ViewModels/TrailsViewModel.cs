using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ParkyWeb.Models.ViewModels
{
    public class TrailsViewModel
    {
        public IEnumerable<SelectListItem> NationalParkList { get; set; }
        public Trail Trail { get; set; }
    }
}
