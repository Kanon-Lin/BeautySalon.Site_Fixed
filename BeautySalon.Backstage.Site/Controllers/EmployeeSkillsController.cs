using BeautySalon.Backstage.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BeautySalon.Backstage.Site.Controllers
{
        public class EmployeeSkillsController : Controller
        {
            private AppDbContext db = new AppDbContext();

            [HttpGet]
            public ActionResult Index()
            {
                var employeeSkills = db.EmployeeSkills.Include(e => e.Employee).Include(e => e.ServiceCategory).OrderBy(e => e.EmployeeID);
                return View(employeeSkills.ToList());
            }
            [HttpGet]
            public ActionResult Details(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                EmployeeSkill employeeSkill = db.EmployeeSkills.Find(id);
                if (employeeSkill == null)
                {
                    return HttpNotFound();
                }
                return View(employeeSkill);
            }

            /// /////////////////////////////////////////////////////////////////////////

            public ActionResult SetEmpSkills(int? employeeID)
            {
                // 查找所有員工
                var employees = db.Employees.ToList();
                ViewBag.EmployeeID = new SelectList(employees, "EmployeeID", "Nickname", employeeID);

                // 查找已經屬於該員工的技能
                var existingSkills = db.EmployeeSkills
                                      .Where(es => es.EmployeeID == employeeID)
                                      .Select(es => es.CategoryID)
                                      .ToList();

                // 準備技能資料，並將已存在的技能標示為已勾選
                var skills = db.ServiceCategories.ToList();
                var skillCategories = skills.Select(s => new SelectListItem
                {
                    Value = s.CategoryID.ToString(),
                    Text = s.CategoryName,
                    Selected = existingSkills.Contains(s.CategoryID) // 如果該技能已經存在，則標記為已選中
                }).ToList();

                ViewBag.SkillCategories = skillCategories;

                return View();
            }





            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult SetEmpSkills(int employeeID, int[] selectedSkills)
            {
                // 查找該員工已經擁有的技能
                var existingSkills = db.EmployeeSkills
                                      .Where(es => es.EmployeeID == employeeID)
                                      .ToList();

                // 刪除取消勾選的技能
                foreach (var skill in existingSkills)
                {
                    if (!selectedSkills.Contains(skill.CategoryID))
                    {
                        db.EmployeeSkills.Remove(skill);
                    }
                }

                // 新增新勾選的技能
                foreach (var skillID in selectedSkills)
                {
                    if (!existingSkills.Any(es => es.CategoryID == skillID))
                    {
                        var newSkill = new EmployeeSkill
                        {
                            EmployeeID = employeeID,
                            CategoryID = skillID
                        };
                        db.EmployeeSkills.Add(newSkill);
                    }
                }

                db.SaveChanges();

                //return RedirectToAction("Index");
                return RedirectToAction("Index", "Employees"); // 回到員工列表頁面
            }

            // GET: EmployeeSkills/Edit/5
            public ActionResult Edit(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                EmployeeSkill employeeSkill = db.EmployeeSkills.Find(id);
                if (employeeSkill == null)
                {
                    return HttpNotFound();
                }
                ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeNo", employeeSkill.EmployeeID);
                ViewBag.CategoryID = new SelectList(db.ServiceCategories, "CategoryID", "CategoryName", employeeSkill.CategoryID);
                return View(employeeSkill);
            }

            // POST: EmployeeSkills/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit([Bind(Include = "EmployeeSkillsID,EmployeeID,CategoryID")] EmployeeSkill employeeSkill)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(employeeSkill).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeNo", employeeSkill.EmployeeID);
                ViewBag.CategoryID = new SelectList(db.ServiceCategories, "CategoryID", "CategoryName", employeeSkill.CategoryID);
                return View(employeeSkill);
            }

            // GET: EmployeeSkills/Delete/5
            public ActionResult Delete(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                EmployeeSkill employeeSkill = db.EmployeeSkills.Find(id);
                if (employeeSkill == null)
                {
                    return HttpNotFound();
                }
                return View(employeeSkill);
            }

            // POST: EmployeeSkills/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
                EmployeeSkill employeeSkill = db.EmployeeSkills.Find(id);
                db.EmployeeSkills.Remove(employeeSkill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }

