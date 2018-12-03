using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManager.WPF.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public IList<string> Categories { get; set; }

        public Department()
        {
            Categories = new List<string>();
        }
    }
}
