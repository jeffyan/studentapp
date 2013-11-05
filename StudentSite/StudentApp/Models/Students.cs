using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace StudentApp.Models
{
    public class Student
    {
        [XmlElement()]
        public int StudentID { get; set; }

        [XmlElement()]
        public string FirstName { get; set; }

        [XmlElement()]
        public string LastName { get; set; }

        [XmlElement()]
        public string Grade { get; set; }

        [XmlArray("Fees")]
        [XmlArrayItem("Fee", typeof(Fee))]
        public List<Fee> fees = new List<Fee>();

    }

    [XmlRoot("ArrayOfStudent")]
    public class ArrayOfStudent
    {
        [XmlElement("Student")]
        public List<Student> studentsList = new List<Student>();

    }

    public class Fee
    {
        public string FeeID { get; set; }
        public string Name { get; set; }
        public string AmountAssessed { get; set; }
        public string AmountPaid { get; set; }
        public string Rank { get; set; }
        public string DueDate { get; set; }
    }

    public class FeePayment
    {
        public int StudentID { get; set; }
        public string paymentType { get; set; }
        public string amount { get; set; }
    }

    public class XmlFileUtil
    {
        private static string xmlpath = HttpContext.Current.Server.MapPath("~/App_data/students.xml");
        public static ArrayOfStudent Deserialize()
        {
            ArrayOfStudent students;
            XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfStudent));

            using (StreamReader reader = new StreamReader(xmlpath))
            {
                students = (ArrayOfStudent)serializer.Deserialize(reader);
            }
            return students;
        }

        public static void SaveStudent(Student newStudent)
        {
            ArrayOfStudent students = XmlFileUtil.Deserialize();
            Student oldStudent = students.studentsList.Find(delegate(Student s)
            {
                return (s.StudentID == newStudent.StudentID);
            });
            students.studentsList.Remove(oldStudent);
            students.studentsList.Add(newStudent);

            XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfStudent));

            using (StreamWriter writer = new StreamWriter(xmlpath))
            {
                serializer.Serialize(writer, students);
            }


        }
    }
}