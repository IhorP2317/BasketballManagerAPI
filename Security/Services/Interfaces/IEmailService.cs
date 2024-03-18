﻿namespace Security.Services.Interfaces {
    public interface IEmailService
    {
        Task<bool> SendEmail(string receiverEmail, string subject, string text);
    }
}