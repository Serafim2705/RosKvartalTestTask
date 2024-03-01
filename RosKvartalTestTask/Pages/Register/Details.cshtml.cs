using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosKvartalTestTask.Data;
using RosKvartalTestTask.Models;

namespace RosKvartalTestTask.Pages.Register
{
    public class DetailsModel : PageModel
    {
        private readonly RosKvartalTestTask.Data.RosKvartalTestTaskContext _context;

        public DetailsModel(RosKvartalTestTask.Data.RosKvartalTestTaskContext context)
        {
            _context = context;
        }

        public InspectionsRegister InspectionsRegister { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inspectionsregister = await _context.InspectionsRegister.FirstOrDefaultAsync(m => m.Id == id);
            if (inspectionsregister == null)
            {
                return NotFound();
            }
            else
            {
                InspectionsRegister = inspectionsregister;
            }
            return Page();
        }
    }
}
