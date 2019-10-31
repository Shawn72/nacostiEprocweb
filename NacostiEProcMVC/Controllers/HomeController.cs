using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NacostiEProcMVC.Models;
using Newtonsoft.Json;

namespace NacostiEProcMVC.Controllers
{
    public class HomeController : BaseController
    {
        //Hosted web API REST Service base url  
        //string Baseurl = "http://41.89.63.253:1010/";
        private string Baseurl = "http://localhost:1010/";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }
        
        public ActionResult UploadFiles()
        {
            return View();
        }

        public ActionResult AdminIndex()
        {
            return View();
        }

        public ActionResult ApplyTender()
        {
            return View();
        }

        public ActionResult ApplyTenderPublic()
        {
            return View();
        }

        public ActionResult BlogForum()
        {
            return View();
        }

        public ActionResult ContactsIndex()
        {
            return View();
        }



        public ActionResult ViewOneTenderRef()
        {
            var vendorNo = Session["vendorNo"].ToString();
            var tenderNu = Request.QueryString["tenderNo"];
            var uploadedFiles = new List<UploadedFile>();

            if (vendorNo.Contains(":"))
                vendorNo = vendorNo.Replace(":", "_");

            if (tenderNu.Contains(":"))
                tenderNu = tenderNu.Replace(":", "_");

            var rootFolder = Server.MapPath("~/Uploads");
            var subfolder = Path.Combine(rootFolder, vendorNo + "/" + tenderNu);

            if (!Directory.Exists(subfolder))
                Directory.CreateDirectory(subfolder);

            var files = Directory.GetFiles(subfolder);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                var uploadedFile = new UploadedFile { FileName = Path.GetFileName(file) };
                uploadedFile.Size = fileInfo.Length;

                uploadedFile.Path = (subfolder) + "/" + Path.GetFileName(file);
                uploadedFiles.Add(uploadedFile);
            }

