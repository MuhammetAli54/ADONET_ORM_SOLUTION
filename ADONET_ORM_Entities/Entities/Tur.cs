using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADONET_ORM_Common;
using System.Threading.Tasks;

namespace ADONET_ORM_Entities.Entities
{
    [Table(TableName ="Turler",IdentityColumn ="TurId",PrimaryColumn ="TurId")]
    public class Tur
    {
        public int TurId { get; set; }
        public string TurAdi { get; set; }
        public DateTime GuncellenmeTarihi { get; set; }

        
    }
}
