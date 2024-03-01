using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RosKvartalTestTask.Data;
using RosKvartalTestTask.Models;

namespace RosKvartalTestTask.Pages.Register
{
    public class EditModel : PageModel
    {
        private readonly RosKvartalTestTask.Data.RosKvartalTestTaskContext _context;

        public EditModel(RosKvartalTestTask.Data.RosKvartalTestTaskContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InspectionsRegister InspectionsRegister { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inspectionsregister =  await _context.InspectionsRegister.FirstOrDefaultAsync(m => m.Id == id);
            if (inspectionsregister == null)
            {
                return NotFound();
            }
            InspectionsRegister = inspectionsregister;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(InspectionsRegister).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InspectionsRegisterExists(InspectionsRegister.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool InspectionsRegisterExists(int id)
        {
            return _context.InspectionsRegister.Any(e => e.Id == id);
        }
    }
}
