using Application.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit.Encodings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    public class ResetPasswordTokens
    {

        public String ResetToken { get; set; }
        public DateTime Expiry { get; set; }
        public Boolean isUsed { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public AppUsers User { get; set; }
    }
}
