using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy.Models;

public class DirectionsController : Controller
{
    private readonly AcademyContext _context;

    public DirectionsController(AcademyContext context)
    {
        _context = context;
    }

    // GET: DIRECTIONS
    public async Task<IActionResult> Index(string sortOrder)
    {
        ViewData["NameSortParam"] = sortOrder == "name_desc" ? "name_asc" : "name_desc";
        IQueryable<Direction> directions = from d in _context.Directions select d;

        switch (sortOrder)
        {
            case "name_asc": directions = directions.OrderBy(d => d.direction_name); break;
            case "name_desc": directions = directions.OrderByDescending(d => d.direction_name); break;
        }

        return View(await directions.ToListAsync());
    }

    // GET: DIRECTIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var direction = await _context.Directions
            .FirstOrDefaultAsync(m => m.direction_id == id);
        if (direction == null) return NotFound();

        return View(direction);
    }

    // GET: DIRECTIONS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DIRECTIONS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("direction_id,direction_name")] Direction direction)
    {
        if (ModelState.IsValid)
        {
            _context.Add(direction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(direction);
    }

    // GET: DIRECTIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var direction = await _context.Directions.FindAsync(id);
        if (direction == null) return NotFound();
        return View(direction);
    }

    // POST: DIRECTIONS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("direction_id,direction_name")] Direction direction)
    {
        if (id != direction.direction_id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(direction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectionExists(direction.direction_id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(direction);
    }

    // GET: DIRECTIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var direction = await _context.Directions
            .FirstOrDefaultAsync(m => m.direction_id == id);
        if (direction == null) return NotFound();

        return View(direction);
    }

    // POST: DIRECTIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var direction = await _context.Directions.FindAsync(id);
        if (direction != null)
        {
            _context.Directions.Remove(direction);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DirectionExists(int direction_id)
    {
        return _context.Directions.Any(e => e.direction_id == direction_id);
    }
}
