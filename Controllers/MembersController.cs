using InterviewTasks.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace InterviewTasks.Controllers
{
    public class MembersController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public MembersController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            List<MemberModel> members = new List<MemberModel>();
            string connection = "server=.\\sqlexpress;database = interviewtask; integrated security = true";
            string query = "select * from Member";
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
           SqlDataReader reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {
                while (reader.Read())
                {
                    MemberModel model = new MemberModel()
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        Gender = (string)reader["Gender"],
                        Address = (string)reader["Address"],
                        Photo = (string)reader["Photo"]
                    };
                    members.Add(model);

                }
            }
            con.Close();
            return View(members);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(MemberModel model, IFormFile? files)
        {
            string connection = "server=.\\sqlexpress;database = interviewtask; integrated security = true";
            string query = $"insert into Member values ('{model.Name}','{model.PhoneNumber}','{model.Gender}','{model.Address}','{model.Photo}')";
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
           int count = cmd.ExecuteNonQuery();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(wwwRootPath + @"\Images\");
            var extension = Path.GetExtension(files.FileName);
            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                files.CopyTo(fileStreams);
            }
            model.Photo = @"\Images\" + fileName + extension;
            if (count > 0)
            {
                return RedirectToAction("Index");
            }
            con.Close();
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            MemberModel model = null;
            string connection = "server=.\\sqlexpress;database = interviewtask; integrated security = true";
            string query = $"select * from Member where id = {id}";
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    model = new MemberModel()
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        Gender = (string)reader["Gender"],
                        Address = (string)reader["Address"],
                        Photo = (string)reader["Photo"]
                    };
                   
                }
               
            }
            con.Close();
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(MemberModel model/*, IFormFile? files*/)
        {
            string connection = "server=.\\sqlexpress;database = interviewtask; integrated security = true";
            string query = $"update Member set Name = '{model.Name}',PhoneNumber = '{model.PhoneNumber}',Gender = '{model.Gender}',Address = '{model.Address}',Photo = '{model.Photo}' where Id = {model.Id}";
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
           /* string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(wwwRootPath + @"\Images\products");
            var extension = Path.GetExtension(files.FileName);
            if (model.Photo != null)
            {
                var oldImagePath = Path.Combine(wwwRootPath, model.Photo.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }*/
            int count = cmd.ExecuteNonQuery();
            if (count > 0)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int  id)
        {
            MemberModel model = null;
            string connection = "server=.\\sqlexpress;database = interviewtask; integrated security = true";
            string query = $"select * from Member where id = {id}";
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
               while(reader.Read())
                {
                    model = new MemberModel()
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        Gender = (string)reader["Gender"],
                        Address = (string)reader["Address"],
                        Photo = (string)reader["Photo"]
                    };
                }

            }
            con.Close();
            return View(model);
        }
        [HttpPost , ActionName("Delete")]
        public IActionResult Deleted(int id)
        {
            string connection = "server=.\\sqlexpress;database = interviewtask; integrated security = true";
            string query = $"delete from Member where id = {id}";
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int count = cmd.ExecuteNonQuery();
            if(count > 0)
            {
                return RedirectToAction("Index");
            }
            con.Close();
            return View();
        }
    }
}
