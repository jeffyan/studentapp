using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;

namespace StudentApp.Controllers
{
    public class StudentsController : ApiController
    {
        public IEnumerable<Student> GetAllStudents()
        {
            ArrayOfStudent students = XmlFileUtil.Deserialize();
            return students.studentsList;
        }

        public Student Get(int id)
        {
            ArrayOfStudent students = XmlFileUtil.Deserialize();
            var student = students.studentsList.FirstOrDefault((s) => s.StudentID == id);
            return student;
        }
        
    }
}
