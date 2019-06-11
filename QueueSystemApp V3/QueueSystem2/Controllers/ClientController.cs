using System;
using QueueSystem2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using System.Web.Hosting;
using System.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace QueueSystem2.Controllers
{
    public class ClientController : ApiController
    {
        QueueSystemDBEntities6 db = new QueueSystemDBEntities6();
       
        //show all clients
        [HttpGet]
        public IHttpActionResult GetClients()
        {
            var clients = db.user.Select(c => new PocoClient
            {
                id = c.id,
                block = (bool)c.block,
                name = c.name,
                gender = c.gender,
                ssn = c.ssn,
                phone = c.phone,
                mail = c.mail,
                person_id = (int)c.person_id,
                username = c.username,
                password = c.password

            }).Where(c => c.person_id == 4).ToList();
            if (clients == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(clients);
            }
        }

        [HttpGet]
        public IHttpActionResult GetClient(int client_id)
        {
            var client = db.user.Select(c => new PocoClient
            {
                id = c.id,
                block = (bool)c.block,
                name = c.name,
                gender = c.gender,
                ssn = c.ssn,
                phone = c.phone,
                mail = c.mail,
                person_id = (int)c.person_id,
                username = c.username,
                password = c.password

            }).Where(c => c.id == client_id).ToList();
            if (client == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(client);
            }
        }

        //*****************************************Register*****************************************//
        //run on browser and type this
        //http://localhost:numofport/api/Client/GetClientRegister?name=gehad&&gender=false&&ssn=55288755&&phone=015852&&mail=g@g.com&&username=gogo&&pasword=123
        [HttpPost]
        public IHttpActionResult Register(string name, string gender, string ssn, string phone, string mail, string username
            , string password)
        {
            user client = new user
            {
                block = false,
                name = name,
                gender = gender,
                ssn = ssn,
                phone = phone,
                mail = mail,
                person_id = 4,
                username = username,
                password = password,
                count = 0

            };
            List<user> users = db.user.Where(c => c.username == client.username || c.mail == client.mail || c.ssn == client.ssn).ToList();
            if (ModelState.IsValid)
            {
                if (users.Count == 0)
                {
                    db.user.Add(client);
                    db.SaveChanges();
                    return Ok(client);

                }
                else
                {
                    return BadRequest("this username or mail or ssn already exists...");
                }

            }
            else
            {
                return BadRequest();
            }
        }


        //*****************************************login*****************************************//
        //run on browser and type this
        //http://localhost:numofport/api/Client/GetClientLogin?username=gehad&&password=123
        [HttpGet]
        public IHttpActionResult Login(string mail, string password)
        {
            var client = db.user.Select(c => new PocoClient
            {
                id = c.id,
                block = (bool)c.block,
                name = c.name,
                gender = c.gender,
                ssn = c.ssn,
                phone = c.phone,
                mail = c.mail,
                person_id = (int)c.person_id,
                username = c.username,
                password = c.password

            }).SingleOrDefault(c => c.mail == mail && c.password == password);
            if (client == null)
            {
                return BadRequest("this user not found...");
            }
            else
            {
                return Ok(client);
            }

        }

        //*****************************************update profile*****************************************//
        [HttpPost]
        public IHttpActionResult UpdateProfile(user client)
        {
            var cli = db.user.SingleOrDefault(c => c.id == client.id);
            var clients = db.user.Where(c => c.mail == client.mail || c.username == client.username).ToList();
            clients.Remove(cli);
            if(client == null)
            {
                return BadRequest("This Client does not exist...");
            }
            else if(client.block == true)
            {
                return BadRequest("You are blocked...");
            }
            else if(clients.Count() != 0)
            {
                return BadRequest("this username or mail or ssn already exists...");
            }
            else
            {
                cli.name = client.name;
                cli.phone = client.phone;
                cli.mail = client.mail;
                cli.username = client.username;
                cli.password = client.password;
                db.SaveChanges();
                return Ok();
            }
        }

        //*****************************************show all organizations*****************************************//
        //run on browser http://localhost:numofport/api/Client/GetOrganizations
        [HttpGet]
        public IHttpActionResult GetOrganizations()
        {

            var orgs = db.organization.Select(c => new PocoOrganizations
            {
                id = c.id,
                name = c.name,
                manager_id = (int)c.manger_id,
                manager_name = c.user.name,
            }).ToList();
            if (orgs == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(orgs);
            }

        }

        //*****************************************Search about organization*****************************************//
        [HttpGet]
        public HttpResponseMessage SearchOrg(string name)
        {
            try
            {
                var httpResponseMessage = new HttpResponseMessage();
                httpResponseMessage.Content = new StringContent
                    (JsonConvert.SerializeObject(db.organization.Where(c => c.name.Contains(name)).ToList()));
                httpResponseMessage.Content.Headers.ContentType = new
                    System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return httpResponseMessage;
            }
            catch
            {
                return null;
            }
        }

        //*****************************************show branches of selected organization*****************************************//
        //run on browser http://localhost:numofport/api/Client/GetBranches?org_id=1
        [HttpGet]
        public IHttpActionResult GetBranches(int org_id)
        {
            var org = db.organization.SingleOrDefault(c => c.id == org_id);
            if(org == null)
            {
                return BadRequest("This organization does not exists...");
            }
            else
            {
                var branches = db.Branch.Select(c => new PocoBranches
                {
                    id = c.id,
                    branch_location = c.branch_location,
                    org_id = (int)c.organization_id
                }).Where(c => c.org_id == org_id).ToList();
                if (branches == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(branches);
                }
            }  
        }

        //*****************************************show services of selected organization*****************************************//
        //run on browser http://localhost:numofport/api/Client/GetServices?org_id=1
        [HttpGet]
        public IHttpActionResult GetServices(int org_id)
        {
            var org = db.organization.SingleOrDefault(c => c.id == org_id);
            if (org == null)
            {
                return BadRequest("This organization does not exists...");
            }
            else
            {
                var services = db.Services_.Select(c => new PocoServices
                {
                    id = c.id,
                    name = c.name,
                    start = (TimeSpan)c.starttime,
                    end = (TimeSpan)c.endtime,
                    period = (TimeSpan)c.period,
                    break_start = (TimeSpan)c.breakStart,
                    break_period = (TimeSpan)c.breakPeriod,
                    org_id = (int)c.org_id
                }).Where(c => c.org_id == org_id).ToList();
                if (services == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(services);
                }
            }
        }

        //*****************************************show all requirements doc for some service*****************************************//
        //run on browser http://localhost:numofport/api/Client/GetReqDocs?service_id=1
        [HttpGet]
        public IHttpActionResult GetReqDocs(int service_id)
        {
            var service = db.Services_.SingleOrDefault(c => c.id == service_id);
            if(service == null)
            {

                return BadRequest("this service not exists...");
            }
            else
            {
                var req_docs = db.Requirments_Doc.Select(c => new PocoReqDocs
                {
                    id = c.id,
                    doc_id = (int)c.doc_id,
                    doc_name = c.Doc.name,
                    service_id = (int)c.Service_id,
                    service_name = c.Services_.name,
                    notes = c.notes
                }).Where(x => x.service_id == service_id).ToList();

                if (req_docs == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(req_docs);
                }
            }

        }


        [HttpGet]
        public List<PocoReqDocs> GetReqDocs1(int service_id)
        {
            var req_docs = db.Requirments_Doc.Select(c => new PocoReqDocs
            {
                id = c.id,
                doc_id = (int)c.doc_id,
                doc_name = c.Doc.name,
                service_id = (int)c.Service_id,
                service_name = c.Services_.name,
                notes = c.notes
            }).Where(x => x.service_id == service_id).ToList();

            if (req_docs == null)
            {
                return null;
            }
            else
            {
                return req_docs;
            }
        }

        //employee requests
        public int EmployeeRequests(int emp_id)
        {
            var emp = db.user.SingleOrDefault(c => c.id == emp_id);
            if(emp == null)
            {
                return 0;
            }
            else
            {
                int count = 0;
                var allReq = db.Request.Select(c => new PocoRequest
                {
                    id = c.id,
                    dateTime = (DateTime)c.timeOfReq,
                    client_id = (int)c.user_id,
                    emp_id = (int)c.empoyee_id,
                    service_id = (int)c.service_id,
                    state = c.state
                }).ToList();
                foreach (var x in allReq)
                {
                    if (x.emp_id == emp_id)
                    {
                        count++;
                    }
                }
                return count;
            }
            
        }

        //file upload

        [HttpPost]
        public IHttpActionResult UploadFile(int request_id ,int service_id , List<string> files)
        {
            var reqDocs = GetReqDocs1(service_id);
            var req = db.Request.SingleOrDefault(c => c.id == request_id); 
            if (files != null)
            {
                if(files.Count != reqDocs.Count)
                {
                    db.Request.Remove(req);
                    db.SaveChanges();
                    return BadRequest("Uploaded files more or less than that required...");
                }
                else
                { 
                    for (int i = 0; i < files.Count; i++)
                    {
                        if (files[i] != null)
                        {
                            var reqAtt = new Request_attachement
                            {
                                path = files[i],
                                request_id = request_id,
                                Requried_id = reqDocs[i].id
                            };
                            db.Request_attachement.Add(reqAtt);
                            db.SaveChanges();
                        }
                    }
                }
                return Ok("Upload Successeed...");
            }
            else
            {
                db.Request.Remove(req);
                db.SaveChanges();
                return BadRequest("You must upload files...");
            }

            // Return status code  
            
        }

        //*****************************************make request*****************************************//
        //, [FromUri] string[] docPathes
        //api/Client/MakeRequst?branch_id=4&&client_id=34&&service_id=8&&files=f1&files=f2
        [HttpPost]
        public IHttpActionResult MakeRequst(int branch_id, int client_id, int service_id , [FromUri] List<string> files)
        {
            var client = db.user.SingleOrDefault(c => c.id == client_id);
            var service = db.Services_.SingleOrDefault(c => c.id == service_id);
            var branch = db.Branch.SingleOrDefault(c => c.id == branch_id);
            if (client == null)
            {
                return BadRequest("This client does not exist...");
            }
            else if (service == null)
            {
                return BadRequest("This service does not exist...");
            }
            else if (branch == null)
            {
                return BadRequest("This branch does not exist...");
            }
            else
            {
                var empReq = db.Emp_Org_Services.Where(c => c.branch_id == branch_id && c.service_id == service_id).ToList();
                List<int> reqCounters = new List<int>();
                foreach (var i in empReq)
                {
                    int count = EmployeeRequests((int)i.emp_id);
                    reqCounters.Add(count);
                }
                int a = reqCounters.Min();
                int b = reqCounters.IndexOf(a);
                var d = empReq[b];

                var request = new Request
                {
                    timeOfReq = DateTime.Now,
                    user_id = client_id,
                    service_id = service_id,
                    empoyee_id = d.emp_id,
                    state = "pending",
                    branch_id = branch_id
                };

                var x = db.Request.Where(c => c.branch_id == branch_id && c.service_id == service_id && c.user_id == client_id && c.state == "pending").ToList();
                var finish = db.Request.Where(c => c.branch_id == branch_id && c.service_id == service_id && c.user_id == client_id && c.state == "finished").ToList();
                if (ModelState.IsValid)
                {
                    if (x.Count == 0)
                    {
                        if(finish != null)
                        {
                            db.Request.Add(request);
                            db.SaveChanges();
                            var client_name = db.user.SingleOrDefault(c => c.id == client_id).username;
                            var service_name = db.Services_.SingleOrDefault(c => c.id == service_id).name;
                            var noti = new Notification
                            {
                                msg = "client " + client_name + " sent a request for service " + service_name,
                                seen = false,
                                dateTime = DateTime.Now,
                                type_noti = "request",
                                emp_id = request.empoyee_id,
                                client_id = request.user_id,
                                type_noti_id = request.id
                            };
                            db.Notification.Add(noti);
                            db.SaveChanges();
                            return UploadFile(request.id, service_id, files);
                        }
                        else
                        {
                            return BadRequest("You can't send another request for same service before you reserve...");
                        }
                       
                    }
                    else
                    {
                        return BadRequest("This request sent before please wait respond...");
                    }

                }
                else
                {
                    return BadRequest("Data not valid...");
                }
            }
        }

        //encode image to base64
        [HttpGet]
        public string EncodeBase64(string toEncode)
        {
            byte[] toEncodeAdBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAdBytes);
            return returnValue;
        }

        //decode base64 to string
        [HttpGet]
        public string DecodeBase64(string toDecode)
        {
            byte[] b = Convert.FromBase64String(toDecode);
            var strOriginal = System.Text.Encoding.UTF8.GetString(b);
            return strOriginal;
        }

        //get attachments which uploaded for some request
        [HttpGet]
        public List<string> GetAttachments(int req_id)
        {
            List<string> pathes = new List<string>();
            var req = db.Request_attachement.Where(c => c.request_id == req_id).ToList();
            if (req == null)
            {
                return null;
            }
            else
            {
                foreach (var x in req)
                {
                    pathes.Add(x.path);
                }
                return pathes;
            }

        }

        

        //*****************************************show requests*****************************************//
        [HttpGet]
        public IHttpActionResult GetRequests(int client_id)
        {
            var user = db.user.SingleOrDefault(c => c.id == client_id);
            if(user == null)
            {
                return BadRequest("No Client...");
            }
            else
            {
                var requests = db.Request.Select(c => new PocoRequest
                {
                    id = c.id,
                    dateTime = (DateTime)c.timeOfReq,
                    client_id = (int)c.user_id,
                    emp_id = (int)c.empoyee_id,
                    emp_name = c.user.name,
                    service_id = (int)c.service_id,
                    service_name = c.Services_.name,
                    state = c.state,
                    branch_id = (int)c.branch_id,
                    branch_location = c.Branch.branch_location
                }).Where(c => c.client_id == client_id).ToList();
                int count = 1;
                for(int x=0;x<requests.Count;x++)
                {
                    requests[x].req_name = "Req" + count;
                    requests[x].reqatt = GetAttachments(requests[x].id);
                    count++;
                }
                if (requests == null)
                {
                    return BadRequest("No Requests...");
                }
                return Ok(requests);
            }
            
        }


        [HttpPost]
        public IHttpActionResult UpdateFile(int request_id , List<string> files)
        {
            var reqAtt = db.Request_attachement.Where(c => c.request_id == request_id).ToList();;

            if (files.Count > 0)
            {
                if (files.Count != reqAtt.Count)
                {
                    return BadRequest("Uploaded files more or less than that required...");
                }
                else
                {
                    //Loop through uploaded files  
                    for (int i = 0; i < files.Count; i++)
                    {
                        if (files[i] != null)
                        {
                            reqAtt[i].path = files[i];
                            db.SaveChanges();
                        }
                    }
                }
                return Ok("Upload Successeed...");
            }
            else
            {
                return BadRequest("You must upload files...");
            }

            // Return status code  

        }

        //*****************************************update request*****************************************//
        [HttpPost]
        public IHttpActionResult UpdateRequst(int requset_id , [FromUri] List<string> files)
        {
            
            var newRequest = db.Request.SingleOrDefault(c=> c.id == requset_id && c.state == "pending" || c.state == "");
            if(newRequest == null)
            {
                return BadRequest("no request exists...");
            }
            else
            {
                var newReqAtt = db.Request_attachement.Where(c => c.request_id == newRequest.id).ToList();
                if(newReqAtt == null)
                {
                    return BadRequest("no documents exists...");
                }
                else
                {
                    var request = db.Request.SingleOrDefault(c => c.id == requset_id);
                    var client_name = db.user.SingleOrDefault(c => c.id == request.user_id).username;
                    var service_name = db.Services_.SingleOrDefault(c => c.id == request.service_id).name;
                    var noti = new Notification
                    {
                        msg = "client " + client_name + " update a request for service " + service_name,
                        seen = false,
                        dateTime = DateTime.Now,
                        type_noti = "request",
                        emp_id = request.empoyee_id,
                        client_id = request.user_id,
                        type_noti_id = request.id
                    };
                    db.Notification.Add(noti);
                    db.SaveChanges();
                    return UpdateFile(requset_id , files);
                }
            }
            
        }

        //*****************************************Delete request*****************************************//
        [HttpDelete]
        public IHttpActionResult DeleteRequest(int requset_id)
        {
            var req = db.Request.SingleOrDefault(c => c.id == requset_id);
            if(req == null)
            {
                return BadRequest("No request exists...");
            }
            else
            {
                db.Request.Remove(req);
                db.SaveChanges();
                return Ok();
            }
            
        }


        //get all appointments 

        [HttpGet]
        public List<PocoAppointments> GetAppointments(int branch_id,int service_id)
        {
            var appointments = db.Appointments_.Select(c => new PocoAppointments
            {
                id = c.id,
                dateTime = (DateTime)c.time,
                service_id = (int)c.service_id,
                user_id = (int)c.user_id,
                branch_id = (int)c.branch_id
            }).Where(x => x.service_id == service_id && x.branch_id == branch_id).ToList();

            if (appointments == null)
            {
                return null;
            }
            else
            {
                return appointments;
            }

        }

        //*****************************************show available time for service*****************************************//
        [HttpGet]
        public List<string> GetAvailableTime(int branch_id,int service_id, string date)
        {
            Services_ service = db.Services_.SingleOrDefault(c => c.id == service_id);
            TimeSpan workPeriod = (TimeSpan)service.endtime - (TimeSpan)service.starttime;
            int workPeriodMinutes = (int)workPeriod.TotalMinutes;
            TimeSpan period = (TimeSpan)service.period;
            int servicePeriodMinutes = (int)period.TotalMinutes;
            int numRoles = workPeriodMinutes / servicePeriodMinutes;
            List<string> times = new List<string>();
            TimeSpan start = (TimeSpan)service.starttime;
            TimeSpan breakStart = (TimeSpan)service.breakStart;
            TimeSpan breakPeriod = (TimeSpan)service.breakPeriod;
            int breakPeriodMinutes = (int)breakPeriod.TotalMinutes;
            int breakRoles = breakPeriodMinutes / servicePeriodMinutes;
            TimeSpan breakEnd = breakStart + breakPeriod;

            for (int i = 0; i <= numRoles - breakRoles; i++)
            {
                if (start == breakStart)
                {
                    start = breakEnd;
                }
                else
                {
                    string s = start.ToString();
                    times.Add(s);
                    start = start + period;
                }

            }

            

            var service_emps = db.Emp_Org_Services.Where(c => c.service_id == service_id && c.branch_id == branch_id).ToList();

            int emps = service_emps.Count();
            List<string> allTimes = new List<string>();
            for(int i = 0; i < emps; i++)
            {
                foreach(var x in times)
                {
                    allTimes.Add(x);
                }
                
            }

            var app = GetAppointments(branch_id, service_id);
            foreach (var x in app)
            {
                string d = x.dateTime.ToString("yyyy-MM-dd");
                if (date.Equals(d))
                {
                    string t = x.dateTime.ToString("H:mm:ss");
                    TimeSpan ts = TimeSpan.Parse(t);
                    allTimes.Remove(t);
                }
            }


            return allTimes;
        }

        //*****************************************reserve*****************************************//
        [HttpPost]
        public IHttpActionResult Reserve(int branch_id,int client_id , int service_id , string date , string time)
        {
            var req = db.Request.SingleOrDefault(c => c.branch_id == branch_id && c.user_id == client_id &&
            c.service_id == service_id && c.state == "accepted");
            var allTimes = GetAvailableTime(branch_id, service_id, date);
            var dateTime = Convert.ToDateTime(date + "T" + time);
			PocoReservation res = new PocoReservation();
            if (req == null)
            {
				res.msg = "You must send request before reserve...";
            }
            else if(req.state == "pending")
            {
                res.msg = "Your request is pending...";
            }
            else if (req.state == "rejected")
            {
                res.msg = "Your request is rejected...";
            }
            else if (dateTime < DateTime.Now)
            {
                res.msg = "You can't reserve in old date...";
            }
            else if (allTimes == null)
            {
                res.msg = "No available times in this day...";
            }
            else if (req.state == "finished")
            {
                res.msg = "This reservation already exists...";
            }
            else
            {
                var timeOfChange = (DateTime)req.timeOfChange;
                DateTime timeToReserve = timeOfChange.AddDays((double)req.Services_.toFinish);
                if(dateTime < timeToReserve)
                {
                    res.msg = "Please wait to finish your paper , you can reserve after " + timeToReserve.ToString("dd/MM/yyyy") + "...";
                }
                else
                {
                    var appoint = new Appointments_
                    {
                        time = dateTime,
                        service_id = service_id,
                        user_id = client_id,
                        branch_id = branch_id,
                        state = "pending"
                    };
                    if (ModelState.IsValid)
                    {
                        req.state = "finished";
                        db.Appointments_.Add(appoint);
                        db.SaveChanges();
                        res.msg = "Reservation Done Successfully...";
                    }
                    else
                    {
                        res.msg = "data not valid...";
                    }
                }
                
            }
			return Ok(res);

        }

        //*****************************************get all reservation for some client*****************************************//
        [HttpGet]
        public IHttpActionResult GetReservations(int client_id)
        {
            var user = db.user.SingleOrDefault(c => c.id == client_id);
            if (user == null)
            {
                return BadRequest("No Client...");
            }
            else
            {
                var apps = db.Appointments_.Select(c => new PocoAppointments
                {
                    id = c.id,
                    dateTime = (DateTime)c.time,
                    service_id = (int)c.service_id,
                    service_name = c.Services_.name,
                    user_id = (int)c.user_id,
                    branch_id = (int)c.branch_id,
                    branch_location = c.Branch.branch_location
                }).Where(c => c.user_id == client_id).ToList();
                int count = 1;

                foreach (var x in apps)
                {
                    x.res_name = "Res" + count;
                    count++;
                    var reminder = x.dateTime -  DateTime.Now;
                    if(reminder.TotalSeconds<0)
                    {
                        apps.Remove(x);
                    }
                    else
                    {
                        x.days = reminder.Duration().Days;
                        x.hours = reminder.Duration().Hours;
                        x.minutes = reminder.Duration().Minutes;
                        x.seconds = reminder.Duration().Seconds;
                    }
                    
                }

                apps.OrderBy(x => x.dateTime).ToList();
                
                if(apps == null)
                {
                    return BadRequest("No reservations...");
                }
                else
                {
                    return Ok(apps);
                }
            }
        }

        //*****************************************update reservation*****************************************//
        [HttpPost]
        public IHttpActionResult UpdateReservation(int reservation_id , string date , string time)
        {
            DateTime dateTime = DateTime.Parse((date + "T" + time));
            var app0 = db.Appointments_.SingleOrDefault(c => c.id == reservation_id);
            
            if (app0 == null)
            {
                return BadRequest("this reservation not exists...");
            }
            else
            {
                var reqA = db.Request.SingleOrDefault(c => c.branch_id == app0.branch_id && c.user_id == app0.user_id &&
                c.service_id == app0.service_id && c.state == "finished");
                var timeOfChange = (DateTime)reqA.timeOfChange;
                DateTime timeToReserve = timeOfChange.AddDays((double)reqA.Services_.toFinish);
                var allTimes = GetAvailableTime((int)app0.branch_id, (int)app0.service_id, date);
                if (dateTime < DateTime.Now)
                {
                    return BadRequest("You can not reserve in old date...");
                }
                else if (dateTime < timeToReserve)
                {
                    return BadRequest("Please wait to finish your paper , you can reserve after "
                        + timeToReserve.ToString("dd/MM/yyyy") + "...");
                }
                else if (allTimes == null)
                {
                    return BadRequest("No available times in this day...");
                }
                else
                {
                    app0.time = dateTime;
                    db.SaveChanges();
                    return Ok();
                }
            }
           
        }


        //*****************************************delete reservation*****************************************//
        [HttpPost]
        public IHttpActionResult DeleteReservation(PocoAppointments app)
        {
            var app1 = db.Appointments_.SingleOrDefault(c => c.id == app.id);
            if (app1 == null)
            {
                return BadRequest("this reservation not exists...");
            }
            else
            {
                var req = db.Request.SingleOrDefault(c => c.service_id == app.service_id && c.user_id == app.user_id && c.branch_id == app.branch_id);
                req.state = "accepted";
                db.Appointments_.Remove(app1);
                db.SaveChanges();
                return Ok("Deleted Succussfully...");
            }
        }








        //*************************************** get all unavailabe dates for service***************************************//
        [HttpGet]
        public List<string> GetUnAvailableDates(int branch_id,int service_id)
        {
            List<string> dates = new List<string>();
            var apps = db.Appointments_.Where(c => c.branch_id == branch_id && c.service_id == service_id).ToList();
            foreach (var x in apps)
            {
                string date = ((DateTime)x.time).ToString("yyyy-MM-dd");
                dates.Add(date);
            }
            List<string> datesDistinct = dates.Distinct().ToList();
            List<string> unAvailableDates = new List<string>();
            List<string> availableDates = new List<string>();
            foreach (var x in datesDistinct)
            {
                var allTimes = GetAvailableTime(branch_id , service_id , x);
                if (allTimes.Count == 0)
                {
                    unAvailableDates.Add(x);
                }
                else
                {
                    availableDates.Add(x);
                }
            }
            return unAvailableDates;
        }



        [HttpGet]
        public IHttpActionResult Search(string name)
        {
            
            List<string> names = new List<string>();
            var orgs = db.organization.Select(c => new PocoOrganizations
            {
                id = c.id,
                name = c.name,
                manager_id = (int)c.manger_id,
                manager_name = c.user.name
            }).Where(c => c.name.StartsWith(name)).ToList();

            var branches = db.Branch.Select(c => new PocoBranches
            {
                id = c.id,
                branch_location = c.branch_location,
                org_id = (int)c.organization_id,
            }).Where(c => c.branch_location.StartsWith(name)).ToList();

            var services = db.Services_.Select(c => new PocoServices
            {
                id = c.id,
                name = c.name,
                start = (TimeSpan)c.starttime,
                end = (TimeSpan)c.endtime,
                period = (TimeSpan)c.period,
                break_start = (TimeSpan)c.breakStart,
                break_period = (TimeSpan)c.breakPeriod,
                org_id = (int)c.org_id
            }).Where(c => c.name.StartsWith(name)).ToList();

            if (orgs.Count != 0)
            {
                return Ok(orgs);
            }

            else if (branches.Count != 0)
            {
                return Ok(branches);
            }

            else if (services.Count != 0)
            {
                return Ok(services);
            }

            else
            {
                return BadRequest("No result for your search...");
            } 

        }



        //***************************************Send complaint and inquire***************************************//
        [HttpPost]
        public IHttpActionResult Send_Complaint_And_Inquire(string msg , string type , int client_id , int branch_id)
        {
            if(msg == null || type == null || (type != "complaint" && type != "inquire" && type != "feedback"))
            {
                return BadRequest("Data sent not valid...");
            }
            else
            {
                Messages message = new Messages
                {
                    msg = msg,
                    replay=null,
                    type = type,
                    status = false,
                    from = client_id,
                    to = branch_id,
                    emp_id = null
                };
                if (ModelState.IsValid)
                {
                    db.Messages.Add(message);
                    db.SaveChanges();
                    var client_name = db.user.SingleOrDefault(c => c.id == client_id).username;
                    if(type == "complaint")
                    {
                        var noti = new Notification
                        {
                            msg = "client " + client_name + " sent complaint : " + msg,
                            seen = false,
                            dateTime = DateTime.Now,
                            type_noti = "complaint",
                            client_id = client_id,
                            type_noti_id = message.Id
                        };
                        db.Notification.Add(noti);
                        db.SaveChanges();
                    }
                    else if (type == "inquire")
                    {
                        var noti = new Notification
                        {
                            msg = "client " + client_name + " sent inquire : " + msg + " ?",
                            seen = false,
                            dateTime = DateTime.Now,
                            type_noti = "inquire",
                            client_id = client_id,
                            type_noti_id = message.Id
                        };
                        db.Notification.Add(noti);
                        db.SaveChanges();
                    }
                    else if (type == "feedback")
                    {
                        var noti = new Notification
                        {
                            msg = "client " + client_name + " sent feedback : " + msg,
                            seen = false,
                            dateTime = DateTime.Now,
                            type_noti = "feedback",
                            client_id = client_id,
                            type_noti_id = message.Id
                        };
                        db.Notification.Add(noti);
                        db.SaveChanges();
                    }
                    return Ok();
                }
                else
                {
                    return BadRequest("Model state not valid...");
                }
                
            }
            
        }

        [HttpGet]
        public IHttpActionResult Get_Notificaions(int client_id)
        {
            var notis = db.Notification.Select(c => new PocoNotifications {
                    id = c.Id,
                    msg = c.msg,
                    seen = (bool)c.seen,
                    dateTime = (DateTime)c.dateTime,
                    type_noti = c.type_noti,
                    type_noti_id = (int)c.type_noti_id,
                    emp_id = (int)c.emp_id,
                    client_id = (int)c.client_id
            }).Where(x => x.client_id == client_id && x.type_noti == "replyrequest");
            var notis1 = notis.OrderByDescending(c => c.dateTime).ToList();
            return Ok(notis1);
        }

        [HttpGet]
        public IHttpActionResult Get_Seen_Notificaions(int client_id)
        {
            var seen_notis = db.Notification.Select(c => new PocoNotifications
            {
                id = c.Id,
                msg = c.msg,
                seen = (bool)c.seen,
                dateTime = (DateTime)c.dateTime,
                type_noti = c.type_noti,
                type_noti_id = (int)c.type_noti_id,
                emp_id = (int)c.emp_id,
                client_id = (int)c.client_id
            }).Where(x => x.client_id == client_id && x.seen == true && x.type_noti == "replyrequest");
            var seen_notis1 = seen_notis.OrderByDescending(c => c.dateTime).ToList();
            return Ok(seen_notis1);
        }

        [HttpGet]
        public IHttpActionResult Get_UnSeen_Notificaions(int client_id)
        {
            var unseen_notis = db.Notification.Select(c => new PocoNotifications
            {
                id = c.Id,
                msg = c.msg,
                seen = (bool)c.seen,
                dateTime = (DateTime)c.dateTime,
                type_noti = c.type_noti,
                type_noti_id = (int)c.type_noti_id,
                emp_id = (int)c.emp_id,
                client_id = (int)c.client_id
            }).Where(x => x.client_id == client_id && x.seen == false && x.type_noti == "replyrequest");
            var unseen_notis1 = unseen_notis.OrderByDescending(c => c.dateTime).ToList();
            return Ok(unseen_notis1);
        }

        [HttpGet]
        public IHttpActionResult Count_UnSeen_Notis(int client_id)
        {
            int count_unseen_notis = db.Notification.Where(x => x.client_id == client_id && x.seen == false && x.type_noti == "replyrequest").Count();
            return Ok(count_unseen_notis);
        }

        [HttpPost]
        public IHttpActionResult SeeNotifications(int noti_id)
        {
            var noti = db.Notification.SingleOrDefault(c => c.Id == noti_id);
            noti.seen = true;
            db.SaveChanges();
            return Ok();
        }
    }
}
