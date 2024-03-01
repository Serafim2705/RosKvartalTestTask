using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosKvartalTestTask.Data;
using RosKvartalTestTask.Models;
using X.PagedList;

namespace RosKvartalTestTask.Pages.Register
{
    public class IndexModel : PageModel
    {
        private readonly RosKvartalTestTask.Data.RosKvartalTestTaskContext _context;

        public IndexModel(RosKvartalTestTask.Data.RosKvartalTestTaskContext context)
        {
            _context = context;
        }

        //public IList<InspectionsRegister> InspectionsRegister { get;set; } = default!;
        public IPagedList<InspectionsRegister> InspectionsRegister { get; set; } = default!;

        public async Task OnGetAsync([FromQuery] int page1 = 1, [FromQuery] int pageSize = 10)
        {
            Console.WriteLine(page1.ToString());
            Console.WriteLine(pageSize.ToString());
            IQueryable<InspectionsRegister> dataQuery = _context.InspectionsRegister.OrderBy(d => d.Id);
            //InspectionsRegister = await _context.InspectionsRegister.ToListAsync();
            InspectionsRegister = await dataQuery.ToPagedListAsync(page1, pageSize);
        }
    }
}
