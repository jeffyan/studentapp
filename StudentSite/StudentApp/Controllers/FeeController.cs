using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StudentApp.Controllers
{
    public class FeeController : ApiController
    {
        
        // POST api/fee
        public void Post(FeePayment fp)
        {
            ArrayOfStudent students = XmlFileUtil.Deserialize();
            Student student = students.studentsList.FirstOrDefault((s) => s.StudentID == fp.StudentID);

            // sort fee by payment type
            if (fp.paymentType == "Rank")
            {
                student.fees.Sort(delegate(Fee x, Fee y)
                {
                    return x.Rank.CompareTo(y.Rank);
                });
            } 
            else if (fp.paymentType == "Date")
            {
                student.fees.Sort(delegate(Fee x, Fee y)
                {
                    return x.DueDate.CompareTo(y.DueDate);
                });
            }

            List<Fee> newFees = new List<Fee>();
            double payment = 0;
            double.TryParse(fp.amount, out payment);

            //calculate fee payments
            foreach (Fee f in student.fees)
            {
                double amountAssessed = 0;
                double amountPaid = 0;

                double.TryParse(f.AmountAssessed, out amountAssessed);
                double.TryParse(f.AmountPaid, out amountPaid);

                if (amountPaid < amountAssessed)
                {
                    if (payment+amountPaid < amountAssessed)
                    {
                        f.AmountPaid = Math.Round(payment+amountPaid, 2).ToString();
                        payment = 0;
                    }
                    else
                    {
                        f.AmountPaid = amountAssessed.ToString();
                        payment = Math.Round(payment + amountPaid - amountAssessed, 2);
                    }
                }

                newFees.Add(f);
            }

            student.fees = newFees;

            // save to xml
            XmlFileUtil.SaveStudent(student);
        }
    }
}
