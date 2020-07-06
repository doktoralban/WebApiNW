using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using WebApiNW.App_Data;

namespace WebApiNW.Controllers
{
    public class SiparislerController : ApiController
    {
        NorthwindEntities db = new NorthwindEntities();


        //[AllowAnonymous]
        //[HttpGet]
        //public IQueryable<Orders> Get()
        //{
        //    return db.Orders;
        //}


        //[Route("Orders/{EmployeId}/EmployeID")]
        //[HttpGet]
        //public IQueryable<Orders> GetbyEmp(int EmployeID)
        //{            
        //    return db.Orders.Where(p=>p.EmployeeID == EmployeID);
        //}

        //public IQueryable<Orders> GetbyES(int EmployeID,int ShipVia)
        //{
        //    return db.Orders.Where(p => p.EmployeeID == EmployeID && p.ShipVia==ShipVia);
        //}

        
        //[HttpGet]
        public IQueryable GetSiparisDetaybyOrderID(int SiparisID)
        {
            var s1 = (from u in db.Order_Details
                      where u.OrderID == SiparisID
                      select new {u.OrderID,u.Products.ProductName
                      ,u.Quantity,u.UnitPrice,
                      SatirToplamı= u.Quantity * u.UnitPrice
                      });


            return s1;
        }

        public IQueryable GetbyEmployeeID(int EmployeID)
        {
            var s1 = (from u in db.Orders
                      where u.EmployeeID == EmployeID
                      select new {u.CustomerID,u.EmployeeID,u.Freight,u.OrderDate,u.OrderID
                      ,u.RequiredDate,u.ShipAddress,u.ShipCity,u.ShipCountry,u.ShipVia }) ;

            return s1;
        }



        [Route("api/Siparisler/GetSiparisDetaybyOrderID/{OrderID}")]
        public IQueryable GetSiparistekiUrunlerbyOrderID(int OrderID)
        {
            var s1 = (from u in db.Order_Details
                      where u.OrderID == OrderID
                      select new
                      {
                          u.ProductID,
                          u.Products.ProductName,
                          u.Products.UnitPrice,
                          u.Products.UnitsOnOrder,
                          u.OrderID
                      });

            return s1;
        }


        [Route("Siparisler/ps/{ProductID}")]
        public IQueryable GetSiparislerbyProductID(int ProductID)
        {
            var  s1 = (from u in db.Order_Details                       
                      where u.ProductID == ProductID
                      select  u.OrderID).Distinct() ; 

            var s2 = (from u in db.Orders
                      where s1.Contains(u.OrderID)
                      select new
                      {
                          u.CustomerID,
                          u.EmployeeID,
                          u.Freight,
                          u.OrderDate,
                          u.OrderID,
                          u.RequiredDate,
                          u.ShipAddress,
                          u.ShipCity,
                          u.ShipCountry,
                          u.ShipVia
                      });


            return s2;
        }

        //https://docs.microsoft.com/tr-tr/aspnet/web-api/overview/odata-support-in-aspnet-web-api/supporting-odata-query-options
        //http://localhost:51062/a2?$top=10&$skip=20
        //http://localhost:51062/a2?$orderby=OrderDate,%20CustomerID%20desc
        //http://localhost:51062/a2?$filter=Freight%20lt%2033 (33 ten küçük)
        //http://localhost:51062/a2?$filter=Freight%20le%208  
        //http://localhost:51062/a2?Freight=150 ([FromODataUri] seçeneğiyle
        //
        [HttpGet]
        [EnableQuery]
        public IQueryable MiktaraGore([FromODataUri] decimal Freight)
        { 
            var s1 = (from u in db.Orders
                      where u.Freight >= Freight
                      select new
                      {
                          u.CustomerID,
                          u.EmployeeID,
                          u.Freight,
                          u.OrderDate,
                          u.OrderID
                      ,
                          u.RequiredDate,
                          u.ShipAddress,
                          u.ShipCity,
                          u.ShipCountry,
                          u.ShipVia
                      }).AsQueryable();

            return s1;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Siparisler/CR/{CustomerID}")]
        [ResponseType(typeof(Orders))]
        public IHttpActionResult SonSiparisbyCustomerID(string CustomerID)
        { 
            var s1 = (from u in db.Orders
                      where u.CustomerID == CustomerID
                      select new
                      {
                          u.CustomerID,
                          u.EmployeeID,
                          u.Freight,
                          u.OrderDate,
                          u.OrderID
                      ,
                          u.RequiredDate,
                          u.ShipAddress,
                          u.ShipCity,
                          u.ShipCountry,
                          u.ShipVia
                      }).OrderByDescending(p => p.OrderID).FirstOrDefault();

            if (s1 == null)
            {
                return NotFound();
            }
            return Ok(s1);

            //return s1;
        }



        //[AllowAnonymous]
        [HttpGet]
        [Route("Siparisler/CMS/{CustomerID}")]
        [ResponseType(typeof(Orders))]
        public IHttpActionResult SiparislerbyCustomerID(string CustomerID)
        {
            var s1 = (from u in db.Orders
                      where u.CustomerID == CustomerID
                      select new
                      {
                          u.CustomerID,
                          u.ShipCity,
                          u.ShipCountry                       

                      }) ;

            if (s1 == null)
            {
                return NotFound();
            }
            return Ok(s1); 
        }





    }
}
