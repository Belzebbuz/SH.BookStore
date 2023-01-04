using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SH.Bookstore.Identity.Infrastructure.Services.Messages;
public record ToggleUserStatusRequest(string UserId, bool IsActive);
