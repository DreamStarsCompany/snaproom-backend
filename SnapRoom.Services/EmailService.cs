﻿using Microsoft.Extensions.Configuration;
using SnapRoom.Common.Enum;
using SnapRoom.Contract.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SnapRoom.Services
{
	public class EmailService
	{
		private readonly IConfiguration _confg;

		public EmailService(IConfiguration config)
		{
			_confg = config;
		}

		public Task SendVerificationMail(Account account)
		{
			string mail = "kidsvaccinecorp@gmail.com";
			string pw = "swdg enlv vfrk rpdf";

			SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential(mail, pw)
			};

			string backendUrl = _confg["BACKEND_URL"]!;

			string verificationUrl = $"{backendUrl}/api/auth/verify-account?token={account.VerificationToken}";
			string message = "";
			string imageUrl = "https://dataimage.blob.core.windows.net/snaproom/app-banner.png";


			if (account.Role == RoleEnum.Customer)
			{
				message = $@"
					<div style='font-family: Arial, sans-serif; line-height: 1.5;'>
						<div style='text-align: center; margin-bottom: 20px;'>
							<img src='{imageUrl}' alt='SnapRoom Banner' style='max-width: 100%; height: auto;' />
						</div>

						<p>Cảm ơn bạn đã đăng ký tài khoản tại <strong>SnapRoom</strong>! Để hoàn tất quá trình đăng ký, vui lòng xác minh email của bạn bằng cách nhấn vào nút bên dưới:</p>

						<p style='text-align: center;'>
							<a href='{verificationUrl}' 
							   style='display: inline-block; padding: 10px 15px; background-color: #4CAF50; color: white;
									  text-decoration: none; border-radius: 5px; font-weight: bold;'>
								🔗 Nhấn vào đây để xác minh
							</a>
						</p>

						<p>Liên kết này sẽ hết hạn sau <strong>15 phút</strong>, vui lòng xác minh tài khoản sớm nhất có thể.</p>

						<p>Nếu bạn không yêu cầu tạo tài khoản, vui lòng bỏ qua email này.</p>

						<p>Trân trọng,<br><strong>Đội ngũ hỗ trợ SnapRoom</strong></p>
					</div>";

			}
			else
			{
				message = $@"
					<div style='font-family: Arial, sans-serif; line-height: 1.5;'>
						<div style='text-align: center; margin-bottom: 20px;'>
							<img src='{imageUrl}' alt='SnapRoom Banner' style='max-width: 100%; height: auto;' />
						</div>

						<p>Chào mừng bạn đến với <strong>SnapRoom</strong>! Cảm ơn bạn đã đăng ký trở thành <strong>nhà thiết kế</strong> của chúng tôi. Để hoàn tất quá trình tạo tài khoản, vui lòng xác minh email bằng cách nhấn vào nút bên dưới:</p>

						<p style='text-align: center;'>
							<a href='{verificationUrl}' 
							   style='display: inline-block; padding: 10px 15px; background-color: #2196F3; color: white;
									  text-decoration: none; border-radius: 5px; font-weight: bold;'>
								🔗 Xác minh email của bạn
							</a>
						</p>

						<p>Liên kết này sẽ hết hạn sau <strong>15 phút</strong>. Vui lòng hoàn tất xác minh để bắt đầu chia sẻ những thiết kế tuyệt vời của bạn.</p>

						<p>Nếu bạn không gửi yêu cầu tạo tài khoản, vui lòng bỏ qua email này.</p>

						<p>Thân ái,<br><strong>Đội ngũ SnapRoom</strong></p>
					</div>";
			}

			var mailMessage = new MailMessage
			{
				From = new MailAddress(mail),
				Subject = "Xác minh tài khoản SnapRoom",
				Body = message,
				IsBodyHtml = true
			};

			// Thêm người nhận
			mailMessage.To.Add(account.Email);

			// Gửi email
			return client.SendMailAsync(mailMessage);
		}

		public Task SendResetPasswordEmail(string email, string resetToken)
		{
			var mail = "kidsvaccinecorp@gmail.com";
			var pw = "swdg enlv vfrk rpdf";

			var client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential(mail, pw)
			};

			var backendUrl = _confg["BACKEND_URL"];
			string resetUrl = $"{backendUrl}/api/authentication/verify-reset-password?token={resetToken}";

			var message = $@"
				<div style='font-family: Arial, sans-serif; line-height: 1.5;'>
					<p>Chúng tôi nhận được yêu cầu khôi phục mật khẩu cho tài khoản của bạn tại <strong>Kids Vaccine</strong>. Nếu bạn đã yêu cầu điều này, vui lòng nhấn vào nút bên dưới để đặt lại mật khẩu:</p>

					<p style='text-align: center;'>
						<a href='{resetUrl}' 
						   style='display: inline-block; padding: 10px 15px; background-color: #FF5733; color: white;
								  text-decoration: none; border-radius: 5px; font-weight: bold;'>
							🔑 Đặt lại mật khẩu
						</a>
					</p>

					<p>Liên kết này sẽ hết hạn sau <strong>15 phút</strong>, vui lòng đặt lại mật khẩu sớm nhất có thể.</p>

					<p>Nếu bạn không yêu cầu khôi phục mật khẩu, vui lòng bỏ qua email này.</p>

					<p>Trân trọng,<br><strong>Bộ phận hỗ trợ KVC</strong></p>
				</div>";

			var mailMessage = new MailMessage
			{
				From = new MailAddress(mail),
				Subject = "Xác minh tài khoản Kids Vaccine",
				Body = message,
				IsBodyHtml = true
			};

			// Thêm người nhận
			mailMessage.To.Add(email);

			// Gửi email
			return client.SendMailAsync(mailMessage);
		}

		public Task SendUpdateEmailEmail(string newEmail, string otp)
		{
			var mail = "kidsvaccinecorp@gmail.com";
			var pw = "swdg enlv vfrk rpdf";

			var client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential(mail, pw)
			};

			var backendUrl = _confg["BACKEND_URL"];
			string updateEmailUrl = $"{backendUrl}/api/authentication/verify-reset-password?token={otp}";

			var message = $@"
				<div style='font-family: Arial, sans-serif; line-height: 1.5;'>
					<p>Bạn đã yêu cầu thay đổi địa chỉ email cho tài khoản của mình tại <strong>Kids Vaccine</strong>.</p>

					<p>Mã xác minh của bạn là:</p>

					<p style='text-align: center; font-size: 24px; font-weight: bold; background-color: #f3f3f3; padding: 10px; 
							  display: inline-block; border-radius: 5px;'>
						{otp}
					</p>

					<p>Vui lòng nhập mã này trong vòng <strong>15 phút</strong> để hoàn tất quá trình thay đổi email.</p>

					<p>Nếu bạn không yêu cầu thay đổi email, vui lòng bỏ qua email này.</p>

					<p>Trân trọng,<br><strong>Bộ phận hỗ trợ KVC</strong></p>
				</div>";
			var mailMessage = new MailMessage
			{
				From = new MailAddress(mail),
				Subject = "Xác minh tài khoản Kids Vaccine",
				Body = message,
				IsBodyHtml = true
			};

			// Thêm người nhận
			mailMessage.To.Add(newEmail);

			// Gửi email
			return client.SendMailAsync(mailMessage);

		}

	}
}
