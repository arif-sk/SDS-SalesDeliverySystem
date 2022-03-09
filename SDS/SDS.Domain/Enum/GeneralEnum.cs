using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDS.Domain.Enum
{
    public class EnumExtensions
    {
        public enum OrderStatusEnum
        {
            Open = 0,
            InProgress = 1,
            Completed = 2,
            Rejected = 3,
            Due = 4,
            Pending = 5,
            Approved = 6
        }

        public enum UserRoleEnum
        {
            [Description("Admin")]
            Admin = 0,
            [Description("Sales Executive")]
            OrderGenerator = 1,
            [Description("Sales Manager")]
            OrderApproval = 2
        }
    }
}
