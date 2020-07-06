using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
//using System.Web.Mvc;
using WebApiNW.App_Data;

namespace WebApiNW.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly NorthwindEntities db = new NorthwindEntities();

        // GET: api/Products
        public async Task<System.Web.Http.IHttpActionResult> GetProducts()
        {
            var s1 =await (from u in db.Products
                      select new { u.ProductID
                      , u.ProductName
                      , u.UnitPrice }).ToListAsync();

            if (s1 == null)
            {
                return NotFound();
            }

            return Ok(s1);
        }

        [HttpPost]
        [Route("Urunler/EnPahali")]
        public async Task<System.Web.Http.IHttpActionResult> FiyatiEnYuksek([System.Web.Http.FromBody]  sistemUser User)
        {
            var a1 = User.userName;
            var a2 = User.passWord;

            var s1 = await (from u in db.Products
                            select new
                            {
                                u.ProductName
                            ,
                                u.UnitPrice
                            }).OrderByDescending(p => p.UnitPrice).Take(1).ToListAsync();

            if (s1 == null)
            {
                return NotFound();
            }

            return Ok(s1.FirstOrDefault().ProductName);
        }


        //[HttpPost]
        //[Route("Urunler/Urunler1_1")]
        //public async Task<JsonResult>  Urunler1(string  userName,string userPass)
        //{
        //    //TODO: kullanıcı kontrolü
        //    var s1 = await (from u in db.Products
        //                    select new { u.ProductID, u.ProductName }
        //                   ).ToListAsync();


        //    return new JsonResult()
        //    {
        //        Data = "1",
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };


             

        //}




        [HttpGet]
        [Route("Urunler/FiyataGoreTop10")]
        public async Task<System.Web.Http.IHttpActionResult> FiyatTop10()
        {
            var s1 = await (from u in db.Products
                            select new
                            {
                                u.ProductID
                            ,
                                u.ProductName
                            ,
                                u.UnitPrice
                            }).OrderByDescending(p=>p.UnitPrice).Take(10).ToListAsync();

            if (s1 == null)
            {
                return NotFound();
            }

            return Ok(s1);
        }

        [HttpGet]
        [Route("EnUcuz")]
        public async Task<System.Web.Http.IHttpActionResult> EnUcuz()
        {
            var s1 = await (from u in db.Products
                            where u.UnitPrice>0
                            select new
                            {
                                u.ProductID
                            ,
                                u.ProductName
                            ,
                                u.UnitPrice
                            }).OrderBy(p => p.UnitPrice).FirstOrDefaultAsync();

            if (s1 == null)
            {
                return NotFound();
            }

            return Ok(s1);
        }
         



        // GET: api/Products/5
        [ResponseType(typeof(Products))]
        public async Task<System.Web.Http.IHttpActionResult> GetProduct(int id)
        {
            Products products = await db.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }
 
 

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductsExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }



    }
}