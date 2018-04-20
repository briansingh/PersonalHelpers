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
    builder.EntitySet<MethodCallSummary>("MethodCallSummaries");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [EnableCors(origins: "http://localhost:9000", headers: "*", methods: "*")]
    public class MethodCallSummariesController : ODataController
    {
        private Registry1_100Entities db = new Registry1_100Entities();

        // GET: odata/MethodCallSummaries
        [EnableQuery]
        public IQueryable<MethodCallSummary> GetMethodCallSummaries()
        {
            return db.MethodCallSummaries;
        }

        // GET: odata/MethodCallSummaries(5)
        [EnableQuery]
        public SingleResult<MethodCallSummary> GetMethodCallSummary([FromODataUri] int key)
        {
            return SingleResult.Create(db.MethodCallSummaries.Where(methodCallSummary => methodCallSummary.ID == key));
        }

        /*
        // PUT: odata/MethodCallSummaries(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<MethodCallSummary> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MethodCallSummary methodCallSummary = await db.MethodCallSummaries.FindAsync(key);
            if (methodCallSummary == null)
            {
                return NotFound();
            }

            patch.Put(methodCallSummary);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MethodCallSummaryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(methodCallSummary);
        }

        // POST: odata/MethodCallSummaries
        public async Task<IHttpActionResult> Post(MethodCallSummary methodCallSummary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MethodCallSummaries.Add(methodCallSummary);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MethodCallSummaryExists(methodCallSummary.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(methodCallSummary);
        }

        // PATCH: odata/MethodCallSummaries(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<MethodCallSummary> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MethodCallSummary methodCallSummary = await db.MethodCallSummaries.FindAsync(key);
            if (methodCallSummary == null)
            {
                return NotFound();
            }

            patch.Patch(methodCallSummary);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MethodCallSummaryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(methodCallSummary);
        }

        // DELETE: odata/MethodCallSummaries(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            MethodCallSummary methodCallSummary = await db.MethodCallSummaries.FindAsync(key);
            if (methodCallSummary == null)
            {
                return NotFound();
            }

            db.MethodCallSummaries.Remove(methodCallSummary);
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
        private bool MethodCallSummaryExists(int key)
        {
            return db.MethodCallSummaries.Count(e => e.ID == key) > 0;
        }
        */
    }
}
