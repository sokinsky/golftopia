﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA.Data.Models {
    public class Event : Model {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
