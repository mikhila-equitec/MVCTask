using StudentCrud.Models;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;  
using System.Web;
using Dapper;
using System.Web.Services.Description;
namespace StudentCrud.Repository
{
    public class StudentRepocs
    {
        public readonly string conn = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
        public List<StudentModel> showData()
        {
            using (var con = new SqlConnection(conn))
            {
                con.Open();
                string sql = "Select * from studentData sd left join Departments d on sd.DeptID= d.DeptID";

                return con.Query<StudentModel>(sql).ToList();
            }
        }
        //Insert
        public void insertData(StudentModel sm)
        {
            using (var con = new SqlConnection(conn))
            {
                try
                {

                    con.Open();
                    string sql = "insert into studentData(SName,RollNo,DeptID,DOB,Gender,SAddress,PhoneNum) values(@SName,@RollNo,@DeptID,@DOB,@Gender,@SAddress,@PhoneNum)";
                    con.Execute(sql, sm);
                }
                catch (Exception e)
                {
                    
                }
                
            }
        }

        //Delte
        public void deleteData(int Id)
        {
            using (var con = new SqlConnection(conn))
            {
                con.Open();
                string sql1 = "select * from studentData where Id=@Id";
                var e = con.Query<StudentModel>(sql1, new { Id });
                var del = "insert into studentData_Backup values(@SName,@RollNo,@DeptID,@DOB,@Gender,@SAddress,@PhoneNum)";
                con.Execute(del, e);
                string sql = "delete from studentData where Id=@Id";
                con.Execute(sql, new { Id });
            }
        }
        //ShowDekete
        public List<StudentModel> showDelete()
        {
            using (var con = new SqlConnection(conn))
            {
                con.Open();
                string sql = "select * from studentData_Backup sdb left join departments d on sdb.DeptId=d.DeptId ";
                return con.Query<StudentModel>(sql).ToList();
            }
        }


        //Restore
        public void restoreData(int id)
        {
            using (var con = new SqlConnection(conn))
            {
                con.Open();
                string sql = "SELECT * FROM studentData_Backup WHERE Id = @Id";
                var student = con.QuerySingleOrDefault<StudentModel>(sql, new { Id = id });
                    string sqlr = @" INSERT INTO studentData (SName, RollNo, DeptID, DOB, Gender, SAddress, PhoneNum) VALUES (@SName, @RollNo, @DeptID, @DOB, @Gender, @SAddress, @PhoneNum)";
                    con.Execute(sqlr, student);
                    string sql1 = "DELETE FROM studentData_Backup WHERE Id = @Id";
                con.Execute(sql1, new { Id = id });
            }
        }

        //View
        public StudentModel viewData(int Id)
        {
            using (var con= new SqlConnection(conn))
            {
                con.Open();
                string sql = "select sdb.SName, sdb.RollNo, sdb.DOB, sdb.Gender, sdb.SAddress, sdb.PhoneNum, d.Dept FROM studentData sdb LEFT JOIN Departments d ON sdb.DeptId = d.DeptId WHERE sdb.Id = @Id";
                //string sql = "select * from studentData";
                return con.QueryFirstOrDefault<StudentModel>(sql, new { Id });
            }
        }

        //ViewDeleted
        public StudentModel viewDeletedData(int Id)
        {
            using (var con = new SqlConnection(conn))
            {
                con.Open();
                string sql = "select sdb.SName, sdb.RollNo, sdb.DOB, sdb.Gender, sdb.SAddress, sdb.PhoneNum, d.Dept FROM studentData_Backup sdb LEFT JOIN Departments d ON sdb.DeptId = d.DeptId WHERE sdb.Id = @Id";
                //string sql = "select * from studentData";
                return con.QueryFirstOrDefault<StudentModel>(sql, new { Id });
            }
        }

        //DeltePermanent
        public void deletePData(int Id)
        {
            using (var con = new SqlConnection(conn))
            {
                con.Open();
                string sql = "delete from studentData_Backup where Id=@Id";
                con.Execute(sql, new { Id });
            }
        }

       
        public void updateData(StudentModel student)
        {
            using (var con = new SqlConnection(conn))
            {
                string sql = "update studentData SET SName = @SName, RollNo = @RollNo, DOB = @DOB, Gender = @Gender, SAddress = @SAddress, PhoneNum = @PhoneNum WHERE Id = @Id";
                con.Execute(sql, student);
            }
        }


    }
}