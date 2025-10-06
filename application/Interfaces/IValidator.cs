﻿using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Interfaces
{
    public interface IValidator
    {
        GenericResponse GetResponse(string message, bool isSuccess);
    }
}
