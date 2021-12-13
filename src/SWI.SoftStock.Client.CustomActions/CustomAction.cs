namespace SWI.SoftStock.Client.CustomActions
{
	using System;
	using System.Globalization;
	using Microsoft.Deployment.WindowsInstaller;
	using SWI.SoftStock.Client.Repositories;
	using SWI.SoftStock.Client.Storages;
	using System.Collections.Generic;
	using System.Security.Principal;

	public class CustomActions
	{
		[CustomAction]
		public static ActionResult BeforeUninstallAction(Session session)
		{
			session.Log("Begin BeforeUninstallAction");
			try
			{
				session.Log("BeforeUninstallAction condition true: clean local storage");
				var localStorage = new LocalStorage(new FileRepository());
				localStorage.RemoveAll();
			}
			catch (Exception e)
			{
				session.Log(e.ToString());
				return ActionResult.Failure;
			}
			session.Log("End BeforeUninstallAction");
			return ActionResult.Success;
		}

		/// <summary>
		/// Запускаем из WIX как deffered, потому что необхолдимо доп права на сонфиг файл при записи uniqueCompanyId
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		[CustomAction]
		public static ActionResult SetCompanyUniqueIdAction(Session session)
		{
			session.Log("Begin SetCompanyUniqueIdAction");

			CustomActionData data = session.CustomActionData;
			
			foreach (var key in data.Keys)
			{
				session.Log(string.Format("{0}:{1}", key, data[key]));
			}
			try
			{
				WindowsIdentity curIdentity = WindowsIdentity.GetCurrent();
				WindowsPrincipal myPrincipal = new WindowsPrincipal(curIdentity);

				List<string> groups = new List<string>();

				foreach (IdentityReference irc in curIdentity.Groups)
				{
					groups.Add(((NTAccount) irc.Translate(typeof (NTAccount))).Value);
				}

				session.Log(string.Format(@"Name:{0} System:{1} Authenticated:{2} BuiltinAdmin:{3} Identity:{4} Groups:{5}",
					curIdentity.Name, curIdentity.IsSystem, curIdentity.IsAuthenticated,
					myPrincipal.IsInRole(WindowsBuiltInRole.Administrator) ? "True" : "False", myPrincipal.Identity,
					string.Join(string.Format(",{0}\t\t", Environment.NewLine), groups.ToArray())));
				
				var localStorage = new LocalStorage(new FileRepository());
				var uniqueCompanyId = data["SETTINGSUSERID"];
				session.Log(string.Format("uniqueCompanyId:{0}", uniqueCompanyId));
				localStorage.SetCompanyId(new Guid(uniqueCompanyId));
			}
			catch (Exception e)
			{
				session.Log(e.ToString());
				return ActionResult.Failure;
			}
			session.Log("End SetCompanyUniqueIdAction");
			return ActionResult.Success;
		}

		[CustomAction]
		public static ActionResult CheckAction(Session session)
		{
			session.Log("Begin CheckAction");
			session["USERIDCHECKED"] = "0";
			var request = new CheckRequest();
			request.ServiceAddress = session["ServiceAddress"];
			request.UniqueCompanyId = session["SETTINGSUSERID"];
			try
			{
				var checker = new WsChecker();
				var isChecked = checker.Check(request);
				session["USERIDCHECKED"] = isChecked.ToString(CultureInfo.InvariantCulture);
			}
			catch (Exception e)
			{
				session["CheckActionMessage"] = e.ToString();
				session["USERIDCHECKED"] = "2";
				return ActionResult.Success;
			}
			session.Log("End CheckAction");
			return ActionResult.Success;
		}
	}
}