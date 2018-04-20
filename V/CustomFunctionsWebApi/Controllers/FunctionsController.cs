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
    builder.EntitySet<CustomFunction>("Functions");
    builder.EntitySet<CustomFunctionAttribute>("CustomFunctionAttributes"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [EnableCors(origins: "http://localhost:9000", headers: "*", methods: "*")]
    public class FunctionsController : ODataController
    {
        private Registry1_100Entities db = new Registry1_100Entities();

        // GET: odata/Functions
        [EnableQuery(MaxExpansionDepth = 4)]
        public IQueryable<CustomFunction> GetFunctions()
        {
            return db.CustomFunctions;
        }

        // GET: odata/Functions(5)
        [EnableQuery(MaxExpansionDepth=4)]
        public SingleResult<CustomFunction> GetCustomFunction([FromODataUri] string DB, [FromODataUri] int FunctionID)
        {
            return SingleResult.Create(db.CustomFunctions.Where(customFunction => customFunction.DB == DB && customFunction.FunctionID == FunctionID));
        }

        /*
        // PUT: odata/Functions(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<CustomFunction> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustomFunction customFunction = await db.CustomFunctions.FindAsync(key);
            if (customFunction == null)
            {
                return NotFound();
            }

            patch.Put(customFunction);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomFunctionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(customFunction);
        }

        // POST: odata/Functions
        public async Task<IHttpActionResult> Post(CustomFunction customFunction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CustomFunctions.Add(customFunction);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomFunctionExists(customFunction.DB))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(customFunction);
        }

        // PATCH: odata/Functions(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<CustomFunction> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustomFunction customFunction = await db.CustomFunctions.FindAsync(key);
            if (customFunction == null)
            {
                return NotFound();
            }

            patch.Patch(customFunction);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomFunctionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(customFunction);
        }

        // DELETE: odata/Functions(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            CustomFunction customFunction = await db.CustomFunctions.FindAsync(key);
            if (customFunction == null)
            {
                return NotFound();
            }

            db.CustomFunctions.Remove(customFunction);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
        */

        // GET: odata/Functions(5)/CustomFunctionAttributes
        [EnableQuery]
        public IQueryable<CustomFunctionAttribute> GetAttributes([FromODataUri] string key)
        {
            return db.CustomFunctions.Where(m => m.DB == key).SelectMany(m => m.CustomFunctionAttributes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /*
        private bool CustomFunctionExists(string key)
        {
            return db.CustomFunctions.Count(e => e.DB == key) > 0;
        }
        */
    }
}