            return View(uploadedFiles);
        }

        [HttpPost]
        public JsonResult SubmitTenderApp(string myBidamount, string myTendorNo)
        {
            try
            {
                var nvWebref = WsConfig.EProcWebRef;
                if (string.IsNullOrWhiteSpace(myBidamount))
                    return Json("BidamountEmpty", JsonRequestBehavior.AllowGet);
                
                var status = nvWebref.FnApplyforTender(myTendorNo, Convert.ToDecimal(myBidamount), (string)Session["vendorNo"], (string)Session["email"]);

                var res = status.Split('*');
                switch (res[0])
                {
                    case "success":
                        return Json("submitted success", JsonRequestBehavior.AllowGet);

                    default:
                        return Json(res[1], JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SubmitTenderAppPublic(string myBidamount, string myTendorNo)
        {
            try
            {
                var nvWebref = WsConfig.EProcWebRef;
                if (string.IsNullOrWhiteSpace(myBidamount))
                    return Json("BidamountEmpty", JsonRequestBehavior.AllowGet);

                var status = nvWebref.FnApplyforOpenTender(myTendorNo, Convert.ToDecimal(myBidamount), (string)Session["contactNo"], (string)Session["email"]);

                var res = status.Split('*');
                switch (res[0])
                {
                    case "success":
                        return Json("submitted success", JsonRequestBehavior.AllowGet);

                    default:
                        return Json(res[1], JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAllFilesnUpload(IEnumerable<HttpPostedFileBase> postedFiles)
        {
            try
            {
                var cntNo = Session["contactNo"].ToString();
                if (cntNo.Contains(":"))
                    cntNo = cntNo.Replace(":", "_");
                var rootFolder = Server.MapPath("~/Uploads");
                var subfolder = Path.Combine(rootFolder, cntNo);

                if (!Directory.Exists(subfolder))
                    Directory.CreateDirectory(subfolder);
                foreach (var file in postedFiles)
                {
                    if (file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string ext = Path.GetExtension(file.FileName);
                        file.SaveAs(subfolder + "/" + fileName);
                        ViewBag.Message += string.Format("<b>{0}</b> uploaded successfully.<br />", fileName);
                    }
                }
            }
            #pragma warning disable 168
            catch (Exception exp)
            #pragma warning restore 168
            {
                // ignored
            }

            return View("UploadFiles");
        }


        [HttpPost]
        public ActionResult UploadTenderDocs(IEnumerable<HttpPostedFileBase> postedFiles )
        {
            var vendorNo = Session["vendorNo"].ToString();
            var tenderNu = Session["tendNo"].ToString();
            try
            {

                if (vendorNo.Contains(":"))
                    vendorNo = vendorNo.Replace(":", "_");

                if (tenderNu.Contains(":"))
                    tenderNu = tenderNu.Replace(":", "_");

                var rootFolder = Server.MapPath("~/Uploads");
                var subfolder = Path.Combine(rootFolder, vendorNo+"/"+ tenderNu);

                if (!Directory.Exists(subfolder))
                    Directory.CreateDirectory(subfolder);
                foreach (var file in postedFiles)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        string ext = Path.GetExtension(file.FileName);
                        file.SaveAs(subfolder + "/" + fileName);
                        ViewBag.Message += string.Format("<b>{0}</b> uploaded successfully.<br />", fileName);

                    }
                }
                //Session.Remove("Smessage");
               // Session["Smessage"] = message;
            }
            #pragma warning disable 168
            catch (Exception exp)
            #pragma warning restore 168
            {
                // ignored
            }
            return View("ReturnMessagePop");
            // return RedirectToAction("ApplyTender", new { tendorNo = Session["tendNo"].ToString() } );
        }

        [HttpPost]
        public ActionResult UploadTenderDocsPublic(IEnumerable<HttpPostedFileBase> postedFiles)
        {
            var cntNo = Session["contactNo"].ToString();
            var tenderNu = Session["tendNo"].ToString();
            try
            {

                if (cntNo.Contains(":"))
                    cntNo = cntNo.Replace(":", "_");

                if (tenderNu.Contains(":"))
                    tenderNu = tenderNu.Replace(":", "_");

                var rootFolder = Server.MapPath("~/Uploads");
                var subfolder = Path.Combine(rootFolder, cntNo + "/" + tenderNu);

                if (!Directory.Exists(subfolder))
                    Directory.CreateDirectory(subfolder);
                foreach (var file in postedFiles)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        string ext = Path.GetExtension(file.FileName);
                        file.SaveAs(subfolder + "/" + fileName);
                        ViewBag.Message += string.Format("<b>{0}</b> uploaded successfully.<br />", fileName);

                    }
                }
                
            }
            #pragma warning disable 168
            catch (Exception exp)
            #pragma warning restore 168
            {
                // ignored
            }
            return View("ReturnMessagePoppublic");
            // return RedirectToAction("ApplyTender", new { tendorNo = Session["tendNo"].ToString() } );
        }
        public ActionResult ReturnMessagePop()
        {
            return View();
        }

        public ActionResult ReturnMessagePoppublic()
        {
            return View();
        }

        public ActionResult UploadSpecificTenderDocs(IEnumerable<HttpPostedFileBase> postedFiles)
        {
            var vendorNo = Session["vendorNo"].ToString();
            var tenderNu = Session["tendNo"].ToString();
            try
            {

                if (vendorNo.Contains(":"))
                    vendorNo = vendorNo.Replace(":", "_");

                if (tenderNu.Contains(":"))
                    tenderNu = tenderNu.Replace(":", "_");

                var rootFolder = Server.MapPath("~/Uploads");
                var subfolder = Path.Combine(rootFolder, vendorNo + "/" + tenderNu);

                if (!Directory.Exists(subfolder))
                    Directory.CreateDirectory(subfolder);
                foreach (var file in postedFiles)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        string ext = Path.GetExtension(file.FileName);
                        file.SaveAs(subfolder + "/" + fileName);
                        ViewBag.Message += string.Format("<b>{0}</b> uploaded successfully.<br />", fileName);

                    }
                }
                //Session.Remove("Smessage");
                // Session["Smessage"] = message;
            }
            #pragma warning disable 168
            catch (Exception exp)
            #pragma warning restore 168
            {
                // ignored
            }
            return View("ReturnMessagePop");
        }
        public ActionResult UploadsList()
        {
            var uploadedFiles = new List<UploadedFile>();
            var cntNo = Session["contactNo"].ToString();
            if (cntNo.Contains(":"))
                cntNo = cntNo.Replace(":", "_");

            var rootFolder = Server.MapPath("~/Uploads");
            var subfolder = Path.Combine(rootFolder, cntNo);
            if (!Directory.Exists(subfolder))
                Directory.CreateDirectory(subfolder);

            var files = Directory.GetFiles(subfolder);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                var uploadedFile = new UploadedFile { FileName = Path.GetFileName(file) };
                uploadedFile.Size = fileInfo.Length;

                uploadedFile.Path = (subfolder)+"/" + Path.GetFileName(file);
                uploadedFiles.Add(uploadedFile);
            }

            return View(uploadedFiles);
        }
        public ActionResult UploadedDocsPerApplicant()
        {
            var tendorNumber = Request.QueryString["tendorNo"];
            var vendorNo = Request.QueryString["vendorNo"];

            var uploadedFiles = new List<UploadedFile>();

            if (vendorNo.Contains(":"))
                vendorNo = vendorNo.Replace(":", "_");

            if (tendorNumber.Contains(":"))
                tendorNumber = tendorNumber.Replace(":", "_");

            var rootFolder = Server.MapPath("~/Uploads");
            var subfolder = Path.Combine(rootFolder, vendorNo + "/" + tendorNumber);

            if (!Directory.Exists(subfolder))
                Directory.CreateDirectory(subfolder);

            var files = Directory.GetFiles(subfolder);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                var uploadedFile = new UploadedFile { FileName = Path.GetFileName(file) };
                uploadedFile.Size = fileInfo.Length;

                uploadedFile.Path = (subfolder) + "/" + Path.GetFileName(file);
                uploadedFiles.Add(uploadedFile);
            }

            return View(uploadedFiles);
        }

        public ActionResult UploadedTenderDocs()
        {
            var vendorNo = Session["vendorNo"].ToString();
            var tenderNu = Session["tendNo"].ToString();
            var entryNu = Session["entryNo"].ToString();
            var uploadedFiles = new List<UploadedFile>();
            var cntNo = Session["contactNo"].ToString();

           

            if (vendorNo.Contains(":"))
                vendorNo = vendorNo.Replace(":", "_");

            if (tenderNu.Contains(":"))
                tenderNu = tenderNu.Replace(":", "_");

            var rootFolder = Server.MapPath("~/Uploads");
            var subfolder = Path.Combine(rootFolder, vendorNo + "/" + tenderNu);
         
            if (!Directory.Exists(subfolder))
                Directory.CreateDirectory(subfolder);

            var files = Directory.GetFiles(subfolder);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                var uploadedFile = new UploadedFile { FileName = Path.GetFileName(file) };
                uploadedFile.Size = fileInfo.Length;

                uploadedFile.Path = (subfolder) + "/" + Path.GetFileName(file);
                uploadedFiles.Add(uploadedFile);
            }

            return View(uploadedFiles);
        }

        public ActionResult DeleteApplication(string pId)
        {
            return View("ApplyPrequalification");
        }
        
        [HttpPost]
        public JsonResult DeleteFile(string filepath)
        {
            try
            {
                System.IO.File.Delete(filepath);
                //  Alert("Deleted successfully", Enum.NotificationType.success);
                return Json("File Deleted", JsonRequestBehavior.AllowGet);
            }
                #pragma warning disable 168
            catch (Exception exp)
                #pragma warning restore 168
            {
                return Json("Snaaap!, file failed to delete!", JsonRequestBehavior.AllowGet);
            }

        }
        public async  Task<ActionResult> OpenRfQs()
        {
            //var nvOdata = WsConfig.ODataObj();
            //var openRfQs = nvOdata.ProcurementRequest.Where(r => r.Process_Type == "RFQ" && r.Quotation_Finished == false).ToList();
            //return View(openRfQs);
            List<ProcurementModel> openrfq = new List<ProcurementModel>();
            try
            {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage res = await client.GetAsync("api/GetOpenRfQs/RFQ/false/" + (string)Session["vendorNo"]);

                //Checking the response is successful or not which is sent using HttpClient  
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var rfqResponse = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list  
                    openrfq = JsonConvert.DeserializeObject<List<ProcurementModel>>(rfqResponse);
                }
               }
            }
            #pragma warning disable 168
            catch (Exception ex)
            #pragma warning restore 168
            {
                openrfq = null;
            }
            return View(openrfq);

        }

        public ActionResult SupplierCategory()
        {
            var nvOdata = WsConfig.ODataObj();
            var supplierlines = nvOdata.SupplyCategoryHeader.Where(r => r.Fiscal_Year == Convert.ToString(Session["currentYear"])).ToList();
            return View(supplierlines);
        }
        

        //protected void GetmeYears()
        //{
        //    // Clear items:    
        //    ddlYears.Items.Clear();
        //    // Add default item to the list
        //    ddlYears.Items.Add("--Select Year--");
        //    // Start loop
        //    for (int i = 0; i < 20; i++)
        //    {
        //        // For each pass add an item
        //        // Add a number of years (negative, which will subtract) to current year
        //        ddlYears.Items.Add(DateTime.Now.AddYears(-i).Year.ToString());
        //    }

        //}

        private static List<SelectListItem> FiscalYear()
        {
            var nvOdata = WsConfig.ODataObj();
            var myYears = nvOdata.FiscalYears.ToList();
            List<SelectListItem> yearItems = myYears.Select(myyear => new SelectListItem
            {
                Text = myyear.Code,
                Value = myyear.Code
            }).ToList();
            return yearItems;
        }

        public ActionResult FiscalYears()
        {
            DropdownListsModel dropdownmodel = new DropdownListsModel();
            dropdownmodel.MyDropdownList = FiscalYear();
            return View(dropdownmodel);
        }

        private static List<SelectListItem> SupplierCategories()
        {
            var nvOdata = WsConfig.ODataObj();
            var mySupplyCats = nvOdata.SupplierCategories.ToList();
            List<SelectListItem> supplierItems = mySupplyCats.Select(supplier => new SelectListItem
            {
                Text = supplier.Description,
                Value = supplier.Category_Code
            }).ToList();
            return supplierItems;
        }

        public ActionResult AllSupplierCategories()
        {
            DropdownListsModel mysupplier = new DropdownListsModel();
            mysupplier.MyDropdownList = SupplierCategories();
            return View(mysupplier);
        }

        private static List<SelectListItem> PostalCode()
        {
            var nvOdata = WsConfig.ODataObj();
            var myPostcode = nvOdata.postcodes.ToList();
            List<SelectListItem> postalitems = myPostcode.Select(posta => new SelectListItem
            {
                Text = posta.Code,
                Value = posta.Code
            }).ToList();
            return postalitems;
        }
        private static List<SelectListItem> Country()
        {
            var nvOdata = WsConfig.ODataObj();
            var myCountry = nvOdata.Countries.ToList();
            List<SelectListItem> countryitems = myCountry.Select(nchi => new SelectListItem
            {
                Text = nchi.Code,
                Value = nchi.Name
            }).ToList();
            return countryitems;
        }

        public ActionResult PostalCodeList()
        {
            DropdownListsModel myposta = new DropdownListsModel();
            myposta.MyDropdownList = PostalCode();
            return View(myposta);
        }

        public ActionResult CountryList()
        {
            DropdownListsModel mycountry = new DropdownListsModel();
            mycountry.MyDropdownList = Country();
            return View(mycountry);
        }

        public JsonResult SelectedPosta(string postcode)
        {
            var nvOdata = WsConfig.ODataObj();
            var mycity = nvOdata.postcodes.Where(c => c.Code == postcode).ToList();
            var result = mycity.FirstOrDefault();

            if (result != null)
            {
                return Json(result.City, JsonRequestBehavior.AllowGet);
            }
            return Json("notfound", JsonRequestBehavior.AllowGet); 
        }

        public ActionResult ApplyPrequalification()
        {
            return View();
        }

        public async Task<ActionResult> OpenTenders()
        {
            List<ProcurementModel> opentenders = new List<ProcurementModel>();
               using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage res = await client.GetAsync("api/GetOpenTenders");

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var rfqResponse = res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list  
                        opentenders = JsonConvert.DeserializeObject<List<ProcurementModel>>(rfqResponse);
                    }
                }
             
                return View(opentenders);
           }


        public async Task<ActionResult> AllContactsList()
        {
            List<ContactsModel> allcontacts = new List<ContactsModel>();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage res = await client.GetAsync("api/GetAllCustomers");

                //Checking the response is successful or not which is sent using HttpClient  
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var jsonResponse = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list  
                    allcontacts = JsonConvert.DeserializeObject<List<ContactsModel>>(jsonResponse);
                }
            }

            return View(allcontacts);
        }

        public async Task<ActionResult> AllTenderApplications()
        {
            List<TenderModel> alltenders = new List<TenderModel>();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage res = await client.GetAsync("api/GetTenderApplications");

                //Checking the response is successful or not which is sent using HttpClient  
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var jsonResponse = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list  
                    alltenders = JsonConvert.DeserializeObject<List<TenderModel>>(jsonResponse);
                }
            }

            return View(alltenders);
        }
        public FileResult DownloadFile(string filePath)
        {
            var fileVirtualPath = filePath;
            return File(fileVirtualPath, "application/force-download", Path.GetFileName(fileVirtualPath));
        }
        public async Task<ActionResult> AppliedTenders()
        {
            List<TenderModel> appliedtenders = new List<TenderModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage res = await client.GetAsync("api/GetTenderApplication/" + (string)Session["vendorNo"]);

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var rfqResponse = res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list  
                        appliedtenders = JsonConvert.DeserializeObject<List<TenderModel>>(rfqResponse);
                    }
                }
            }
            #pragma warning disable 168
            catch (Exception ex)
            #pragma warning restore 168
            {
                appliedtenders = null;
            }
            return View(appliedtenders);
        }

        public async Task<ActionResult> AppliedTendersOpen()
        {
            List<TenderModel> appliedtenders = new List<TenderModel>();
              using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage res = await client.GetAsync("api/GetTenderApplications");

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var rfqResponse = res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list  
                        appliedtenders = JsonConvert.DeserializeObject<List<TenderModel>>(rfqResponse);
                    }
                    //select filter from returned Json
                    var record = (from a in appliedtenders orderby a.Contact_No select a).Where(r => r.E_Mail == (string)Session["email"]).ToList();
                    return View(record);
                }
           
        }

        public async Task<ActionResult> AwardedTenders()
        {
            List<TenderModel> appliedtenders = new List<TenderModel>();
              using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage res = await client.GetAsync("api/GetTenderApplication/" + (string)Session["vendorNo"]);

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var rfqResponse = res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list  
                        appliedtenders = JsonConvert.DeserializeObject<List<TenderModel>>(rfqResponse);
                    }
                }
                //select filter from returned Json
                var record = (from a in appliedtenders orderby a.Vendor_No select a).Where(r => r.Successful == true).ToList();
                return View(record);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult RegisterVendor(VendorModel vendormodel)
        {
            try
            {
                var nvWebref = WsConfig.EProcWebRef;
                if (string.IsNullOrWhiteSpace(vendormodel.VendorName))
                    return Json("VendorEmpty", JsonRequestBehavior.AllowGet);
                if (string.IsNullOrWhiteSpace(vendormodel.Email))
                    return Json("EmailEmpty", JsonRequestBehavior.AllowGet);
                if (string.IsNullOrWhiteSpace(vendormodel.KraPin))
                    return Json("KRAEmpty", JsonRequestBehavior.AllowGet);
                if (string.IsNullOrWhiteSpace(vendormodel.Password1))
                    return Json("PasswordEmpty", JsonRequestBehavior.AllowGet);
                if (string.IsNullOrWhiteSpace(vendormodel.VendorName))
                    return Json("Password2Empty", JsonRequestBehavior.AllowGet);
                if (vendormodel.Password1 != vendormodel.Password2)
                    return Json("PasswordMismatched", JsonRequestBehavior.AllowGet);

                var status = nvWebref.FnRegisterVendor(vendormodel.VendorName, "", vendormodel.Country,
                    vendormodel.Address, vendormodel.PostalCode,
                    vendormodel.Phonenumber, vendormodel.Email, vendormodel.KraPin, vendormodel.Taxpin,
                    EncryptP(vendormodel.Password1), EncryptP(vendormodel.Password2));

                var res = status.Split('*');
                switch (res[0])
                {
                    case "success":
                        return Json("Your account created successfully!", JsonRequestBehavior.AllowGet);
                       
                    default:
                        return Json(res[1], JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult PrequalifiedApplications()
        {
            var nvOdata = WsConfig.ODataObj();
            var preqlflines = nvOdata.PrequalifiedSuppliers.Where(r => r.Contact_No == Convert.ToString(Session["contactNo"]) && r.Pre_Qualified==false);
            return View(preqlflines);
        }

        public ActionResult AcceptedPreQualification()
        {
            var nvOdata = WsConfig.ODataObj();
            var acceptedPreq = nvOdata.PrequalifiedSuppliers.Where(r => r.Contact_No == Convert.ToString(Session["contactNo"]) && r.Pre_Qualified == true);
            return View(acceptedPreq);
        }

        public JsonResult ApplyforPreQualifc(string selectedcategory, string selectedfyear)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(selectedfyear))
                    return Json("Select Fiscal Year!", JsonRequestBehavior.AllowGet);
                if (string.IsNullOrWhiteSpace(selectedcategory))
                    return Json("Select Category", JsonRequestBehavior.AllowGet);

                var nvWebref = WsConfig.EProcWebRef;
                var tContactNo = Convert.ToString(Session["contactNo"]);
                var tPassword = Convert.ToString(Session["password"]);
                var status =  nvWebref.FnApplyPreQualification(tContactNo, selectedcategory, tPassword, selectedfyear);
                var res = status.Split('*');
                switch (res[0])
                {
                    case "success":
                        return Json("Your registration for prequalification has been received!", JsonRequestBehavior.AllowGet);
                    default:
                        return Json(res[1], JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        static string EncryptP(string mypass)
        {
            //encryptpassword:
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(mypass));
                return Convert.ToBase64String(data);
            }
        }

        public void CountApplicationsInfo(string vendorNum)
        {
            try
            {
                var nvWebref = WsConfig.EProcWebRef;
                Session["openTenders"] = nvWebref.FnCountOpenTenders();
                Session["appliedTenders"] = nvWebref.FnCountAppliedTenders(vendorNum);
                Session["tendersAwarded"] = nvWebref.FnCountTendersAwarded(vendorNum);
            }
            #pragma warning disable 168
            catch (Exception ex)
            #pragma warning restore 168
            {
                // ignored
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult CheckLogin(string myUsername, string myPassword)
        {
            try
            {
                var nvOdata = WsConfig.ODataObj();
                if (string.IsNullOrWhiteSpace(myUsername))
                    return Json("UsernameEmpty", JsonRequestBehavior.AllowGet);
                if (string.IsNullOrWhiteSpace(myPassword))
                    return Json("PasswordEmpty", JsonRequestBehavior.AllowGet);

                var loginresult = nvOdata.PortalUsers.Where(o => o.Email == myUsername);
                var result = loginresult.FirstOrDefault();
                if (result != null)
                {
                    Session["userNo"] = result.CustomerNo;
                    if (result.Password != EncryptP(myPassword))
                        return Json("PasswordMismatched", JsonRequestBehavior.AllowGet);

                    var contactDetails = nvOdata.contacts.Where(m => m.E_Mail == myUsername).ToList().SingleOrDefault();
                    if (contactDetails!=null)
                    {
                        //set Sessions here
                        Session["name"] = contactDetails.Name;
                        Session["email"] = contactDetails.E_Mail;
                        Session["password"] = contactDetails.password;
                        Session["contactNo"] = contactDetails.No;
                        Session["currentYear"] = WsConfig.EProcWebRef.GetCurrentYear();
                       
                    }
                    //check if the are in Vendor table
                    var vendorcollection = nvOdata.Vendors.Where(t => t.Primary_Contact_No == (string)Session["contactNo"]).ToList();

                    if (result.isAdmin == true && result.ActivatedAsVendor==true)
                    {
                        Session["isAdmin"] = "administrator";
                        
                        foreach (var vend in vendorcollection)
                        {
                            Session["vendorNo"] = vend.No;
                        }
                        CountApplicationsInfo((string)Session["vendorNo"]);
                        return Json("Loginadmin", JsonRequestBehavior.AllowGet);
                    }
                    if (result.isAdmin == false && result.ActivatedAsVendor == true)
                    {
                        Session["isAdmin"] = "customer";
                        foreach (var vend in vendorcollection)
                        {
                            Session["vendorNo"] = vend.No;
                        }
                        return Json("Logincustomer", JsonRequestBehavior.AllowGet);
                    }
                   
                    if (result.isAdmin == false && result.ActivatedAsVendor == false)
                    {
                        Session["isAdmin"] = "contact";
                        Session["vendorNo"] = null;
                        return Json("Logincontact", JsonRequestBehavior.AllowGet);
                    }


                }
                return Json("InvalidLogin", JsonRequestBehavior.AllowGet);
             }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CheckLogout()
        {
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.AppendHeader("Cache-Control", "no-store");
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        public async Task<ActionResult> TendersOpentoPublic()
        {
            List<ProcurementModel> opentenders = new List<ProcurementModel>();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage res = await client.GetAsync("api/GetOpenTenders");

                //Checking the response is successful or not which is sent using HttpClient  
                if (res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var rfqResponse = res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list  
                    opentenders = JsonConvert.DeserializeObject<List<ProcurementModel>>(rfqResponse);
                }
            }

            return View(opentenders);
        }

      

        //[HttpPost]
        //public JsonResult ResetPassword(string customerNo)
        //{
        //    var navConnect = NAVConfig.ObjNav;
        //    var resetPresult = _logic.GetEcommerceUsers().Where(o => o.customerNo == customerNo);
        //    var result = resetPresult.FirstOrDefault();

        //    if (string.IsNullOrEmpty(customerNo))
        //    {
        //        return Json("stringNullError", JsonRequestBehavior.AllowGet);
        //    }

        //    if (result != null)
        //    {
        //        try
        //        {
        //            var status = navConnect.ForgotPassword(customerNo);
        //            var info = status.Split('*');

        //            switch (info[0])
        //            {

        //                case "success":
        //                    //TempData["success"] = info[1];
        //                    return Json("pChangesuccess", JsonRequestBehavior.AllowGet);
        //                case "danger":
        //                    // TempData["red"] = info[1];
        //                    return Json("pChangefailed", JsonRequestBehavior.AllowGet);
        //            }

        //        }
        //        catch (Exception)
        //        {
        //            return Json("ExcepError", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        return Json("userNullError", JsonRequestBehavior.AllowGet);
        //    }

        //    return Json(customerNo, JsonRequestBehavior.AllowGet);
        //}



        //[HttpPost]
        //public JsonResult ChangePassword(string currentPassword, string newPassword, string newpassConfirm)
        //{
        //    var customerNo = (string)Session["customerNo"];
        //    var navConnect = NAVConfig.ObjNav;
        //    var resetPresult = _logic.GetEcommerceUsers().Where(o => o.customerNo == customerNo);
        //    var result = resetPresult.FirstOrDefault();

        //    if (string.IsNullOrEmpty(customerNo))
        //    {
        //        return Json("stringNullError", JsonRequestBehavior.AllowGet);
        //    }

        //    if (result != null)
        //    {
        //        try
        //        {
        //            var status = navConnect.ForgotPassword(customerNo);
        //            var info = status.Split('*');

        //            switch (info[0])
        //            {

        //                case "success":
        //                    //TempData["success"] = info[1];
        //                    return Json("pChangesuccess", JsonRequestBehavior.AllowGet);
        //                case "danger":
        //                    // TempData["red"] = info[1];
        //                    return Json("pChangefailed", JsonRequestBehavior.AllowGet);
        //            }

        //        }
        //        catch (Exception)
        //        {
        //            return Json("ExcepError", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        return Json("userNullError", JsonRequestBehavior.AllowGet);
        //    }





        //    //    var customerNo = Session["customerNo"] as string;
        //    //    var tCurrentPassword = password.Text.Trim();
        //    //    var tNewPassword = newPassord.Text.Trim();
        //    //    var tConfirmPassword = confirmPassword.Text.Trim();

        //    //    //var hasNumber = new Regex(@"[0-9]+");
        //    //    //var hasUpperChar = new Regex(@"[A-Z]+");
        //    //    var hasMiniMaxChars = new Regex(@".{4,8}");
        //    //    //var hasLowerChar = new Regex(@"[a-z]+");
        //    //    //var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");


        //    //    if (string.IsNullOrEmpty(customerNo))
        //    //    {
        //    //        feedback.InnerHtml = "<div class='alert alert-danger'>Seems you are not logged in. Kindly log in again.</div>";
        //    //        return;
        //    //    }
        //    //    if (string.IsNullOrWhiteSpace(tCurrentPassword) || string.IsNullOrWhiteSpace(tNewPassword))
        //    //    {
        //    //        feedback.InnerHtml = "<div class='alert alert-danger'>Password  fields should not be empty!!</div>";
        //    //        return;
        //    //    }
        //    //    //if (!hasLowerChar.IsMatch(tConfirmPassword))
        //    //    //{
        //    //    //    feedback.InnerHtml = "<div class='alert alert-danger'>Password should contain Atleast one lower case letter</div>";
        //    //    //    return;
        //    //    //}
        //    //    //if (!hasUpperChar.IsMatch(tConfirmPassword))
        //    //    //{
        //    //    //    feedback.InnerHtml = "<div class='alert alert-danger'>Password should contain Atleast one upper case letter</div>";
        //    //    //    return;
        //    //    //}
        //    //    if (!hasMiniMaxChars.IsMatch(tConfirmPassword))
        //    //    {
        //    //        feedback.InnerHtml = "<div class='alert alert-danger'>Password should be between 4 and 8 characters</div>";
        //    //        return;
        //    //    }
        //    //    // if (!hasNumber.IsMatch(tConfirmPassword))
        //    //    //{
        //    //    //    feedback.InnerHtml = "<div class='alert alert-danger'>Password should contain Atleast one numeric value</div>";
        //    //    //    return;
        //    //    //}

        //    //    // if (!hasSymbols.IsMatch(tConfirmPassword))
        //    //    //{
        //    //    //    feedback.InnerHtml = "<div class='alert alert-danger'>Password should contain Atleast one special case characters</div>";
        //    //    //    return;
        //    //    //}
        //    //    if (tNewPassword == tConfirmPassword)
        //    //    {
        //    //        if (tNewPassword.Length < 4)
        //    //        {
        //    //            feedback.InnerHtml = "<div class='alert alert-danger'>New password is too short.</div>";
        //    //        }
        //    //        else
        //    //        {
        //    //            var status = Config.ObjNav.ResetPassword(customerNo, tCurrentPassword, tNewPassword, tConfirmPassword);
        //    //            var info = status.Split('*');
        //    //            if (info[0] == "success")
        //    //            {
        //    //                Session["changedPassword"] = true;
        //    //                Session["logout"] = true;
        //    //                Response.Redirect("Index");

        //    //            }
        //    //            feedback.InnerHtml = "<div class='alert alert-" + info[0] + "'>" + info[1] + "</div>";
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        feedback.InnerHtml = "<div class='alert alert-danger'>New password does not match confirm password.</div>";
        //    //    }
        //    //}

        //    return Json(customerNo, JsonRequestBehavior.AllowGet);
        //}

    }
}