using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class ResponseDto <T>
    {
        public ResponseDto()
        {
            Success = true;
            Errors= new List<string>();
            StatusCode = HttpStatusCode.OK;

        }

        public bool Success { get; set; }

        public String Message { get; set; }

        public T Result { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        private List<string> _errors;

        public List<string> Errors { 
            
            get { return _errors; }
            set {
                _errors = value; 
            if(value.Count > 0) 
                { 
                    Success = false; 
                }
            }
        
        }

        public void AddError(string message)
        {
            if (!String.IsNullOrEmpty(message))
            {
                Errors.Add(message);
                Success = false;
            }
        }
    }
}
