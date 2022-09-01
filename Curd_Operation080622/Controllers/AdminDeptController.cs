using Microsoft.AspNetCore.Mvc;
using Curd_Operation080622.Models;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using ClosedXML.Excel;
using System.IO;
//using iText.Kernel.Pdf;
//using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace Curd_Operation080622.Controllers
{   
    public class AdminDeptController : Controller
    {
        private readonly DatabseContext _db;

        public AdminDeptController(DatabseContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Create(int id=0)
        {
            ViewBag.BT = "Save";
            Teachernew obj =new Teachernew();


            obj.lstGenders = _db.Genders.ToList();
            var HData = _db.Hobbies.ToList();
            obj.lstHobbies = HData.Select(x => new HobbieDetail
            {
                Hobby_Id = x.Hobby_Id,
                Hobby_Name = x.Hobby_Name,
                ischecked = false,

            }).ToList();

            if (id>0)
            {
                var data = (from a in _db.teachers where a.Teacher_Id==id select a).ToList();
                obj.Teacher_Id = data[0].Teacher_Id;
                obj.Teacher_Name = data[0].Teacher_Name;
                obj.Teacher_Address = data[0].Teacher_Address;
                obj.Teacher_Age = data[0].Teacher_Age;
                obj.Teahching_Class = data[0].Teahching_Class;
                obj.Teacher_Salary = data[0].Teacher_Salary;
                obj.DOJ = data[0].DOJ;
                obj.Country = data[0].Country;
                obj.Gender = data[0].Gender;
                obj.State = data[0].State;
                //obj.Hobbie = data[0].Hobbie;
                string[] arr = data[0].Hobbie.Split(',');
                foreach(var a in obj.lstHobbies)
                {

                foreach(var b in arr)
                {
                    if(a.Hobby_Name==b)
                        {
                            a.ischecked= true;
                            break;
                        }

                }

                }

                ViewBag.BT = "Update";
            }
            //obj.lstHobbies = _db.Hobbies.ToList();
          
            ViewBag.ctr = _db.Countries.ToList();
            ViewBag.STT = _db.tblStates.Where(m => m.CId == obj.Country).ToList();
            return View(obj);


        }

        [HttpPost]
        public IActionResult Create(Teachernew _tech)
        {
            string HOBS = "";
            foreach(var a in _tech.lstHobbies)
            {
                if(a.ischecked==true)
                {
                    HOBS += a.Hobby_Name + ",";
                }
            }
            HOBS = HOBS.TrimEnd(',');

            Teacher _TCH = new Teacher();
            _TCH.Teacher_Id = _tech.Teacher_Id;
            _TCH.Teacher_Name= _tech.Teacher_Name;
            _TCH.Teacher_Salary=_tech.Teacher_Salary;
            _TCH.Teacher_Address= _tech.Teacher_Address;
            _TCH.Teacher_Age=_tech.Teacher_Age;
            _TCH.Teahching_Class = _tech.Teahching_Class;
            _TCH.DOJ=_tech.DOJ;
            _TCH.Country = _tech.Country;
            _TCH.Gender = _tech.Gender;
            _TCH.Gender = _tech.Gender;
            _TCH.State = _tech.State;


            if (_tech.Teacher_Id > 0)
            {
               
                _db.Entry(_TCH).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                _db.teachers.Add(_TCH);
            }
          
            _db.SaveChanges();
            return RedirectToAction("Show");
        }
        [HttpGet]
        public IActionResult Show()
        {

            //var data = _db.teachers.ToList();
            var data = (from a in _db.teachers
                        join b in _db.Countries 
                        on a.Country equals b.CId

                        join c in _db.Genders
                        on a.Gender equals c.Gender_Id

                        join d in _db.tblStates
                        on a.State equals d.SId

                        into Con
                        from d in Con.DefaultIfEmpty()

                        select new TeacherJoin
                        {
                            Teacher_Id = a.Teacher_Id,
                            Teacher_Age = a.Teacher_Age,
                            Teacher_Name = a.Teacher_Name,
                            Teacher_Salary = a.Teacher_Salary,
                            Teahching_Class = a.Teahching_Class,
                            Teacher_Address = a.Teacher_Address,
                            DOJ=a.DOJ,
                            CName=b.CName,
                            Hobbie=a.Hobbie,
                            //State=d.SName,

                            Gender_Name = c.Gender_Name

                            //Hobby_Name = d.Hobby_Name,

                            //Country=a.Country


                        }
                        ).ToList();
            return View(data);
        }


        public JsonResult GetState(int A)
        {
            var data = (from a in _db.tblStates where a.CId == A select a).ToList();
            return Json(data);
        }

        public IActionResult Delete(int id=0)
        {
            var data=_db.teachers.Find(id);
            _db.teachers.Remove(data);
            _db.SaveChanges();
            return RedirectToAction("Show");
        }


        public IActionResult ExportExcel()
        {
            List<Teacher> Report = (from a in this._db.teachers.Take(200)
                                    select a).ToList();
            return View(Report);
        }


        [HttpPost]
        public IActionResult Export()
        {
            DataTable dt = new DataTable("Excel_Sheet_Report");
            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("Teacher_Id"),
                                        new DataColumn("Teachet_Name"),
                                        new DataColumn("Teacher_Age"),
                                        new DataColumn("Teacher_Address"),
                                        new DataColumn("Teacher_Salary"),
                                        new DataColumn("Teahching_Class"),
                                        new DataColumn("DOJ"),
                                        new DataColumn("Country"),
                                        new DataColumn("Gender"),
                                        new DataColumn("Hobbie")});

            var teachers = from a in this._db.teachers.Take(200)
                          select a;

            foreach (var teacher in teachers)
            {
                dt.Rows.Add(teacher.Teacher_Id,
                    teacher.Teacher_Name,
                    teacher.Teacher_Age,
                    teacher.Teacher_Address,
                    teacher.Teacher_Salary,
                    teacher.Teahching_Class,
                    teacher.DOJ.ToString(),
                    teacher.Country,
                    teacher.Gender,
                    teacher.Hobbie);}

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Excel_Login_Report.xlsx");
                }
            }
        }



        [HttpPost]
        public IActionResult PDF()
        {

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                var image = iTextSharp.text.Image.GetInstance("wwwroot/PICS/Clg.jpg");
                image.Alignment = Element.ALIGN_CENTER;
                document.Add(image);
                ///report heading
                Paragraph para1 = new Paragraph("NALANDA PUBLIC SCHOOL", new Font(Font.FontFamily.COURIER, 30));
                para1.Alignment = Element.ALIGN_CENTER;
                document.Add(para1);
                
                Paragraph para2 = new Paragraph("Student Login Report", new Font(Font.FontFamily.HELVETICA, 15));
                para2.Alignment = Element.ALIGN_CENTER;
                document.Add(para2);

                Paragraph para3 = new Paragraph(" ", new Font(Font.FontFamily.HELVETICA, 10));
                para3.Alignment = Element.ALIGN_CENTER;
                document.Add(para3);

                ///for table generate
                PdfPTable Tblreg = new PdfPTable(10);


                PdfPCell cell1 = new PdfPCell(new Phrase("Teacher_Id", new Font(Font.FontFamily.HELVETICA, 13)));
                cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell1.BorderWidthBottom = 1f;
                cell1.BorderWidthLeft = 1f;
                cell1.BorderWidthRight = 1f;
                cell1.BorderWidthTop = 1f;
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.VerticalAlignment = Element.ALIGN_CENTER;
                Tblreg.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase("Teacher_Name", new Font(Font.FontFamily.HELVETICA, 13)));
                cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell2.BorderWidthBottom = 1f;
                cell2.BorderWidthLeft = 1f;
                cell2.BorderWidthRight = 1f;
                cell2.BorderWidthTop = 1f;
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.VerticalAlignment = Element.ALIGN_CENTER;
                Tblreg.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase("Teacher_Age", new Font(Font.FontFamily.HELVETICA, 13)));
                cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell3.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell3.BorderWidthBottom = 1f;
                cell3.BorderWidthLeft = 1f;
                cell3.BorderWidthRight = 1f;
                cell3.BorderWidthTop = 1f;
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell3.VerticalAlignment = Element.ALIGN_CENTER;
                Tblreg.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase("Teacher_Address", new Font(Font.FontFamily.HELVETICA, 13)));
                cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell4.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell4.BorderWidthBottom = 1f;
                cell4.BorderWidthLeft = 1f;
                cell4.BorderWidthRight = 1f;
                cell4.BorderWidthTop = 1f;
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.VerticalAlignment = Element.ALIGN_CENTER;
                Tblreg.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase("Teacher_Salary", new Font(Font.FontFamily.HELVETICA, 13)));
                cell5.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell5.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell5.BorderWidthBottom = 1f;
                cell5.BorderWidthLeft = 1f;
                cell5.BorderWidthRight = 1f;
                cell5.BorderWidthTop = 1f;
                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5.VerticalAlignment = Element.ALIGN_CENTER;
                Tblreg.AddCell(cell5);

                PdfPCell cell6 = new PdfPCell(new Phrase("Teahching_Class", new Font(Font.FontFamily.HELVETICA, 13)));
                cell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell6.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell6.BorderWidthBottom = 1f;
                cell6.BorderWidthLeft = 1f;
                cell6.BorderWidthRight = 1f;
                cell6.BorderWidthTop = 1f;
                cell6.HorizontalAlignment = Element.ALIGN_CENTER;
                cell6.VerticalAlignment = Element.ALIGN_CENTER;
                Tblreg.AddCell(cell6);


                PdfPCell cell7 = new PdfPCell(new Phrase("Country", new Font(Font.FontFamily.HELVETICA, 13)));
                cell7.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell7.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell7.BorderWidthBottom = 1f;
                cell7.BorderWidthLeft = 1f;
                cell7.BorderWidthRight = 1f;
                cell7.BorderWidthTop = 1f;
                cell7.HorizontalAlignment = Element.ALIGN_CENTER;
                cell7.VerticalAlignment = Element.ALIGN_CENTER;
                Tblreg.AddCell(cell7);

                PdfPCell cell8 = new PdfPCell(new Phrase("Gender", new Font(Font.FontFamily.HELVETICA, 13)));
                cell8.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell8.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell8.BorderWidthBottom = 1f;
                cell8.BorderWidthLeft = 1f;
                cell8.BorderWidthRight = 1f;
                cell8.BorderWidthTop = 1f;
                cell8.HorizontalAlignment = Element.ALIGN_CENTER;
                cell8.VerticalAlignment = Element.ALIGN_CENTER;
                Tblreg.AddCell(cell8);

                PdfPCell cell9 = new PdfPCell(new Phrase("Hobbie", new Font(Font.FontFamily.HELVETICA, 13)));
                cell9.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell9.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell9.BorderWidthBottom = 1f;
                cell9.BorderWidthLeft = 1f;
                cell9.BorderWidthRight = 1f;
                cell9.BorderWidthTop = 1f;
                cell9.HorizontalAlignment = Element.ALIGN_CENTER;
                cell9.VerticalAlignment = Element.ALIGN_CENTER;
                Tblreg.AddCell(cell9);

                PdfPCell cell10 = new PdfPCell(new Phrase("DOJ", new Font(Font.FontFamily.HELVETICA, 13)));
                cell10.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell10.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell10.BorderWidthBottom = 1f;
                cell10.BorderWidthLeft = 1f;
                cell10.BorderWidthRight = 1f;
                cell10.BorderWidthTop = 1f;
                cell10.HorizontalAlignment = Element.ALIGN_CENTER;
                cell10.VerticalAlignment = Element.ALIGN_CENTER;
                Tblreg.AddCell(cell10);

                var data = (from a in _db.teachers select a).ToList();
                for (int i = 0; i < data.Count; i++)
                {
                   
                    PdfPCell cell_1 = new PdfPCell(new Phrase(data[i].Teacher_Id.ToString()));
                    PdfPCell cell_2 = new PdfPCell(new Phrase(data[i].Teacher_Name));
                    PdfPCell cell_3 = new PdfPCell(new Phrase(data[i].Teacher_Age.ToString()));
                    PdfPCell cell_4 = new PdfPCell(new Phrase(data[i].Teacher_Address));
                    PdfPCell cell_5 = new PdfPCell(new Phrase(data[i].Teacher_Salary.ToString()));
                    PdfPCell cell_6 = new PdfPCell(new Phrase(data[i].Teahching_Class));
                    PdfPCell cell_7 = new PdfPCell(new Phrase(data[i].Country.ToString()));
                    PdfPCell cell_8 = new PdfPCell(new Phrase(data[i].Gender.ToString()));
                    PdfPCell cell_9 = new PdfPCell(new Phrase(data[i].Hobbie.ToString()));
                    PdfPCell cell_10 = new PdfPCell(new Phrase(data[i].DOJ.ToString()));
                
         
                    cell_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_3.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_5.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_6.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_7.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_8.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_9.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_10.HorizontalAlignment = Element.ALIGN_CENTER;
                    // cell_4.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblreg.AddCell(cell_1);
                    Tblreg.AddCell(cell_2);
                    Tblreg.AddCell(cell_3);
                    Tblreg.AddCell(cell_4);
                    Tblreg.AddCell(cell_5);
                    Tblreg.AddCell(cell_6);
                    Tblreg.AddCell(cell_7);
                    Tblreg.AddCell(cell_8);
                    Tblreg.AddCell(cell_9);
                    Tblreg.AddCell(cell_10);
                }
                //var data = _db.tblregs.Find(id);

                //var data = _db.tblregs.ToList();
                document.Add(Tblreg);
                document.Close();
                writer.Close();
                var constant = ms.ToArray();
                return File(constant, "applaction/vnd", "Login_Report.pdf");
            }

        }


        public IActionResult Details(int id)
        {
            var detail = _db.teachers.Where(x => x.Teacher_Id == id).FirstOrDefault();
            return View(detail);
        }
    }
}
