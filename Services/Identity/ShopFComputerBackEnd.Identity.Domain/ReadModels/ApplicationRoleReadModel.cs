﻿using Iot.Core.Domain.Interfaces.Audited;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.ReadModels
{
    public class ApplicationRoleReadModel : IdentityRole<Guid>, IAuditedEntity
    {
        public DateTimeOffset CreatedTime {get ; set;}

        public Guid? CreatedBy {get ; set;}

        public DateTimeOffset ModifiedTime {get ; set;}

        public Guid? ModifiedBy {get ; set;}

        public bool IsDeleted {get ; set;}

        public Guid? DeletedBy {get ; set;}

        public DateTimeOffset DeletedTime {get ; set;}
    }
}
