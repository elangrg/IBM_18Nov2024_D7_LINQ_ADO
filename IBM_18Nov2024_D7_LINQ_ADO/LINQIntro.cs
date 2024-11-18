using System;
using System.Collections.Generic;
using System.Linq;

//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBM_18Nov2024_D7_LINQ_ADO
{
    internal class LINQIntro
    {
        static void Main(string[] args)
        {

            List<Subject> lstSubjects = new List<Subject>() 
            { 
                new Subject { SubjectID=1, SubjectName="CSharp", SubjectDescription="C-Sharp Programming", SubjectType="PL"},
                new Subject { SubjectID=2, SubjectName="ADO.NET", SubjectDescription="DB API", SubjectType="DB"},
                new Subject { SubjectID=2, SubjectName="VB", SubjectDescription="VB Programming", SubjectType="PL"},
            };


            IEnumerable<Subject> qry = lstSubjects.Where(s => 
            s.SubjectType == "PL");

            foreach (var sub in qry)
            {
                Console.WriteLine(sub.SubjectName);
            }

            Console.WriteLine(  lstSubjects.Count(s=> 
            s.SubjectType == "PL"));

            Console.ReadKey();

        }
    }


    class Subject
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectDescription { get; set; }
        public string SubjectType { get; set; } 
    }

}
