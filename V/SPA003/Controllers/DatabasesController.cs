using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using SPA003.Models;

namespace SPA003.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using SPA003.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Database>("Databases");
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DatabasesController : ODataController
    {
        private DBContext db = new DBContext();

        // GET: odata/Databases
        [Queryable]
        public IQueryable<Models.Database> GetDatabases()
        {
            return db.Databases;
        }

        // GET: odata/Databases(5)
        [Queryable]
        public SingleResult<Models.Database> GetDatabase([FromODataUri] string key)
        {
            return SingleResult.Create(db.Databases.Where(database => database.DB == key));
        }

        // PUT: odata/Databases(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<Models.Database> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Models.Database database = await db.Databases.FindAsync(key);
            if (database == null)
            {
                return NotFound();
            }

            patch.Put(database);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatabaseExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(database);
        }

        // POST: odata/Databases
        public async Task<IHttpActionResult> Post(Models.Database database)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Databases.Add(database);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DatabaseExists(database.DB))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(database);
        }

        // PATCH: odata/Databases(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<Models.Database> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Models.Database database = await db.Databases.FindAsync(key);
            if (database == null)
            {
                return NotFound();
            }

            patch.Patch(database);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatabaseExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(database);
        }

        // DELETE: odata/Databases(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            Models.Database database = await db.Databases.FindAsync(key);
            if (database == null)
            {
                return NotFound();
            }

            db.Databases.Remove(database);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DatabaseExists(string key)
        {
            return db.Databases.Count(e => e.DB == key) > 0;
        }
    }
}
