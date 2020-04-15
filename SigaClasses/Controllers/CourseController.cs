using SigaClasses.Models;
using System.Web.Http;

namespace SigaClasses.Controllers
{
    public class CourseController : ApiController
    {

        [HttpGet]
        public Disciplinas Get(string Curso)
        {
            return SigaDriver.GetCurrentClassesInfo(Curso);
        }
        [HttpGet]
        public Disciplinas Get()
        {
            return SigaDriver.GetAllClassesInfo();
        }
    }
}
