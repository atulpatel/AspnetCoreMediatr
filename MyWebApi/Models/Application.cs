using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public string ApplicationType { get; set; }
        public string ApplicantName { get; set; }
        public DateTime ApplicantDateOfBirth { get; set; }
        public string ApplicationAddress { get; set; }
        public string ApplicationCity { get; set; }
        public string ApplicationState { get; set; }
        public string ApplicationZip { get; set; }
        public string ApplicantEmailId { get; set; }
        public DateTime ApplicationCreationDate { get; set; }
        public DateTime ApplicationLastModifiedDate { get; set; }
    }
}
