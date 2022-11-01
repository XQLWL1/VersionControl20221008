using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMaintenance2022.Entities
{
    class User
    {

        public Guid ID { get; set; } = Guid.NewGuid();

        //7. feladat. Törölni kell ezeket - én kikommentelem:
        //public string FirstName { get; set; }

        //public string LastName { get; set; }

        //7. feladat - át kell ezt a részt alakítani:
        /*public string FullName
        {
            get
            { return FirstName + ' ' + LastName; }
        }*/

        public string FullName { get; set; }
    }
}
