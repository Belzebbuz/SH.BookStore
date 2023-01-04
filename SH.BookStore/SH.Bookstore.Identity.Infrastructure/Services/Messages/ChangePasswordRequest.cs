namespace SH.Bookstore.Identity.Infrastructure.Services.Messages;
public record ChangePasswordRequest(string OldPassword, string NewPassword, string ConfirmNewPassword);

