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
    builder.EntitySet<Method>("Methods");
    builder.EntitySet<MethodCall>("MethodCalls"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [EnableCors(origins: "http://localhost:9000", headers: "*", methods: "*")]
    public class MethodsController : ODataController
    {
        private Registry1_100Entities db = new Registry1_100Entities();

        // GET: odata/Methods
        [EnableQuery(MaxExpansionDepth = 4)]
        public IQueryable<Method> GetMethods()
        {
            return db.Methods;
        }

        // GET: odata/Methods(5)
        [EnableQuery(MaxExpansionDepth=4)]
        public SingleResult<Method> GetMethod([FromODataUri] int key)
        {
            return SingleResult.Create(db.Methods.Where(method => method.ID == key));
        }

        /*
        // PUT: odata/Methods(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Method> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Method method = await db.Methods.FindAsync(key);
            if (method == null)
            {
                return NotFound();
            }

            patch.Put(method);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MethodExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(method);
        }

        // POST: odata/Methods
        public async Task<IHttpActionResult> Post(Method method)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Methods.Add(method);
            await db.SaveChangesAsync();

            return Created(method);
        }

        // PATCH: odata/Methods(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Method> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Method method = await db.Methods.FindAsync(key);
            if (method == null)
            {
                return NotFound();
            }

            patch.Patch(method);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MethodExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(method);
        }

        // DELETE: odata/Methods(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Method method = await db.Methods.FindAsync(key);
            if (method == null)
            {
                return NotFound();
            }

            db.Methods.Remove(method);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Methods(5)/MethodCalls
        [EnableQuery]
        public IQueryable<MethodCall> GetMethodCalls([FromODataUri] int key)
        {
            return db.Methods.Where(m => m.ID == key).SelectMany(m => m.MethodCalls);
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
        private bool MethodExists(int key)
        {
            return db.Methods.Count(e => e.ID == key) > 0;
        }
        */ 
    }
}
