using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using OnlineGame.Web.Models;

namespace OnlineGame.Web.Controllers
{
    public class GamersController : Controller
    {
        private OnlineGameEntities1 db = new OnlineGameEntities1();

        public ActionResult GamersByTeam()
        {
            List<TeamTotals> teamTotalses =
                db.Gamer.Include("Team")
                    .GroupBy(g => g.Team.Name)
                    .Select(gamer => new TeamTotals
                    {
                        Name = gamer.Key,
                        Total = gamer.Count()
                    }).ToList();
            return View(teamTotalses);

        }

        // GET: Gamers
        public async Task<ActionResult> Index()
        {
            IQueryable<Gamer> gamer = db.Gamer.Include(g => g.Team);

            return View(await gamer.ToListAsync());  //~/Views/Gamers/Index.cshtml
            //return View("Index", await gamer.ToListAsync());    //~/Views/Gamers/Index.cshtml
            //return View("Index.cshtml", await gamer.ToListAsync());    //  Error
            //return View("~/Views/Gamers/Index.cshtml", await gamer.ToListAsync());
        }

        // GET: Gamers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gamer gamer = await db.Gamer.FindAsync(id);
            if (gamer == null)
            {
                return HttpNotFound();
            }
            return View(gamer);
        }

        // GET: Gamers/Create
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Team, "Id", "Name");
            return View();
        }

        // POST: Gamers/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Gender,City,DateOfBirth,TeamId")] Gamer gamer)
        {
            if (string.IsNullOrEmpty(gamer.Name))
            {
                ModelState.AddModelError("Name", "Name is required.");
            }


            if (ModelState.IsValid)
            {
                db.Gamer.Add(gamer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Team, "Id", "Name", gamer.TeamId);
            return View(gamer);
        }

        // GET: Gamers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gamer gamer = await db.Gamer.FindAsync(id);
            if (gamer == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamId = new SelectList(db.Team, "Id", "Name", gamer.TeamId);
            return View(gamer);
        }

        // POST: Gamers/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Gender,City,DateOfBirth,TeamId")] Gamer gamer)
        {
            //Get the gamer
            Gamer gamerFromDb = db.Gamer.Single( g => g.Id == gamer.Id);
            //Update the gamerFromDb
            gamerFromDb.Id = gamer.Id;
            gamerFromDb.Gender = gamer.Gender;
            gamerFromDb.City = gamer.City;
            gamerFromDb.TeamId = gamer.TeamId;

            //In the beginning, gamer.Name is null.
            //In order to pass ModelState.IsValid,
            //we need to set value for gamer.Name
            gamer.Name = gamerFromDb.Name;


            if (ModelState.IsValid)
            {
                //db.Entry(gamer).State = EntityState.Modified;

                //Update the entity by gamerFromDb, and set state as EntityState.Modified
                db.Entry(gamerFromDb).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Team, "Id", "Name", gamer.TeamId);
            return View(gamer);
        }

        // GET: Gamers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gamer gamer = await db.Gamer.FindAsync(id);
            if (gamer == null)
            {
                return HttpNotFound();
            }
            return View(gamer);
        }

        // POST: Gamers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Gamer gamer = await db.Gamer.FindAsync(id);
            db.Gamer.Remove(gamer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
