using StudentCrud.Models;
using StudentCrud.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data;
using System.Web.UI;

namespace StudentCrud.Controllers
{
    public class HomeController : Controller
    {

        StudentRepocs std = new StudentRepocs();
        string con = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;

        //public ActionResult Show()
        //{
        //    return View(std.showData());
        //}

        //Insert
        public ActionResult Insert()
        {

            using (var conn = new SqlConnection(con))
            {
                string sql = "select * from Departments";
                var ex = conn.Query<StudentModel>(sql);
                ViewBag.dept = ex;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Insert(StudentModel sm)
        {
            try
            {
                std.insertData(sm);
                TempData["ins"] = "Added";

            }
            catch (Exception e)
            {
                ViewBag.ins = e;
                TempData["ins"] = "Error: " + e;
                return View(sm);
            }
            return RedirectToAction("StudentShow");
        }
        public ActionResult Delete(int id)
        {
            try
            {
                std.deleteData(id);
                TempData["del"] = "Deleted";
            }
            catch(Exception e)
            {
                TempData["del"] = "Error: " + e;
            }
            return RedirectToAction("StudentShow");
        }

        public ActionResult ShowDeleted()
        {
            return View(std.showDelete());
        }

        public ActionResult Restore(int id)
        {
            std.restoreData(id);
            return RedirectToAction("StudentShow");
        }

        public ActionResult ViewD(int id)
        {

            return View(std.viewData(id));
        }
        public ActionResult ViewDeleted(int id)
        {

            return View(std.viewDeletedData(id));
        }


        //public ActionResult RestoreAll(int id)
        //{
        //    std.restoreData(id);
        //    return RedirectToAction("Show");
        //}
        public ActionResult DeletePermanent(int id)
        {
            std.deletePData(id);
            return RedirectToAction("ShowDeleted");
        }
        //public ActionResult ShowAll(int page = 1)
        //{
        //    int pageSize = 5; // Number of records per page
        //    var (students, totalCount) = std.showPaginatedData(page);

        //    ViewBag.CurrentPage = page;
        //    ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        //    return View(students);
        //}
        public ActionResult Edit(int id)
        {
            var student = std.viewData(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }


        //controller
        //[HttpGet]
        //public ActionResult Edit(int Id)
        //{
        //    using (var conn = new SqlConnection(con))
        //    {
        //        string sql = "select * from Departments";
        //        var ex = conn.Query<StudentModel>(sql).ToList();
        //        ViewBag.dept = ex;
        //    }
        //    ViewBag.Id = Id;
        //    return View();
        //}

        [HttpPost]
        public JsonResult Edit(StudentModel student)
        {
            try
            {
                std.updateData(student); 
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public ActionResult StudentShow(int pageNumber = 1, int pageSize = 5)
        {
            using (var conn = new SqlConnection(con))
            {
                
                conn.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@PageNumber", pageNumber);
                parameters.Add("@PageSize", pageSize);
                int count = (pageNumber - 1) * pageSize + 1;
                ViewBag.c = count;
                using (var multi = conn.QueryMultiple("GetPaginatedStudents", parameters, commandType: CommandType.StoredProcedure))
                {
                    var students = multi.Read<StudentModel>().ToList();
                    var totalRecords = multi.Read<int>().FirstOrDefault();

                    ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                    ViewBag.CurrentPage = pageNumber;

                    return View(students);
                }
            }
        }

    }
}