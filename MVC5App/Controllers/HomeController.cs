using MVC5App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5App.Controllers
{
    public class HomeController : Controller
    {
        StudentEntities1 db = new StudentEntities1();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetStudentList()
        {
            List<StudentModel> StudList = db.StudentDatas.Select(x => new StudentModel
            {
                StudentId = x.StudentId,
                Name=x.Name,
                Roll_NO=x.Roll_NO,
                Department=x.Department
            }).ToList();

            return Json(StudList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudentById(int StudentId)
        {
            StudentData model = db.StudentDatas.Where(x => x.StudentId == StudentId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDataInDatabase(StudentModel model)
        {
            var result = false;
            try
            {
                if (model.StudentId > 0)
                {
                    StudentData Stu = db.StudentDatas.SingleOrDefault(x =>x.StudentId == model.StudentId);
                    Stu.Name = model.Name;
                    Stu.Roll_NO = model.Roll_NO;
                    Stu.Department = model.Department;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    StudentData Stu = new StudentData();
                    Stu.Name = model.Name;
                    Stu.Roll_NO = model.Roll_NO;
                    Stu.Department = model.Department;
                    db.StudentDatas.Add(Stu);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteStudentRecord(int StudentId)
        {
            bool result = false;
            StudentData Stu = db.StudentDatas.SingleOrDefault(x => x.StudentId == StudentId);
            if (Stu != null)
            {
                db.StudentDatas.Remove(Stu);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}