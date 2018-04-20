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
using CustomFunctionsWebApi.Models;
using System.Web.Http.Cors;

namespace CustomFunctionsWebApi.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using CustomFunctionsWebApi.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<DatabaseName>("Databases");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [EnableCors(origins: "http://localhost:9000", headers: "*", methods: "*")]
    public class DatabasesController : ODataController
    {
        private Registry1_100Entities db = new Registry1_100Entities();

        // GET: odata/Databases
        [EnableQuery]
        public IQueryable<DatabaseName> GetDatabases()
        {
            return db.DatabaseNames;
        }

        // GET: odata/Databases(5)
        [EnableQuery]
        public SingleResult<DatabaseName> GetDatabaseName([FromODataUri] string key)
        {
            return SingleResult.Create(db.DatabaseNames.Where(databaseName => databaseName.DB == key));
        }

        /*
        // PUT: odata/Databases(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<DatabaseName> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DatabaseName databaseName = await db.DatabaseNames.FindAsync(key);
            if (databaseName == null)
            {
                return NotFound();
            }

            patch.Put(databaseName);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatabaseNameExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(databaseName);
        }

        // POST: odata/Databases
        public async Task<IHttpActionResult> Post(DatabaseName databaseName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DatabaseNames.Add(databaseName);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DatabaseNameExists(databaseName.DB))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(databaseName);
        }

        // PATCH: odata/Databases(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<DatabaseName> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DatabaseName databaseName = await db.DatabaseNames.FindAsync(key);
            if (databaseName == null)
            {
                return NotFound();
            }

            patch.Patch(databaseName);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatabaseNameExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(databaseName);
        }

        // DELETE: odata/Databases(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            DatabaseName databaseName = await db.DatabaseNames.FindAsync(key);
            if (databaseName == null)
            {
                return NotFound();
            }

            db.DatabaseNames.Remove(databaseName);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
        */

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /*
        private bool DatabaseNameExists(string key)
        {
            return db.DatabaseNames.Count(e => e.DB == key) > 0;
        }
        */
    }
}
