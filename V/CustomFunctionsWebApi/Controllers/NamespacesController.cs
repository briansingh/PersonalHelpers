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
    builder.EntitySet<Namespace>("Namespaces");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [EnableCors(origins: "http://localhost:9000", headers: "*", methods: "*")]
    public class NamespacesController : ODataController
    {
        private Registry1_100Entities db = new Registry1_100Entities();

        // GET: odata/Namespaces
        [EnableQuery]
        public IQueryable<Namespace> GetNamespaces()
        {
            return db.Namespaces;
        }

        // GET: odata/Namespaces(5)
        [EnableQuery]
        public SingleResult<Namespace> GetNamespace([FromODataUri] string key)
        {
            return SingleResult.Create(db.Namespaces.Where(@namespace => @namespace.NamespaceName == key));
        }
        
        /*
        // PUT: odata/Namespaces(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<Namespace> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Namespace @namespace = await db.Namespaces.FindAsync(key);
            if (@namespace == null)
            {
                return NotFound();
            }

            patch.Put(@namespace);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NamespaceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(@namespace);
        }

        // POST: odata/Namespaces
        public async Task<IHttpActionResult> Post(Namespace @namespace)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Namespaces.Add(@namespace);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NamespaceExists(@namespace.NamespaceName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(@namespace);
        }

        // PATCH: odata/Namespaces(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<Namespace> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Namespace @namespace = await db.Namespaces.FindAsync(key);
            if (@namespace == null)
            {
                return NotFound();
            }

            patch.Patch(@namespace);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NamespaceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(@namespace);
        }

        // DELETE: odata/Namespaces(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            Namespace @namespace = await db.Namespaces.FindAsync(key);
            if (@namespace == null)
            {
                return NotFound();
            }

            db.Namespaces.Remove(@namespace);
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
        private bool NamespaceExists(string key)
        {
            return db.Namespaces.Count(e => e.NamespaceName == key) > 0;
        }
        */
    }
}
