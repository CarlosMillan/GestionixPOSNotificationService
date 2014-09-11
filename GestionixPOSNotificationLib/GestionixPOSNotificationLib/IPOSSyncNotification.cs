using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GestionixPOSNotificationLib
{  
  [ServiceContract(CallbackContract = typeof(IPOSSyncNotificationCallBack))] 
  public interface IPOSSyncNotification
  {
	[OperationContract(IsOneWay = true)]
	void Authenticate(string apikey);

	[OperationContract(IsOneWay = true)]
	void Renew(string apikey);

	[OperationContract(IsOneWay = true)]
	void Exit();

	[OperationContract(IsOneWay = true)]
	void NotifyChanges();
  }

  public interface IPOSSyncNotificationCallBack
  {
	[OperationContract(IsOneWay = true)]
	void IsAuthenticated(bool Authenticated);

	[OperationContract(IsOneWay = true)]
	void AvailableChanges();
  }
  
  public class UserCompany
  {
	public string APIKey { get; set; }
	public IPOSSyncNotificationCallBack channel { get; set; }
  }
}
