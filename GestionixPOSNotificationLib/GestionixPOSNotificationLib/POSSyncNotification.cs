using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace GestionixPOSNotificationLib
{ 
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public class POSSyncNotification : IPOSSyncNotification
  {
	private static List<UserCompany> _subscribers = new List<UserCompany>();	

	#region Public Methods
	public void Authenticate(string apikey)
	{
	  IPOSSyncNotificationCallBack callback = OperationContext.Current.GetCallbackChannel<IPOSSyncNotificationCallBack>();
	  Authenticate(apikey, callback);
	}

	public void Renew(string apikey)
	{
	  IPOSSyncNotificationCallBack Callback = OperationContext.Current.GetCallbackChannel<IPOSSyncNotificationCallBack>();
	  Exit(Callback);
	  Authenticate(apikey, Callback);
	}

	public void Exit()
	{
	  IPOSSyncNotificationCallBack Callback = OperationContext.Current.GetCallbackChannel<IPOSSyncNotificationCallBack>();
	  Exit(Callback);
	}

	public void NotifyChanges(string apikey)
	{
	  if (_subscribers.Exists(x => x.APIKey.Equals(apikey)))
	  {
		IPOSSyncNotificationCallBack Callback = _subscribers.Find(x => x.APIKey.Equals(apikey)).channel;
		Callback.AvailableChanges();
	  }
	}
	#endregion

	#region Private Methods
	private void Authenticate(string apikey, IPOSSyncNotificationCallBack callback)
	{	  
	  Subscribe(apikey, callback);
	  callback.IsAuthenticated(true);
	}

	private void Exit(IPOSSyncNotificationCallBack callback)
	{
	  UnSubscribe(callback);
	}

	private void Subscribe(string apikey, IPOSSyncNotificationCallBack callback)
	{
	  try
	  {
		if (!_subscribers.Exists(x => x.channel == callback))
		{
		  _subscribers.Add(new UserCompany() { APIKey = apikey });
		}
	  }
	  catch{}
	}

	private void UnSubscribe(IPOSSyncNotificationCallBack callback)
	{
	  try
	  {
		_subscribers.Remove(_subscribers.Find(x => x.channel == callback));
	  }
	  catch {}
	}
	#endregion
  }
}
