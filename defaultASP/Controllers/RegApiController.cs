using Antlr.Runtime.Misc;
using defaultASP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Data.SqlClient;

namespace defaultASP.Controllers
{
    public class RegApiController : ApiController
    {
        string connectionString = @"data source=localhost\SQLEXPRESS;initial catalog=WebApi2;integrated security=True;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework;";
        string tableName = "staff";
        public IHttpActionResult GetAllStaff()
        {
            IList<StaffModel> staff = null;

            using (var ctx = new WebApi2Entities())
            {
                staff = ctx.staffs.Include("WebApi2Entities")
                            .Select(s => new StaffModel()
                            {
                                staff_id = s.staff_id,
                                first_name = s.first_name,
                                last_name = s.last_name,
                                email = s.email,
                                phone = s.phone,
                                active = s.active,
                                store_id = s.store_id
                            }).ToList<StaffModel>();
            }

            if (staff.Count == 0)
            {
                return NotFound();
            }
            
            return Ok(staff);
        }
        public IHttpActionResult GetStaffById(int id)
        {
            IList<StaffModel> staffList = null;
            using (var ctx = new WebApi2Entities())
            {
                staffList = ctx.staffs.Include("WebApi2Entities")
                    .Where(i => i.staff_id == id)
                    .Select(i => new StaffModel()
                    {
                        staff_id = i.staff_id,
                        first_name = i.first_name,
                        last_name = i.last_name,
                        email = i.email,
                        phone = i.phone,
                        active = i.active,
                        store_id = i.store_id
                    }).ToList<StaffModel>();
            }
            if (staffList.Count == 0)
            {
                return NotFound();
            }
            return Ok(staffList);
        }
        public IHttpActionResult PostNewStaff(StaffModel staff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data is formatted incorrectly");
            }
            using (var con = new SqlConnection(connectionString))
            {
                string query = $"insert into {tableName} (first_name, last_name, email, phone, active, store_id) values('{staff.first_name}', '{staff.last_name}', '{staff.email}', '{staff.phone}', '{staff.active}', {staff.store_id});";
                using (var cmd = new SqlCommand(query, con))
                {

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
            /*
            using (var ctx = new WebApi2Entities())
            {
                ctx.staffs.Add(new staff()
                {
                    staff_id = staff.staff_id,
                    first_name = staff.first_name,
                    last_name = staff.last_name,
                    email = staff.email,
                    phone = staff.phone,
                    active = staff.active,
                    store_id = staff.store_id
                });
                ctx.SaveChanges();
            }
            */
            return Ok("Inserted new staff.");
        }
        public IHttpActionResult Put(StaffModel staff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data is formatted incorrectly");
            }
            using (var con = new SqlConnection(connectionString))
            {
                string query = $"update {tableName} set first_name = '{staff.first_name}', last_name = '{staff.last_name}', email = '{staff.email}', phone = '{staff.phone}', active = '{staff.active}', store_id = {staff.store_id} where staff_id={staff.staff_id}";
                using (var cmd = new SqlCommand(query, con))
                {

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
            /*
            using (var ctx = new WebApi2Entities())
            {
                var staffSelected = ctx.staffs.Where(i => i.staff_id == staff.staff_id).First();

                if (staffSelected != null)
                {
                    staffSelected.first_name = staff.first_name;
                    staffSelected.last_name = staff.last_name;
                    staffSelected.email = staff.email;
                    staffSelected.phone = staff.phone;
                    staffSelected.active = staff.active;
                    staffSelected.store_id = staff.store_id;
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            */
            return Ok($"Staff {staff.staff_id} updated");
        }
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest($"{id} is an invalid staff id.");
            }
            using(var con = new SqlConnection(connectionString))
            {
                string query = $"delete from {tableName} where staff_id={id}";
                using(var cmd = new SqlCommand(query, con))
                {

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    } catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
            /*
            using (var ctx = new WebApi2Entities())
            {
                var staff = ctx.staffs
                    .Where(i => i.staff_id == id)
                    .FirstOrDefault();
                ctx.Entry(staff).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            */
            return Ok($"Staff {id} deleted");
        }
    }
}