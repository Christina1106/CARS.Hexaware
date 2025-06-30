using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CrimeAnalysis.App.MyExceptions
{
    public class IncidentNumberNotFoundException : ApplicationException
    {
        public IncidentNumberNotFoundException(string message) : base(message) { }
    }
}
