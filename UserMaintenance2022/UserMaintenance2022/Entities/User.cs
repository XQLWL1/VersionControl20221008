﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMaintenance2022.Entities
{
    class User
    {

        public Guid ID { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get
            { return FirstName + ' ' + LastName; }
        }
    }
}
