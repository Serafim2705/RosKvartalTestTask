using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RosKvartalTestTask.Data;
using RosKvartalTestTask.Models;

namespace RosKvartalTestTask.Pages.Register
{
    public class CreateModel : PageModel
    {
        private readonly RosKvartalTestTask.Data.RosKvartalTestTaskContext _context;

        public CreateModel(RosKvartalTestTask.Data.RosKvartalTestTaskContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public InspectionsRegister InspectionsRegister { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InspectionsRegister.Add(InspectionsRegister);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
