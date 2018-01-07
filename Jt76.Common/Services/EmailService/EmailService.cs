namespace Jt76.Common.Services.EmailService
{
	using System;
	using System.IO;
	using ConfigSettings;
	using ConfigSettings.EmailSettings;
	using Enums;
	using Interfaces;
	using MailKit.Net.Smtp;
	using Microsoft.Extensions.Options;
	using MimeKit;
	using Models;

	//https://components.xamarin.com/gettingstarted/mimekit
	public class EmailService : IEmailService
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly EmailSettings _emailSettings;
		private readonly IFileService _fileService;


		/// <summary>
		///     This is a config based email service
		/// </summary>
		/// <param name="applicationSettings">Used for application mailing values</param>
		/// <param name="emailSettings">Settings for smtp or pickup directory</param>
		/// <param name="fileService">Email directory found in App_Data used for this service</param>
		public EmailService(
			IOptions<ApplicationSettings> applicationSettings,
			IOptions<EmailSettings> emailSettings,
			IFileService fileService)
		{
			_applicationSettings = applicationSettings.Value;
			_emailSettings = emailSettings.Value;
			_fileService = fileService;
		}

		/// <summary>
		///     A simple email service, for non multiple address sets.
		///     Note: Please see the <see cref="Email" /> object
		///     overload for multiple address sets
		/// </summary>
		/// <param name="to">The primary to email address</param>
		/// <param name="carbonCopy">Carbon copy address</param>
		/// <param name="backupCarbonCopy">Backup carbon copy address</param>
		/// <param name="subject">The email's subject line</param>
		/// <param name="message">The emails message as a string</param>
		public async void SendMail(
			string to,
			string carbonCopy,
			string backupCarbonCopy,
			string subject,
			string message)
		{
			MimeMessage mimeMessage = new MimeMessage();
			mimeMessage.From.Add(new MailboxAddress(_applicationSettings.Name, _emailSettings.EmailAddress));

			mimeMessage.To.Add(new MailboxAddress(to));

			if (!string.IsNullOrWhiteSpace(carbonCopy))
				mimeMessage.To.Add(new MailboxAddress(carbonCopy));
			if (!string.IsNullOrWhiteSpace(backupCarbonCopy))
				mimeMessage.To.Add(new MailboxAddress(backupCarbonCopy));

			mimeMessage.Subject = subject;
			mimeMessage.Body = new TextPart("plain")
			{
				Text = message
			};

			/*
				var bodyBuilder = new BodyBuilder();
				bodyBuilder.HtmlBody = @"<b>This is bold and this is <i>italic</i></b>";
				message.Body = bodyBuilder.ToMessageBody();
			*/

			if (_emailSettings.UsePickupDirectory)
				using (StreamWriter data =
					File.CreateText(
						Path.Combine(
							_fileService.GetDirectoryFolderLocation(DirectoryFolders.Email),
							Guid.NewGuid() + "_Email.eml")))
				{
					mimeMessage.WriteTo(data.BaseStream);
				}
			else
				using (SmtpClient client = new SmtpClient())
				{
					await client.ConnectAsync(_emailSettings.Smtp, _emailSettings.Port);

					// Note: since we don't have an OAuth2 token, disable 	
					// the XOAUTH2 authentication mechanism.     
					client.AuthenticationMechanisms.Remove("XOAUTH2");

					await client.AuthenticateAsync(_emailSettings.EmailAddress, _emailSettings.EmailPassword);
					await client.SendAsync(mimeMessage);
					await client.DisconnectAsync(true);
				}
		}


		/// <summary>
		///     Sends an email using an email class allowing for list entries
		///     Note: For email address collections this is an intelligent MimeMessage.
		///     If a BCC address is a To Address it will not be added as a BCC and
		///     if there are duplicates at any level, they will not be added.
		/// </summary>
		/// <param name="email">An <see cref="Email" /> object</param>
		public async void SendMail(Email email)
		{
			MimeMessage mimeMessage = new MimeMessage();
			mimeMessage.From.Add(new MailboxAddress(_applicationSettings.Name, _emailSettings.EmailAddress));

			//Note: this is an intelligent MimeMessage
			//If a BCC address is a To Address it will not be added as a BCC
			//If there are duplicates at any level, they will not be added
			email.ToAddresses.ForEach(x => mimeMessage.To.Add(new MailboxAddress(x)));
			email.CarbonCopies.ForEach(x => mimeMessage.Cc.Add(new MailboxAddress(x)));
			email.BackupCarbonCopies.ForEach(x => mimeMessage.Bcc.Add(new MailboxAddress(x)));

			mimeMessage.Subject = email.Message;
			mimeMessage.Body = new TextPart("plain")
			{
				Text = email.Body
			};

			/*
				var bodyBuilder = new BodyBuilder();
				bodyBuilder.HtmlBody = @"<b>This is bold and this is <i>italic</i></b>";
				message.Body = bodyBuilder.ToMessageBody();
			*/

			if (_emailSettings.UsePickupDirectory)
				using (StreamWriter data =
					File.CreateText(
						Path.Combine(
							_fileService.GetDirectoryFolderLocation(DirectoryFolders.Email),
							Guid.NewGuid() + "_Email.eml")))
				{
					mimeMessage.WriteTo(data.BaseStream);
				}
			else
				using (SmtpClient client = new SmtpClient())
				{
					await client.ConnectAsync(_emailSettings.Smtp, _emailSettings.Port);

					// Note: since we don't have an OAuth2 token, disable 	
					// the XOAUTH2 authentication mechanism.     
					client.AuthenticationMechanisms.Remove("XOAUTH2");

					await client.AuthenticateAsync(_emailSettings.EmailAddress, _emailSettings.EmailPassword);
					await client.SendAsync(mimeMessage);
					await client.DisconnectAsync(true);
				}
		}
	}
}