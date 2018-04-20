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
    builder.EntitySet<CustomFunctionSummary>("FunctionSummaries");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [EnableCors(origins: "http://localhost:9000", headers: "*", methods: "*")]
    public class FunctionSummariesController : ODataController
    {
        private Registry1_100Entities db = new Registry1_100Entities();

        // GET: odata/FunctionSummaries
        [EnableQuery]
        public IQueryable<CustomFunctionSummary> GetFunctionSummaries()
        {
            return db.CustomFunctionSummaries;
        }

        
        // GET: odata/FunctionSummaries(5)
        [EnableQuery]
        public SingleResult<CustomFunctionSummary> GetCustomFunctionSummary([FromODataUri] string DB, [FromODataUri] int FunctionID)
        {
            return SingleResult.Create(db.CustomFunctionSummaries.Where(customFunctionSummary => customFunctionSummary.DB == DB && customFunctionSummary.FunctionID == FunctionID));
        }

        /*
        // PUT: odata/FunctionSummaries(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<CustomFunctionSummary> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustomFunctionSummary customFunctionSummary = await db.CustomFunctionSummaries.FindAsync(key);
            if (customFunctionSummary == null)
            {
                return NotFound();
            }

            patch.Put(customFunctionSummary);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomFunctionSummaryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(customFunctionSummary);
        }

        // POST: odata/FunctionSummaries
        public async Task<IHttpActionResult> Post(CustomFunctionSummary customFunctionSummary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CustomFunctionSummaries.Add(customFunctionSummary);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomFunctionSummaryExists(customFunctionSummary.DB))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(customFunctionSummary);
        }

        // PATCH: odata/FunctionSummaries(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<CustomFunctionSummary> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustomFunctionSummary customFunctionSummary = await db.CustomFunctionSummaries.FindAsync(key);
            if (customFunctionSummary == null)
            {
                return NotFound();
            }

            patch.Patch(customFunctionSummary);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomFunctionSummaryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(customFunctionSummary);
        }

        // DELETE: odata/FunctionSummaries(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            CustomFunctionSummary customFunctionSummary = await db.CustomFunctionSummaries.FindAsync(key);
            if (customFunctionSummary == null)
            {
                return NotFound();
            }

            db.CustomFunctionSummaries.Remove(customFunctionSummary);
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
        private bool CustomFunctionSummaryExists(string key)
        {
            return db.CustomFunctionSummaries.Count(e => e.DB == key) > 0;
        }
        */
    }
}
