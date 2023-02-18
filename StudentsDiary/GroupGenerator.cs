using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsDiary
{
    public class GroupGenerator
    {         
        public List<StudentGroup> IdGroupGenerator()
        {
            var abc = "abc";      
            var studentGroups = new List<StudentGroup>();
            for (int i = 1; i < 4; i++)
            {
                for (int z = 0; z < 3; z++)
                {
                    var letter = abc[z];
                    var group = new StudentGroup
                    {
                        IdGroup = i + letter.ToString(),                       
                };
                    studentGroups.Add(group);
                }
            }          
            return studentGroups;
        }
    }
}
